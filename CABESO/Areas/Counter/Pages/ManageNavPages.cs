using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;

namespace CABESO.Areas.Counter.Pages
{
    /// <summary>
    /// Eine Hilfsklasse zur Navigation innerhalb der Area "Counter"
    /// </summary>
    public static class ManageNavPages
    {
        /// <summary>
        /// Der Name der Seite mit der Modellklasse <see cref="IndexModel"/>
        /// </summary>
        public static string Index => "Index";

        /// <summary>
        /// Der Name der Seite mit der Modellklasse <see cref="OrderHistoryModel"/>
        /// </summary>
        public static string OrderHistory => "OrderHistory";

        /// <summary>
        /// Der Name der Seite mit der Modellklasse <see cref="EditOrderModel"/>
        /// </summary>
        public static string EditOrder => "EditOrder";

        /// <summary>
        /// Der Name der Seite mit der Modellklasse <see cref="ProductsModel"/>
        /// </summary>
        public static string Products => "Products";

        /// <summary>
        /// Der Name der Seite mit der Modellklasse <see cref="AddProductModel"/>
        /// </summary>
        public static string AddProduct => "AddProduct";

        /// <summary>
        /// Der Name der Seite mit der Modellklasse <see cref="AllergensModel"/>
        /// </summary>
        public static string Allergens => "Allergens";

        /// <summary>
        /// Der Name der Seite mit der Modellklasse <see cref="AddAllergenModel"/>
        /// </summary>
        public static string AddAllergen => "AddAllergen";

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
        /// Die Navigationsinformation für die Seite mit der Modellklasse <see cref="OrderHistoryModel"/>
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
        /// Die Navigationsinformation für die Seite mit der Modellklasse <see cref="ProductsModel"/>
        /// </summary>
        /// <param name="viewContext">
        /// Der Ansichtkontext
        /// </param>
        /// <returns>
        /// Die Navigationsinformation
        /// </returns>
        public static string ProductsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Products);

        /// <summary>
        /// Die Navigationsinformation für die Seite mit der Modellklasse <see cref="AddProductModel"/>
        /// </summary>
        /// <param name="viewContext">
        /// Der Ansichtkontext
        /// </param>
        /// <returns>
        /// Die Navigationsinformation
        /// </returns>
        public static string AddProductNavClass(ViewContext viewContext) => PageNavClass(viewContext, AddProduct);

        /// <summary>
        /// Die Navigationsinformation für die Seite mit der Modellklasse <see cref="AllergensModel"/>
        /// </summary>
        /// <param name="viewContext">
        /// Der Ansichtkontext
        /// </param>
        /// <returns>
        /// Die Navigationsinformation
        /// </returns>
        public static string AllergensNavClass(ViewContext viewContext) => PageNavClass(viewContext, Allergens);

        /// <summary>
        /// Die Navigationsinformation für die Seite mit der Modellklasse <see cref="AddAllergenModel"/>
        /// </summary>
        /// <param name="viewContext">
        /// Der Ansichtkontext
        /// </param>
        /// <returns>
        /// Die Navigationsinformation
        /// </returns>
        public static string AddAllergenNavClass(ViewContext viewContext) => PageNavClass(viewContext, AddAllergen);

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