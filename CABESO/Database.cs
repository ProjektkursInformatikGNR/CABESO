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

        public static T Create<T>(string condition, params ValueTuple<string, string>[] names)
        {
            object[] data = Select<T>(condition, names).FirstOrDefault();
            PropertyInfo[] properties = typeof(T).GetProperties();
            if (data == null || data.Length != properties.Length)
                return default;
            T obj = Activator.CreateInstance<T>();
            for (int i = 0; i < properties.Length; i++)
                properties[i].SetValue(obj, Program.Convert(data[i].ToString(), properties[i].PropertyType));
            return obj;
        }

        public static IEnumerable<T> Enumerate<T>(params ValueTuple<string, string>[] names)
        {
            object[][] data = Select<T>(names: names);
            PropertyInfo[] properties = typeof(T).GetProperties();
            if (data == null || data.Length == 0 || data[0].Length != properties.Length)
                yield break;
            foreach (object[] d in data)
            {
                T obj = Activator.CreateInstance<T>();
                for (int i = 0; i < properties.Length; i++)
                    properties[i].SetValue(obj, Program.Convert(d[i].ToString(), properties[i].PropertyType));
                yield return obj;
            }
        }

        public static object[][] Select(string table, string condition, params string[] columns)
        {
            return Select($"SELECT {(columns == null || columns.Length == 0 ? "*" : string.Join(", ", Array.ConvertAll(columns, s => $"[{s}]")))} FROM [dbo].[{table}]{(string.IsNullOrEmpty(condition) ? "" : $" WHERE {condition}")};");
        }

        public static object[][] Select<T>(string condition = null, params ValueTuple<string, string>[] names)
        {
            return Select(typeof(T).Name + "s", condition, Array.ConvertAll(typeof(T).GetProperties(), property => names.FirstOrDefault(name => name.Item1.Equals(property.Name)).Item2 ?? property.Name));
        }

        public static object[][] Select(string query)
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

        public static void Add(string table, object values)
        {
            SqlExecute($"INSERT INTO [{table}] ({string.Join(", ", Array.ConvertAll(values.GetType().GetProperties(), property => string.Format("[{0}]", property.Name)))}) VALUES ({string.Join(", ", Array.ConvertAll(values.GetType().GetProperties(), property => string.Format("'{0}'", property.GetValue(values))))});");
        }

        public static void Update(string table, string condition, object values)
        {
            SqlExecute($"UPDATE [{table}] SET {string.Join(", ", Array.ConvertAll(values.GetType().GetProperties(), property => string.Format("[{0}] = '{1}'", property.Name, property.GetValue(values))))} WHERE {condition};");
        }

        public static void Delete(string table, string condition)
        {
            SqlExecute($"DELETE FROM [{table}] WHERE {condition};");
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

        public static ApplicationDbContext Context { get; private set; }
    }
}
