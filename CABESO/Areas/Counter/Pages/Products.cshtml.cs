using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CABESO.Areas.Counter.Pages
{
    [Authorize(Roles = "Admin,Employee")]
    public class ProductsModel : PageModel
    {
        public string NameSort { get; set; }
        public string PriceSort { get; set; }
        public Product[] Products { get; set; }

        public string SearchKeyWord { get; set; }

        public ProductsModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            Products = EnumerateProducts();
        }

        private Product[] EnumerateProducts()
        {
            return Array.ConvertAll(Database.SqlQuery("Products", null, "Name", "Price"), p => new Product() { Name = p[0].ToString(), Price = (decimal)p[1] });
        }

        public void OnGet(string sortOrder, string search)
        {
            SearchKeyWord = search ?? string.Empty;
            if (!string.IsNullOrEmpty(SearchKeyWord))
                Products = Products.Where(product => product.Name.Contains(SearchKeyWord, StringComparison.OrdinalIgnoreCase)).ToArray();

            NameSort = string.IsNullOrEmpty(sortOrder) ? "!n" : "";
            PriceSort = sortOrder == "p" ? "!p" : "p";

            IEnumerable<Product> products = Products;

            switch (sortOrder)
            {
                case "!n":
                    products = products.OrderByDescending(product => product.Name);
                    break;
                case "p":
                    products = products.OrderBy(product => product.Price);
                    break;
                case "!p":
                    products = products.OrderByDescending(product => product.Price);
                    break;
                default:
                    products = products.OrderBy(product => product.Name);
                    break;
            }

            Products = products.ToArray();
        }

        public class Product
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string SearchKeyWord { get; set; }
        }

        public IActionResult OnPost()
        {
            return RedirectToAction("Products", "Counter", new { sortOrder = string.Empty, search = Input.SearchKeyWord?.Trim() ?? string.Empty });
        }
    }
}