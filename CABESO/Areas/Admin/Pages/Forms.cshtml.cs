using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace CABESO.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class FormsModel : PageModel
    {
        public Form[] Forms;

        public string Sort { get; set; }

        public string SearchKeyWord { get; set; }

        public FormsModel(ApplicationDbContext context)
        {
            Forms = context.Forms.ToArray();
        }

        public void OnGet(string search, string sortOrder)
        {
            SearchKeyWord = search ?? string.Empty;
            if (!string.IsNullOrEmpty(SearchKeyWord))
                Forms = Forms.Search(search, form => form.ToString()).ToArray();
            Sort = string.IsNullOrEmpty(sortOrder) ? "!" : "";

            IOrderedEnumerable<Form> forms = Forms.OrderBy(user => 0);
            switch (sortOrder)
            {
                case "!":
                    forms = forms.OrderByDescending(form => form.GetGrade().Year);
                    break;
                case "":
                    forms = forms.OrderBy(form => form.GetGrade().Year);
                    break;
            }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string SearchKeyWord { get; set; }
        }

        public IActionResult OnPost()
        {
            return RedirectToAction("Forms", "Admin", new { sortOrder = string.Empty, search = Input.SearchKeyWord?.Trim() ?? string.Empty });
        }
    }
}