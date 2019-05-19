﻿using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CABESO.Areas.Orders.Pages
{
    [Authorize]
    public class PlaceOrderModel : PageModel
    {
        public Product[] Products { get; private set;}
        public Product SelectedProduct { get; private set; }
        public string Vegan { get; private set; }
        public string Vegetarian { get; private set; }
        public string Information { get; private set; }

        private readonly ApplicationDbContext _context;

        public PlaceOrderModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Products = _context.Products.OrderBy(product => product.Name).ToArray();
            UpdateTable();
        }

        public IActionResult OnPost()
        {
            _context.Orders.Add(new CurrentOrder() { User = User.GetIdentityUser(), Product = _context.Products.Find(Input.ProductId), OrderTime = DateTime.UtcNow, Number = Input.Number, Notes = Input.Notes, CollectionTime = Input.CollectionTime.ToUniversalTime(), PreparationTime = null });
            _context.SaveChanges();
            return RedirectToPage();
        }

        public void UpdateTable()
        {
            if (SelectedProduct == null)
                SelectedProduct = _context.Products.Find(20);
            else
                SelectedProduct = _context.Products.Find(Input.ProductId);

            if (SelectedProduct.Vegan)
            {
                Vegan = "Vegan: ✓";
                Vegetarian = "Vegetarisch: ✓";
            }
            else if (SelectedProduct.Vegetarian)
            {
                Vegan = "Vegan: X";
                Vegetarian = "Vegetarisch: ✓";
            }
            else
            {
                Vegan = "Vegan: X";
                Vegetarian = "Vegetarisch: X";
            }
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

            [Display(Name = "Gewünschte Abholzeit:")]
            public DateTime CollectionTime { get; set; }
        }
    }
}