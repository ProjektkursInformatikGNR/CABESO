using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;

namespace CABESO.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        public IdentityUser[] Users;

        public string FirstNameSort { get; set; }
        public string LastNameSort { get; set; }
        public string RoleSort { get; set; }
        public string FormSort { get; set; }
        public string AdminSort { get; set; }
        public string EmployeeSort { get; set; }

        public string SearchKeyWord { get; set; }

        public IndexModel(ApplicationDbContext context)
        {
            Users = context.Users.ToArray();
        }

        public void OnGet(string search, string sortOrder)
        {
            SearchKeyWord = search ?? string.Empty;
            if (!string.IsNullOrEmpty(SearchKeyWord))
                Users = Users.Where(user => Array.TrueForAll(SearchKeyWord.Split(' '), s => Array.Exists(new[] { user.GetForm()?.ToString(), user.GetName().FirstName, user.GetName().LastName }, e => Program.Matches(e, s)))).ToArray();

            FirstNameSort = string.IsNullOrEmpty(sortOrder) ? "!fn" : "";
            LastNameSort = sortOrder == "ln" ? "!ln" : "ln";
            RoleSort = sortOrder == "r" ? "!r" : "r";
            FormSort = sortOrder == "f" ? "!f" : "f";
            AdminSort = sortOrder == "a" ? "!a" : "a";
            EmployeeSort = sortOrder == "e" ? "!e" : "e";

            IOrderedEnumerable<IdentityUser> users = Users.OrderBy(user => 0);
            switch (sortOrder)
            {
                case "!fn":
                    users = users.OrderByDescending(user => user.GetName().FirstName);
                    break;
                case "ln":
                    users = users.OrderBy(user => user.GetName().LastName);
                    break;
                case "!ln":
                    users = users.OrderByDescending(user => user.GetName().LastName);
                    break;
                case "r":
                    users = users.OrderBy(user => user.GetRoleName());
                    break;
                case "!r":
                    users = users.OrderByDescending(user => user.GetRoleName());
                    break;
                case "f":
                    users = users.OrderBy(user => user.GetForm().ToString());
                    break;
                case "!f":
                    users = users.OrderByDescending(user => user.GetForm().ToString());
                    break;
                case "a":
                    users = users.OrderBy(user => !user.IsAdmin());
                    break;
                case "!a":
                    users = users.OrderByDescending(user => !user.IsAdmin());
                    break;
                case "e":
                    users = users.OrderBy(user => !user.IsEmployee());
                    break;
                case "!e":
                    users = users.OrderByDescending(user => !user.IsEmployee());
                    break;
                default:
                    users = users.OrderBy(user => user.GetName().FirstName);
                    break;
            }

            Users = users.ThenBy(user => user.GetName().LastName).ToArray();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string SearchKeyWord { get; set; }
        }

        public IActionResult OnPost()
        {
            return RedirectToAction("Index", "Admin", new { sortOrder = string.Empty, search = Input.SearchKeyWord?.Trim() ?? string.Empty });
        }
    }
}