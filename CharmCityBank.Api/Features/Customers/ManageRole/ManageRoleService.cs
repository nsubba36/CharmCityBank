using CharmCityBank.Api.Infrastructure.Identity;
using CharmCityBank.Api.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CharmCityBank.Api.Features.Customers.ManageRole;
// TODO: Add to AuditLog table
public class ManageRoleService(
    AppDbContext dbContext,
    RoleManager<IdentityRole> roleManager,
    UserManager<ApplicationUser> userManager)
{
    public async Task<GetRolesResponse?> GetRolesAsync(long customerNumber)
    {
        var user = await GetUserByCustomerNumberAsync(customerNumber);
        if (user is null)
            return null;
        var roles = await userManager.GetRolesAsync(user);
        return new GetRolesResponse(customerNumber, roles);
    }

    public async Task<ManageRoleResponse> AddRoleAsync(long customerNumber, ManageRoleRequest request)
    {
        var user = await GetUserByCustomerNumberAsync(customerNumber);
        if (user is null)
            return new ManageRoleResponse(false, "Invalid customer number", null, []);

        var currRoles = await userManager.GetRolesAsync(user);

        if (!await RolesExistAsync(request.Roles))
            return new ManageRoleResponse(false, "One or more requested roles do not exist.", customerNumber,
                currRoles);

        var roleToAdd = new List<string>();

        foreach (var role in request.Roles)
        {
            if (!await userManager.IsInRoleAsync(user, role))
                roleToAdd.Add(role);
        }

        if (roleToAdd.Count == 0)
            return new ManageRoleResponse(true, "No new roles added.", customerNumber, currRoles);

        var roleResult = await userManager.AddToRolesAsync(user, roleToAdd);

        if (!roleResult.Succeeded)
            return new ManageRoleResponse(false, "Failed to add requested roles.", customerNumber, currRoles);

        var updatedRoles = await userManager.GetRolesAsync(user);

        return new ManageRoleResponse(true, "Roles added successfully.", customerNumber, updatedRoles);
    }

    public async Task<ManageRoleResponse> RemoveRoleAsync(long customerNumber, ManageRoleRequest request)
    {
        var user = await GetUserByCustomerNumberAsync(customerNumber);
        if (user is null)
            return new ManageRoleResponse(false, "Invalid customer number.", null, []);

        var currRoles = await userManager.GetRolesAsync(user);

        if (!await RolesExistAsync(request.Roles))
            return new ManageRoleResponse(false, "One or more requested roles do not exist.", customerNumber,
                currRoles);

        foreach (var role in request.Roles)
        {
            if (!await userManager.IsInRoleAsync(user, role))
                return new ManageRoleResponse(false, "Customer does not currently have one or more requested roles.",
                    customerNumber, currRoles);
        }

        var roleResult = await userManager.RemoveFromRolesAsync(user, request.Roles);

        if (!roleResult.Succeeded)
            return new ManageRoleResponse(false, "Failed to remove roles.", customerNumber, currRoles);

        var updatedRoles = await userManager.GetRolesAsync(user);

        return new ManageRoleResponse(true, "Roles removed successfully.", customerNumber, updatedRoles);
    }

    public async Task<ManageRoleResponse> ChangeRoleAsync(long customerNumber, ManageRoleRequest request)
    {
        var user = await GetUserByCustomerNumberAsync(customerNumber);

        if (user is null)
            return new ManageRoleResponse(false, "Invalid customer number.", null, []);

        var currRoles = await userManager.GetRolesAsync(user);

        if (!await RolesExistAsync(request.Roles))
            return new ManageRoleResponse(false, "One or more requested roles do not exist.", customerNumber,
                currRoles);

        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            if (currRoles.Count > 0)
            {
                var removeRoles = await userManager.RemoveFromRolesAsync(user, currRoles);

                if (!removeRoles.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return new ManageRoleResponse(false, "Failed to remove roles.", customerNumber, currRoles);
                }
            }

            var addResult = await userManager.AddToRolesAsync(user, request.Roles);

            if (!addResult.Succeeded)
            {
                await transaction.RollbackAsync();
                return new ManageRoleResponse(false, "Failed to add new requested roles.", customerNumber, currRoles);
            }

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            return new ManageRoleResponse(false, "Failed to update roles due to a transaction error.", customerNumber,
                currRoles);
        }

        var updatedRoles = await userManager.GetRolesAsync(user);

        return new ManageRoleResponse(true, "Roles updated successfully.", customerNumber, updatedRoles);
    }

    private async Task<ApplicationUser?> GetUserByCustomerNumberAsync(long customerNumber)
    {
        var customer = await dbContext.Customers.AsNoTracking()
            .FirstOrDefaultAsync(c => c.CustomerNumber == customerNumber);

        if (customer is null)
            return null;

        return await userManager.FindByIdAsync(customer.IdentityUserId);
    }

    private async Task<bool> RolesExistAsync(IEnumerable<string> roles)
    {
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                return false;
        }

        return true;
    }
}