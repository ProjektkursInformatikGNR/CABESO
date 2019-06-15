using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CABESO.Areas.Orders.Pages
{
    [Authorize]
    public class PlaceOrderModel : PageModel
    {
        public Product[] Products { get; private set;}
        public TimeRange[] ValidCollectionTimes { get; private set; }
        public string[] ValidCollectionPlaces { get => new[] { "E-Gebäude", "G-Gebäude" }; }

        private readonly ApplicationDbContext _context; //Das Vermittlungsobjekt der Datenbankanbindung

        public PlaceOrderModel(ApplicationDbContext context)
        {
            _context = context;
            ValidCollectionTimes = CurrentOrder.ValidCollectionTimes();
            Products = new[] { new Product() { Id = -1 } }.Concat(_context.Products.OrderBy(product => product.Name)).ToArray();
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ValidCollectionTimes.Any(tr => tr.Includes(Input.CollectionTime)))
                ModelState.AddModelError(string.Empty, "Zur angegebenen Abholzeit steht die Cafeteria leider nicht zur Verfügung.");
            if (ModelState.IsValid)
            {
                for (int order = 0; order < Input.ProductIds.Length; order++)
                    if (Input.ProductIds[order] > 0)
                        _context.Orders.Add(new CurrentOrder() { User = User.GetIdentityUser(), Product = _context.Products.Find(Input.ProductIds[order]), OrderTime = DateTime.UtcNow, Number = Input.Numbers[order], Notes = Input.Notes, CollectionTime = Input.CollectionTime.ToUniversalTime(), CollectionPlace = Input.CollectionPlace, PreparationTime = null });
                _context.SaveChanges();
                return RedirectToPage();
            }
            return Page();
        }

        /// <summary>
		/// Ein Hilfsobjekt, das die Eingabeinformationen der Weboberfläche zwischenspeichert.
		/// </summary>
		[BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
		/// Eine Datenstruktur zur Zwischenspeicherung der Eingabeinformationen
		/// </summary>
		public class InputModel
        {
            [Display(Name = "Wähle ein Gericht aus:")]
            public int[] ProductIds { get; set; }

            [Display(Name = "Anzahl:")]
            public int[] Numbers { get; set; }

            [Display(Name = "Weitere Anmerkungen:")]
            public string Notes { get; set; }

            [Display(Name = "Gewünschte Abholzeit:")]
            public DateTime CollectionTime { get; set; }

            [Display(Name = "Gewünschter Abholort:")]
            public string CollectionPlace { get; set; }
        }
    }
}