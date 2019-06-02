using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CABESO.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class EditFormModel : PageModel
    {
        
        private readonly ApplicationDbContext _context;
        public static Form CurrentForm;

        public EditFormModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Gib bitte den Jahrgang an.")]
            [Display(Name = "Derzeitiger Jahrgang")]
            public int Year { get; set; }

            [Display(Name = "Klassenzug")]
            public string Stream { get; set; }
        }

        public void OnGet(int id)
        {
            CurrentForm = _context.Forms.Find(id);
        }

        public IActionResult OnPost()
        {
            CurrentForm.Enrolment = ((Form.Grade) Input.Year).Enrolment;
            CurrentForm.Stream = string.IsNullOrEmpty(Input.Stream) ? "" : Input.Stream;
            _context.Forms.Update(CurrentForm);
            _context.SaveChanges();
            return LocalRedirect("~/Admin/Forms");
        }
    }
}