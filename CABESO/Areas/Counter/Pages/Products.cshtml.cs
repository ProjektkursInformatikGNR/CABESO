using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CABESO.Areas.Counter.Pages
{
    /// <summary>
    /// Die Modellklasse der Razor Page zur Übersicht der Produkte
    /// </summary>
    [Authorize(Roles = "Admin,Employee")]
    public class ProductsModel : PageModel
    {
        /// <summary>
        /// Das Sortierverhalten der Tabelle in Bezug auf die Produktbezeichnung
        /// </summary>
        public string NameSort { get; set; }

        /// <summary>
        /// Das Sortierverhalten der Tabelle in Bezug auf den Produktpreis
        /// </summary>
        public string PriceSort { get; set; }

        /// <summary>
        /// Das Sortierverhalten der Tabelle in Bezug auf den Rabatt auf das Produkt
        /// </summary>
        public string SaleSort { get; set; }

        /// <summary>
        /// Das Sortierverhalten der Tabelle in Bezug auf die Allergene im Produkt
        /// </summary>
        public string AllergensSort { get; set; }

        /// <summary>
        /// Das Sortierverhalten der Tabelle in Bezug auf die Kompatibilität des Produkts mit Vegetarismus
        /// </summary>
        public string VegetarianSort { get; set; }

        /// <summary>
        /// Das Sortierverhalten der Tabelle in Bezug auf die Kompatibilität des Produkts mit Veganismus
        /// </summary>
        public string VeganSort { get; set; }

        /// <summary>
        /// Das Sortierverhalten der Tabelle in Bezug auf die Größe des Produkts
        /// </summary>
        public string SizeSort { get; set; }

        /// <summary>
        /// Das Sortierverhalten der Tabelle in Bezug auf den Pfandbetrag des Produkts
        /// </summary>
        public string DepositSort { get; set; }

        /// <summary>
        /// Das Sortierverhalten der Tabelle in Bezug auf weitere Anmerkungen zum Produkt
        /// </summary>
        public string InformationSort { get; set; }

        /// <summary>
        /// Zur Verfügung stehende Produkte
        /// </summary>
        public Product[] Products { get; private set; }

        /// <summary>
        /// Die Zuordnung der IDs der Allergene mit ihren Indizes
        /// </summary>
        public Dictionary<int, int> AllergenIndices { get; private set; }

        /// <summary>
        /// Der zu suchende Ausdruck
        /// </summary>
        public string SearchKeyWord { get; set; }

        private readonly ApplicationDbContext _context; //Das Vermittlungsobjekt der Datenbankanbindung

        /// <summary>
        /// Erzeugt ein neues <see cref="ProductsModel"/>.
        /// </summary>
        /// <param name="context">
        /// Der Datenbankkontext per Dependency Injection
        /// </param>
        public ProductsModel(ApplicationDbContext context)
        {
            _context = context;
            Products = _context.Products.ToArray();
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, wenn die Weboberfläche angefordert wird.<para>
        /// Er konfiguriert gegebenenfalls die Tabelle gemäß der Suchanfrage.</para>
        /// </summary>
        /// <param name="search">
        /// Der zu suchende Ausdruck
        /// </param>
        /// <param name="sortOrder">
        /// Das Sortierverhalten
        /// </param>
        public void OnGet(string sortOrder, string search)
        {
            SearchKeyWord = search ?? string.Empty;
            if (!string.IsNullOrEmpty(SearchKeyWord))
                Products = Products.Where(product => product.Name.Contains(SearchKeyWord, StringComparison.OrdinalIgnoreCase)).ToArray();

            NameSort = string.IsNullOrEmpty(sortOrder) ? "!n" : "";
            PriceSort = sortOrder == "p" ? "!p" : "p";
            SaleSort = sortOrder == "sa" ? "!sa" : "sa";
            AllergensSort = sortOrder == "a" ? "!a" : "a";
            VegetarianSort = sortOrder == "v" ? "!v" : "v";
            VeganSort = sortOrder == "vv" ? "!vv" : "vv";
            SizeSort = sortOrder == "si" ? "!si" : "si";
            DepositSort = sortOrder == "d" ? "!d" : "d";
            InformationSort = sortOrder == "i" ? "!i" : "i";

            IOrderedEnumerable<Product> products = Products.OrderBy(product => 0);

            switch (sortOrder)
            {
                case "!n":
                    products = products.OrderByDescending(product => product.Name);
                    break;
                case "p":
                    products = products.OrderBy(product => product.Price);
                    break;
                case "!p":
                    products = products.OrderByDescending(product => product.Price).ThenBy(product => product.Name);
                    break;
                case "sa":
                    products = products.OrderBy(product => product.Sale);
                    break;
                case "!sa":
                    products = products.OrderByDescending(product => product.Sale);
                    break;
                case "a":
                    products = products.OrderBy(product => product.Allergens.FirstOrDefault());
                    break;
                case "!a":
                    products = products.OrderByDescending(product => product.Allergens.FirstOrDefault());
                    break;
                case "v":
                    products = products.OrderBy(product => !product.Vegetarian);
                    break;
                case "!v":
                    products = products.OrderByDescending(product => !product.Vegetarian);
                    break;
                case "vv":
                    products = products.OrderBy(product => !product.Vegan);
                    break;
                case "!vv":
                    products = products.OrderByDescending(product => !product.Vegan);
                    break;
                case "si":
                    products = products.OrderBy(product => product.Size);
                    break;
                case "!si":
                    products = products.OrderByDescending(product => product.Size);
                    break;
                case "d":
                    products = products.OrderBy(product => product.Deposit);
                    break;
                case "!d":
                    products = products.OrderByDescending(product => product.Deposit);
                    break;
                case "i":
                    products = products.OrderBy(product => product.Information);
                    break;
                case "!i":
                    products = products.OrderByDescending(product => product.Information);
                    break;
                default:
                    products = products.OrderBy(product => product.Name);
                    break;
            }
            Products = products.ThenBy(product => product.Name).ToArray();

            AllergenIndices = new Dictionary<int, int>();
            int i = 0;
            foreach (Allergen allergen in _context.Allergens)
                AllergenIndices.Add(allergen.Id, ++i);
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
            /// Der zu suchende Ausdruck (optional)
            /// </summary>
            public string SearchKeyWord { get; set; }
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, sobald das "POST"-Event auslöst wird (hier durch Betätigung des "Suchen"-Buttons).<para>
        /// Er lädt die Weboberfläche neu, sodass der Suchanfrage Rechnung getragen werden kann.</para>
        /// </summary>
        /// <returns>
        /// Die Anweisung zum Neuladen der Oberfläche
        /// </returns>
        public IActionResult OnPost()
        {
            return RedirectToAction("Products", "Counter", new { sortOrder = string.Empty, search = Input.SearchKeyWord?.Trim() ?? string.Empty });
        }
    }
}