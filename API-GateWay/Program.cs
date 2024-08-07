using API_GateWay.Extension;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOAuth2Configuration(builder.Configuration);

var app = builder.Build();

app.UseAuthentication();

app.MapGet("/", (HttpContext ctx) =>
{
    var claims = ctx.User.Claims.Select(e => new { e.Type, e.Value }).ToList();
    return claims;
});

app.MapGet("/login", () => Results.Challenge(
    properties: new AuthenticationProperties { RedirectUri = "http://localhost:5218" },
    authenticationSchemes: new List<string> { "github" }
));

app.Run();