using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CABESO.Data;
using CABESO.Properties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CABESO.Areas.Counter.Pages
{
    public class ImageUploadModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Product[] Products { get; private set; }

        public ImageUploadModel(ApplicationDbContext context)
        {
            _context = context;
            Products = _context.Products.ToArray();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public IFormFile Image { get; set; }

            [Required]
            public int ProductId { get; set; }
        }

        public async Task<IActionResult> OnPost()
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "images", Input.ProductId + Path.GetExtension(Input.Image.FileName));
            if (Input.Image.Length > 0)
            {
                using (Stream stream = new FileStream(filePath, FileMode.Create))
                    await Input.Image.CopyToAsync(stream);
            }
            Product product = _context.Products.Find(Input.ProductId);
            product.Picture = Input.ProductId + Path.GetExtension(Input.Image.FileName);
            _context.Update(product);
            _context.SaveChanges();
            return Page();
        }
    }
}