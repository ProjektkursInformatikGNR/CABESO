using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;

namespace CABESO.Areas.Orders.Pages
{
    public static class ManageNavPages
    {
        public static string Index => "Index";
        public static string PlaceOrder => "PlaceOrder";
        public static string OrderHistory => "OrderHistory";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);
        public static string PlaceOrderNavClass(ViewContext viewContext) => PageNavClass(viewContext, PlaceOrder);
        public static string OrdersHistoryNavClass(ViewContext viewContext) => PageNavClass(viewContext, OrderHistory);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            string activePage = viewContext.ViewData["ActivePage"] as string ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return activePage.Equals(page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}