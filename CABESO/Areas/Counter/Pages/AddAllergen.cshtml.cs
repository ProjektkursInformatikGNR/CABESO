using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CABESO.Areas.Counter.Pages
{
    [Authorize(Roles = "Admin,Employee")]
    public class AddAllergenModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddAllergenModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Gib bitte die Beschreibung an.")]
            [Display(Name = "Beschreibung")]
            public string Description { get; set; }
        }

        public IActionResult OnPost()
        {
            Allergen allergen = new Allergen()
            {
                Description = Input.Description
            };
            _context.Allergens.Add(allergen);
            _context.SaveChanges();
            return RedirectToPage();
        }
    }
}