using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CABESO.Areas.Counter.Pages
{
    [Authorize(Roles = "Admin,Employee")]
    public class EditAllergenModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public static Allergen CurrentAllergen;

        public EditAllergenModel(ApplicationDbContext context)
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

        public void OnGet(int id)
        {
            CurrentAllergen = _context.Allergens.Find(id);
        }

        public IActionResult OnPost()
        {
            CurrentAllergen.Description = Input.Description;
            _context.Allergens.Update(CurrentAllergen);
            _context.SaveChanges();
            return LocalRedirect("~/Counter/Allergens");
        }
    }
}