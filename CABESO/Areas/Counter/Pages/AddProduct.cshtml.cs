using CABESO.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace CABESO.Areas.Counter.Pages
{
    public class AddProductModel : PageModel
    {
        public string[] Allergens { get; private set; }

        private readonly ApplicationDbContext _context;

        public AddProductModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnPost()
        {
            Product product = new Product
            {
                Name = Input.Name,
                Price = FromInput(Input.Price) ?? 0m,
                Sale = FromInput(Input.Sale),
                Vegetarian = Input.Vegetarian,
                Vegan = Input.Vegan,
                Size = Input.Size,
                Deposit = FromInput(Input.Deposit),
                Allergens = Input.Allergens,
                Information = Input.Information
            };
            EntityEntry entry = _context.Products.Add(product);
            if (Input.Image != null)
            {
                product.Image = entry.Property("Id") + Path.GetExtension(Input.Image.FileName);
                string filePath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "images", product.Image);
                if (Input.Image.Length > 0)
                    using (Stream stream = new FileStream(filePath, FileMode.Create))
                        await Input.Image.CopyToAsync(stream);
            }
            _context.SaveChanges();
            return LocalRedirect("~/Counter/Products");
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

            [Required]
            [Display(Name = "Allergene")]
            public string[] Allergens { get; set; }

            [Display(Name = "Weitere Hinweise")]
            public string Information { get; set; }

            public bool DeleteImage { get; set; }
        }

        public decimal? FromInput(string number)
        {
            bool b = decimal.TryParse(number, NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal d);
            return b ? d : null as decimal?;
        }
    }
}