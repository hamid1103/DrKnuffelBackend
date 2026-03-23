using DrKnuffelBackEnd.Repositories.Step;
using DrKnuffelBackEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace DrKnuffelBackEnd.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class StepController : ControllerBase
{
    private readonly IStepRepo _stepRepo;
    private readonly IAuthenticationService _authenticationService;

    public StepController(IStepRepo _stepRepo, IAuthenticationService _authenticationService)
    {
        this._stepRepo = _stepRepo;
        this._authenticationService = _authenticationService;
    }

    [HttpGet(Name="GetSteps")]
    public async Task<ActionResult<List<Models.Step>>> GetAsync()
    {
        //To be secured with role checks later
        var list = await _stepRepo.GetSteps();
        return Ok(list);
    }

    [HttpPost(Name = "InsertStep")]
    public async Task<ActionResult> InsertAsync(Models.Step stepData)
    {
        //To be secured with role checks later
        await _stepRepo.InsertAsync(stepData);
        return Ok();
    }
}