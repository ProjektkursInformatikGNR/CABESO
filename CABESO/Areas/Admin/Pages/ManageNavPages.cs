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
        public static string AddForm => "AddForm";
        public static string EditUser => "EditUser";
        public static string Forms => "Forms";
        public static string EditForm => "EditForm";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);
        public static string GenerateCodesNavClass(ViewContext viewContext) => PageNavClass(viewContext, GenerateCodes);
        public static string AddUserNavClass(ViewContext viewContext) => PageNavClass(viewContext, AddUser);
        public static string AddFormNavClass(ViewContext viewContext) => PageNavClass(viewContext, AddForm);
        public static string EditUserNavClass(ViewContext viewContext) => PageNavClass(viewContext, EditUser);
        public static string FormsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Forms);
        public static string EditFormNavClass(ViewContext viewContext) => PageNavClass(viewContext, EditForm);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            string activePage = viewContext.ViewData["ActivePage"] as string ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return activePage.Equals(page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}