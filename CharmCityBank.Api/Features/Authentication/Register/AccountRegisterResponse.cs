namespace CharmCityBank.Api.Features.Authentication.Register;

public record AccountRegisterResponse(bool Succeeded, string? Token, DateTime? ExpiresAt, IEnumerable<string>? Errors);