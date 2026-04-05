using DrKnuffelBackEnd.Models;
using DrKnuffelBackEnd.Repositories.Progress;
using DrKnuffelBackEnd.Repositories.Step;
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

    public ProgressController(IProgressRepo progressRepo, IStepRepo stepRepo, IAuthenticationService _authenticationService)
    {
        _progressRepo = progressRepo;
        this._stepRepo = stepRepo;
        this._authenticationService = _authenticationService;
    }

    [HttpGet(Name = "GetProgressItems")]
    public async Task<ActionResult<List<Models.Progress>>> GetAsync()
    {
        string userId = _authenticationService.GetCurrentAuthenticatedUserId();
        if (!string.IsNullOrEmpty(userId))
        {
            var list = await _progressRepo.GetAsyncByUserId(userId);
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
            model.StepId = (Guid)step.Id;
        }
        model.Id = Guid.NewGuid();
        await _progressRepo.InsertAsync(model);
        return Ok();
    }

    [HttpPost("Bulk",Name = "InsertProgressBulk")]
    public async Task<ActionResult> InsertBulkAsync(BulkUploadProgress progressBulk)
    {
        foreach (var stepIndex in progressBulk.Steps)
        {
            Models.Progress prog = new Progress();
            prog.Id = Guid.NewGuid();
            prog.UserDataId = progressBulk.UserDataId;
            
            Step step = await _stepRepo.GetStepByStepOrder(stepIndex);
            prog.StepId = step.Id;
            //Why do we have this...
            prog.Completed = true;
            //I can understand this one tho.
            prog.Completed_at = DateTime.Now;

            await _progressRepo.InsertAsync(prog);
        }
        return Ok();
    }
}

public class BulkUploadProgress
{
    public string UserDataId { get; set; }
    public List<int> Steps { get; set; }
}