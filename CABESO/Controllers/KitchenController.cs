using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CABESO.Controllers
{
    /// <summary>
    /// Der Controller der Area "Kitchen" ist zuständig für die Ausführung der Actions.
    /// </summary>
    public class KitchenController : Controller
    {
        private readonly ApplicationDbContext _context; //Das Vermittlungsobjekt der Datenbankanbindung

        /// <summary>
        /// Erzeugt einen neuen <see cref="KitchenController"/>.
        /// </summary>
        /// <param name="context">
        /// Der Datenbankkontext
        /// </param>
        public KitchenController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Die Weiterleitung zur Razor Page "Index"
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
        /// Entfernt die gegebene <see cref="Order"/> aus der Datenbank.
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
            return LocalRedirect("~/Kitchen/Index");
        }

        /// <summary>
        /// Kennzeichnet eine <see cref="Order"/> als zubereitet, sodass diese nicht länger in der Tabelle angezeigt wird.
        /// </summary>
        /// <param name="id">
        /// Die ID der zubereiteten Bestellung
        /// </param>
        /// <returns>
        /// Die Anweisung zur Rückkehr zur Bestellungsübersicht
        /// </returns>
        [Authorize(Roles = "Employee,Admin")]
        public IActionResult OrderPrepared(int id)
        {
            if (_context.Orders.Find(id) is CurrentOrder order)
            {
                order.PreparationTime = DateTime.UtcNow;
                _context.SaveChanges();
            }
            return LocalRedirect("~/Kitchen/Index");
        }
    }
}