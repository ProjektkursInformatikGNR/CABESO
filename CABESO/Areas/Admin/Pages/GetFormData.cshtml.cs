using System;
using System.Linq;
using CABESO.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CABESO.Areas.Admin.Pages
{
    /// <summary>
    /// Eine Hilfsklasse zur AJAX-Kommunikation der <seealso cref="Areas_Admin_Pages_AddForm"/>-Weboberfläche.
    /// </summary>
    public class GetFormDataModel : PageModel
    {
        private readonly ApplicationDbContext _context; //Das Vermittlungsobjekt der Datenbankanbindung

        /// <summary>
        /// Erzeugt ein neues <seealso cref="GetFormDataModel"/>.
        /// </summary>
        /// <param name="context">
        /// Der Datenbankkontext per Dependency Injection
        /// </param>
        public GetFormDataModel(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, sobald eine AJAX-Anfrage gestartet wird.<para>
        /// Er liefert zu einer gegebenen Jahrgangstufe die jeweilige Belegung der Klassenzüge.</para>
        /// </summary>
        /// <param name="data">
        /// Die AJAX-Anfragedaten (hier die Jahrgangsstufe)
        /// </param>
        /// <returns>
        /// Die Belegung der gegebenen Jahrgangsstufe im JSON-Format
        /// </returns>
        public IActionResult OnGet(string data)
        {
            if (string.IsNullOrEmpty(data))
                return NotFound();
            if (int.TryParse(data, out int grade))
            {
                Form.Grade g = grade;
                if (g == null)
                    return NotFound();
                return new JsonResult(Array.ConvertAll(Form.GetStreams().ToArray(), stream => _context.Forms.Any(form => form.Enrolment == g.Enrolment && ((string.IsNullOrEmpty(form.Stream) && stream == '-') || (form.Stream != null && form.Stream.Length > 0 && form.Stream[0] == stream)))));
            }
            return new JsonResult(new bool[0]);
        }
    }
}