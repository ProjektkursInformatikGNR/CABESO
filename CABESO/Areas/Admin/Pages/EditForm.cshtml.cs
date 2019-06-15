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
        
        private readonly ApplicationDbContext _context; //Das Vermittlungsobjekt der Datenbankanbindung
        public static Form CurrentForm;

        public EditFormModel(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
		/// Ein Hilfsobjekt, das die Eingabeinformationen der Weboberfläche zwischenspeichert.
		/// </summary>
		[BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
		/// Eine Datenstruktur zur Zwischenspeicherung der Eingabeinformationen
		/// </summary>
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
            if (ModelState.IsValid)
            {
                CurrentForm.Enrolment = ((Form.Grade)Input.Year).Enrolment;
                CurrentForm.Stream = string.IsNullOrEmpty(Input.Stream) ? "" : Input.Stream;
                _context.Forms.Update(CurrentForm);
                _context.SaveChanges();
                return LocalRedirect("~/Admin/Forms");
            }
            return Page();
        }
    }
}