using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace CABESO.Areas.Orders.Pages
{
    /// <summary>
    /// Eine Hilfsklasse zur AJAX-Kommunikation der "PlaceOrder"-Weboberfläche.
    /// </summary>
    [Authorize]
    public class GetProductDataModel : PageModel
    {
        private readonly ApplicationDbContext _context; //Das Vermittlungsobjekt der Datenbankanbindung

        /// <summary>
        /// Erzeugt ein neues <see cref="GetProductDataModel"/>.
        /// </summary>
        /// <param name="context">
        /// Der Datenbankkontext per Dependency Injection
        /// </param>
        public GetProductDataModel(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, sobald eine AJAX-Anfrage gestartet wird.<para>
        /// Er liefert zu einem gegebenen Produkt die jeweiligen Details.</para>
        /// </summary>
        /// <param name="id">
        /// Die ID des gegebenen Produkts
        /// </param>
        /// <returns>
        /// Die Produktdetails im JSON-Format
        /// </returns>
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