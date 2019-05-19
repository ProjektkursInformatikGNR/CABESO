using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace CABESO.Areas.Counter.Pages
{
    [Authorize(Roles = "Admin")]
    public class AllergensModel : PageModel
    {
        public Allergen[] Allergens;

        public string Sort { get; set; }

        public string SearchKeyWord { get; set; }

        public AllergensModel(ApplicationDbContext context)
        {
            Allergens = context.Allergens.ToArray();
        }

        public void OnGet(string search, string sortOrder)
        {
            SearchKeyWord = search ?? string.Empty;
            if (!string.IsNullOrEmpty(SearchKeyWord))
                Allergens = Allergens.Search(search, allergen => allergen.ToString()).ToArray();
            Sort = string.IsNullOrEmpty(sortOrder) ? "!" : "";

            IOrderedEnumerable<Allergen> allergens = Allergens.OrderBy(allergen => 0);
            switch (sortOrder)
            {
                case "!":
                    allergens = allergens.OrderByDescending(allergen => allergen.Description);
                    break;
                default:
                    allergens = allergens.OrderBy(allergen => allergen.Description);
                    break;
            }
            Allergens = allergens.ToArray();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string SearchKeyWord { get; set; }
        }

        public IActionResult OnPost()
        {
            return RedirectToAction("Allergens", "Counter", new { sortOrder = string.Empty, search = Input.SearchKeyWord?.Trim() ?? string.Empty });
        }
    }
}