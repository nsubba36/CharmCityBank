namespace CharmCityBank.Api.Features.Auth.Login;

public record LoginResponse(string Token, DateTime ExpiresAtUtc);