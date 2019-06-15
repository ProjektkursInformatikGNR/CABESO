using CABESO.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace CABESO.Areas.Orders.Pages
{
    public class GetProductDataModel : PageModel
    {
        private readonly ApplicationDbContext _context; //Das Vermittlungsobjekt der Datenbankanbindung

        public GetProductDataModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int id)
        {
            Product product = _context.Products.Find(id);
            if (product == null)
                return new NotFoundResult();
            return new JsonResult(new
            {
                image = product.Image ?? "",
                vegetarian = product.Vegetarian ? "✓" : "X",
                vegan = product.Vegan ? "✓" : "X",
                allergens = string.Join(", ", Array.ConvertAll(product.Allergens ?? new Allergen[0], allergen => allergen.Description)),
                information = product.Information ?? "",
                price = product.Price
            });
        }
    }
}