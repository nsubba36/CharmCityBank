using System.ComponentModel.DataAnnotations;

namespace CharmCityBank.Api.Domain.Customers;

public class Address
{
    public long Id { get; set; }
    public string StreetAddress { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    [StringLength(2, MinimumLength = 2)]
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public Customer Customer { get; set; } = null!;
    public Guid CustomerId { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
}