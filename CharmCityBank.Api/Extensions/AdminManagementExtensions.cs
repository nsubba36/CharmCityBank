using CharmCityBank.Api.Features.Customers.ManageRole;

namespace CharmCityBank.Api.Extensions;

public static class AdminManagementExtensions
{
    public static IServiceCollection AddAdminManagement(this IServiceCollection services)
    {
        services.AddScoped<ManageRoleService>();
        return services;
    }
}