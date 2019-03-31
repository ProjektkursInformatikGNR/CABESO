﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CABESO.Areas.Counter.Pages
{
    [Authorize(Roles = "Employee, Admin")]
    public class IndexModel : PageModel
    {
        public IndexModel()
        {
        }

        public void OnGet(string sortOrder)
        {
        }
    }
}