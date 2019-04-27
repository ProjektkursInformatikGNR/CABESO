using CABESO.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CABESO.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(RoleManager<IdentityRole> roleManager)
        {
            foreach (string role in new[] { "Admin", "Teacher", "Student", "Employee" })
                if (!roleManager.RoleExistsAsync(role).Result)
                    while (!roleManager.CreateAsync(new IdentityRole(role)).Result.Succeeded) ;
            new Client();
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("error/404")]
        public IActionResult PageNotFound()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}