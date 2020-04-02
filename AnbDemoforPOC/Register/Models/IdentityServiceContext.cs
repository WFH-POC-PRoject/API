using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Register.Models
{
    public class IdentityServiceContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public IdentityServiceContext(DbContextOptions<IdentityServiceContext> options) : base(options)
        {
        }
    }
}
