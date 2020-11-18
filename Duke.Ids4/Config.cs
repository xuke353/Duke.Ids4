// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Duke.Ids4.Models;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Duke.Ids4
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                 new ApiScope("api1"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client {
                    ClientId = "js",
                    ClientName = "avaScript Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                   // AllowedGrantTypes = GrantTypes.Code,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris =           { "http://localhost:5004/callback.html" },
                    PostLogoutRedirectUris = { "http://localhost:5004/index.html" },
                    AllowedCorsOrigins =     { "http://localhost:5004" },

                    AccessTokenLifetime=3600,

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    }
                }
            };

        public static IEnumerable<ApiResource> ApiResources =>
           new ApiResource[]
           {
               new ApiResource("api1","#api1")
               {
                    Scopes = { "api1"},
                    ApiSecrets = new List<Secret>()
                    {
                        new Secret("api_secret".Sha256())
                    },
               }
           };

        public static IEnumerable<User> Users =>
           new User[]
           {
               new User
               {
                    Id = 1,
                    Name="TEST",
                    UserName = "test",
                    Email = "test@ids4.com",
                    CreateTime = DateTime.Now,
                    Sex = Sex.Female
               },
               new User
               {
                    Id = 2,
                    Name="DUKE",
                    UserName = "duke",
                    Email = "duke@ids4.com",
                    CreateTime = DateTime.Now,
                    Sex = Sex.Male
               }
           };

        public static IEnumerable<Role> Roles =>
           new Role[]
           {
               new Role
               {
                    Id = 1,
                    Name="admin",
                    NormalizedName = "NADMIN",
                    Description = "Super admin"
               }
           };

        public static IEnumerable<UserRole> UserRoles =>
           new UserRole[]
           {
               new UserRole
               {
                    UserId = 1,
                    RoleId = 1
               }
           };
    }
}