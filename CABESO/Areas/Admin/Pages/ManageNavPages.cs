using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;

namespace CABESO.Areas.Admin.Pages
{
    /// <summary>
    /// Eine Hilfsklasse zur Navigation innerhalb der Area "Admin"
    /// </summary>
    public static class ManageNavPages
    {
        /// <summary>
        /// Der Name der Seite <seealso cref="Areas_Admin_Pages_Index"/>
        /// </summary>
        public static string Index => "Index";

        /// <summary>
        /// Der Name der Seite <seealso cref="Areas_Admin_Pages_GenerateCodes"/>
        /// </summary>
        public static string GenerateCodes => "GenerateCodes";

        /// <summary>
        /// Der Name der Seite <seealso cref="Areas_Admin_Pages_EditUser"/>
        /// </summary>
        public static string EditUser => "EditUser";

        /// <summary>
        /// Der Name der Seite <seealso cref="Areas_Admin_Pages_Forms"/>
        /// </summary>
        public static string Forms => "Forms";

        /// <summary>
        /// Der Name der Seite <seealso cref="Areas_Admin_Pages_EditForm"/>
        /// </summary>
        public static string EditForm => "EditForm";

        /// <summary>
        /// Die Navigationsinformation für die Seite <seealso cref="Areas_Admin_Pages_Index"/>
        /// </summary>
        /// <param name="viewContext">
        /// Der Ansichtkontext
        /// </param>
        /// <returns>
        /// Die Navigationsinformation
        /// </returns>
        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        /// <summary>
        /// Die Navigationsinformation für die Seite <seealso cref="Areas_Admin_Pages_GenerateCodes"/>
        /// </summary>
        /// <param name="viewContext">
        /// Der Ansichtkontext
        /// </param>
        /// <returns>
        /// Die Navigationsinformation
        /// </returns>
        public static string GenerateCodesNavClass(ViewContext viewContext) => PageNavClass(viewContext, GenerateCodes);

        /// <summary>
        /// Die Navigationsinformation für die Seite <seealso cref="Areas_Admin_Pages_EditUser"/>
        /// </summary>
        /// <param name="viewContext">
        /// Der Ansichtkontext
        /// </param>
        /// <returns>
        /// Die Navigationsinformation
        /// </returns>
        public static string EditUserNavClass(ViewContext viewContext) => PageNavClass(viewContext, EditUser);

        /// <summary>
        /// Die Navigationsinformation für die Seite <seealso cref="Areas_Admin_Pages_Forms"/>
        /// </summary>
        /// <param name="viewContext">
        /// Der Ansichtkontext
        /// </param>
        /// <returns>
        /// Die Navigationsinformation
        /// </returns>
        public static string FormsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Forms);

        /// <summary>
        /// Die Navigationsinformation für die Seite <seealso cref="Areas_Admin_Pages_EditForm"/>
        /// </summary>
        /// <param name="viewContext">
        /// Der Ansichtkontext
        /// </param>
        /// <returns>
        /// Die Navigationsinformation
        /// </returns>
        public static string EditFormNavClass(ViewContext viewContext) => PageNavClass(viewContext, EditForm);

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