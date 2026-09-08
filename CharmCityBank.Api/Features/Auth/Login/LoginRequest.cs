using System.ComponentModel.DataAnnotations;

namespace CharmCityBank.Api.Features.Auth.Login;

public record LoginRequest(
    [Required] [EmailAddress] string Email,
    [Required] string Password
);