using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace CABESO.Data
{
    public class ApplicationDbContext : IdentityDbContext, IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public ApplicationDbContext()
            : base(SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), Startup.DefaultConnection).Options)
        {
        }

        public ApplicationDbContext CreateDbContext(string[] args)
        {
            return this;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Form> Forms { get; set; }
        public DbSet<CurrentOrder> Orders { get; set; }
        public DbSet<HistoricOrder> HistoricOrders { get; set; }
        public DbSet<RegistrationCode> Codes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Form>().Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Entity<HistoricOrder>().Property(p => p.User).HasColumnName("UserId").HasConversion(v => v.Id, v => Users.Find(v));
            builder.Entity<HistoricOrder>().Property(p => p.Product).HasColumnName("ProductId").HasConversion(v => v.Id, v => Products.Find(v));
            builder.Entity<CurrentOrder>().Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Entity<CurrentOrder>().Property(p => p.User).HasColumnName("UserId").HasConversion(v => v.Id, v => Users.Find(v));
            builder.Entity<CurrentOrder>().Property(p => p.Product).HasColumnName("ProductId").HasConversion(v => v.Id, v => Products.Find(v));
            builder.Entity<Product>().Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Entity<Product>().Property(p => p.Allergens).HasConversion(v => string.Join('|', v), v => v.Split('|', StringSplitOptions.None));
            builder.Entity<RegistrationCode>().HasKey("Code");
            builder.Entity<RegistrationCode>().Property(p => p.Role).HasConversion(v => v.Name, v => Roles.FirstOrDefault(role => role.Name.Equals(v)));
            base.OnModelCreating(builder);
        }
    }
}