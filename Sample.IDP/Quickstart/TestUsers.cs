// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServerHost.Quickstart.UI
{
    public class TestUsers
    {
        public static List<TestUser> Users
        {
            get
            {
                return new List<TestUser>
                {
                    new TestUser
                    {
                        SubjectId = "12345",
                        Username = "admin",
                        Password = "@Admin123",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Administrator"),
                            new Claim(JwtClaimTypes.Role, "admin"),
                            new Claim(JwtClaimTypes.Email, "Administrator@test.com"),
                        }
                    },
                    new TestUser
                    {
                        SubjectId = "11111",
                        Username = "testuser",
                        Password = "@Test123",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Test User"),
                            new Claim(JwtClaimTypes.Role, "user"),
                            new Claim(JwtClaimTypes.Email, "testuser@test.com"),
                        }
                    }
                };
            }
        }
    }
}