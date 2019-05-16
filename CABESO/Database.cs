using CABESO.Data;
using CABESO.Properties;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

namespace CABESO
{
    public static class Database
    {
        public static IWebHost MigrateDatabase(this IWebHost webHost, out ApplicationDbContext context)
        {
            IServiceScopeFactory serviceScopeFactory = webHost.Services.GetService(typeof(IServiceScopeFactory)) as IServiceScopeFactory;
            context = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
            return webHost;
        }

        public static readonly IdentityRole[] Roles = Startup.Context.Roles.Where(role => !role.Name.Equals(Resources.Admin)).ToArray();

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

        public static IdentityRole GetRole(this IdentityUser user)
        {
            RetrieveRoles(user, out IdentityRole role, out _, out _);
            return role;
        }

        public static bool IsAdmin(this IdentityUser user)
        {
            RetrieveRoles(user, out _, out bool admin, out _);
            return admin;
        }

        public static bool IsEmployee(this IdentityUser user)
        {
            RetrieveRoles(user, out _, out _, out bool employee);
            return employee;
        }
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

        public static Name GetName(this IdentityUser user)
        {
            return new Name(user.Email, GetRole(user)?.Name.Equals(Resources.Student) ?? false);
        }

        public static IdentityUser GetUserById(string id)
        {
            return Startup.Context.Users.Find(id);
        }

        public static IdentityUser GetIdentityUser(this ClaimsPrincipal principal)
        {
            return Startup.Context.Users.FirstOrDefault(user => user.UserName.Equals(principal.Identity.Name, StringComparison.OrdinalIgnoreCase)) ?? new IdentityUser();
        }

        public static string GetDisplayName(this IdentityRole role)
        {
            return Resources.ResourceManager.GetString(role.Name + "Display");
        }

        public static string GetDisplayFormat(this DateTime dt)
        {
            return dt.ToLocalTime().ToString(CultureInfo.CurrentCulture);
        }

        public static string GetSqlFormat(this DateTime dt)
        {
            return dt.ToUniversalTime().ToString("yyyyMMdd hh:mm:ss tt", CultureInfo.InvariantCulture);
        }
        public static string GetHtmlFormat(this DateTime dt)
        {
            return dt.ToString("s").Remove(16);
        }
        public static IEnumerable<T> Search<T>(this IEnumerable<T> list, string search, params Func<T, string>[] entries)
        {
            if (entries == null || entries.Length == 0 || string.IsNullOrEmpty(search))
                return new T[0];

            string[] searchWords = search.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return list.Where(element => searchWords.Any(searchWord => entries.Any(entry => entry.Invoke(element).Contains(searchWord, StringComparison.OrdinalIgnoreCase))));
        }

        public static string SqlNow
        {
            get => DateTime.Now.GetSqlFormat();
        }

        public static string GetDisplayFormat(this decimal dec)
        {
            return string.Format("{0:C2}", dec).Replace(' ', '\u00A0');
        }
        public static string GetDisplayFormat(this decimal? dec)
        {
            return dec.HasValue ? GetDisplayFormat(dec.Value) : string.Empty;
        }
    }
}