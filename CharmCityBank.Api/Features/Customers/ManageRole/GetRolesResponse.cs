namespace CharmCityBank.Api.Features.Customers.ManageRole;

public record GetRolesResponse(long CustomerNumber, IEnumerable<string> Roles);