using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CABESO.Views.Admin
{
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult GenerateCodes()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddUser()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user != null && !user.Id.Equals(User.GetIdentityUser().Id))
                await _userManager.DeleteAsync(user);
            return LocalRedirect("~/Admin/Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EditUser()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeactivateCode(string code)
        {
            _context.Codes.Remove(_context.Codes.Find(code));
            _context.SaveChanges();
            return LocalRedirect("~/Admin/GenerateCodes");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult RemoveForm(int id)
        {
            Form form = _context.Forms.Find(id);
            _context.Forms.Remove(form);
            _context.SaveChanges();
            return LocalRedirect("~/Admin/Forms");
        }


        [HttpPost]
        public IActionResult Generate(int number, string role)
        {
            for (int i = 0; i < number; i++)
                GenerateCode(role);
            return LocalRedirect("~/Admin/GenerateCodes");
        }

        public void GenerateCode(string role)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const int length = 10;

            Random random = new Random();
            string code = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            _context.Codes.Add(new RegistrationCode() { Code = code, CreationTime = DateTime.UtcNow, Role = _context.Roles.FirstOrDefault(r => r.Name.Equals(role)) });
            _context.SaveChanges();
        }

        [HttpPost]
        public IActionResult DeactivateOldCodes(DateTime limit)
        {
            foreach (RegistrationCode code in _context.Codes)
                if (code.CreationTime.Date <= limit)
                    _context.Codes.Remove(code);
            _context.SaveChanges();
            return LocalRedirect("~/Admin/GenerateCodes");
        }
    }
}