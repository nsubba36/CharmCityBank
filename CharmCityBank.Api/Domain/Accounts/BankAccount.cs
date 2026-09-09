using System.ComponentModel.DataAnnotations;
using CharmCityBank.Api.Domain.Customers;
using CharmCityBank.Api.Domain.Transactions;
using CharmCityBank.Api.Domain.Transfers;

namespace CharmCityBank.Api.Domain.Accounts;

public class BankAccount
{
    public long Id { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public AccountType AccountType { get; set; }
    public decimal Balance { get; set; }
    public BankAccountStatus Status { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ClosedAtUtc { get; set; }
    [Timestamp] public byte[] RowVersion { get; set; } = null!;
    public ICollection<BankTransaction> Transactions { get; set; } = [];
    public ICollection<Transfer> OutgoingTransfers { get; set; } = [];
    public ICollection<Transfer> IncomingTransfers { get; set; } = [];
    
}