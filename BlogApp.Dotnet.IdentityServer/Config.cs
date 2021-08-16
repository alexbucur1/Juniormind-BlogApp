using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;

namespace BlogApp.Dotnet.IdentityServer
{
    public static class Config
    {
                public static IEnumerable<ApiResource> ApiResources => new[]
                {
      new ApiResource("BlogApp.Dotnet.API")
      {
        ApiSecrets = new List<Secret> {new Secret("ScopeSecret".Sha256())},
        UserClaims = new List<string> { JwtClaimTypes.Id, JwtClaimTypes.Role },
        Scopes = new List<string> { "Api.Scope" }
      },
      new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<ApiScope> ApiScopes => new[]
        {
            new ApiScope("Api.Scope", "BlogApp API Project Scope", new[] { JwtClaimTypes.Id, JwtClaimTypes.Role}),
            new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
        };

                public static IEnumerable<Client> Clients =>
  new[]
  {

        new Client
        {
          ClientId = "Angular.client",
          ClientName = "Angular Client",

          AllowedGrantTypes = GrantTypes.Implicit,
          ClientSecrets = {new Secret("Secret".Sha256())},

          AllowAccessTokensViaBrowser = true,
          AccessTokenLifetime = 86400,
          RequireConsent = false,

          RedirectUris = new List<string>
                    {
                        "https://localhost:5002/api/Posts",
                    },

          AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "Api.Scope",
                "user_profile",
                IdentityServerConstants.LocalApi.ScopeName
            }
        },
        new Client()
        {
          ClientId = "Nest.client",
          ClientName = "Nest Client",

          AllowedGrantTypes = GrantTypes.Implicit,
          ClientSecrets = {new Secret("Secret".Sha256())},

          AllowAccessTokensViaBrowser = true,
          AccessTokenLifetime = 86400,
          RequireConsent = false,

          RedirectUris = new List<string>
                    {
                        "https://localhost:3000/api/Posts",
                    },

          AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "Api.Scope",
                "user_profile",
                IdentityServerConstants.LocalApi.ScopeName
            }
        }
  };

                public static IEnumerable<IdentityResource> IdentityResources =>
                          new[]
          {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResource(
            name: "user_profile",
            displayName: "User Profile",
            userClaims: new[] { JwtClaimTypes.Id, JwtClaimTypes.Role })
          };
    }
}
