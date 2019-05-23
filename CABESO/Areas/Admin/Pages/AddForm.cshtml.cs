using CABESO.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CABESO.Areas.Admin.Pages
{
    public class AddFormModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddFormModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                for (int stream = 0; stream < 27; stream++)
                {
                    if (Input.Streams[stream] && _context.Forms.Where(form => form.GetGrade() != null && form.GetGrade().Year == Input.Grade && (form.Stream.Length > 0 && form.Stream[0] == 'A' + stream - 1 || string.IsNullOrEmpty(form.Stream) && stream == 0)).Count() == 0)
                        _context.Forms.Add(new Form() { Stream = stream == 0 ? "" : ((char)('A' + stream - 1)).ToString(), Enrolment = ((Form.Grade) Input.Grade).Enrolment });
                }
                _context.SaveChanges();
                return LocalRedirect("~/Admin/Forms");
            }
            return Page();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Derzeitiger Jahrgang")]
            public int Grade { get; set; }

            [Display(Name = "Klassenzüge")]
            public bool[] Streams { get; set; }
        }
    }
}