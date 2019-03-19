using CABESO.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace CABESO
{
    public class Program
    {
        public const string ErrorMessage = "Gib bitte {0} an.";
        public static Dictionary<string, string> Translations { get; private set; }

        public static void Main(string[] args)
        {
            Translations = new Dictionary<string, string>();
            Translations.Add("Student", "Schüler*in");
            Translations.Add("Teacher", "Lehrer*in");
            Translations.Add("True", "ja");
            Translations.Add("False", "nein");

            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build()
                .MigrateDatabase()
                .Run();
        }
    }

    public static class Database
    {
        public static IWebHost MigrateDatabase(this IWebHost webHost)
        {
            var serviceScopeFactory = webHost.Services.GetService(typeof(IServiceScopeFactory)) as IServiceScopeFactory;
            Context = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
            Context.Database.Migrate();

            return webHost;
        }

        public static void SqlExecute(string query)
        {
            Context.Database.OpenConnection();

            using (DbCommand command = Context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.ExecuteNonQuery();
            }

            Context.Database.CloseConnection();
        }

        public static object[][] SqlQuery(string table, string condition, params string[] columns)
        {
            return SqlQuery($"SELECT {string.Join(", ", Array.ConvertAll(columns, s => $"[{s}]"))} FROM [dbo].[{table}]{(string.IsNullOrEmpty(condition) ? "" : $" WHERE {condition}")};");
        }

        public static object[][] SqlQuery(string query)
        {
            List<object[]> data = new List<object[]>();
            Context.Database.OpenConnection();

            using (DbCommand command = Context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                DbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    data.Add(row);
                }
            }

            Context.Database.CloseConnection();
            return data.ToArray();
        }

        public static ApplicationDbContext Context { get; private set; }
    }
}