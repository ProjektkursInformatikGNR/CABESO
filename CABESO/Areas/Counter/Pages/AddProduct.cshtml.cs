using CABESO.Data;
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
    public class AddProductModel : PageModel
    {
        public Allergen[] Allergens { get; private set; }

        private readonly ApplicationDbContext _context;

        public AddProductModel(ApplicationDbContext context)
        {
            _context = context;
            Allergens = _context.Allergens.ToArray();
        }

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

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(AllowEmptyStrings = false)]
            [Display(Name = "Bezeichnung")]
            public string Name { get; set; }

            [Display(Name = "Bild")]
            public IFormFile Image { get; set; }

            [Required(AllowEmptyStrings = false)]
            [Display(Name = "Preis")]
            public string Price { get; set; }

            [Display(Name = "Rabatt")]
            public string Sale { get; set; }

            [Required]
            [Display(Name = "Vegetarisch?")]
            public bool Vegetarian { get; set; }

            [Required]
            [Display(Name = "Vegan?")]
            public bool Vegan { get; set; }

            [Display(Name = "Größe")]
            public string Size { get; set; }

            [Display(Name = "Pfand")]
            public string Deposit { get; set; }

            [Display(Name = "Weitere Hinweise")]
            public string Information { get; set; }

            public bool DeleteImage { get; set; }

            [Display(Name = "Allergene")]
            public string[] SelectedAllergens { get; set; }
        }

        public string ToInput(decimal? number) => string.Format(CultureInfo.InvariantCulture, "{0}", number ?? string.Empty as object);
        public decimal? FromInput(string number)
        {
            bool b = decimal.TryParse(number, NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal d);
            return b ? d : null as decimal?;
        }
    }
}