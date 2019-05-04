using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace CABESO.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class GenerateCodesModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Anzahl")]
            public int Number { get; set; }

            [Required]
            [Display(Name = "Rolle")]
            public string Role { get; set; }
        }

        public RegistrationCode[] Codes;
        public string[] Roles;

        public void OnGet()
        {
            Codes = Database.Context.Codes.OrderByDescending(code => code.CreationTime).ToArray();
            Roles = Array.ConvertAll(Database.Context.Roles.Where(role => role.Id != Database.AdminId).ToArray(), role => role.Name);
        }

        public IActionResult OnPost()
        {
            for (int i = 0; i < Input.Number; i++)
                GenerateCode(Input.Role);
            OnGet();
            return Page();
        }

        public void GenerateCode(string role)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const int length = 10;

            Random random = new Random();
            string code = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            Database.Context.Codes.Add(new RegistrationCode() { Code = code, CreationTime = DateTime.Now, Role = role });
            Database.Context.SaveChanges();
        }
    }
}