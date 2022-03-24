// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Sample.IDP
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource("Roles",
                   new []{
                            JwtClaimTypes.Role
                        })
            };

        public static IEnumerable<ApiScope> ApiScopes =>
           new ApiScope[]
           {
                new ApiScope("Sample.App.FullAccess")
           };

        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            //Audience
            new ApiResource("Sample.App")
            {
                UserClaims =
                {
                    JwtClaimTypes.Name,
                    JwtClaimTypes.Profile,
                    JwtClaimTypes.Email,
                    JwtClaimTypes.Role
                },
                Scopes =
                {
                    "Sample.App.FullAccess"
                }
            }
        };


        public static IEnumerable<Client> Clients =>
            new Client[]
            {

                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "Sample.App",

                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris =
                    {
                        "http://localhost:3000/signin-oidc",
                        "https://localhost:5001/swagger/oauth2-redirect.html"
                    },

                    FrontChannelLogoutUri = "http://localhost:3000/signout-oidc",

                    PostLogoutRedirectUris = { "https://localhost:3000/signout-oidc" },

                    AllowOfflineAccess = true,

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "Roles",
                        "Sample.App.FullAccess"
                    },

                    AllowedCorsOrigins =
                    {
                        "http://localhost:3000",
                        "https://localhost:5001"
                    },

                    AllowAccessTokensViaBrowser = true
                },
            };
    }
}