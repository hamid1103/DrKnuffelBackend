using DrKnuffelBackEnd.Models;
using DrKnuffelBackEnd.Repositories.Progress;
using DrKnuffelBackEnd.Repositories.Step;
using DrKnuffelBackEnd.Repositories.UserData;
using DrKnuffelBackEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace DrKnuffelBackEnd.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class ProgressController : ControllerBase
{
    private readonly IProgressRepo _progressRepo;
    private readonly IStepRepo _stepRepo;
    private readonly IAuthenticationService _authenticationService;
    private readonly IExtraUserData _extraUserDataRepo;

    public ProgressController(IProgressRepo progressRepo, IExtraUserData userDataRepoRepo, IStepRepo stepRepo, IAuthenticationService _authenticationService)
    {
        _progressRepo = progressRepo;
        _extraUserDataRepo = userDataRepoRepo;
        this._stepRepo = stepRepo;
        this._authenticationService = _authenticationService;
    }

    [HttpGet(Name = "GetProgressItems")]
    public async Task<ActionResult<List<Models.Progress>>> GetAsync()
    {
        string userId = _authenticationService.GetCurrentAuthenticatedUserId();
        if (!string.IsNullOrEmpty(userId))
        {
            //Get UserData related to user.
            Models.UserData eud = await _extraUserDataRepo.GetAsyncByUserId(userId);
            var list = await _progressRepo.GetAsyncByUserDataId(eud.Id.ToString());
            return Ok(list);
        }
        //Just in case :)
        return Unauthorized();
    }

    [HttpPost(Name = "InsertProgressItem")]
    public async Task<ActionResult> InsertAsync(Models.Progress model)
    {
        if (model.StepOrder != null)
        {
            Models.Step step = await _stepRepo.GetStepByStepOrder((int)model.StepOrder);
            model.Step_id = (Guid)step.Id;
        }
        model.Id = Guid.NewGuid();
        await _progressRepo.InsertAsync(model);
        return Ok();
    }

    [HttpPost("Bulk",Name = "InsertProgressBulk")]
    public async Task<ActionResult> InsertBulkAsync(BulkUploadProgress progressBulk)
    {
        //We need to prevent redundant, duplicated save entries
        List<Progress> UserProgress = (List<Progress>)await _progressRepo.GetAsyncByUserDataId(progressBulk.UserData_id);
        
        foreach (var stepIndex in progressBulk.Steps)
        {
            Models.Progress prog = new Progress();
            prog.Id = Guid.NewGuid();
            prog.UserData_id = Guid.Parse(progressBulk.UserData_id);
            
            Step step = await _stepRepo.GetStepByStepOrder(stepIndex);
            prog.Step_id = step.Id;
            //Why do we have this...
            prog.Completed = true;
            //I can understand this one tho.
            prog.Completed_at = DateTime.Now;

            if (!UserProgress.Any(x => x.Step_id == prog.Step_id))
            {
                await _progressRepo.InsertAsync(prog);
            }
        }
        return Ok();
    }
}

public class BulkUploadProgress
{
    public string? UserData_id { get; set; }
    public List<int>? Steps { get; set; }
}