using CharmCityBank.Api.Domain.Accounts;
using CharmCityBank.Api.Infrastructure.Persistence;
using CharmCityBank.Api.Utility;
using Microsoft.EntityFrameworkCore;

namespace CharmCityBank.Api.Features.Accounts.OpenAccount;

public class OpenAccountService(AppDbContext dbContext)
{
    public async Task<OpenAccountResult> OpenAccountAsync(string userId, OpenAccountRequest request)
    {
        var customer = await dbContext.Customers.FirstOrDefaultAsync(c => c.IdentityUserId == userId);

        if (customer == null)
            return new OpenAccountResult(OpenAccountStatus.InvalidCustomer);

        if (!Enum.IsDefined(request.AccountType))
        {
            return new OpenAccountResult(OpenAccountStatus.InvalidAccountType);
        }
        
        if (request.Balance < 0)
        {
            return new OpenAccountResult(OpenAccountStatus.InvalidInitialDeposit);
        }

        var invalidInitialDeposit = request.AccountType == AccountType.Checking && request.Balance < 25m;

        if (invalidInitialDeposit)
        {
            return new OpenAccountResult(OpenAccountStatus.InvalidInitialDeposit);
        }

        string accountNumber;

        do
        {
            accountNumber = Utilities.GenRandomAlphanumeric(10);
        } while (await dbContext.BankAccounts.AnyAsync(a => a.AccountNumber == accountNumber));

        var account = new BankAccount
        {
            AccountNumber = accountNumber,
            CustomerId = customer.Id,
            Balance = request.Balance,
            AccountType = request.AccountType,
            Status = BankAccountStatus.Active
        };

        dbContext.BankAccounts.Add(account);

        await dbContext.SaveChangesAsync();
        return new OpenAccountResult(OpenAccountStatus.Success, new OpenAccountResponse
        {
            AccountNumber = account.AccountNumber,
            Id = account.Id,
            Balance = account.Balance,
            AccountType = account.AccountType,
            CreatedAtUtc = account.CreatedAtUtc,
        });
    }
}