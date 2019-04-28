using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CABESO.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        public Client[] Clients;

        public string FirstNameSort { get; set; }
        public string LastNameSort { get; set; }
        public string RoleSort { get; set; }
        public string FormSort { get; set; }
        public string AdminSort { get; set; }
        public string EmployeeSort { get; set; }

        public string SearchKeyWord { get; set; }

        public IndexModel()
        {
            Clients = Client.Enumerate();
        }

        public void OnGet(string search, string sortOrder)
        {
            SearchKeyWord = search ?? string.Empty;
            if (!string.IsNullOrEmpty(SearchKeyWord))
                Clients = Clients.Where(client => Array.TrueForAll(SearchKeyWord.Split(' '), s => Array.Exists(new[] { Database.GetFormName(client.FormId), client.Name.FirstName, client.Name.LastName }, e => Program.Matches(e, s)))).ToArray();

            FirstNameSort = string.IsNullOrEmpty(sortOrder) ? "!fn" : "";
            LastNameSort = sortOrder == "ln" ? "!ln" : "ln";
            RoleSort = sortOrder == "r" ? "!r" : "r";
            FormSort = sortOrder == "f" ? "!f" : "f";
            AdminSort = sortOrder == "a" ? "!a" : "a";
            EmployeeSort = sortOrder == "e" ? "!e" : "e";

            IOrderedEnumerable<Client> users = Clients.OrderBy(client => 0);
            foreach (Client client in users)
                client.Role = Program.Translations.GetValueOrDefault(client.Role);

            switch (sortOrder)
            {
                case "!fn":
                    users = users.OrderByDescending(client => client.Name.FirstName);
                    break;
                case "ln":
                    users = users.OrderBy(client => client.Name.LastName);
                    break;
                case "!ln":
                    users = users.OrderByDescending(client => client.Name.LastName);
                    break;
                case "r":
                    users = users.OrderBy(client => client.Role);
                    break;
                case "!r":
                    users = users.OrderByDescending(client => client.Role);
                    break;
                case "f":
                    users = users.OrderBy(client => Database.GetFormName(client.FormId));
                    break;
                case "!f":
                    users = users.OrderByDescending(client => Database.GetFormName(client.FormId));
                    break;
                case "a":
                    users = users.OrderBy(client => !client.Admin);
                    break;
                case "!a":
                    users = users.OrderByDescending(client => !client.Admin);
                    break;
                case "e":
                    users = users.OrderBy(client => !client.Employee);
                    break;
                case "!e":
                    users = users.OrderByDescending(client => !client.Employee);
                    break;
                default:
                    users = users.OrderBy(client => client.Name.FirstName);
                    break;
            }

            Clients = users.ThenBy(client => client.Name.LastName).ToArray();
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