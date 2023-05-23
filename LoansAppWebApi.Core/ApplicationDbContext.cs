using LoansAppWebApi.Models.DbModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LoansAppWebApi.Core
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts) : base(opts)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {


            // Fluent api

            // Loans configuration
            builder.Entity<Loans>()
                .ToTable(b => b.HasCheckConstraint("CK_Loans_EndDate", "[EndDate] >= [StartDate]"));

            builder.Entity<Loans>()
                .Property(x => x.SumOfLoan)
                .HasColumnType("DECIMAL(18, 4)");

            builder.Entity<Loans>()
                .Property(x => x.PercentsInYear)
                .HasColumnType("DECIMAL(18, 4)");

            builder.Entity<Loans>()
                .Property(x => x.StartDate)
                .HasColumnType("datetime");
            builder
                .Entity<Loans>()
                .Property(x => x.EndDate).HasColumnType("datetime");
                
            base.OnModelCreating(builder);
        }

        public DbSet<Loans> Loans { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
