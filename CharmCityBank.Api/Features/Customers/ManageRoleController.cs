using CharmCityBank.Api.Features.Customers.ManageRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CharmCityBank.Api.Features.Customers;

[ApiController]
[Route("api/admin/customers/{customerNumber}/roles")]
[Authorize(Roles = "Admin")]
public class ManageRoleController(ManageRoleService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetRolesAsync([FromRoute] long customerNumber)
    {
        var result = await service.GetRolesAsync(customerNumber);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> ChangeRolesAsync([FromRoute] long customerNumber,
        [FromBody] ManageRoleRequest request)
    {
        var result = await service.ChangeRoleAsync(customerNumber, request);

        if (!result.Succeeded)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPatch]
    public async Task<IActionResult> AddRolesAsync([FromRoute] long customerNumber,
        [FromBody] ManageRoleRequest request)
    {
        var result = await service.AddRoleAsync(customerNumber, request);

        if (!result.Succeeded)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveRolesAsync([FromRoute] long customerNumber,
        [FromBody] ManageRoleRequest request)
    {
        var result = await service.RemoveRoleAsync(customerNumber, request);

        if (!result.Succeeded)
            return BadRequest(result);

        return Ok(result);
    }
}