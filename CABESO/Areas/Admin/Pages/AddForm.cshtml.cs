using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CABESO.Areas.Admin.Pages
{
    /// <summary>
    /// Die Modellklasse der Razor Page zum Hinzufügen einer Schulklasse
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AddFormModel : PageModel
    {
        private readonly ApplicationDbContext _context; //Das Vermittlungsobjekt der Datenbankanbindung //Das Vermittlungsobjekt der Datenbankanbindung

        /// <summary>
        /// Erzeugt ein neues <see cref="AddFormModel"/>.
        /// </summary>
        /// <param name="context">
        /// Der Datenbankkontext per Dependency Injection
        /// </param>
        public AddFormModel(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, sobald das "POST"-Event auslöst wird (hier durch Betätigung des "Hinzufügen"-Buttons).
        /// </summary>
        /// <returns>
        /// Ein <see cref="IActionResult"/>, das bestimmt, wie nach Behandlung des Event vorgegangen werden soll.
        /// </returns>
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                for (int stream = 0; stream < 27; stream++)
                {
                    if (Input.Streams[stream] && _context.Forms.Where(form => form.GetGrade() != null && form.GetGrade().Year == Input.Grade && (form.Stream.Length > 0 && form.Stream[0] == 'A' + stream - 1 || string.IsNullOrEmpty(form.Stream) && stream == 0)).Count() == 0)
                        _context.Forms.Add(new Form() { Stream = stream == 0 ? "" : ((char)('A' + stream - 1)).ToString(), Enrolment = ((Grade) Input.Grade).Enrolment });
                }
                _context.SaveChanges();
                return LocalRedirect("~/Admin/Forms");
            }
            return Page();
        }

        /// <summary>
        /// Ein Hilfsobjekt, das die Eingabeinformationen der Weboberfläche zwischenspeichert.
        /// </summary>
        /// <summary>
		/// Ein Hilfsobjekt, das die Eingabeinformationen der Weboberfläche zwischenspeichert.
		/// </summary>
		[BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        /// Eine Datenstruktur zur Zwischenspeicherung der Eingabeinformationen
        /// </summary>
        /// <summary>
		/// Eine Datenstruktur zur Zwischenspeicherung der Eingabeinformationen
		/// </summary>
		public class InputModel
        {
            /// <summary>
            /// Der Jahrgang der hinzuzufügenden Schulklasse (erforderlich)
            /// </summary>
            [Required]
            [Display(Name = "Derzeitiger Jahrgang")]
            public int Grade { get; set; }

            /// <summary>
            /// Ein Array zur Auswahl der hinzuzufügenden Klassenzüge (optional)
            /// </summary>
            [Display(Name = "Klassenzüge")]
            public bool[] Streams { get; set; }
        }
    }
}