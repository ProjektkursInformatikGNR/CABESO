using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;

namespace CABESO.Areas.Counter.Pages
{
    public static class ManageNavPages
    {
        public static string Index => "Index";
        public static string OrderHistory => "OrderHistory";
        public static string EditOrder => "EditOrder";
        public static string Products => "Products";
        public static string AddProduct => "AddProduct";
        public static string Allergens => "Allergens";
        public static string AddAllergen => "AddAllergen";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);
        public static string OrderHistoryNavClass(ViewContext viewContext) => PageNavClass(viewContext, OrderHistory);
        public static string EditOrderNavClass(ViewContext viewContext) => PageNavClass(viewContext, EditOrder);
        public static string ProductsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Products);
        public static string AddProductNavClass(ViewContext viewContext) => PageNavClass(viewContext, AddProduct);
        public static string AllergensNavClass(ViewContext viewContext) => PageNavClass(viewContext, Allergens);
        public static string AddAllergenNavClass(ViewContext viewContext) => PageNavClass(viewContext, AddAllergen);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            string activePage = viewContext.ViewData["ActivePage"] as string ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return activePage.Equals(page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}