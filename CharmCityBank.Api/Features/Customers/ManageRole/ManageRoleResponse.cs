namespace CharmCityBank.Api.Features.Customers.ManageRole;

public record ManageRoleResponse(bool Succeeded, string? Message, long? CustomerNumber, IEnumerable<string> Roles);