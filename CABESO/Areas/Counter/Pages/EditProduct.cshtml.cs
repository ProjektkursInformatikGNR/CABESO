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
    /// Die Modellklasse der Razor Page zur Bearbeitung eines Produkts
    /// </summary>
    [Authorize(Roles = "Admin,Employee")]
    public class EditProductModel : PageModel
    {
        /// <summary>
        /// Das zu bearbeitende Produkt
        /// </summary>
        public static Product CurrentProduct { get; private set; }

        /// <summary>
        /// Die zur Verfügung stehenden Allergene
        /// </summary>
        public Allergen[] Allergens { get; private set; }

        private readonly ApplicationDbContext _context; //Das Vermittlungsobjekt der Datenbankanbindung

        /// <summary>
        /// Erzeugt ein neues <see cref="EditProductModel"/>.
        /// </summary>
        /// <param name="context">
        /// Der Datenbankkontext per Dependency Injection
        /// </param>
        public EditProductModel(ApplicationDbContext context)
        {
            _context = context;
            Allergens = _context.Allergens.ToArray();
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, wenn die Weboberfläche angefordert wird.<para>
        /// Er initialisiert das zu bearbeitende Produkt <see cref="CurrentProduct"/> anhand der ID.</para>
        /// </summary>
        /// <param name="id">
        /// Die ID des zu bearbeitenden Produkts
        /// </param>
        public void OnGet(int id)
        {
            CurrentProduct = _context.Products.Find(id);
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, sobald das "POST"-Event auslöst wird (hier durch Betätigung des "Bearbeiten"-Buttons).<para>
        /// Er bearbeitet auf Grundlage der eingegebenen Informationen das gegebenene Produkt.</para>
        /// </summary>
        /// <returns>
        /// Ein <see cref="IActionResult"/>, das bestimmt, wie nach Behandlung des Event vorgegangen werden soll.
        /// </returns>
        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                if (Input.RemoveImage)
                {
                    System.IO.File.Delete(Path.Combine(Environment.CurrentDirectory, "wwwroot", "images", CurrentProduct.Image));
                    CurrentProduct.Image = string.Empty;
                }
                else if (Input.Image != null)
                {
                    CurrentProduct.Image = CurrentProduct.Id + Path.GetExtension(Input.Image.FileName);
                    string filePath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "images", CurrentProduct.Image);
                    if (Input.Image.Length > 0)
                    {
                        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                        using (Stream stream = new FileStream(filePath, FileMode.Create))
                            await Input.Image.CopyToAsync(stream);
                    }
                }

                CurrentProduct.Name = Input.Name;
                CurrentProduct.Price = FromInput(Input.Price) ?? 0m;
                CurrentProduct.Sale = FromInput(Input.Sale);
                CurrentProduct.Vegetarian = Input.Vegetarian;
                CurrentProduct.Vegan = Input.Vegan;
                CurrentProduct.Size = Input.Size;
                CurrentProduct.Deposit = FromInput(Input.Deposit);
                List<Allergen> selectedAllergens = new List<Allergen>();
                for (int i = 0; i < Allergens.Length; i++)
                    if (Input.SelectedAllergens[i].Equals("True", StringComparison.OrdinalIgnoreCase))
                        selectedAllergens.Add(Allergens[i]);
                CurrentProduct.Allergens = selectedAllergens.ToArray();
                CurrentProduct.Information = Input.Information;
                _context.Products.Update(CurrentProduct);
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
            /// Die Bezeichnung des zu bearbeitenden Produkts (erforderlich)
            /// </summary>
            [Required(AllowEmptyStrings = false)]
            [Display(Name = "Bezeichnung")]
            public string Name { get; set; }

            /// <summary>
            /// Die Abbildung des zu bearbeitenden Produkts (optional)
            /// </summary>
            [Display(Name = "Bild")]
            public IFormFile Image { get; set; }

            /// <summary>
            /// Der Preis des zu bearbeitenden Produkts (erforderlich)
            /// </summary>
            [Required(AllowEmptyStrings = false)]
            [Display(Name = "Preis")]
            public string Price { get; set; }

            /// <summary>
            /// Der Rabatt auf das zu bearbeitende Produkt (optional)
            /// </summary>
            [Display(Name = "Rabatt")]
            public string Sale { get; set; }

            /// <summary>
            /// Der Indikator, ob das zu bearbeitende Produkt vegetarisch ist (erforderlich)
            /// </summary>
            [Required]
            [Display(Name = "Vegetarisch?")]
            public bool Vegetarian { get; set; }

            /// <summary>
            /// Der Indikator, ob das zu bearbeitende Produkt vegan ist (erforderlich)
            /// </summary>
            [Required]
            [Display(Name = "Vegan?")]
            public bool Vegan { get; set; }

            /// <summary>
            /// Die Größe des zu bearbeitenden Produkts (optional)
            /// </summary>
            [Display(Name = "Größe")]
            public string Size { get; set; }

            /// <summary>
            /// Der Pfandbetrag des zu bearbeitenden Produkts (optional)
            /// </summary>
            [Display(Name = "Pfand")]
            public string Deposit { get; set; }

            /// <summary>
            /// Weitere Hinweise zum zu bearbeitenden Produkt (optional)
            /// </summary>
            [Display(Name = "Weitere Hinweise")]
            public string Information { get; set; }

            /// <summary>
            /// (versteckter) Indikator, ob die Abbildung des zu bearbeitenden Produkts entfernt werden soll
            /// </summary>
            public bool RemoveImage { get; set; }

            /// <summary>
            /// Die für das zu bearbeitende Produkt ausgewählten Allergene (erforderlich)
            /// </summary>
            [Required]
            [Display(Name = "Allergene")]
            public string[] SelectedAllergens { get; set; }
        }

        /// <summary>
        /// Konvertiert den Preisbetrag vom <see cref="decimal"/>-Format ins HTML-Format als <see cref="string"/>.
        /// </summary>
        /// <param name="number">
        /// Der Preis als Dezimalwert
        /// </param>
        /// <returns>
        /// Der HTML-Input
        /// </returns>
        public string ToInput(decimal? number) => string.Format(CultureInfo.InvariantCulture, "{0}", number ?? string.Empty as object);

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