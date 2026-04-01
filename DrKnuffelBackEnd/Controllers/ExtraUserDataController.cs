using DrKnuffelBackEnd.Repositories.UserData;
using DrKnuffelBackEnd.Repositories.Progress;
using DrKnuffelBackEnd.Repositories.Step;
using DrKnuffelBackEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace DrKnuffelBackEnd.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class ExtraUserDataController : ControllerBase
{
    private readonly IExtraUserData _userData;
    private readonly IAuthenticationService _authenticationService;

    public ExtraUserDataController(IExtraUserData _userData, IAuthenticationService _authenticationService)
    {
        this._userData = _userData;
        this._authenticationService = _authenticationService;
    }

    [HttpGet(Name = "GetUserData")]
    public async Task<ActionResult<List<Models.UserData>>> GetAsync()
    {
        string userId = _authenticationService.GetCurrentAuthenticatedUserId();
        if (!string.IsNullOrEmpty(userId))
        {
            var list = await _userData.GetAsyncByUserId(userId);
            return Ok(list);
        }
        return Unauthorized();
    }

    [HttpPost(Name = "InsertUserData")]
    public async Task<ActionResult> InsertAsync(Models.UserData userData)
    {
        var userId = _authenticationService.GetCurrentAuthenticatedUserId();
        //await _userData.AddAsync(userData);
        //return Ok();

        userData.Id = Guid.NewGuid();
        userData.UserId = userId;
        await _userData.AddAsync(userData);
        //return CreatedAtRoute("GetuserDataId", new { objectId = userData.Id }, userData);
        return Ok(userData);
    }
}