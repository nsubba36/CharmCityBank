namespace CharmCityBank.Api.Features.Authentication.Login;

public record LoginResponse(string Token, DateTime ExpiresAtUtc);