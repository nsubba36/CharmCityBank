using System.ComponentModel.DataAnnotations;

namespace CharmCityBank.Api.Features.Customers.ManageRole;

public record ManageRoleRequest([Required, MinLength(1)] string[] Roles);