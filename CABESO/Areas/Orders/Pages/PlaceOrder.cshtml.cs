using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;

namespace CABESO.Areas.Orders.Pages
{
    public class PlaceOrderModel : PageModel
    {

        private readonly UserManager<IdentityUser> _userManager;

        public PlaceOrderModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult OnPost()
        {
            Database.Add("Orders", new { ClientId = Client.Create(_userManager, User).Id, Input.ProductId, OrderTime = Database.SqlNow, Input.Number, Input.Notes, CollectionTime = Database.SqlTimeFormat(Input.CollectionTime) });
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