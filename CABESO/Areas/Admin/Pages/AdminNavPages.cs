using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;

namespace CABESO.Areas.Admin.Pages
{
    /// <summary>
    /// Eine Hilfsklasse zur Navigation innerhalb der Area "Admin"
    /// </summary>
    public static class AdminNavPages
    {
        /// <summary>
        /// Der Name der Seite mit der Modellklasse <see cref="IndexModel"/>
        /// </summary>
        public static string Index => "Index";

        /// <summary>
        /// Der Name der Seite mit der Modellklasse <see cref="GenerateCodesModel"/>
        /// </summary>
        public static string GenerateCodes => "GenerateCodes";

        /// <summary>
        /// Der Name der Seite mit der Modellklasse <see cref="EditUserModel"/>
        /// </summary>
        public static string EditUser => "EditUser";

        /// <summary>
        /// Der Name der Seite mit der Modellklasse <see cref="FormsModel"/>
        /// </summary>
        public static string Forms => "Forms";

        /// <summary>
        /// Der Name der Seite mit der Modellklasse <see cref="EditFormModel"/>
        /// </summary>
        public static string EditForm => "EditForm";

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
        /// Die Navigationsinformation für die Seite mit der Modellklasse <see cref="GenerateCodesModel"/>
        /// </summary>
        /// <param name="viewContext">
        /// Der Ansichtkontext
        /// </param>
        /// <returns>
        /// Die Navigationsinformation
        /// </returns>
        public static string GenerateCodesNavClass(ViewContext viewContext) => PageNavClass(viewContext, GenerateCodes);

        /// <summary>
        /// Die Navigationsinformation für die Seite mit der Modellklasse <see cref="EditUserModel"/>
        /// </summary>
        /// <param name="viewContext">
        /// Der Ansichtkontext
        /// </param>
        /// <returns>
        /// Die Navigationsinformation
        /// </returns>
        public static string EditUserNavClass(ViewContext viewContext) => PageNavClass(viewContext, EditUser);

        /// <summary>
        /// Die Navigationsinformation für die Seite mit der Modellklasse <see cref="FormsModel"/>
        /// </summary>
        /// <param name="viewContext">
        /// Der Ansichtkontext
        /// </param>
        /// <returns>
        /// Die Navigationsinformation
        /// </returns>
        public static string FormsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Forms);

        /// <summary>
        /// Die Navigationsinformation für die Seite mit der Modellklasse <see cref="EditFormModel"/>
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