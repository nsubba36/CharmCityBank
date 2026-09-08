using CharmCityBank.Api.Domain.Accounts;
using CharmCityBank.Api.Domain.Transactions;

namespace CharmCityBank.Api.Domain.Transfers;

public class Transfer
{
    public long Id { get; set; }
    public long FromAccountId { get; set; }
    public BankAccount FromAccount { get; set; } = null!;
    public long ToAccountId { get; set; }
    public BankAccount ToAccount { get; set; } = null!;
    public decimal Amount { get; set; }
    public string? Memo { get; set; }
    public TransferStatus Status { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAtUtc { get; set; }
    public ICollection<BankTransaction> Transactions { get; set; } = [];
}