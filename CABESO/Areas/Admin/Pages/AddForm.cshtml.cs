using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CABESO.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class AddFormModel : PageModel
    {        
        private readonly ApplicationDbContext _context;

        public AddFormModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Geben Sie bitte den Jahrgang an.")]
            [Display(Name = "Jahrgang")]
            public int Year { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Geben Sie bitte die Klasse an.")]
            [Display(Name = "Klasse")]
            public string Section { get; set; }

            public int FormId { get; set; }
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            _context.Forms.Add(new Form() { Year = Input.Year, Section = Input.Section });
            _context.SaveChanges();
            return LocalRedirect("~/Admin/Forms");
        }
    }
}