using CABESO.Data;
using CABESO.Properties;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CABESO.Areas.Counter.Pages
{
    public class EditOrderModel : PageModel
    {
        public Product[] Products { get; private set; }
        public static Order CurrentOrder { get; private set; }
        public bool StuckAsAdmin { get; private set; }
        public Form[] Forms { get; private set; }

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public EditOrderModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task OnGet(string id)
        {
            Products = _context.Products.ToArray();
            //CurrentOrder = _context.Orders.Find(id);
            StuckAsAdmin = (await _userManager.GetUsersInRoleAsync(Resources.Admin)).Count <= 1;
            Forms = _context.Forms.OrderBy(form => form.ToString()).ToArray();
        }

        public async Task<IActionResult> OnPost()
        {
            //CurrentOrder.Product = _context.Orders.Find();
            CurrentOrder.Number = Input.Number;
            CurrentOrder.Notes = Input.Notes;
            CurrentOrder.OrderTime = Input.CollectionTime;
            _context.Orders.Update(CurrentOrder as CurrentOrder);
            _context.SaveChanges();
            return LocalRedirect("~/Counter/Orders");
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Gericht")]
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