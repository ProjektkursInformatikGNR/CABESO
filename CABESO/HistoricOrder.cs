using CABESO.Data;
using System;
using System.Reflection;

namespace CABESO
{
    public class HistoricOrder
    {
        public int Id { get; set; }
        public Client Client { get; set; }
        public Product Product { get; set; }
        public DateTime OrderTime { get; set; }
        public string Notes { get; set; }
        public int Number { get; set; }
        public DateTime CollectionTime { get; set; }

        private ApplicationDbContext _context;

        public void Reactivate()
        {
            Database.Add("Orders", new { Id, ClientId = Client.Id, ProductId = Product.Id, OrderTime, Notes, Number, CollectionTime });
            Database.Delete("OrderHistory", $"[Id] = '{Id}'");
        }

        public override bool Equals(object obj)
        {
            return obj is HistoricOrder && (obj as HistoricOrder).Id == Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static implicit operator HistoricOrder(Order order)
        {
            HistoricOrder historicOrder = new HistoricOrder();
            foreach (PropertyInfo property in typeof(HistoricOrder).GetProperties())
                property.SetValue(historicOrder, typeof(Order).GetProperty(property.Name).GetValue(order));
            return historicOrder;
        }
    }
}