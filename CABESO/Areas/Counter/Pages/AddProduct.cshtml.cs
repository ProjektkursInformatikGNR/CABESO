using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CABESO.Areas.Counter.Pages
{
    /// <summary>
    /// Die Modellklasse der Razor Page zum Hinzufügen eines Produkts
    /// </summary>
    [Authorize(Roles = "Admin,Employee")]
    public class AddProductModel : PageModel
    {
        /// <summary>
        /// Die zur Verfügung stehende Allergene
        /// </summary>
        public Allergen[] Allergens { get; private set; }

        private readonly ApplicationDbContext _context; //Das Vermittlungsobjekt der Datenbankanbindung

        /// <summary>
        /// Erzeugt ein neues <see cref="AddProductModel"/>.
        /// </summary>
        /// <param name="context">
        /// Der Datenbankkontext per Dependency Injection
        /// </param>
        public AddProductModel(ApplicationDbContext context)
        {
            _context = context;
            Allergens = _context.Allergens.ToArray();
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, sobald das "POST"-Event auslöst wird (hier durch Betätigung des "Hinzufügen"-Buttons).<para>
        /// Er erstellt auf Grundlage der eingegebenen Informationen ein neues Produkt.</para>
        /// </summary>
        /// <returns>
        /// Ein <see cref="IActionResult"/>, das bestimmt, wie nach Behandlung des Event vorgegangen werden soll.
        /// </returns>
        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                List<Allergen> selectedAllergens = new List<Allergen>();
                for (int i = 0; i < Allergens.Length; i++)
                    if (Input.SelectedAllergens[i].Equals("True", StringComparison.OrdinalIgnoreCase))
                        selectedAllergens.Add(Allergens[i]);
                Product product = new Product()
                {
                    Name = Input.Name,
                    Price = FromInput(Input.Price) ?? 0m,
                    Sale = FromInput(Input.Sale),
                    Vegetarian = Input.Vegetarian,
                    Vegan = Input.Vegan,
                    Size = Input.Size,
                    Deposit = FromInput(Input.Deposit),
                    Information = Input.Information,
                    Allergens = selectedAllergens.ToArray()
                };
                _context.Products.Add(product);
                _context.SaveChanges();
                if (Input.Image != null)
                {
                    product.Image = product.Id + Path.GetExtension(Input.Image.FileName);
                    string filePath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "images", product.Image);
                    if (Input.Image.Length > 0)
                    {
                        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                        using (Stream stream = new FileStream(filePath, FileMode.CreateNew))
                            await Input.Image.CopyToAsync(stream);
                    }
                    _context.Products.Update(product);
                }
                _context.SaveChanges();
                return LocalRedirect("~/Counter/Products");
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
            /// Die Bezeichnung des hinzuzufügenden Produkts (erforderlich)
            /// </summary>
            [Required(AllowEmptyStrings = false)]
            [Display(Name = "Bezeichnung")]
            public string Name { get; set; }

            /// <summary>
            /// Die Abbildung des hinzuzufügenden Produkts (optional)
            /// </summary>
            [Display(Name = "Bild")]
            public IFormFile Image { get; set; }

            /// <summary>
            /// Der Preis des hinzuzufügenden Produkts (erforderlich)
            /// </summary>
            [Required(AllowEmptyStrings = false)]
            [Display(Name = "Preis")]
            public string Price { get; set; }

            /// <summary>
            /// Der Rabatt auf das hinzuzufügende Produkt (optional)
            /// </summary>
            [Display(Name = "Rabatt")]
            public string Sale { get; set; }

            /// <summary>
            /// Der Indikator, ob das hinzuzufügende Produkt vegetarisch ist (erforderlich)
            /// </summary>
            [Required]
            [Display(Name = "Vegetarisch?")]
            public bool Vegetarian { get; set; }

            /// <summary>
            /// Der Indikator, ob das hinzuzufügende Produkt vegan ist (erforderlich)
            /// </summary>
            [Required]
            [Display(Name = "Vegan?")]
            public bool Vegan { get; set; }

            /// <summary>
            /// Die Größe des hinzuzufügenden Produkts (optional)
            /// </summary>
            [Display(Name = "Größe")]
            public string Size { get; set; }

            /// <summary>
            /// Der Pfandbetrag des hinzuzufügenden Produkts (optional)
            /// </summary>
            [Display(Name = "Pfand")]
            public string Deposit { get; set; }

            /// <summary>
            /// Weitere Hinweise zum hinzuzufügenden Produkt (optional)
            /// </summary>
            [Display(Name = "Weitere Hinweise")]
            public string Information { get; set; }

            /// <summary>
            /// Die für das hinzuzufügende Produkt ausgewählten Allergene (erforderlich)
            /// </summary>
            [Required]
            [Display(Name = "Allergene")]
            public string[] SelectedAllergens { get; set; }
        }

        /// <summary>
        /// Konvertiert die Preiseingabe vom <see cref="string"/>-Format ins <see cref="decimal"/>-Format.
        /// </summary>
        /// <param name="number">
        /// Die Benutzereingabe
        /// </param>
        /// <returns>
        /// Der Dezimalwert
        /// </returns>
        public decimal? FromInput(string number)
        {
            bool b = decimal.TryParse(number, NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal d);
            return b ? d : null as decimal?;
        }
    }
}