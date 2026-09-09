using System.ComponentModel.DataAnnotations;
using CharmCityBank.Api.Domain.Accounts;

namespace CharmCityBank.Api.Features.Accounts.OpenAccount;

public record OpenAccountRequest
{
    [Required]
    public AccountType AccountType { get; init; }
    [Required]
    public decimal Balance { get; set; }
};