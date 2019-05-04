using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;

namespace CABESO.Areas.Admin.Pages
{
    public static class ManageNavPages
    {
        public static string Index => "Index";
        public static string GenerateCodes => "GenerateCodes";
        public static string AddUser => "AddUser";
        public static string EditUser => "EditUser";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);
        public static string GenerateCodesNavClass(ViewContext viewContext) => PageNavClass(viewContext, GenerateCodes);
        public static string AddUserNavClass(ViewContext viewContext) => PageNavClass(viewContext, AddUser);
        public static string EditUserNavClass(ViewContext viewContext) => PageNavClass(viewContext, EditUser);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            string activePage = viewContext.ViewData["ActivePage"] as string ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return activePage.Equals(page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}