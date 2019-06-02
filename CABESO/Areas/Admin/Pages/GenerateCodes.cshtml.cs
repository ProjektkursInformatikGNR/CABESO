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
        public GenerateInputModel GenerateInput { get; set; }

        [BindProperty]
        public DeactivateInputModel DeactivateInput { get; set; }

        public RegistrationCode[] Codes { get; private set; }

        private readonly ApplicationDbContext _context;

        public GenerateCodesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public class GenerateInputModel
        {
            [Required]
            [Display(Name = "Anzahl")]
            public int Number { get; set; }

            [Required]
            [Display(Name = "Rolle")]
            public string Role { get; set; }
        }

        public class DeactivateInputModel
        {
            [Required]
            [Display(Name = "Altersgrenze")]
            public DateTime Limit { get; set; }
        }

        public void OnGet()
        {
            Codes = _context.Codes.OrderByDescending(code => code.CreationTime).ToArray();
        }
    }
}