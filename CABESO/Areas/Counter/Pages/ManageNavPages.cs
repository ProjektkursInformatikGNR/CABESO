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
        /// Der Name der Seite <seealso cref="Areas_Counter_Pages_Index"/>
        /// </summary>
        public static string Index => "Index";

        /// <summary>
        /// Der Name der Seite <seealso cref="Areas_Counter_Pages_OrderHistory"/>
        /// </summary>
        public static string OrderHistory => "OrderHistory";

        /// <summary>
        /// Der Name der Seite <seealso cref="Areas_Counter_Pages_EditOrder"/>
        /// </summary>
        public static string EditOrder => "EditOrder";

        /// <summary>
        /// Der Name der Seite <seealso cref="Areas_Counter_Pages_Products"/>
        /// </summary>
        public static string Products => "Products";

        /// <summary>
        /// Der Name der Seite <seealso cref="Areas_Counter_Pages_AddProduct"/>
        /// </summary>
        public static string AddProduct => "AddProduct";

        /// <summary>
        /// Der Name der Seite <seealso cref="Areas_Counter_Pages_Allergens"/>
        /// </summary>
        public static string Allergens => "Allergens";

        /// <summary>
        /// Der Name der Seite <seealso cref="Areas_Counter_Pages_AddAllergen"/>
        /// </summary>
        public static string AddAllergen => "AddAllergen";

        /// <summary>
        /// Die Navigationsinformation für die Seite <seealso cref="Areas_Counter_Pages_Index"/>
        /// </summary>
        /// <param name="viewContext">
        /// Der Ansichtkontext
        /// </param>
        /// <returns>
        /// Die Navigationsinformation
        /// </returns>
        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        /// <summary>
        /// Die Navigationsinformation für die Seite <seealso cref="Areas_Counter_Pages_OrderHistory"/>
        /// </summary>
        /// <param name="viewContext">
        /// Der Ansichtkontext
        /// </param>
        /// <returns>
        /// Die Navigationsinformation
        /// </returns>
        public static string OrderHistoryNavClass(ViewContext viewContext) => PageNavClass(viewContext, OrderHistory);

        /// <summary>
        /// Die Navigationsinformation für die Seite <seealso cref="Areas_Counter_Pages_EditOrder"/>
        /// </summary>
        /// <param name="viewContext">
        /// Der Ansichtkontext
        /// </param>
        /// <returns>
        /// Die Navigationsinformation
        /// </returns>
        public static string EditOrderNavClass(ViewContext viewContext) => PageNavClass(viewContext, EditOrder);

        /// <summary>
        /// Die Navigationsinformation für die Seite <seealso cref="Areas_Counter_Pages_Products"/>
        /// </summary>
        /// <param name="viewContext">
        /// Der Ansichtkontext
        /// </param>
        /// <returns>
        /// Die Navigationsinformation
        /// </returns>
        public static string ProductsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Products);

        /// <summary>
        /// Die Navigationsinformation für die Seite <seealso cref="Areas_Counter_Pages_AddProduct"/>
        /// </summary>
        /// <param name="viewContext">
        /// Der Ansichtkontext
        /// </param>
        /// <returns>
        /// Die Navigationsinformation
        /// </returns>
        public static string AddProductNavClass(ViewContext viewContext) => PageNavClass(viewContext, AddProduct);

        /// <summary>
        /// Die Navigationsinformation für die Seite <seealso cref="Areas_Counter_Pages_Allergens"/>
        /// </summary>
        /// <param name="viewContext">
        /// Der Ansichtkontext
        /// </param>
        /// <returns>
        /// Die Navigationsinformation
        /// </returns>
        public static string AllergensNavClass(ViewContext viewContext) => PageNavClass(viewContext, Allergens);

        /// <summary>
        /// Die Navigationsinformation für die Seite <seealso cref="Areas_Counter_Pages_AddAllergen"/>
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