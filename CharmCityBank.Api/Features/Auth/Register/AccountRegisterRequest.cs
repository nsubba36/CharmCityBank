using System.ComponentModel.DataAnnotations;

namespace CharmCityBank.Api.Features.Auth.Register;

public record AccountRegisterRequest
{
    [Required] [StringLength(100)] public required string FirstName { get; init; }
    [Required] [StringLength(100)]  public required string LastName { get; init; }
    [Required] [EmailAddress] public required string Email { get; init; }
    [Required] public required string Password { get; init; }
    [Required] [Compare(nameof(Password))] public required string ConfirmPassword { get; init; }
    [Required] [Phone] public required string PhoneNumber { get; init; }
    [Required] [StringLength(500)]  public required string StreetAddress { get; init; }
    [Required] [StringLength(100)]  public required string City { get; init; }
    [Required] [StringLength(2, MinimumLength = 2)] public required string State { get; init; }
    [Required] [StringLength(15)]  public required string ZipCode { get; init; }
};