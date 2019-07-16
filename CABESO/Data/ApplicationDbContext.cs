using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace CABESO.Data
{
    /// <summary>
    /// Der Datenbankkontext des Projekts unter Anwendung des Entity Framework Core
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext, IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        /// <summary>
        /// Erzeugt einen neuen <see cref="ApplicationDbContext"/> auf Grundlage gegebener Einstellungen.
        /// </summary>
        /// <param name="options">
        /// Die Kontexteinstellungen
        /// </param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Erzeugt einen neuen <see cref="ApplicationDbContext"/> mit Standardeinstellungen.
        /// </summary>
        public ApplicationDbContext()
            : base(SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), Startup.DefaultConnection).Options)
        {
        }

        /// <summary>
        /// Erzeugt einen neuen <see cref="ApplicationDbContext"/> auf Grundlage gegebener Einstellungen.
        /// </summary>
        /// <param name="args">
        /// Die Kontexteinstellungen
        /// </param>
        /// <returns>
        /// Der erzeugte <see cref="ApplicationDbContext"/>.
        /// </returns>
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            return this;
        }

        /// <summary>
        /// Beschreibt den Konfigurationsvorgang des <see cref="ApplicationDbContext"/>.<para>
        /// Der Datenbankkontext wird mit den Projekteinstellungen und der SQL-Server-Verbindung verknüpft.</para>
        /// </summary>
        /// <param name="optionsBuilder">
        /// Die Erzeugerinstanz der Datenbankkontexteinstellungen
        /// </param>
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

        /// <summary>
        /// Die Abbildung der Datentabelle "Products"
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Die Abbildung der Datentabelle "Forms"
        /// </summary>
        public DbSet<Form> Forms { get; set; }

        /// <summary>
        /// Die Abbildung der Datentabelle "Orders"
        /// </summary>
        public DbSet<CurrentOrder> Orders { get; set; }

        /// <summary>
        /// Die Abbildung der Datentabelle "OrderHistory"
        /// </summary>
        public DbSet<HistoricOrder> HistoricOrders { get; set; }

        /// <summary>
        /// Die Abbildung der Datentabelle "Codes"
        /// </summary>
        public DbSet<RegistrationCode> Codes { get; set; }

        /// <summary>
        /// Die Abbildung der Datentabelle "Allergens"
        /// </summary>
        public DbSet<Allergen> Allergens { get; set; }

        /// <summary>
        /// Beschreibt besondere Vorgänge bei der Kommunikation zwischen Datenbank und Datenbankkontext (z. B. Konvertierungen oder automatisch befüllte Datenfelder).
        /// </summary>
        /// <param name="builder">
        /// Die Erzeugerinstanz der Datenbankmodellierung
        /// </param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Form>().Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Entity<Allergen>().Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Entity<Product>().Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Entity<Product>().Property(p => p.Allergens).HasConversion(v => v == null ? "" : string.Join('|', Array.ConvertAll(v, allergen => allergen.Id)), v => string.IsNullOrEmpty(v) ? new Allergen[0] : Array.ConvertAll(v.Split('|', StringSplitOptions.RemoveEmptyEntries), id => new ApplicationDbContext().Allergens.Find(int.Parse(id))));
            builder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(5, 2)");
            builder.Entity<Product>().Property(p => p.Sale).HasColumnType("decimal(5, 2)");
            builder.Entity<Product>().Property(p => p.Deposit).HasColumnType("decimal(5, 2)");
            builder.Entity<HistoricOrder>().Property(p => p.User).HasColumnName("UserId").HasConversion(v => v.Id, v => Users.Find(v));
            builder.Entity<HistoricOrder>().Property(p => p.Product).HasColumnName("ProductId").HasConversion(v => v.Id, v => Products.Find(v));
            builder.Entity<CurrentOrder>().Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Entity<CurrentOrder>().Property(p => p.User).HasColumnName("UserId").HasConversion(v => v.Id, v => Users.Find(v));
            builder.Entity<CurrentOrder>().Property(p => p.Product).HasColumnName("ProductId").HasConversion(v => v.Id, v => Products.Find(v));
            builder.Entity<RegistrationCode>().HasKey("Code");
            builder.Entity<RegistrationCode>().Property(p => p.Role).HasConversion(v => v.Name, v => Roles.FirstOrDefault(role => role.Name.Equals(v)));
            base.OnModelCreating(builder);
        }
    }
}