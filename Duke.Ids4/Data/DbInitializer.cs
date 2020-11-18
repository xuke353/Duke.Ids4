using Duke.Ids4.Models;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Duke.Ids4.Data
{
    public static class DbInitializer
    {
        public static void InitializeDatabase(this IApplicationBuilder app, ILogger logger)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                logger.LogInformation("Seeding database...");
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in Config.Clients)
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.IdentityResources)
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiScopes.Any())
                {
                    foreach (var resource in Config.ApiScopes)
                    {
                        context.ApiScopes.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.ApiResources)
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                var appContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                appContext.Database.Migrate();
                if (!appContext.Users.Any())
                {
                    var userMgr = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                    var roleMgr = serviceScope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

                    foreach (var user in Config.Users)
                    {
                        var identityResult = userMgr.CreateAsync(user, "1qazZAQ!").Result;
                        if (!identityResult.Succeeded)
                        {
                            throw new Exception(identityResult.Errors.First().Description);
                        }

                        var roles = from r in Config.Roles
                                    join ur in Config.UserRoles
                                    on r.Id equals ur.RoleId
                                    where ur.UserId == user.Id
                                    select r;

                        var claims = new List<Claim>{
                                    new Claim(JwtClaimTypes.Name, user.Name),
                                    new Claim(JwtClaimTypes.Email, user.Email),
                                    new Claim(JwtClaimTypes.Subject, user.Id.ToString()),
                                };
                        claims.AddRange(roles.Select(s => new Claim(JwtClaimTypes.Role, s.Id.ToString())));
                        identityResult = userMgr.AddClaimsAsync(user, claims).Result;
                        if (!identityResult.Succeeded)
                        {
                            throw new Exception(identityResult.Errors.First().Description);
                        }
                    }

                    foreach (var role in Config.Roles)
                    {
                        var identityResult1 = roleMgr.CreateAsync(role).Result;
                        if (!identityResult1.Succeeded)
                        {
                            throw new Exception(identityResult1.Errors.First().Description);
                        }
                    }

                    foreach (var ur in Config.UserRoles)
                    {
                        appContext.UserRoles.Add(ur);
                    }

                    appContext.SaveChanges();
                }
                logger.LogInformation("Done seeding database");
            }
        }
    }
}