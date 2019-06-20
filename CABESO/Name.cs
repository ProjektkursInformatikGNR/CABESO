using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace CABESO
{
    /// <summary>
    /// Die Abbildung des Namens eines Benutzers mit Vor- und Nachnamen
    /// </summary>
    public class Name : IComparable
    {
        /// <summary>
        /// Erzeugt einen neuen <see cref="Name"/>, indem die Informationen der wwschool-Adresse entnommen werden.
        /// </summary>
        /// <param name="mail">
        /// Die wwschool-Adresse des Benutzers
        /// </param>
        /// <param name="student">
        /// Der Indikator, ob der Benutzer ein Schüler bzw. eine Schülerin ist (bedeutet eine Umkehrung der Reihenfolge von Vor- und Nachnamen)
        /// </param>
        public Name(string mail, bool student)
        {
            if (string.IsNullOrEmpty(mail))
                return;

            Match[] matches = Regex.Matches(mail, @"([^0-9\.])+(?=.*@gnr\.wwschool\.de)").ToArray();

            if (matches.Length != 2)
            {
                FirstName = mail;
                return;
            }

            string[] names = Array.ConvertAll(matches, m => m.Value.ToLower());

            if (!student)
                names = names.Reverse().ToArray();

            char[] firstName = names[1].ToCharArray();
            firstName[0] = char.ToUpper(firstName[0]);
            for (int i = 1; i < firstName.Length; i++)
                if (!char.IsLetter(firstName[i - 1]))
                    firstName[i] = char.ToUpper(firstName[i]);

            string[] lastNames = names[0].Split('-');
            lastNames[lastNames.Length - 1] = char.ToUpper(lastNames[lastNames.Length - 1][0]) + (lastNames[lastNames.Length - 1].Length > 1 ? lastNames[lastNames.Length - 1].Substring(1) : "");

            FirstName = new string(firstName);
            LastName = string.Join(' ', lastNames);
        }

        /// <summary>
        /// Der Vorname des Benutzers
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Der Nachname des Benutzers
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Die standardmäßige Objektbeschreibung entspricht hier der Abfolge von Vor- und Nachnamen.
        /// </summary>
        /// <returns>
        /// Die Beschreibung des Objekts
        /// </returns>
        public override string ToString()
        {
            return (FirstName + " " + LastName).Trim();
        }

        /// <summary>
        /// Als Vergleichsoperation werden hier die jeweiligen Zeichenketten miteinander verglichen.
        /// </summary>
        /// <param name="obj">
        /// Ein Vergleichsobjekt
        /// </param>
        /// <returns>
        /// Gibt <c>true</c> bei Übereinstimmung wieder, sonst <c>false</c>.
        /// </returns>
        public int CompareTo(object obj)
        {
            if (obj is Name)
                return ToString().CompareTo((obj as Name).ToString());
            else if (obj is string)
                return ToString().CompareTo(obj as string);
            else
                return -1;
        }

        /// <summary>
        /// Konvertiert den gegebenen Namen in einen <see cref="string"/> mithilfe der Methode <see cref="ToString"/>.
        /// </summary>
        /// <param name="name">
        /// Der zu konvertierende Name
        /// </param>
        public static implicit operator string(Name name)
        {
            return name?.ToString();
        }
    }
}