using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace CABESO
{
    public class Program
    {
        public const string ErrorMessage = "Gib bitte {0} an.";
        public static Dictionary<string, string> Translations { get; private set; }

        public static string Greeting(Name name = null)
        {
            string greeting;
            switch (DateTime.Now.Hour)
            {
                case int hour when (hour < 5 || hour >= 22):
                    greeting = "Gute Nacht";
                    break;
                case int hour when (hour >= 5 && hour < 12):
                    greeting = "Guten Morgen";
                    break;
                case int hour when (hour >= 12 && hour < 18):
                    greeting = "Guten Nachmittag";
                    break;
                case int hour when (hour >= 18 && hour < 22):
                    greeting = "Guten Abend";
                    break;
                default:
                    greeting = "Hallo";
                    break;
            }
            return string.Format("{0}, {1}!", greeting, name ?? "ihr Luschen");
        }

        public static bool Matches(string entry, string search)
        {
            return entry.Contains(search, StringComparison.OrdinalIgnoreCase);
        }

        public static string TimeDisplay(DateTime dt)
        {
            return dt.ToLocalTime().ToString(CultureInfo.CurrentCulture);
        }

        public static void Main(string[] args)
        {
            Translations = new Dictionary<string, string>
            {
                { "Student", "Schüler*in" },
                { "Teacher", "Lehrer*in" },
                { "Employee", "Mitarbeiter*in" },
                { "True", "ja" },
                { "False", "nein" }
            };

            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build()
                .MigrateDatabase()
                .Run();
        }
    }
}