using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CABESO.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class GenerateCodesModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public RegistrationCode[] Codes { get; private set; }

        private readonly ApplicationDbContext _context;

        public GenerateCodesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public class InputModel
        {
            [Required]
            [Display(Name = "Anzahl")]
            public int Number { get; set; }

            [Required]
            [Display(Name = "Rolle")]
            public string Role { get; set; }
        }

        public void OnGet()
        {
            Codes = _context.Codes.OrderByDescending(code => code.CreationTime).ToArray();
        }

        public IActionResult OnPost()
        {
            for (int i = 0; i < Input.Number; i++)
                GenerateCode(Input.Role);
            return RedirectToPage();
        }

        public void GenerateCode(string role)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const int length = 10;

            Random random = new Random();
            string code = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            _context.Codes.Add(new RegistrationCode() { Code = code, CreationTime = DateTime.UtcNow, Role = _context.Roles.FirstOrDefault(r => r.Name.Equals(role)) });
            _context.SaveChanges();
        }
    }
}