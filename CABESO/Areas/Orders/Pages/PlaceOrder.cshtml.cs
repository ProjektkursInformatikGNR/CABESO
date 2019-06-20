using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CABESO.Areas.Orders.Pages
{
    /// <summary>
    /// Die Modellklasse der Razor Page zum Aufgeben einer Bestellung
    /// </summary>
    [Authorize]
    public class PlaceOrderModel : PageModel
    {
        /// <summary>
        /// Die zur Verfügung stehenden Produkte
        /// </summary>
        public Product[] Products { get; private set;}

        /// <summary>
        /// Die zur Verfügung stehenden Zeiträume zur Abholung
        /// </summary>
        public TimeRange[] ValidCollectionTimes { get; private set; }

        /// <summary>
        /// Die möglichen Abholorte (E-Gebäude und G-Gebäude)
        /// </summary>
        public string[] ValidCollectionPlaces { get => new[] { "E-Gebäude", "G-Gebäude" }; }

        private readonly ApplicationDbContext _context; //Das Vermittlungsobjekt der Datenbankanbindung

        /// <summary>
        /// Erzeugt ein neues <see cref="PlaceOrderModel"/>.
        /// </summary>
        /// <param name="context">
        /// Der Datenbankkontext per Dependency Injection
        /// </param>
        public PlaceOrderModel(ApplicationDbContext context)
        {
            _context = context;
            ValidCollectionTimes = CurrentOrder.ValidCollectionTimes();
            Products = new[] { new Product() { Id = -1 } }.Concat(_context.Products.OrderBy(product => product.Name)).ToArray();
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, sobald das "POST"-Event auslöst wird (hier durch Betätigung des "Bestellen"-Buttons).<para>
        /// Er gibt die Bestellungen auf Grundlage der eingegebenen Produktauswahlen auf.</para>
        /// </summary>
        /// <returns>
        /// Ein <see cref="IActionResult"/>, das bestimmt, wie nach Behandlung des Event vorgegangen werden soll.
        /// </returns>
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
            /// <summary>
            /// Die IDs der zu bestellenden Produkte (erforderlich)
            /// </summary>
            [Required]
            [Display(Name = "Wähle ein Gericht aus:")]
            public int[] ProductIds { get; set; }

            /// <summary>
            /// Die jeweilige Menge der Bestellungen (erforderlich)
            /// </summary>
            [Required]
            [Display(Name = "Anzahl:")]
            public int[] Numbers { get; set; }

            /// <summary>
            /// Weitere Anmerkungen zur Bestellung (optional)
            /// </summary>
            [Display(Name = "Weitere Anmerkungen:")]
            public string Notes { get; set; }

            /// <summary>
            /// Die gewünschte Zeit der Abholung der Bestellung (erforderlich)
            /// </summary>
            [Required]
            [Display(Name = "Gewünschte Abholzeit:")]
            public DateTime CollectionTime { get; set; }

            /// <summary>
            /// Der gewünschte Ort der Abholung der Bestellung (erforderlich)
            /// </summary>
            [Required]
            [Display(Name = "Gewünschter Abholort:")]
            public string CollectionPlace { get; set; }
        }
    }
}