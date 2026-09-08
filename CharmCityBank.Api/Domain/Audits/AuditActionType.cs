namespace CharmCityBank.Api.Domain.Audits;

public enum AuditActionType : byte
{
    AccountCreated = 1,
    AccountFrozen = 2,
    AccountUnfrozen = 3,
    AccountClosed = 4,
    DepositMade = 5,
    WithdrawMade = 6,
    TransferCreated = 7,
    TransferCompleted = 8,
    TransferFailed = 9,
    TransferRevered = 10,
    UserDisabled = 11,
    UserEnabled = 12
}