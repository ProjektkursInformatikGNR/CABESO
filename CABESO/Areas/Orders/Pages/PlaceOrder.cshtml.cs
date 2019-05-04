using CABESO.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CABESO.Areas.Orders.Pages
{
    public class PlaceOrderModel : PageModel
    {
        public Product[] Products { get; private set; }

        private readonly ApplicationDbContext _context;

        public PlaceOrderModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Products = _context.Products.ToArray();
        }

        public IActionResult OnPost()
        {
            _context.Orders.Add(new CurrentOrder() { User = User.GetIdentityUser(), Product = _context.Products.Find(Input.ProductId), OrderTime = DateTime.UtcNow, Number = Input.Number, Notes = Input.Notes, CollectionTime = Input.CollectionTime.ToUniversalTime() });
            _context.SaveChanges();
            return RedirectToPage();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Wähle ein Gericht aus:")]
            public int ProductId { get; set; }

            [Display(Name = "Anzahl der Bestellungen:")]
            public int Number { get; set; }

            [Display(Name = "Weitere Anmerkungen:")]
            public string Notes { get; set; }

            [Display(Name = "Für wann soll das Gericht zubereitet werden?")]
            public DateTime CollectionTime { get; set; }
        }
    }
}