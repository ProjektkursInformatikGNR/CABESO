using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CABESO
{
    /// <summary>
    /// Die Abbildung einer aufgegebenen Bestellung
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Die ID der gegebenen Bestellung
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Der Besteller bzw. die Bestellerin
        /// </summary>
        public IdentityUser User { get; set; }

        /// <summary>
        /// Das bestellte Produkt
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// Die Zeit der Aufgabe der Bestellung
        /// </summary>
        public DateTime OrderTime { get; set; }

        /// <summary>
        /// Weitere Anmerkungen zur Bestellung
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Die Anzahl der bestellten Produkte
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Die Zeit der vollendeten Zubereitung
        /// </summary>
        public DateTime? PreparationTime { get; set; }

        /// <summary>
        /// Die Zeit der (geplanten) Abholung der Bestellung
        /// </summary>
        public DateTime CollectionTime { get; set; }

        /// <summary>
        /// Der Abholort
        /// </summary>
        public string CollectionPlace { get; set; }

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
            return obj is Order && (obj as Order).Id == Id;
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

    /// <summary>
    /// Eine bereits behandelte und archivierte <see cref="Order"/>
    /// </summary>
    [Table("OrderHistory")]
    public class HistoricOrder : Order
    {
        /// <summary>
        /// Konvertiert eine archivierte <see cref="HistoricOrder"/> in eine aktuelle <see cref="CurrentOrder"/>.
        /// </summary>
        /// <returns>
        /// Die konvertierte <see cref="CurrentOrder"/>
        /// </returns>
        public CurrentOrder ToCurrentOrder() => new CurrentOrder()
        {
            CollectionPlace = CollectionPlace,
            CollectionTime = CollectionTime,
            Id = Id,
            Notes = Notes,
            Number = Number,
            OrderTime = OrderTime,
            PreparationTime = PreparationTime,
            Product = Product,
            User = User
        };
    }

    /// <summary>
    /// Eine aufgegebene und noch zu behandelnde <see cref="Order"/>
    /// </summary>
    [Table("Orders")]
    public class CurrentOrder : Order
    {
        /// <summary>
        /// Konvertiert eine aktuelle <see cref="CurrentOrder"/> in eine archivierte <see cref="HistoricOrder"/>.
        /// </summary>
        /// <returns>
        /// Die konvertierte <see cref="HistoricOrder"/>
        /// </returns>
        public HistoricOrder ToHistoricOrder() => new HistoricOrder()
        {
            CollectionPlace = CollectionPlace,
            CollectionTime = CollectionTime,
            Id = Id,
            Notes = Notes,
            Number = Number,
            OrderTime = OrderTime,
            PreparationTime = PreparationTime,
            Product = Product,
            User = User
        };

        /// <summary>
        /// Ermittelt alle gültigen Abholzeiten innerhalb der nächsten Woche.
        /// </summary>
        /// <returns>
        /// Gültige Abholzeiten
        /// </returns>
        public static TimeRange[] ValidCollectionTimes()
        {
            TimeRange[] validDays = ((TimeRange) (DateTime.Now.AddHours(1), DateTime.Now.AddDays(7))).SplitDays()
                .Where(day => day.Start.DayOfWeek != DayOfWeek.Saturday //kein Samstag
                && day.Start.DayOfWeek != DayOfWeek.Sunday //kein Sonntag
                && !Holidays.Create().GetHolidays().Any(holiday => holiday.Date == day.Start.Date)) //kein Feiertag
                .ToArray();
            foreach (TimeRange day in validDays)
            {
                if (day.Start.TimeOfDay < new TimeSpan(8, 15, 0))
                    day.Start = day.Start.Date.Add(new TimeSpan(8, 15, 0));
                day.End = day.Start.Date.Add(new TimeSpan(12, 50, 0));
            }
            return validDays;
        }
    }

    /// <summary>
    /// Repräsentation einer Zeitspanne mit Start- sowie Endzeitpunkt
    /// </summary>
    public class TimeRange
    {
        /// <summary>
        /// Der Start der Zeitspanne
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// Das Ende der Zeitspanne
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// Erzeugt eine neue <see cref="TimeRange"/>
        /// </summary>
        /// <param name="start">
        /// Der Start der Zeitspanne
        /// </param>
        /// <param name="end">
        /// Das Ende der Zeitspanne
        /// </param>
        public TimeRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// Die Länge der Zeitspanne
        /// </summary>
        public TimeSpan Duration { get => End - Start; }

        /// <summary>
        /// Gibt an, ob sich ein gegebener Zeitpunkt innerhalb der Zeitspanne befindet.
        /// </summary>
        /// <param name="dt">
        /// Der zu überprüfende Zeitpunkt
        /// </param>
        /// <returns>
        /// Gibt <c>true</c> zurück, wenn der Zeitpunkt innerhalb der Zeitspanne liegt, sonst <c>false</c>.
        /// </returns>
        public bool Includes(DateTime dt)
        {
            return dt >= Start && dt <= End;
        }

        /// <summary>
        /// Gibt an, ob sich eine gegebene Zeitspanne innerhalb der Zeitspanne befindet.
        /// </summary>
        /// <param name="tr">
        /// Die zu überprüfende Zeitspanne
        /// </param>
        /// <returns>
        /// Gibt <c>true</c> zurück, wenn die Zeitspanne innerhalb der Zeitspanne liegt, sonst <c>false</c>.
        /// </returns>
        public bool Includes(TimeRange tr)
        {
            return tr.Start >= Start && tr.End <= End;
        }

        /// <summary>
        /// Teilt die Zeitspanne in zwei Teilzeitspannen, von denen eine vor dem gegebenen Zeitpunkt liegt und die andere danach.
        /// </summary>
        /// <param name="dt">
        /// Der Grenzzeitpunkt
        /// </param>
        /// <returns>
        /// Der <see cref="Array"/> der beiden Zeitspannen
        /// </returns>
        public TimeRange[] Split(DateTime dt)
        {
            TimeRange range1 = (Start, dt);
            TimeRange range2 = (dt, End);
            List<TimeRange> ranges = new List<TimeRange>();
            if (range1.Duration > TimeSpan.Zero)
                ranges.Add(range1);
            if (range2.Duration > TimeSpan.Zero)
                ranges.Add(range2);
            return ranges.ToArray();
        }

        /// <summary>
        /// Zerlegt die Zeitspanne in die einzelnen Tageskomponenten.
        /// </summary>
        /// <returns>
        /// Die einzelnen Tageskomponenten
        /// </returns>
        public TimeRange[] SplitDays()
        {
            List<TimeRange> days = new List<TimeRange>();
            DateTime split = Start;
            while (split < End)
                days.Add((split, split = split.AddDays(1).Date));
            if (split > End)
                days[days.Count - 1].End = End;
            return days.ToArray();
        }

        /// <summary>
        /// Entfernt aus der Zeitspanne eine gegebene Zeitspanne.
        /// </summary>
        /// <param name="tr">
        /// Die zu entfernende Zeitspanne
        /// </param>
        /// <returns>
        /// Die restliche Zeitspanne
        /// </returns>
        public TimeRange[] Exclude(TimeRange tr)
        {
            return Split(tr.Start).Concat(Split(tr.End)).Except(new[] { tr }).ToArray();
        }

        /// <summary>
        /// Als Vergleichsoperation genügt hier der Vergleich der Start- und Endzeitpunkte.
        /// </summary>
        /// <param name="obj">
        /// Ein Vergleichsobjekt
        /// </param>
        /// <returns>
        /// Gibt <c>true</c> bei Übereinstimmung wieder, sonst <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is TimeRange tr && tr.Start == Start && tr.End == End;
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
        /// Die standardmäßige Objektbeschreibung entspricht hier der Abfolge von Start- und Endzeitpunkt.
        /// </summary>
        /// <returns>
        /// Die Beschreibung des Objekts
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0} - {1}", Start.GetDisplayFormat(), End.GetDisplayFormat());
        }

        /// <summary>
        /// Konvertiert ein <see cref="ValueTuple{DateTime, DateTime}"/> in ein Objekt vom Datentyp <see cref="TimeRange"/>.
        /// </summary>
        /// <param name="range">
        /// Das Tupel des Start- und Endzeitpunkts
        /// </param>
        public static implicit operator TimeRange((DateTime Start, DateTime End) range)
        {
            return new TimeRange(range.Start, range.End);
        }
    }
}