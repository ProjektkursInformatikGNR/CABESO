using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CABESO
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public IdentityUser User { get; set; }
        public Product Product { get; set; }
        public DateTime OrderTime { get; set; }
        public string Notes { get; set; }
        public int Number { get; set; }
        public DateTime? PreparationTime { get; set; }
        public DateTime CollectionTime { get; set; }

        public string CollectionPlace { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Order && (obj as Order).Id == Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    [Table("OrderHistory")]
    public class HistoricOrder : Order
    {
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

    [Table("Orders")]
    public class CurrentOrder : Order
    {
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

        public static TimeRange[] ValidCollectionTimes()
        {
            TimeRange[] validDays = ((TimeRange) (DateTime.Now.AddHours(1), DateTime.Now.AddDays(7))).SplitDays()
                .Where(day => day.Start.DayOfWeek != DayOfWeek.Saturday
                && day.Start.DayOfWeek != DayOfWeek.Sunday
                && !Holidays.Create().GetHolidays().Any(holiday => holiday.Date == day.Start.Date))
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

    public class TimeRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public TimeRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public TimeSpan Duration { get => End - Start; }

        public bool Includes(DateTime dt)
        {
            return dt >= Start && dt <= End;
        }

        public bool Includes(TimeRange tr)
        {
            return tr.Start >= Start && tr.End <= End;
        }

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

        public TimeRange[] Exclude(TimeRange tr)
        {
            return Split(tr.Start).Concat(Split(tr.End)).Except(new[] { tr }).ToArray();
        }

        public override bool Equals(object obj)
        {
            return obj is TimeRange tr && tr.Start == Start && tr.End == End;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Start.GetDisplayFormat(), End.GetDisplayFormat());
        }

        public static implicit operator TimeRange((DateTime Start, DateTime End) range)
        {
            return new TimeRange(range.Start, range.End);
        }
    }
}