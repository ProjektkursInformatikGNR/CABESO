using CABESO.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CABESO.Areas.Counter.Pages
{
    public class EditProductModel : PageModel
    {
        public static Product CurrentProduct { get; private set; }
        public string[] Allergens { get; private set; }

        private readonly ApplicationDbContext _context;

        public EditProductModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet(int id)
        {
            CurrentProduct = _context.Products.Find(id);
        }

        public async Task<IActionResult> OnPost()
        {
            if (Input.DeleteImage)
            {
                System.IO.File.Delete(Path.Combine(Environment.CurrentDirectory, "wwwroot", "images", CurrentProduct.Image));
                CurrentProduct.Image = string.Empty;
            }
            else if (Input.Image != null)
            {
                CurrentProduct.Image = CurrentProduct.Id + Path.GetExtension(Input.Image.FileName);
                string filePath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "images", CurrentProduct.Image);
                if (Input.Image.Length > 0)
                    using (Stream stream = new FileStream(filePath, FileMode.Create))
                        await Input.Image.CopyToAsync(stream);
            }

            CurrentProduct.Name = Input.Name;
            CurrentProduct.Price = FromInput(Input.Price) ?? 0m;
            CurrentProduct.Sale = FromInput(Input.Sale);
            CurrentProduct.Vegetarian = Input.Vegetarian;
            CurrentProduct.Vegan = Input.Vegan;
            CurrentProduct.Size = Input.Size;
            CurrentProduct.Deposit = FromInput(Input.Deposit);
            CurrentProduct.Allergens = Input.Allergens;
            CurrentProduct.Information = Input.Information;
            _context.Products.Update(CurrentProduct);
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

        public string ToInput(decimal? number) => string.Format(CultureInfo.InvariantCulture, "{0}", number ?? string.Empty as object);
        public decimal? FromInput(string number)
        {
            bool b = decimal.TryParse(number, NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal d);
            return b ? d : null as decimal?;
        }
    }
}