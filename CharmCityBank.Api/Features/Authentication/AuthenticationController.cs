using CharmCityBank.Api.Features.Authentication.Login;
using CharmCityBank.Api.Features.Authentication.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CharmCityBank.Api.Features.Authentication;

[ApiController]
[Route("api/auth")]
public class AuthenticationController(AccountRegisterService registerService, LoginService loginService) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        var result = await loginService.LoginAsync(request);

        return result.Status switch
        {
            LoginStatus.Success =>
                Ok(result.Response),

            LoginStatus.LockedOut =>
                Unauthorized(new { message = "Account is temporarily locked." }),

            LoginStatus.Disabled =>
                Unauthorized(new { message = "Account is disabled" }),

            _ =>
                Unauthorized(new { message = "Invalid credentials." })
        };
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] AccountRegisterRequest request)
    {
        var result = await registerService.RegisterUserAsync(request);

        if (!result.Succeeded)
            return BadRequest(result);

        return Ok(result);
    }
}