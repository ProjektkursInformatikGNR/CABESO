using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;

namespace CABESO.Areas.Orders.Pages
{
    public class PlaceOrderModel : PageModel
    {
        public IActionResult OnPost()
        {
            Database.Context.Orders.Add(new CurrentOrder() { User = User.GetIdentityUser(), Product = Product.GetProductById(Input.ProductId), OrderTime = DateTime.Now, Number = Input.Number, Notes = Input.Notes, CollectionTime = Input.CollectionTime });
            Database.Context.SaveChanges();
            return Page();
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