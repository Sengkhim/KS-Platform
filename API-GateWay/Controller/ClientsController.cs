using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;

namespace API_GateWay.Controller;

public class ClientsController(IClientStore clientStore) : ControllerBase
{
    [HttpPost]
    public IActionResult CreateClient([FromBody] Client client)
    {
        // Add logic to save the client (e.g., to a database)
        return Ok(new { message = "Client created", client });
    }
}