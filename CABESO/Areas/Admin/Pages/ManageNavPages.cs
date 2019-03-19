using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;

namespace CABESO.Areas.Admin.Pages
{
    public static class ManageNavPages
    {
        public static string Overview => "Overview";
        public static string AddUser => "AddUser";

        public static string OverviewNavClass(ViewContext viewContext) => PageNavClass(viewContext, Overview);
        public static string AddUserNavClass(ViewContext viewContext) => PageNavClass(viewContext, AddUser);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}