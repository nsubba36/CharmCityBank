using System.Security.Claims;
using CharmCityBank.Api.Features.Accounts.OpenAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace CharmCityBank.Api.Features.Accounts;

[ApiController]
[Route("api/accounts")]
[Authorize]
public class AccountController(OpenAccountService openAccountService) : ControllerBase
{
    [HttpPost("open")]
    public async Task<IActionResult> OpenNewAccountAsync(OpenAccountRequest request)
    {
        var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        
        if (userId == null) 
            return Unauthorized();
        
        var result = await openAccountService.OpenAccountAsync(userId, request);

        return result.Status switch
        {
            OpenAccountStatus.Success =>
                Ok(result.Response),

            OpenAccountStatus.InvalidInitialDeposit =>
                BadRequest(new { message = "Minimum $25 initial deposit required for Checking Account." }),
            
            OpenAccountStatus.InvalidAccountType =>
                BadRequest(new { message = "Invalid account type." }),
            
            OpenAccountStatus.InvalidCustomer =>
                NotFound(new { message = "Customer not found." }),

            _ => StatusCode(500)
        };
    }
}