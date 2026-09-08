using CharmCityBank.Api.Domain.Accounts;
using CharmCityBank.Api.Domain.Transfers;

namespace CharmCityBank.Api.Domain.Transactions;

public class BankTransaction
{
    public long Id { get; set; }
    public long AccountId { get; set; }
    public BankAccount Account { get; set; } = null!;
    public long? TransferId { get; set; }
    public Transfer? Transfer { get; set; }
    public TransactionType TransactionType { get; set; }
    public decimal Amount { get; set; }
    public decimal BalanceBefore { get; set; }
    public decimal BalanceAfter { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; } =  DateTime.UtcNow;
}