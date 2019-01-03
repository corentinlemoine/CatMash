using System;
using System.Collections.Generic;
using System.Text;
using CatMash.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CatMash.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Cat> Cat { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
