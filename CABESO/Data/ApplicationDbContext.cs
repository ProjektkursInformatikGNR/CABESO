using Microsoft.AspNetCore.Identity;
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
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Form> Forms { get; set; }
        public DbSet<CurrentOrder> Orders { get; set; }
        public DbSet<HistoricOrder> HistoricOrders { get; set; }
        public DbSet<RegistrationCode> Codes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Form>().Property(p => p.Id).ValueGeneratedOnAdd().UseSqlServerIdentityColumn();
            builder.Entity<HistoricOrder>().Property(p => p.User).HasColumnName("UserId").HasConversion(v => v.Id, v => CABESO.Database.GetUserById(v));
            builder.Entity<HistoricOrder>().Property(p => p.Product).HasColumnName("ProductId").HasConversion(v => v.Id, v => Product.GetProductById(v));
            builder.Entity<CurrentOrder>().Property(p => p.Id).ValueGeneratedOnAdd().UseSqlServerIdentityColumn();
            builder.Entity<CurrentOrder>().Property(p => p.User).HasColumnName("UserId").HasConversion(v => v.Id, v => CABESO.Database.GetUserById(v));
            builder.Entity<CurrentOrder>().Property(p => p.Product).HasColumnName("ProductId").HasConversion(v => v.Id, v => Product.GetProductById(v));
            builder.Entity<Product>().Property(p => p.Id).ValueGeneratedOnAdd().UseSqlServerIdentityColumn();
            builder.Entity<Product>().Property(p => p.Allergens).HasConversion(v => string.Join('|', v), v => v.Split('|', StringSplitOptions.None));
            builder.Entity<RegistrationCode>().HasKey("Code");
            base.OnModelCreating(builder);
        }
    }
}