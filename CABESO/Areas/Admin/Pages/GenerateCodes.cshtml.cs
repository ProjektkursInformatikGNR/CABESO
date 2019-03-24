using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
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
        }

        public Tuple<string, string>[] Codes;

        public void OnGet()
        {
            object[][] data = Database.SqlQuery("Codes", null, "Code", "CreationTime").OrderByDescending(d => d[1]).ToArray();
            Codes = Array.ConvertAll(data, d => new Tuple<string, string>(d[0].ToString(), DateTime.Parse(d[1].ToString()).ToLocalTime().ToString(CultureInfo.CurrentCulture)));
        }

        public IActionResult OnPost()
        {
            for (int i = 0; i < Input.Number; i++)
                GenerateCode();
            OnGet();
            return Page();
        }

        public void GenerateCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const int length = 10;

            Random random = new Random();
            string code = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            Database.SqlExecute($"INSERT INTO [dbo].[Codes] VALUES ('{code}', '{DateTime.UtcNow.ToString("yyyyMMdd hh:mm:ss tt", CultureInfo.InvariantCulture)}');");
        }
    }
}