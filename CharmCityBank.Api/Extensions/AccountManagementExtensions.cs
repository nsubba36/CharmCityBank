using CharmCityBank.Api.Features.Accounts.OpenAccount;

namespace CharmCityBank.Api.Extensions;

public static class AccountManagementExtensions
{
    public static IServiceCollection AddAccountManagement(this IServiceCollection services)
    {
        services.AddScoped<OpenAccountService>();
        return services;
    }
}