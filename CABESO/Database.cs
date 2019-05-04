using CABESO.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Web;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

namespace CABESO
{
    public static class Database
    {
        public static IWebHost MigrateDatabase(this IWebHost webHost)
        {
            var serviceScopeFactory = webHost.Services.GetService(typeof(IServiceScopeFactory)) as IServiceScopeFactory;
            Context = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
            Context.Database.Migrate();

            return webHost;
        }

        private static string roleName;
        private static bool? admin, employee;
        private static void RetrieveRoles(IdentityUser user)
        {
            foreach (IdentityUserRole<string> role in Context.UserRoles.Where(userRole => userRole.UserId.Equals(user.Id)))
            {
                if (role.RoleId.Equals(AdminId))
                    admin = true;
                else
                {
                    if (role.RoleId.Equals(EmployeeId))
                        employee = true;
                    roleName = Context.Roles.FirstOrDefault(r => r.Id.Equals(role.RoleId))?.Name ?? string.Empty;
                }
            }
        }

        public static string GetRoleName(this IdentityUser user)
        {
            if (string.IsNullOrEmpty(roleName))
                RetrieveRoles(user);
            return roleName ?? string.Empty;
        }

        public static bool IsAdmin(this IdentityUser user)
        {
            if (!admin.HasValue)
                RetrieveRoles(user);
            return admin ?? false;
        }

        public static bool IsEmployee(this IdentityUser user)
        {
            if (!employee.HasValue)
                RetrieveRoles(user);
            return employee ?? false;
        }
        public static Form GetForm(this IdentityUser user)
        {
            DbConnection connection = Context.Database.GetDbConnection();
            connection.ConnectionString = Startup.DefaultConnection;
            using (DbCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = $"SELECT [FormId] FROM [dbo].[AspNetUsers] WHERE [Id] = '{user.Id}';";
                object result = command.ExecuteScalar();
                connection.Close();
                return Form.GetFormById(result is int ? (int?)result : null);
            }
        }

        public static void SetFormId(this IdentityUser user, int? formId)
        {
            DbConnection connection = Context.Database.GetDbConnection();
            connection.ConnectionString = Startup.DefaultConnection;
            using (DbCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = $"UPDATE [dbo].[AspNetUsers] SET [FormId] = '{formId?.ToString() ?? "NULL"}' WHERE [Id]='{user.Id}';";
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
            return Context.Users.Find(id);
        }

        public static IdentityUser GetIdentityUser(this ClaimsPrincipal principal)
        {
            return Context.Users.FirstOrDefault(user => user.UserName.Equals(principal.Identity.Name, StringComparison.OrdinalIgnoreCase)) ?? new IdentityUser();
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
                return Context.Roles.FirstOrDefault(role => role.Name.Equals("Admin")).Id;
            }
        }

        public static string EmployeeId
        {
            get
            {
                return Context.Roles.FirstOrDefault(role => role.Name.Equals("Employee")).Id;
            }
        }

        public static ApplicationDbContext Context { get; private set; }
    }
}