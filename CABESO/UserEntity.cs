using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace CABESO
{
    public class UserEntity
    {
        public string Id { get; set; }
        public Name Name { get; set; }
        public string Form { get; set; }
        public string[] Roles { get; set; }
        public bool Admin { get; set; }

        public object[] ExportArray()
        {
            return new object[] { Name, Form };
        }

        private static string _adminId;
        private static Dictionary<string, string> _roleNames;

        static UserEntity()
        {
            _roleNames = new Dictionary<string, string>();
            Array.ForEach(Database.SqlQuery("AspNetRoles", null, "Id", "Name"), role => _roleNames.Add(role[0].ToString(), role[1].ToString()));

            _adminId = _roleNames.FirstOrDefault(role => role.Value.Equals("Admin")).Key;
        }

        private static UserEntity ConvertData(object[] data)
        {
            object[][] form = Database.SqlQuery("Forms", $"[Id] = '{data[2]}'", "Name");
            if (form == null || form.Length != 1)
                form = new object[][] { new object[] { string.Empty } };

            object[][] enrolledRoles = Database.SqlQuery("AspNetUserRoles", $"[UserId] = '{data[0]}'", "RoleId");
            List<string> roles = new List<string>();
            bool admin = false;
            foreach (object[] enrolledRole in enrolledRoles)
            {
                if (enrolledRole[0].Equals(_adminId))
                    admin = true;
                else
                    roles.Add(_roleNames[enrolledRole[0].ToString()]);
            }

            return new UserEntity() { Name = new Name(data[1].ToString(), !string.IsNullOrEmpty(form[0][0].ToString())), Form = form[0][0].ToString(), Admin = admin, Id = data[0].ToString(), Roles = roles.ToArray() };
        }

        public static UserEntity GetUser(UserManager<IdentityUser> userManager, ClaimsPrincipal principal)
        {
            return GetUser(userManager.GetUserAsync(principal).Result.Id);
        }

        public static UserEntity GetUser(string userId)
        {
            return ConvertData(Database.SqlQuery("AspNetUsers", $"[Id] = '{userId}'", "Id", "UserName", "Form").First());
        }

        public static UserEntity[] EnumerateUsers()
        {
            return Array.ConvertAll(Database.SqlQuery("AspNetUsers", null, "Id", "UserName", "Form"), user => ConvertData(user));
        }
    }

    public class Name
    {
        public Name(string mail, bool student)
        {
            if (string.IsNullOrEmpty(mail))
                return;

            Match[] matches = Regex.Matches(mail, @"([^0-9\.])+(?=.*@gnr.wwschool\.de)").ToArray();

            if (matches.Length != 2)
            {
                FirstName = mail;
                return;
            }

            string[] names = Array.ConvertAll(matches, m => m.Value.ToLower());

            if (!student)
                names = names.Reverse().ToArray();

            char[] firstName = names[1].ToCharArray();
            firstName[0] = char.ToUpper(firstName[0]);
            for (int i = 1; i < firstName.Length; i++)
                if (!char.IsLetter(firstName[i - 1]))
                    firstName[i] = char.ToUpper(firstName[i]);

            string[] lastNames = names[0].Split('-');
            lastNames[lastNames.Length - 1] = char.ToUpper(lastNames[lastNames.Length - 1][0]) + (lastNames[lastNames.Length - 1].Length > 1 ? lastNames[lastNames.Length - 1].Substring(1) : "");

            FirstName = new string(firstName);
            LastName = string.Join(' ', lastNames);
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public override string ToString()
        {
            return (FirstName + " " + LastName).Trim();
        }
    }
}