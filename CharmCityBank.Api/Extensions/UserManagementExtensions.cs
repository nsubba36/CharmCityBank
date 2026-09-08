using CharmCityBank.Api.Features.Authentication.Login;
using CharmCityBank.Api.Features.Authentication.Register;

namespace CharmCityBank.Api.Extensions;

public static class UserManagementExtensions
{
    public static IServiceCollection AddUserManagement (this IServiceCollection services)
    {
        services.AddScoped<AccountRegisterService>();
        services.AddScoped<LoginService>();
        return services;
    }
}