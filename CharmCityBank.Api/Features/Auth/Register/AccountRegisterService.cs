using CharmCityBank.Api.Domain.Audits;
using CharmCityBank.Api.Domain.Customers;
using CharmCityBank.Api.Infrastructure.Authentication;
using CharmCityBank.Api.Infrastructure.Identity;
using CharmCityBank.Api.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace CharmCityBank.Api.Features.Auth.Register;

public class AccountRegisterService(
    UserManager<ApplicationUser> userManager,
    TokenService tokenService,
    AppDbContext dbContext)
{
    public async Task<AccountRegisterResponse> RegisterUserAsync(AccountRegisterRequest request)
    {
        if (request.Password != request.ConfirmPassword)
        {
            return new AccountRegisterResponse(
                Succeeded: false,
                Token: null,
                ExpiresAt: null,
                Errors: ["Passwords don't match."]
            );
        }

        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var identityUser = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber,
            };

            var userResult = await userManager.CreateAsync(identityUser, request.Password);

            if (!userResult.Succeeded)
            {
                await transaction.RollbackAsync();
                return new AccountRegisterResponse(
                    Succeeded: false,
                    Token: null,
                    ExpiresAt: null,
                    Errors: userResult.Errors.Select(x => x.Description)
                );
            }

            var roleResult = await userManager.AddToRoleAsync(identityUser, Roles.User);

            if (!roleResult.Succeeded)
            {
                await transaction.RollbackAsync();
                return new AccountRegisterResponse(
                    Succeeded: false,
                    Token: null,
                    ExpiresAt: null,
                    Errors: roleResult.Errors.Select(x => x.Description)
                );
            }

            var customer = new Customer
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                IdentityUserId = identityUser.Id,
            };

            var address = new Address
            {
                StreetAddress = request.StreetAddress,
                City = request.City,
                State = request.State,
                ZipCode = request.ZipCode,
                CustomerId = customer.Id,
            };
            
            var auditLog = new AuditLog
            {
                IdentityUserId = identityUser.Id,
                ActionType = AuditActionType.CustomerRegistered,
                EntityType = AuditEntityType.Customer,
                EntityId = customer.Id.ToString(),
                Details = "Customer Registration successful",
            };
            
            dbContext.Add(customer);
            dbContext.Add(address);
            dbContext.Add(auditLog);
            await dbContext.SaveChangesAsync();

            await transaction.CommitAsync();

            var roles = new[] { Roles.User };

            var (token, expiresAtUtc) = tokenService.GenerateToken(identityUser, roles);
            
            return new AccountRegisterResponse(
                Succeeded: true,
                Token: token,
                ExpiresAt: expiresAtUtc,
                Errors: []
            );
        }
        catch
        {
            await transaction.RollbackAsync();
            return new AccountRegisterResponse(
                Succeeded: false,
                Token: null,
                ExpiresAt: null,
                Errors: ["Registration failed"]
            );
        }
    }
}