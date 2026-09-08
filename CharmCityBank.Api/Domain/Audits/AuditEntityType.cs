namespace CharmCityBank.Api.Domain.Audits;

public enum AuditEntityType : byte
{
    User = 1,
    Account = 2,
    Transaction = 3,
    Transfer = 4
}