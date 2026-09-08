namespace CharmCityBank.Api.Domain.Accounts;

public enum BankAccountStatus : byte
{
    Active = 1,
    Frozen = 2,
    Closed = 3
}