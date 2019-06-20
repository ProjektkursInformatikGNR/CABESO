using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CABESO.Views.Orders
{
    /// <summary>
    /// Der Controller der Area "Orders" ist zuständig für die Ausführung der Actions.
    /// </summary>
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context; //Das Vermittlungsobjekt der Datenbankanbindung

        /// <summary>
        /// Erzeugt einen neuen <see cref="OrdersController"/>.
        /// </summary>
        /// <param name="context">
        /// Der Datenbankkontext
        /// </param>
        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Die Weiterleitung zur Razor Page <see cref="Areas.Orders.Pages.Areas_Orders_Pages_Index"/>
        /// </summary>
        /// <returns>
        /// Die Anweisung der Weiterleitung
        /// </returns>
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Die Weiterleitung zur Razor Page <see cref="Areas.Orders.Pages.Areas_Orders_Pages_PlaceOrder"/>
        /// </summary>
        /// <returns>
        /// Die Anweisung der Weiterleitung
        /// </returns>
        [Authorize]
        public IActionResult PlaceOrder()
        {
            return View();
        }

        /// <summary>
        /// Die Weiterleitung zur Razor Page <see cref="Areas.Orders.Pages.Areas_Orders_Pages_OrderHistory"/>
        /// </summary>
        /// <returns>
        /// Die Anweisung der Weiterleitung
        /// </returns>
        [Authorize]
        public IActionResult OrderHistory()
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
        [Authorize]
        public IActionResult RemoveOrder(int id)
        {
            CurrentOrder order = _context.Orders.Find(id);
            if (order != null && order.User.Id.Equals(User.GetIdentityUser().Id))
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
            return LocalRedirect("~/Orders/Index");
        }
    }
}