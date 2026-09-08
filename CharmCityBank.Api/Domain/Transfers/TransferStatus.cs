namespace CharmCityBank.Api.Domain.Transfers;

public enum TransferStatus : byte
{
    Pending = 1,
    Completed = 2,
    Failed = 3,
    Reversed = 4
}