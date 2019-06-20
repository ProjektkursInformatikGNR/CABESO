namespace CABESO.Models
{
    /// <summary>
    /// Das View Model des Page Models "Error Model" zur Strukturierung der Fehlerdaten
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Die ID des Error Requests
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Der Indikator, ob die Request-ID in der Weboberfläche angezeigt werden soll.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}