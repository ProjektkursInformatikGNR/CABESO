using CABESO.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CABESO.Areas.Counter.Pages
{
    public class EditOrderModel : PageModel
    {
        public Product[] Products { get; private set; }
        public static Order CurrentOrder { get; private set; }

        private readonly ApplicationDbContext _context;

        public EditOrderModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet(int id)
        {
            Products = _context.Products.ToArray();
            CurrentOrder = _context.Orders.Find(id);
        }

        public IActionResult OnPost()
        {
            CurrentOrder.Product = _context.Products.Find(Input.ProductId);
            CurrentOrder.Number = Input.Number;
            CurrentOrder.Notes = Input.Notes;
            CurrentOrder.CollectionTime = Input.CollectionTime.ToUniversalTime();
            _context.Orders.Update(CurrentOrder as CurrentOrder);
            _context.SaveChanges();
            return LocalRedirect("~/Counter/Index");
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Gericht")]
            public int ProductId { get; set; }

            [Required]
            [Display(Name = "Anzahl der Bestellungen:")]
            public int Number { get; set; }

            [Display(Name = "Weitere Anmerkungen:")]
            public string Notes { get; set; }

            [Required]
            [Display(Name = "Gewünschte Abholzeit:")]
            public DateTime CollectionTime { get; set; }
        }
    }
}