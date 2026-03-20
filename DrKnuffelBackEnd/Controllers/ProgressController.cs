using Microsoft.AspNetCore.Mvc;
using DrKnuffelBackEnd.Models;
using DrKnuffelBackEnd.Repositories;
using DrKnuffelBackEnd.Services;
using DrKnuffelBackEnd.Repositories.Progress;

namespace DrKnuffelBackEnd.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class ProgressController : ControllerBase
{
    private readonly IProgressRepo _progressRepo;
    private readonly IAuthenticationService _authenticationService;

    public ProgressController(IAuthenticationService authenticationService, IProgressRepo progressRepo)
    {
        _progressRepo = progressRepo;
        _authenticationService = authenticationService;
    }

    [HttpGet(Name = "GetProgress")]
    public async Task<ActionResult<List<Progress>>> GetAsync()
    {
        var progress = await _progressRepo.SelectAsync();
        return Ok(progress);
    }

    [HttpGet("{progressById}", Name = "GetProgressById")]
    public async Task<ActionResult<Progress>> GetByIdAsync(Guid progressById)
    {
        var progress = await _progressRepo.GetAsyncById(progressById);

        if (progress == null)
            return NotFound(new ProblemDetails { Detail = $"Progress {progressById} not found" });

        return Ok(progress);
    }

    [HttpGet("userData/{progressByUserId}", Name = "GetProgressByUserId")]
    public async Task<ActionResult<Progress>> GetAsyncByUserId(Guid progressByUserId)
    {
        var progress = await _progressRepo.GetAsyncByUserId(progressByUserId);

        if (progress == null)
            return NotFound(new ProblemDetails { Detail = $"Progress user {progressByUserId} not found" });

        return Ok(progress);
    }


    [HttpPost(Name = "AddProgress")]
    public async Task<ActionResult<Progress>> AddAsync(Progress progress)
    {
        progress.Id = Guid.NewGuid();

        await _progressRepo.InsertAsync(progress);

        return CreatedAtRoute("GetProgressById", new { progressById = progress.Id }, progress);
    }

    /*[HttpPost(Name = "AddProgressByUserId")]
    public async Task<ActionResult<Progress>> AddAsyncByUserId(Progress progress)
    {
        progress.UserDataId = Guid.NewGuid();

        await _progressRepo.InsertAsync(progress);

        return CreatedAtRoute("GetProgressByUserId", new { progressByUserId = progress.UserDataId }, progress);
    }*/
}
