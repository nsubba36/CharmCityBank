using System.ComponentModel.DataAnnotations;

namespace CharmCityBank.Api.Features.Authentication.Login;

public record LoginRequest(
    [Required] [EmailAddress] string Email,
    [Required] string Password
);