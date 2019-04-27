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

        public static object Convert(string s, Type type)
        {
            switch (type)
            {
                case Type t when t.Equals(typeof(int)):
                    return int.Parse(s);
                case Type t when t.Equals(typeof(int?)):
                    return string.IsNullOrEmpty(s) ? null : (int?) int.Parse(s);
                case Type t when t.Equals(typeof(decimal)):
                    return decimal.Parse(s);
                case Type t when t.Equals(typeof(decimal?)):
                    return string.IsNullOrEmpty(s) ? null : (decimal?) decimal.Parse(s);
                case Type t when t.Equals(typeof(bool)):
                    return bool.Parse(s);
                case Type t when t.Equals(typeof(bool?)):
                    return string.IsNullOrEmpty(s) ? null : (bool?) bool.Parse(s);
                case Type t when t.Equals(typeof(DateTime)):
                    return DateTime.Parse(s);
                case Type t when t.Equals(typeof(Client)):
                    return Client.Create(s);
                case Type t when t.Equals(typeof(Product)):
                    return Product.Create(int.Parse(s));
                case Type t when t.Equals(typeof(Order)):
                    return Order.Create(int.Parse(s));
                case Type t when t.Equals(typeof(DBNull)):
                    return null;
                case Type t when t.IsArray:
                    object[] elements = Array.ConvertAll(s.Split('|'), split => Convert(split, t.GetElementType()));
                    Array a = Array.CreateInstance(t.GetElementType(), elements.Length);
                    elements.CopyTo(a, 0);
                    return a;
                default:
                    return s;
            }
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