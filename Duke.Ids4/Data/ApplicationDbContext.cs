using Duke.Ids4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duke.Ids4.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,
                                                            ApplicationRole,
                                                            int,
                                                            IdentityUserClaim<int>,
                                                            ApplicationUserRole,
                                                            IdentityUserLogin<int>,
                                                            IdentityRoleClaim<int>,
                                                            IdentityUserToken<int>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}