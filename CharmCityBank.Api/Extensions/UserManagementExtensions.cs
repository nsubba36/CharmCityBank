using CharmCityBank.Api.Features.Auth.Register;

namespace CharmCityBank.Api.Extensions;

public static class UserManagementExtensions
{
    public static IServiceCollection AddUserManagement (this IServiceCollection services)
    {
        services.AddScoped<AccountRegisterService>();
        
        return services;
    }
}