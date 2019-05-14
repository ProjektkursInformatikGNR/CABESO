using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CABESO.Views.Kitchen
{
    public class KitchenController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KitchenController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Employee,Admin")]
        public IActionResult Index()
        {
            return View();
        }

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
            return LocalRedirect("~/Kitchen/Index");
        }
    }
}