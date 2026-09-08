using CharmCityBank.Api.Domain.Accounts;

namespace CharmCityBank.Api.Domain.Customers;

public class Customer
{
    public Guid Id { get; set; } =  Guid.NewGuid();
    public long CustomerNumber { get; set; }
    public string IdentityUserId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set;} = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get;set; } = DateTime.UtcNow;
    public bool IsDisabled { get; set; }
    public ICollection<Address> Addresses { get; set; } = [];
    public ICollection<BankAccount> BankAccounts { get; set; } = [];
}