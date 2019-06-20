using CABESO.Data;
using CABESO.Properties;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

namespace CABESO
{
    /// <summary>
    /// Eine statische Klasse zur Erweiterung bereits bestehender Klassen
    /// </summary>
    public static class Database
    {
        /// <summary>
        /// Bindet eine Instanz des <see cref="ApplicationDbContext"/> in den Migrationsvorgang ein.
        /// </summary>
        /// <param name="webHost">
        /// Der Host des Webservers
        /// </param>
        /// <param name="context">
        /// Der initialisierte Datenbankkontext
        /// </param>
        /// <returns></returns>
        public static IWebHost MigrateDatabase(this IWebHost webHost, out ApplicationDbContext context)
        {
            IServiceScopeFactory serviceScopeFactory = webHost.Services.GetService(typeof(IServiceScopeFactory)) as IServiceScopeFactory;
            context = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
            return webHost;
        }

        /// <summary>
        /// Die typischerweise zur Auswahl stehenden Rollen (Schüler*in, Lehrer*in, Mitarbeiter*in)
        /// </summary>
        public static readonly IdentityRole[] Roles = Startup.Context.Roles.Where(role => !role.Name.Equals(Resources.Admin)).ToArray();

        /// <summary>
        /// Gibt die Rollenzugehörigkeiten eines gegebenen Benutzers zurück.
        /// </summary>
        /// <param name="user">
        /// Der zu betrachtende Benutzer
        /// </param>
        /// <param name="role">
        /// Wird auf ggf. auf "Student" oder "Teacher" gesetzt, andernfalls bleibt es bei <c>null</c>.
        /// </param>
        /// <param name="admin">
        /// Wird auf <c>true</c> gesetzt, falls <paramref name="user"/> ein Administrator ist.
        /// </param>
        /// <param name="employee">
        /// Wird auf <c>true</c> gesetzt, falls <paramref name="user"/> ein Mitarbeiter bzw. eine Mitarbeiterin ist.
        /// </param>
        private static void RetrieveRoles(IdentityUser user, out IdentityRole role, out bool admin, out bool employee)
        {
            role = null;
            admin = false;
            employee = false;
            foreach (IdentityUserRole<string> userRole in Startup.Context.UserRoles.Where(userRole => userRole.UserId.Equals(user.Id)))
            {
                if (Startup.Context.Roles.Find(userRole.RoleId).Name.Equals(Resources.Admin))
                    admin = true;
                else
                {
                    if (Startup.Context.Roles.Find(userRole.RoleId).Name.Equals(Resources.Admin))
                        employee = true;
                    role = Startup.Context.Roles.Find(userRole.RoleId);
                }
            }
        }

        /// <summary>
        /// Gibt ggf. die Schüler*innen- bzw. Lehrer*innen-Rolle des Benutzers zurück.
        /// </summary>
        /// <param name="user">
        /// Der zu betrachtende Benutzer
        /// </param>
        /// <returns>
        /// Die Rolle des Benutzers
        /// </returns>
        public static IdentityRole GetRole(this IdentityUser user)
        {
            RetrieveRoles(user, out IdentityRole role, out _, out _);
            return role;
        }

        /// <summary>
        /// Gibt an, ob der Benutzer ein Administrator ist.
        /// </summary>
        /// <param name="user">
        /// Der zu betrachtende Benutzer
        /// </param>
        /// <returns>
        /// Gibt <c>true</c> zurück, falls der Benutzer ein Administrator ist, sonst <c>false</c>.
        /// </returns>
        public static bool IsAdmin(this IdentityUser user)
        {
            RetrieveRoles(user, out _, out bool admin, out _);
            return admin;
        }

        /// <summary>
        /// Gibt an, ob der Benutzer ein Mitarbeiter bzw. eine Mitarbeiterin ist.
        /// </summary>
        /// <param name="user">
        /// Der zu betrachtende Benutzer
        /// </param>
        /// <returns>
        /// Gibt <c>true</c> zurück, falls der Benutzer ein Mitarbeiter bzw. eine Mitarbeiterin ist, sonst <c>false</c>.
        /// </returns>
        public static bool IsEmployee(this IdentityUser user)
        {
            RetrieveRoles(user, out _, out _, out bool employee);
            return employee;
        }

