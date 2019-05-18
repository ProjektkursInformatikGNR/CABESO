using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace CABESO.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class EditFormModel : PageModel
    {
        
        private readonly ApplicationDbContext _context;
        public Form CurrentForm;
        private int CurrentFormId;

        public EditFormModel(ApplicationDbContext context)
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

        public void OnGet(int id)
        {
            CurrentForm = _context.Forms.Find(id);
            CurrentFormId = id;
        }

        public IActionResult OnPost()
        {
            //Problem mit Reference statt Instanz
            CurrentForm.Year = Input.Year;
            CurrentForm.Section = Input.Section;
            _context.Forms.Update(CurrentForm);
            _context.SaveChanges();
            return LocalRedirect("~/Admin/Forms");
        }
    }
}