using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;

namespace CABESO.Areas.Orders.Pages
{
    /// <summary>
    /// Eine Hilfsklasse zur Navigation innerhalb der Area "Orders"
    /// </summary>
    public static class ManageNavPages
    {
        /// <summary>
        /// Der Name der Seite mit der Modellklasse <see cref="IndexModel"/>
        /// </summary>
        public static string Index => "Index";

        /// <summary>
        /// Der Name der Seite mit der Modellklasse <see cref="PlaceOrderModel"/>
        /// </summary>
        public static string PlaceOrder => "PlaceOrder";

        /// <summary>
        /// Der Name der Seite mit der Modellklasse <see cref="OrderHistoryModel"/>
        /// </summary>
        public static string OrderHistory => "OrderHistory";

        /// <summary>
        /// Der Name der Seite mit der Modellklasse <see cref="EditOrderModel"/>
        /// </summary>
        public static string EditOrder => "EditOrder";

        /// <summary>
        /// Die Navigationsinformation für die Seite mit der Modellklasse <see cref="IndexModel"/>
        /// </summary>
        /// <param name="viewContext">
        /// Der Ansichtkontext
        /// </param>
        /// <returns>
        /// Die Navigationsinformation
        /// </returns>
        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        /// <summary>
        /// Die Navigationsinformation für die Seite mit der Modellklasse <see cref="PlaceOrderModel"/>
        /// </summary>
        /// <param name="viewContext">
        /// Der Ansichtkontext
        /// </param>
        /// <returns>
        /// Die Navigationsinformation
        /// </returns>
        public static string PlaceOrderNavClass(ViewContext viewContext) => PageNavClass(viewContext, PlaceOrder);

        /// <summary>
        /// Die Navigationsinformation für die Seite mit der Modellklasse <see cref="OrderHistory"/>
        /// </summary>
        /// <param name="viewContext">
        /// Der Ansichtkontext
        /// </param>
        /// <returns>
        /// Die Navigationsinformation
        /// </returns>
        public static string OrderHistoryNavClass(ViewContext viewContext) => PageNavClass(viewContext, OrderHistory);

        /// <summary>
        /// Die Navigationsinformation für die Seite mit der Modellklasse <see cref="EditOrderModel"/>
        /// </summary>
        /// <param name="viewContext">
        /// Der Ansichtkontext
        /// </param>
        /// <returns>
        /// Die Navigationsinformation
        /// </returns>
        public static string EditOrderNavClass(ViewContext viewContext) => PageNavClass(viewContext, EditOrder);

        /// <summary>
        /// Liefert Informationen darüber, ob die gegebene Seite als aktiv angezeigt werden soll.
        /// </summary>
        /// <param name="viewContext">
        /// Der Ansichtkontext
        /// </param>
        /// <param name="page">
        /// Die zu untersuchende Seite
        /// </param>
        /// <returns>
        /// Indikator, ob die Seite als aktiv gekennzeichnet werden soll
        /// </returns>
        private static string PageNavClass(ViewContext viewContext, string page)
        {
            string activePage = viewContext.ViewData["ActivePage"] as string ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return activePage.Equals(page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}