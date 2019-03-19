using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CABESO.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class OverviewModel : PageModel
    {
        public UserEntity[] Users;

        public string FirstNameSort { get; set; }
        public string LastNameSort { get; set; }
        public string RoleSort { get; set; }
        public string FormSort { get; set; }
        public string AdminSort { get; set; }

        public OverviewModel()
        {
            Users = UserEntity.EnumerateUsers();
        }

        public void OnGet(string sortOrder)
        {
            FirstNameSort = string.IsNullOrEmpty(sortOrder) ? "!fn" : "";
            LastNameSort = sortOrder == "ln" ? "!ln" : "ln";
            RoleSort = sortOrder == "r" ? "!r" : "r";
            FormSort = sortOrder == "f" ? "!f" : "f";
            AdminSort = sortOrder == "a" ? "!a" : "a";

            IEnumerable<UserEntity> users = UserEntity.EnumerateUsers();
            foreach (UserEntity user in users)
                user.Roles = Array.ConvertAll(user.Roles, role => Program.Translations[role]);

            switch (sortOrder)
            {
                case "!fn":
                    users = users.OrderByDescending(user => user.Name.FirstName);
                    break;
                case "ln":
                    users = users.OrderBy(user => user.Name.LastName);
                    break;
                case "!ln":
                    users = users.OrderByDescending(user => user.Name.LastName);
                    break;
                case "r":
                    users = users.OrderBy(user => user.Roles.FirstOrDefault());
                    break;
                case "!r":
                    users = users.OrderByDescending(user => user.Roles.FirstOrDefault());
                    break;
                case "f":
                    users = users.OrderBy(user => user.Form);
                    break;
                case "!f":
                    users = users.OrderByDescending(user => user.Form);
                    break;
                case "a":
                    users = users.OrderBy(user => user.Admin);
                    break;
                case "!a":
                    users = users.OrderByDescending(user => user.Admin);
                    break;
                default:
                    users = users.OrderBy(user => user.Name.FirstName);
                    break;
            }

            Users = users.ToArray();
        }
    }
}