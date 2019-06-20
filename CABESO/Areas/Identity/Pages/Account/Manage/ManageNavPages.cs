using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;

namespace CABESO.Areas.Identity.Pages.Account.Manage
{
    /// <summary>
    /// Eine Hilfsklasse zur Navigation innerhalb der Subarea "Account.Manage"
    /// </summary>
    public static class ManageNavPages
    {
        /// <summary>
        /// Der Name der Seite mit der Modellklasse <see cref="IndexModel"/>
        /// </summary>
        public static string Index => "Index";

        /// <summary>
        /// Der Name der Seite mit der Modellklasse <see cref="ChangePasswordModel"/>
        /// </summary>
        public static string ChangePassword => "ChangePassword";

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
        /// Die Navigationsinformation für die Seite mit der Modellklasse <see cref="ChangePasswordModel"/>
        /// </summary>
        /// <param name="viewContext">
        /// Der Ansichtkontext
        /// </param>
        /// <returns>
        /// Die Navigationsinformation
        /// </returns>
        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

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