using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace CABESO.Views.Counter
{
    public class CounterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CounterController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Employee,Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Employee,Admin")]
        public IActionResult Orders()
        {
            return View();
        }

        [Authorize(Roles = "Employee,Admin")]
        public IActionResult Products()
        {
            return View();
        }
    }
}