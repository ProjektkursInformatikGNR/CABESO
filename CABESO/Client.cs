using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;

namespace CABESO
{
    public class Client
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public Name Name { get; set; }
        public int? FormId { get; set; }
        public string Role { get; set; }
        public bool Admin { get; set; }
        public bool Employee { get; set; }

        public object[] ExportArray()
        {
            return new object[] { Name, Database.GetFormName(FormId) };
        }

        private static Client Create(object[] data)
        {
            object[][] enrolledRoles = Database.Select("AspNetUserRoles", $"[UserId] = '{data[0]}'", "RoleId");
            string role = string.Empty;
            bool admin = false, employee = false;
            foreach (object[] enrolledRole in enrolledRoles)
            {
                if (enrolledRole[0].Equals(Database.AdminId))
                    admin = true;
                else
                {
                    if (enrolledRole[0].Equals(Database.EmployeeId))
                        employee = true;
                    role = Database.RoleNames[enrolledRole[0].ToString()];
                }
            }

            return new Client() { Name = new Name(data[1].ToString(), role.Equals("Student")), FormId = (int?) (data[2] is DBNull ? null : data[2]), Admin = admin, Employee = employee, Id = data[0].ToString(), Role = role, Email = data[1].ToString() };
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