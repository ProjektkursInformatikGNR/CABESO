using System.ComponentModel.DataAnnotations.Schema;

namespace CABESO
{
    /// <summary>
    /// Die Datenbankabbildungsklasse mit Bezug zur Tabellenspalte "Products"
    /// </summary>
    [Table("Products")]
    public class Product
    {
        /// <summary>
        /// Die ID des gegebenen Produkts
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Die Bezeichnung des gegebenen Produkts
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Der Preis des gegebene Produkts
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Der Rabatt auf das gegebene Produkt
        /// </summary>
        public decimal? Sale { get; set; }

        /// <summary>
        /// Der Dateipfad zur Abbildung des gegebenen Produkts
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Die Sammlung der Allergene des gegebenen Produkts
        /// </summary>
        public Allergen[] Allergens { get; set; }

        /// <summary>
        /// Der Indikator, ob das gegebene Produkt vegetarisch ist
        /// </summary>
        public bool Vegetarian { get; set; }

        /// <summary>
        /// Der Indikator, ob das gegebene Produkt vegan ist
        /// </summary>
        public bool Vegan { get; set; }

        /// <summary>
        /// Die Größe des gegebenen Produkts
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// Der Pfandbetrag des gegebenen Produkts
        /// </summary>
        public decimal? Deposit { get; set; }

        /// <summary>
        /// Weitere Hinweise zum gegebenen Produkt
        /// </summary>
        public string Information { get; set; }

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
            return obj is Product && (obj as Product).Id == Id;
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
        /// Die standardmäßige Objektbeschreibung entspricht hier der Produktbezeichnung ggf. mit Hinweis auf die Größe und/oder den Pfandbetrag.
        /// </summary>
        /// <returns>
        /// Die Beschreibung des Objekts
        /// </returns>
        public override string ToString()
        {
            return Id == -1 ? string.Empty : string.Format("{0}{1} - {2}", Name, string.IsNullOrEmpty(Size) ? string.Empty : string.Format(" ({0})", Size), string.Format("{0:C2}", Price - (Sale ?? 0m)) + (Deposit.HasValue ? string.Format(" (+ {0:C2} Pfand)", Deposit) : string.Empty)); 
        }
    }
}