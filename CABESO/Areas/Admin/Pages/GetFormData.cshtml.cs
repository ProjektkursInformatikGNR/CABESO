using System;
using System.Linq;
using CABESO.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CABESO.Areas.Admin.Pages
{
    public class GetFormDataModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public GetFormDataModel(ApplicationDbContext context)
        {
            _context = context;
        }

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