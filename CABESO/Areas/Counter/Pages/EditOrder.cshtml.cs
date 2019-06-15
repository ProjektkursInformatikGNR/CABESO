using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CABESO.Areas.Counter.Pages
{
    /// <summary>
    /// Die Modellklasse der Razor Page zur Bearbeitung einer Bestellung
    /// </summary>
    [Authorize(Roles = "Admin,Employee")]
    public class EditOrderModel : PageModel
    {
        /// <summary>
        /// Die zur Verfügung stehenden Produkte
        /// </summary>
        public Product[] Products { get; private set; }

        /// <summary>
        /// Die zu bearbeitende Bestellung
        /// </summary>
        public static Order CurrentOrder { get; private set; }

        private readonly ApplicationDbContext _context; //Das Vermittlungsobjekt der Datenbankanbindung

        /// <summary>
        /// Erzeugt ein neues <seealso cref="EditOrderModel"/>.
        /// </summary>
        /// <param name="context">
        /// Der Datenbankkontext per Dependency Injection
        /// </param>
        public EditOrderModel(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, wenn die Weboberfläche angefordert wird.<para>
        /// Er initialisiert die zu bearbeitende Bestellung <seealso cref="CurrentOrder"/> anhand der ID.</para>
        /// </summary>
        /// <param name="id">
        /// Die ID der zu bearbeitenden Bestellung
        /// </param>
        public void OnGet(int id)
        {
            Products = _context.Products.ToArray();
            CurrentOrder = _context.Orders.Find(id);
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, sobald das "POST"-Event auslöst wird (hier durch Betätigung des "Bearbeiten"-Buttons).<para>
        /// Er bearbeitet auf Grundlage der eingegebenen Informationen die gegebenene Bestellung.</para>
        /// </summary>
        /// <returns>
        /// Ein <seealso cref="IActionResult"/>, das bestimmt, wie nach Behandlung des Event vorgegangen werden soll.
        /// </returns>
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                CurrentOrder.Product = _context.Products.Find(Input.ProductId);
                CurrentOrder.Number = Input.Number;
                CurrentOrder.Notes = Input.Notes;
                CurrentOrder.CollectionTime = Input.CollectionTime.ToUniversalTime();
                _context.Orders.Update(CurrentOrder as CurrentOrder);
                _context.SaveChanges();
                return LocalRedirect("~/Counter/Index");
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
            /// Die ID des zu bestellenden Produkts (erforderlich)
            /// </summary>
            [Required]
            [Display(Name = "Gericht")]
            public int ProductId { get; set; }

            /// <summary>
            /// Die Bestellmenge des gegebenen Produkts (erforderlich)
            /// </summary>
            [Required]
            [Display(Name = "Anzahl:")]
            public int Number { get; set; }

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
        }
    }
}