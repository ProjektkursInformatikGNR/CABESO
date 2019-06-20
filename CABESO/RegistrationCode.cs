using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CABESO
{
    /// <summary>
    /// Die Datenbankabbildungsklasse mit Bezug zur Tabellenspalte "Codes"
    /// </summary>
    [Table("Codes")]
    public class RegistrationCode
    {
        /// <summary>
        /// Der zehnstellige, alphanumerische Registrierungscode
        /// </summary>
        [Key]
        public string Code { get; set; }

        /// <summary>
        /// Die Zeit der Erstellung des Codes
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Die mit dem Code verknüpfte Rolle
        /// </summary>
        public IdentityRole Role { get; set; }

        /// <summary>
        /// Als Vergleichsoperation werden hier die Codes selbst jeweils als <see cref="string"/> miteinander verglichen.
        /// </summary>
        /// <param name="obj">
        /// Ein Vergleichsobjekt
        /// </param>
        /// <returns>
        /// Gibt <c>true</c> bei Übereinstimmung wieder, sonst <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is RegistrationCode && (obj as RegistrationCode).Code.Equals(Code);
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