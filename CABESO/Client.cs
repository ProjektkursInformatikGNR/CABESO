using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CABESO
{
    public class Client
    {
        public string Id { get; set; }
        public Name Name { get; set; }
        public string Form { get; set; }
        public string[] Roles { get; set; }
        public bool Admin { get; set; }
        public bool Employee { get; set; }

        public object[] ExportArray()
        {
            return new object[] { Name, Form };
        }

        private static readonly string _adminId, _employeeId;
        private static readonly Dictionary<string, string> _roleNames;

        static Client()
        {
            _roleNames = new Dictionary<string, string>();
            Array.ForEach(Database.Select("AspNetRoles", null, "Id", "Name"), role => _roleNames.Add(role[0].ToString(), role[1].ToString()));

            _adminId = _roleNames.FirstOrDefault(role => role.Value.Equals("Admin")).Key;
            _employeeId = _roleNames.FirstOrDefault(role => role.Value.Equals("Employee")).Key;
        }

        private static Client Create(object[] data)
        {
            object[][] form = Database.Select("Forms", $"[Id] = '{data[2]}'", "Name");
            if (form == null || form.Length != 1)
                form = new object[][] { new object[] { string.Empty } };

            object[][] enrolledRoles = Database.Select("AspNetUserRoles", $"[UserId] = '{data[0]}'", "RoleId");
            List<string> roles = new List<string>();
            bool admin = false, employee = false;
            foreach (object[] enrolledRole in enrolledRoles)
            {
                if (enrolledRole[0].Equals(_adminId))
                    admin = true;
                else if (enrolledRole[0].Equals(_employeeId))
                    employee = true;
                else
                    roles.Add(_roleNames[enrolledRole[0].ToString()]);
            }
            
            return new Client() { Name = new Name(data[1].ToString(), !string.IsNullOrEmpty(form[0][0].ToString())), Form = form[0][0].ToString(), Admin = admin, Employee = employee, Id = data[0].ToString(), Roles = roles.ToArray() };
        }

        public static Client Create(UserManager<IdentityUser> userManager, ClaimsPrincipal principal)
        {
            return principal == null ? new Client() : Create(userManager.GetUserAsync(principal).Result?.Id);
        }

        public static Client Create(string id)
        {
            return string.IsNullOrEmpty(id) ? new Client() : Create(Database.Select("AspNetUsers", $"[Id] = '{id}'", "Id", "UserName", "Form").FirstOrDefault());
        }

        public static Client[] Enumerate()
        {
            return Array.ConvertAll(Database.Select("AspNetUsers", null, "Id", "UserName", "Form"), client => Create(client));
        }

        public IdentityUser ToIdentityUser(UserManager<IdentityUser> userManager)
        {
            return userManager.FindByIdAsync(Id).Result;
        }

        public override bool Equals(object obj)
        {
            return obj is Client && (obj as Client).Id == Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static implicit operator Client(string id)
        {
            return Create(id);
        }
    }
}