namespace CharmCityBank.Api.Domain.Transactions;

public enum TransactionType : byte
{
    Deposit = 1,
    Withdraw = 2,
    TransferIn = 3,
    TransferOut = 4,
    Reversal = 5
}