using CABESO.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

namespace CABESO
{
    public static class Database
    {
        private static ApplicationDbContext context;

        public static IWebHost MigrateDatabase(this IWebHost webHost)
        {
            IServiceScopeFactory serviceScopeFactory = webHost.Services.GetService(typeof(IServiceScopeFactory)) as IServiceScopeFactory;
            context = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            return webHost;
        }

        public static readonly string[] Roles = new string[] { "Teacher", "Student", "Employee" };

        private static string roleName;
        private static bool admin, employee;
        private static void RetrieveRoles(IdentityUser user)
        {
            roleName = string.Empty;
            admin = false;
            employee = false;
            foreach (IdentityUserRole<string> role in context.UserRoles.Where(userRole => userRole.UserId.Equals(user.Id)))
            {
                if (role.RoleId.Equals(AdminId))
                    admin = true;
                else
                {
                    if (role.RoleId.Equals(EmployeeId))
                        employee = true;
                    roleName = context.Roles.FirstOrDefault(r => r.Id.Equals(role.RoleId))?.Name ?? string.Empty;
                }
            }
        }

        public static string GetRoleName(this IdentityUser user)
        {
            RetrieveRoles(user);
            return roleName;
        }

        public static bool IsAdmin(this IdentityUser user)
        {
            RetrieveRoles(user);
            return admin;
        }

        public static bool IsEmployee(this IdentityUser user)
        {
            RetrieveRoles(user);
            return employee;
        }
        public static Form GetForm(this IdentityUser user)
        {
            DbConnection connection = context.Database.GetDbConnection();
            connection.ConnectionString = Startup.DefaultConnection;
            using (DbCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = $"SELECT [FormId] FROM [dbo].[AspNetUsers] WHERE [Id] = '{user.Id}';";
                object result = command.ExecuteScalar();
                connection.Close();

                if (result is int)
                    return context.Forms.Find(result);
                else
                    return null;
            }
        }

        public static void SetFormId(this IdentityUser user, int? formId)
        {
            DbConnection connection = context.Database.GetDbConnection();
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
            return new Name(user.Email, GetRoleName(user).Equals("Student"));
        }

        public static IdentityUser GetUserById(string id)
        {
            return context.Users.Find(id);
        }

        public static IdentityUser GetIdentityUser(this ClaimsPrincipal principal)
        {
            return context.Users.FirstOrDefault(user => user.UserName.Equals(principal.Identity.Name, StringComparison.OrdinalIgnoreCase)) ?? new IdentityUser();
        }

        public static string SqlNow
        {
            get
            {
                return SqlTimeFormat(DateTime.UtcNow);
            }
        }

        public static string SqlTimeFormat(DateTime dt)
        {
            return dt.ToString("yyyyMMdd hh:mm:ss tt", CultureInfo.InvariantCulture);
        }

        public static string AdminId
        {
            get
            {
                return context.Roles.FirstOrDefault(role => role.Name.Equals("Admin")).Id;
            }
        }

        public static string EmployeeId
        {
            get
            {
                return context.Roles.FirstOrDefault(role => role.Name.Equals("Employee")).Id;
            }
        }
    }
}