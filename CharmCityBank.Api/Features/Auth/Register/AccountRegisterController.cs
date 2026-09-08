using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CharmCityBank.Api.Features.Auth.Register;

[ApiController]
[Route("api/users")]
public class AccountRegisterController(AccountRegisterService service) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] AccountRegisterRequest request)
    {
        var result = await service.RegisterUserAsync(request);

        if (!result.Succeeded)
            return BadRequest(result);

        return Ok(result);
    }
}