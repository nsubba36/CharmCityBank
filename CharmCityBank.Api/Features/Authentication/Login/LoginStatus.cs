namespace CharmCityBank.Api.Features.Authentication.Login;

public enum LoginStatus
{
    Success,
    InvalidCredentials,
    Disabled,
    LockedOut,
}