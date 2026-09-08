using CharmCityBank.Api.Infrastructure.Authentication;
using CharmCityBank.Api.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace CharmCityBank.Api.Features.Authentication.Login;

public class LoginService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    TokenService tokenService)
{
    public async Task<LoginResult> LoginAsync(LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user == null)
            return new LoginResult(LoginStatus.InvalidCredentials);

        if (user.IsDisabled)
            return new LoginResult(LoginStatus.Disabled);

        var signInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, true);

        if (signInResult.IsLockedOut)
            return new LoginResult(LoginStatus.LockedOut);

        if (!signInResult.Succeeded)
            return new LoginResult(LoginStatus.InvalidCredentials);

        var roles = await userManager.GetRolesAsync(user);

        var (token, expiresAtUtc) = tokenService.GenerateToken(user, roles);
        return new LoginResult(
            LoginStatus.Success,
            new LoginResponse(token, expiresAtUtc)
        );
    }
}