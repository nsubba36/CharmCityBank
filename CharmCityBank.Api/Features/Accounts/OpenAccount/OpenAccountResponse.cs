using CharmCityBank.Api.Domain.Accounts;

namespace CharmCityBank.Api.Features.Accounts.OpenAccount;

public record OpenAccountResponse
{
    public long Id { get; init;}
    public string AccountNumber { get; init;} = string.Empty;
    public AccountType AccountType { get; init;}
    public decimal Balance { get; init;}
    public DateTime CreatedAtUtc { get; init;}
};