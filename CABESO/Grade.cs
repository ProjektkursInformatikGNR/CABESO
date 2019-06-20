using CABESO.Properties;
using System;
using System.Linq;

namespace CABESO
{
    /// <summary>
    /// Die Abbildung eines Jahrgangs als Menge von Schulklassen mit gleichem Fortschritt in der Schullaufbahn
    /// </summary>
    public class Grade
    {
        /// <summary>
        /// Erzeugt eine neue <see cref="Grade"/>.
        /// </summary>
        /// <param name="name">
        /// Die Bezeichnung der Jahrgangsstufe
        /// </param>
        /// <param name="enrolment">
        /// Das Jahr des Beginns der fünften Klasse für die Schüler dieses Jahrgangs
        /// </param>
        /// <param name="year">
        /// Der numerische Wert des Jahrgangs als Indikator für den Fortschritt in der Schullaufbahn (5 bis 12/13)
        /// </param>
        private Grade(string name, int enrolment, int year)
        {
            _name = name;
            Enrolment = enrolment;
            Year = year;
        }

        /// <summary>
        /// Das Jahr des Beginns der fünften Klasse für die Schüler dieses Jahrgangs
        /// </summary>
        public int Enrolment { get; private set; }

        /// <summary>
        /// Der numerische Wert des Jahrgangs als Indikator für den Fortschritt in der Schullaufbahn (5 bis 12/13)
        /// </summary>
        public int Year { get; private set; }

        private readonly string _name; //Die Bezeichnung der Jahrgangsstufe

        /// <summary>
        /// Die standardmäßige Objektbeschreibung entspricht hier der Bezeichnung des Jahrgangs.
        /// </summary>
        /// <returns>
        /// Die Beschreibung des Objekts
        /// </returns>
        public override string ToString() => _name;

        /// <summary>
        /// Die vierte Jahrgangsstufe (im Falle einer Registrierung vor Schulbeginn)
        /// </summary>
        public static Grade FOUR => new Grade("4", GetYearStart(DateTime.Now.AddYears(1)), 4);

        /// <summary>
        /// Die fünfte Jahrgangsstufe
        /// </summary>
        public static Grade FIVE => new Grade("5", GetYearStart(DateTime.Now), 5);

        /// <summary>
        /// Die sechste Jahrgangsstufe
        /// </summary>
        public static Grade SIX => new Grade("6", GetYearStart(DateTime.Now.AddYears(-1)), 6);

        /// <summary>
        /// Die siebente Jahrgangsstufe
        /// </summary>
        public static Grade SEVEN => new Grade("7", GetYearStart(DateTime.Now.AddYears(-2)), 7);

        /// <summary>
        /// Die achte Jahrgangsstufe
        /// </summary>
        public static Grade EIGHT => new Grade("8", GetYearStart(DateTime.Now.AddYears(-3)), 8);

        /// <summary>
        /// Die neunte Jahrgangsstufe
        /// </summary>
        public static Grade NINE => new Grade("9", GetYearStart(DateTime.Now.AddYears(-4)), 9);

        /// <summary>
        /// Die zehnte Jahrgangsstufe (ab dem ersten G9-Jahrgang)
        /// </summary>
        public static Grade TEN => AdjustG8(new Grade("10", GetYearStart(DateTime.Now.AddYears(-5)), 10), grade => null);

        /// <summary>
        /// Die Einführungsphase (zehnte bzw. elfte Jahrgangsstufe)
        /// </summary>
        public static Grade EF => AdjustG8(new Grade("EF", GetYearStart(DateTime.Now.AddYears(-6)), 11), grade => { if (grade.Enrolment == 2017) return null; else { grade.Enrolment++; grade.Year--; return grade; } });

        /// <summary>
        /// Das erste Jahr der Qualifikationsphase (elfte bzw. zwölfte Jahrgangsstufe)
        /// </summary>
        public static Grade Q1 => AdjustG8(new Grade("Q1", GetYearStart(DateTime.Now.AddYears(-7)), 12), grade => { if (grade.Enrolment == 2017) return null; else { grade.Enrolment++; grade.Year--; return grade; } });

        /// <summary>
        /// Das zweite Jahr der Qualifikationsphase (zwölfte bzw. 13. Jahrgangsstufe)
        /// </summary>
        public static Grade Q2 => AdjustG8(new Grade("Q2", GetYearStart(DateTime.Now.AddYears(-8)), 13), grade => { if (grade.Enrolment == 2017) return null; else { grade.Enrolment++; grade.Year--; return grade; } });

        /// <summary>
        /// Die Gruppe der Alumnae und Alumni (im Falle einer späten Austragung)
        /// </summary>
        public static Grade GRADUATE => new Grade(Resources.Graduate, -1, -1);

        /// <summary>
        /// Die zur Verfügung stehenden Jahrgangsstufen
        /// </summary>
        public static Grade[] Grades => new[] { FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, EF, Q1, Q2, GRADUATE };

        /// <summary>
        /// Gibt das Jahr des Beginns des zum gegebenen Zeitpunkt laufenden Schuljahres wieder.
        /// </summary>
        /// <param name="time">
        /// Der zu betrachtende Zeitpunkt
        /// </param>
        /// <returns>
        /// Die Jahreszahl des Schuljahres
        /// </returns>
        private static int GetYearStart(DateTime time) => time.Year - (time.Month > 7 ? 0 : 1);

        /// <summary>
        /// Passt einen gegebenen Jahrgang an, falls dieser noch ins System G8 fällt, und gibt ihn wieder zurück.
        /// </summary>
        /// <param name="grade">
        /// Der ggf. anzupassende Jahrgang
        /// </param>
        /// <param name="adjustment">
        /// Die ggf. vorzunehmende Anpassung
        /// </param>
        /// <returns>
        /// Der ggf. angepasste Jahrgang
        /// </returns>
        private static Grade AdjustG8(Grade grade, Func<Grade, Grade> adjustment)
        {
            if (grade.Enrolment <= 2017)
                return adjustment(grade);
            return grade;
        }

        /// <summary>
        /// Extrahiert aus einem Parameter vom Datentyp <see cref="int"/> eine passende Jahrgangsstufe.<para>
        /// Im Intervall [5; 12/13] wird die Zahl als Jahrgang, im Intervall der derzeitigen Einschulungsjahre als ein solches Jahr interpretiert.</para>
        /// </summary>
        /// <param name="i">
        /// Der zu interpretierende Zahlenwert
        /// </param>
        public static implicit operator Grade(int i)
        {
            if (i >= FOUR.Year && i <= Q2.Year)
                return Grades.FirstOrDefault(grade => grade != null && grade.Year == i);
            Grade g;
            if ((g = Grades.FirstOrDefault(grade => grade?.Enrolment == i)) != null)
                return g;
            return GRADUATE;
        }
    }
}