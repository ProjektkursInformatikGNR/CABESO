using CABESO.Data;
using CABESO.Models;
using CABESO.Properties;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CABESO.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(RoleManager<IdentityRole> roleManager, ApplicationDbContext context, SignInManager<IdentityUser> signInManager)
        {
            foreach (string role in new[] { Resources.Admin, Resources.Teacher, Resources.Student, Resources.Employee })
                if (!roleManager.RoleExistsAsync(role).Result)
                    while (!roleManager.CreateAsync(new IdentityRole(role)).Result.Succeeded) ;

            if (context.Users.Count() == 0)
                signInManager.SignOutAsync();

            byte[] img1 = (byte[]) Resources.ResourceManager.GetObject("Image1");
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