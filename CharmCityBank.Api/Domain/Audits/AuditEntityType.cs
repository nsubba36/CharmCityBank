namespace CharmCityBank.Api.Domain.Audits;

public enum AuditEntityType : byte
{
    Customer = 1,
    BankAccount = 2,
    BankTransaction = 3,
    Transfer = 4
}