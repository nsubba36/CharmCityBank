namespace CharmCityBank.Api.Features.Accounts.OpenAccount;

public record OpenAccountResult(OpenAccountStatus Status, OpenAccountResponse? Response = null);