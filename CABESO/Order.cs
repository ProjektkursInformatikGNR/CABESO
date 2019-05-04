using Microsoft.AspNetCore.Identity;
using System;
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
        public DateTime CollectionTime { get; set; }

        public static Order GetOrderById(int id)
        {
            return Database.Context.Orders.Find(id);
        }

        [NotMapped]
        private bool active;

        [NotMapped]
        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                if (active = value)
                {
                    Database.Context.Orders.Add(this as CurrentOrder);
                    Database.Context.HistoricOrders.Remove(this as HistoricOrder);
                    Database.Context.SaveChanges();
                }
                else
                {
                    Database.Context.HistoricOrders.Add(this as HistoricOrder);
                    Database.Context.Orders.Remove(this as CurrentOrder);
                    Database.Context.SaveChanges();
                }
            }
        }

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

    }

    [Table("Orders")]
    public class CurrentOrder : Order
    {

    }
}