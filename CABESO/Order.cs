using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
            CollectionTime = CollectionTime,
            Id = Id,
            Notes = Notes,
            Number = Number,
            OrderTime = OrderTime,
            PreparationTime = PreparationTime,
            Product = Product,
            User = User
        };

        public bool DateIsValid()
        {
            if (CollectionTime.DayOfWeek == DayOfWeek.Saturday || CollectionTime.DayOfWeek == DayOfWeek.Sunday)
                return false;
            if (CollectionTime.TimeOfDay < new TimeSpan(8, 15, 0) || CollectionTime.TimeOfDay > new TimeSpan(12, 50, 0))
                return false;
            Holidays holidays = Holidays.Create();
            return true;
        }
    }
}