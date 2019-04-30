using CABESO.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Reflection;

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

        private static Dictionary<string, string> roleNames;
        private static string adminId, employeeId;

        private static void InitialiseRoles()
        {
            roleNames = new Dictionary<string, string>();
            foreach (object[] role in Select("AspNetRoles", null, "Id", "Name"))
            {
                if (role[1].Equals("Admin"))
                    adminId = role[0].ToString();
                else
                {
                    if (role[1].Equals("Employee"))
                        employeeId = role[0].ToString();
                    roleNames.Add(role[0].ToString(), role[1].ToString());
                }
            }
        }

        public static Dictionary<string, string> RoleNames
        {
            get
            {
                if (roleNames == null)
                    InitialiseRoles();
                return roleNames;
            }
        }

        public static string AdminId
        {
            get
            {
                if (string.IsNullOrEmpty(adminId))
                    InitialiseRoles();
                return adminId;
            }
        }

        public static string EmployeeId
        {
            get
            {
                if (string.IsNullOrEmpty(employeeId))
                    InitialiseRoles();
                return employeeId;
            }
        }

        private static Dictionary<int, string> formNames;

        public static Dictionary<int, string> FormNames
        {
            get
            {
                if (formNames == null)
                {
                    formNames = new Dictionary<int, string>();
                    foreach (object[] form in Select("Forms", null))
                    {
                        int year = (int) form[2];
                        int ef = DateTime.Now.Year - (DateTime.Now.Month > 7 ? 0 : 1) - (year > 2018 ? 6 : 5);
                        string name;
                        switch (ef - year)
                        {
                            case int i when i < 0:
                                name = (i + 10) + form[1].ToString();
                                break;
                            case 0:
                                name = "EF";
                                break;
                            default:
                                name = "Q" + (ef - year);
                                break;
                        }
                        formNames.Add((int) form[0], name);
                    }
                }
                return formNames;
            }
        }

        public static string GetFormName(int? id)
        {
            return id.HasValue && id.Value > 0 ? FormNames[id.Value] : string.Empty;
        }

        public static ApplicationDbContext Context { get; private set; }
    }
}
