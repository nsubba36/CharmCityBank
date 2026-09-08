namespace CharmCityBank.Api.Domain.Audits;

public enum AuditActionType : byte
{
    CustomerRegistered = 1,
    CustomerDisabled = 2,
    CustomerEnabled = 3,
    AccountOpened = 4,
    AccountFrozen = 5,
    AccountUnfrozen = 6,
    AccountClosed = 7,
    DepositMade = 8,
    WithdrawalMade = 9,
    TransferCreated = 10,
    TransferCompleted = 11,
    TransferFailed = 12,
    TransferReversed = 13
}