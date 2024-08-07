using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace API_GateWay.Extension;

public static class OAuth2ServiceExtension
{
    public static void AddOAuth2Configuration(this IServiceCollection service)
    {
        service.AddIdentityServer()
            .AddInMemoryClients(new List<Client>
            {
                new () 
                {
                    ClientId = "client_id",
                    ClientSecrets = { new Secret("client_secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "api1" }
                }
            })
            .AddInMemoryApiResources([
                new ApiResource("api1", "My API")
            ])
            .AddInMemoryIdentityResources(new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            })
            .AddTestUsers([
                new TestUser
                {
                    SubjectId = "1",
                    Username = "tester",
                    Password = "123"
                }
            ]);
        
        service.AddAuthentication()
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "http://localhost:5000";
                options.Audience = "api1";
                options.RequireHttpsMetadata = false;
            });
    }

    public static void AddOAuth2Configuration(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddAuthentication("cookie")
            .AddCookie("cookie")
            .AddOAuth("github",
                options => OAuthConfigureOption(options, configuration));
    }

    private static void OAuthConfigureOption(OAuthOptions options, IConfiguration configuration)
    {
        options.SignInScheme = "cookie";
        options.ClientId = configuration.GetValue<string>("Github:ClientId")!;
        options.ClientSecret = configuration.GetValue<string>("Github:ClientSecret")!;

        options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
        options.TokenEndpoint = "https://github.com/login/oauth/access_token";
        options.CallbackPath= "/oauth/github-cb";
        options.UserInformationEndpoint = "https://github.com/user";
        options.SaveTokens = true;
        
        options.ClaimActions.MapJsonKey(ClaimTypes.Name, "login");
        options.ClaimActions.MapJsonKey("sub", "id");
        
        options.Events = new OAuthEvents
        {
            OnCreatingTicket = async context =>
            {
                var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                var response = await context.Backchannel.SendAsync(request);
                var user = await response.Content.ReadFromJsonAsync<JsonElement>();
                context.RunClaimActions(user);
            }
        };

    }
}