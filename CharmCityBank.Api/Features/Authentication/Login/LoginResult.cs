namespace CharmCityBank.Api.Features.Authentication.Login;

public record LoginResult(LoginStatus Status, LoginResponse? Response = null);