        /// <summary>
        /// Gibt die ihm zugehörige Schulklasse eines Benutzers zurück.
        /// </summary>
        /// <param name="user">
        /// Der zu betrachtende Benutzer
        /// </param>
        /// <returns>
        /// Die Schulklasse des Benutzers (kann <c>null</c> sein)
        /// </returns>
        public static Form GetForm(this IdentityUser user)
        {
            DbConnection connection = Startup.Context.Database.GetDbConnection();
            connection.ConnectionString = Startup.DefaultConnection;
            using (DbCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = $"SELECT [FormId] FROM [dbo].[AspNetUsers] WHERE [Id] = '{user.Id}';";
                object result = command.ExecuteScalar();
                connection.Close();

                if (result is int)
                    return Startup.Context.Forms.Find(result);
                else
                    return null;
            }
        }

        /// <summary>
        /// Legt die Schulklassenzugehörigkeit eines Benutzers anhand der ID der Schulklasse fest.
        /// </summary>
        /// <param name="user">
        /// Der zu betrachtende Benutzer
        /// </param>
        /// <param name="formId">
        /// Die ID der zuzuordnenden Schulklasse
        /// </param>
        public static void SetFormId(this IdentityUser user, int? formId)
        {
            DbConnection connection = Startup.Context.Database.GetDbConnection();
            connection.ConnectionString = Startup.DefaultConnection;
            using (DbCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = $"UPDATE [dbo].[AspNetUsers] SET [FormId] = '{formId?.ToString()}' WHERE [Id]='{user.Id}';";
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        /// <summary>
        /// Gibt den Namen eines gegebenen Benutzers zurück.
        /// </summary>
        /// <param name="user">
        /// Der zu betrachtende Benutzer
        /// </param>
        /// <returns>
        /// Der Name des Benutzers
        /// </returns>
        public static Name GetName(this IdentityUser user)
        {
            return new Name(user.Email, GetRole(user)?.Name.Equals(Resources.Student) ?? false);
        }

        /// <summary>
        /// Gibt anhand der ID den zugeordneten Benutzer zurück.
        /// </summary>
        /// <param name="id">
        /// Die ID des gesuchten Benutzers
        /// </param>
        /// <returns>
        /// Der gesuchte Benutzer
        /// </returns>
        public static IdentityUser GetUserById(string id)
        {
            return Startup.Context.Users.Find(id);
        }

        /// <summary>
        /// Gibt anhand des <see cref="ClaimsPrincipal"/> den zugeordneten Benutzer zurück.
        /// </summary>
        /// <param name="principal">
        /// Das <see cref="ClaimsPrincipal"/> des gesuchten Benutzers
        /// </param>
        /// <returns>
        /// Der gesuchte Benutzer
        /// </returns>
        public static IdentityUser GetIdentityUser(this ClaimsPrincipal principal)
        {
            return Startup.Context.Users.FirstOrDefault(user => user.UserName.Equals(principal.Identity.Name, StringComparison.OrdinalIgnoreCase)) ?? new IdentityUser();
        }

        /// <summary>
        /// Gibt die anzuzeigende Bezeichnung einer gegebenen Rolle zurück.
        /// </summary>
        /// <param name="role">
        /// Die anzuzeigende Rolle
        /// </param>
        /// <returns>
        /// Der Anzeigename der Rolle
        /// </returns>
        public static string GetDisplayName(this IdentityRole role)
        {
            return Resources.ResourceManager.GetString(role.Name + "Display");
        }

        /// <summary>
        /// Konvertiert ein <see cref="DateTime"/>-Objekt in ein lesbares Format ("dd.MM.yyyy HH:mm[:ss]").
        /// </summary>
        /// <param name="dt">
        /// Das zu konvertierende Objekt
        /// </param>
        /// <param name="showSeconds">
        /// Der Indikator, ob im Format auch Sekunden anzuzeigen sind.
        /// </param>
        /// <returns>
        /// Das als <see cref="string"/> konvertierte <see cref="DateTime"/>-Objekt
        /// </returns>
        public static string GetDisplayFormat(this DateTime dt, bool showSeconds = true)
        {
            return showSeconds ? dt.ToLocalTime().ToString(CultureInfo.CurrentCulture) : dt.ToLocalTime().ToString("dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Konvertiert ein <see cref="DateTime"/>-Objekt in das von SQL geforderte Format ("yyyyMMdd hh:mm:ss tt").
        /// </summary>
        /// <param name="dt">
        /// Das zu konvertierende Objekt
        /// </param>
        /// <returns>
        /// Das als <see cref="string"/> konvertierte <see cref="DateTime"/>-Objekt
        /// </returns>
        public static string GetSqlFormat(this DateTime dt)
        {
            return dt.ToUniversalTime().ToString("yyyyMMdd hh:mm:ss tt", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Konvertiert ein <see cref="DateTime"/>-Objekt in das von HTML geforderte Uhrzeitformat ("yyyy-MM-ddTHH:mm").
        /// </summary>
        /// <param name="dt">
        /// Das zu konvertierende Objekt
        /// </param>
        /// <returns>
        /// Das als <see cref="string"/> konvertierte <see cref="DateTime"/>-Objekt
        /// </returns>
        public static string GetHtmlFormat(this DateTime dt)
        {
            return dt.ToString("s").Remove(16);
        }

        /// <summary>
        /// Konvertiert ein <see cref="DateTime"/>-Objekt in das von HTML geforderte Tagesformat ("yyyy-MM-dd").
        /// </summary>
        /// <param name="dt">
        /// Das zu konvertierende Objekt
        /// </param>
        /// <returns>
        /// Das als <see cref="string"/> konvertierte <see cref="DateTime"/>-Objekt
        /// </returns>
        public static string GetHtmlDateFormat(this DateTime dt)
        {
            return dt.ToString("s").Remove(10);
        }

        /// <summary>
        /// Durchsucht eine Datenstruktur nach einem Ausdruck und gibt zutreffende Elemente zurück.
        /// </summary>
        /// <typeparam name="T">
        /// Der generische Datentyp der Datenstruktur
        /// </typeparam>
        /// <param name="list">
        /// Die zu durchsuchende Datenstruktur
        /// </param>
        /// <param name="search">
        /// Der zu suchende Ausdruck
        /// </param>
        /// <param name="entries">
        /// Die Eigenschaften eines Elementes vom Datentyp <typeparamref name="T"/>, die bei der Suche berücksichtigt werden sollen
        /// </param>
        /// <returns>
        /// Alle zutreffenden Elemente aus der Datenstruktur
        /// </returns>
        public static IEnumerable<T> Search<T>(this IEnumerable<T> list, string search, params Func<T, string>[] entries)
        {
            if (entries == null || entries.Length == 0 || string.IsNullOrEmpty(search))
                return new T[0];

            string[] searchWords = search.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return list.Where(element => searchWords.Any(searchWord => entries.Any(entry => entry(element).Contains(searchWord, StringComparison.OrdinalIgnoreCase))));
        }

        /// <summary>
        /// Gibt eine Datenstruktur von Schulklassen zurück, zwischen denen der Benutzer typischerweise wählen kann.
        /// </summary>
        /// <param name="context">
        /// Der Datenbankkontext
        /// </param>
        /// <returns>
        /// Die zur Auswahl stehenden Schulklassen
        /// </returns>
        public static Form[] GetFormsSelect(this ApplicationDbContext context)
        {
            return context.Forms
                .Where(form => form.GetGrade().Year >= Grade.FIVE.Year && form.GetGrade().Year <= Grade.Q2.Year)
                .GroupBy(form => form.ToString())
                .Select(group => group.First())
                .OrderBy(form => form.GetGrade().Year)
                .ToArray();
        }

        /// <summary>
        /// Gibt den jetzigen Zeitpunkt im SQL-Format anhand der Methode <see cref="GetSqlFormat(DateTime)"/> zurück.
        /// </summary>
        public static string SqlNow
        {
            get => DateTime.Now.GetSqlFormat();
        }

        /// <summary>
        /// Konvertiert ein <see cref="decimal"/>-Objekt in einen Geldbetrag.
        /// </summary>
        /// <param name="dec">
        /// Der zu konvertierende Dezimalwert
        /// </param>
        /// <returns>
        /// Der anzuzeigende Geldbetrag
        /// </returns>
        public static string GetDisplayFormat(this decimal dec)
        {
            return string.Format("{0:C2}", dec).Replace(' ', '\u00A0');
        }

        /// <summary>
        /// Konvertiert ein <see cref="Nullable{Decimal}"/>-Objekt in einen Geldbetrag anhand der Methode <see cref="GetDisplayFormat(decimal)"/>.
        /// </summary>
        /// <param name="dec">
        /// Der zu konvertierende Dezimalwert
        /// </param>
        /// <returns>
        /// Der anzuzeigende Geldbetrag
        /// </returns>
        public static string GetDisplayFormat(this decimal? dec)
        {
            return dec.HasValue ? GetDisplayFormat(dec.Value) : string.Empty;
        }
    }
}