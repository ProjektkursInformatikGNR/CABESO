using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CABESO
{
    /// <summary>
    /// Die Datenbankabbildungsklasse mit Bezug zur Tabellenspalte "Forms"
    /// </summary>
    [Table("Forms")]
    public class Form
    {
        /// <summary>
        /// Die ID der gegebenen Schulklasse
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Der Klassenzug der gegebenen Schulklasse
        /// </summary>
        public string Stream { get; set; }

        /// <summary>
        /// Das Jahr des Beginns der fünften Klasse (ohne Wiederholungen o. Ä.) für die Schüler der gegebenen Schulklasse
        /// </summary>
        public int Enrolment { get; set; }

        /// <summary>
        /// Gibt den Jahrgang, dem die Schulklasse zuzuordnen ist, zurück.
        /// </summary>
        /// <returns>
        /// Der zuzuordnende Jahrgang
        /// </returns>
        public Grade GetGrade() => Enrolment;

        /// <summary>
        /// Gibt eine Datenstruktur mit allen möglichen Klassenzügen ('A' bis 'Z' sowie '-' für keinen Klassenzug) zurück.
        /// </summary>
        /// <returns>
        /// Alle möglichen Klassenzüge
        /// </returns>
        public static IEnumerable<char> GetStreams()
        {
            yield return '-';
            for (char c = 'A'; c <= 'Z'; c++)
                yield return c;
        }

        /// <summary>
        /// Die standardmäßige Objektbeschreibung entspricht hier dem Klassennamen (typischerweise Jahrgang und Klassenzug).
        /// </summary>
        /// <returns>
        /// Die Beschreibung des Objekts
        /// </returns>
        public override string ToString()
        {
            return GetGrade().ToString() + (Enrolment > Grade.EF.Enrolment ? Stream : string.Empty);
        }

        /// <summary>
        /// Zur Generierung des Hashs wird auf von <see cref="object"/> geerbte Methode zurückgegriffen.
        /// </summary>
        /// <returns>
        /// Der Hash-Code des Objekts
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Als Vergleichsoperation genügt hier der Vergleich des Unique Keys <see cref="Id"/>.
        /// </summary>
        /// <param name="obj">
        /// Ein Vergleichsobjekt
        /// </param>
        /// <returns>
        /// Gibt <c>true</c> bei Übereinstimmung wieder, sonst <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is Form && (obj as Form).Id == Id;
        }
    }
}