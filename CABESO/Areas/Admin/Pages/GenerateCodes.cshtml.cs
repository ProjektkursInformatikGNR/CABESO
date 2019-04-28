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

        public Tuple<string, string, string>[] Codes;
        public string[] Roles;

        public void OnGet()
        {
            object[][] data = Database.Select("Codes", null, "Code", "CreationTime", "Role").OrderByDescending(d => d[1]).ToArray();
            Codes = Array.ConvertAll(data, d => new Tuple<string, string, string>(d[0].ToString(), DateTime.Parse(d[1].ToString()).ToLocalTime().ToString(CultureInfo.CurrentCulture), string.IsNullOrEmpty(d[2].ToString()) ? string.Empty : Program.Translations.GetValueOrDefault(d[2].ToString())));
            Roles = Array.ConvertAll(Database.Select("AspNetRoles", "[Name]<>'Admin'", "Name"), r => r[0].ToString());
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
            Database.Add("Codes", new { Code = code, CreationTime = Database.SqlNow, Role = role });
        }
    }
}