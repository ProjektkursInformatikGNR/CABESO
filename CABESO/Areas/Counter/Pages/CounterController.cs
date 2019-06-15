using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CABESO.Views.Counter
{
    /// <summary>
    /// Der Controller der Area "Counter" ist zuständig für die Ausführung der Actions.
    /// </summary>
    public class CounterController : Controller
    {
        private readonly ApplicationDbContext _context; //Das Vermittlungsobjekt der Datenbankanbindung

        /// <summary>
        /// Erzeugt einen neuen <seealso cref="CounterController"/>.
        /// </summary>
        /// <param name="context">
        /// Der Datenbankkontext
        /// </param>
        public CounterController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Die Weiterleitung zur Razor Page <seealso cref="Areas.Counter.Pages.Areas_Counter_Pages_Index"/>
        /// </summary>
        /// <returns>
        /// Die Anweisung der Weiterleitung
        /// </returns>
        [Authorize(Roles = "Employee,Admin")]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Die Weiterleitung zur Razor Page <seealso cref="Areas.Counter.Pages.Areas_Counter_Pages_Products"/>
        /// </summary>
        /// <returns>
        /// Die Anweisung der Weiterleitung
        /// </returns>
        [Authorize(Roles = "Employee,Admin")]
        public IActionResult Products()
        {
            return View();
        }

        /// <summary>
        /// Entfernt das gegebene <seealso cref="Product"/> aus der Datenbank.
        /// </summary>
        /// <param name="id">
        /// Die ID des zu entfernenden Produkts
        /// </param>
        /// <returns>
        /// Die Anweisung zur Rückkehr zur Produktübersicht
        /// </returns>
        [Authorize(Roles = "Employee,Admin")]
        public IActionResult RemoveProduct(int id)
        {
            Product product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return LocalRedirect("~/Counter/Products");
        }

        /// <summary>
        /// Entfernt die gegebene <seealso cref="Order"/> aus der Datenbank.
        /// </summary>
        /// <param name="id">
        /// Die ID der zu entfernenden Bestellung
        /// </param>
        /// <returns>
        /// Die Anweisung zur Rückkehr zur Bestellungsübersicht
        /// </returns>
        [Authorize(Roles = "Employee,Admin")]
        public IActionResult RemoveOrder(int id)
        {
            CurrentOrder order = _context.Orders.Find(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
            return LocalRedirect("~/Counter/Index");
        }

        /// <summary>
        /// Archiviert eine Bestellung, indem diese aus der Tabelle "Orders" in "OrderHistory" verschoben wird.
        /// </summary>
        /// <param name="id">
        /// Die ID der zu archivierenden Bestellung
        /// </param>
        /// <returns>
        /// Die Anweisung zur Rückkehr zur Bestellungsübersicht
        /// </returns>
        [Authorize(Roles = "Employee,Admin")]
        public IActionResult ArchiveOrder(int id)
        {
            if (_context.Orders.Find(id) is CurrentOrder order)
            {
                order.CollectionTime = DateTime.UtcNow;
                _context.HistoricOrders.Add(order.ToHistoricOrder());
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
            return LocalRedirect("~/Counter/Index");
        }

        /// <summary>
        /// Entfernt ein gegebenes <seealso cref="Allergen"/> aus der Datenbank.
        /// </summary>
        /// <param name="id">
        /// Die ID des zu entfernenden Allergens
        /// </param>
        /// <returns>
        /// Die Anweisung zur Rückkehr zur Allergenübersicht
        /// </returns>
        [Authorize(Roles = "Employee,Admin")]
        public IActionResult RemoveAllergen(int id)
        {
            Allergen allergen = _context.Allergens.Find(id);
            if (allergen != null)
            {
                _context.Allergens.Remove(allergen);
                _context.SaveChanges();
            }
            return LocalRedirect("~/Counter/Allergens");
        }
    }
}