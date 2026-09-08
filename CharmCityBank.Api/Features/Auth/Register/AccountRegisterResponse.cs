namespace CharmCityBank.Api.Features.Auth.Register;

public record AccountRegisterResponse(bool Succeeded, string? Token, DateTime? ExpiresAt, IEnumerable<string>? Errors);