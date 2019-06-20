using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CABESO
{
    /// <summary>
    /// Die Datenbankabbildungsklasse mit Bezug zur Tabellenspalte "Allergens"
    /// </summary>
    [Table("Allergens")]
    public class Allergen
    {
        /// <summary>
        /// Die ID des gegebenen Allergens
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Die Beschreibung des gegebenen Allergens
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Die standardmäßige Objektbeschreibung entspricht hier der Beschreibung des Allergens.
        /// </summary>
        /// <returns>
        /// Die Beschreibung des Objekts
        /// </returns>
        public override string ToString()
        {
            return Description;
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
            return obj is Allergen && (obj as Allergen).Id == Id;
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
    }
}