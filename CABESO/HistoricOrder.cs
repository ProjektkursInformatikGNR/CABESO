using System;
using System.Linq;
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
        public static HistoricOrder Create(int id)
        {
            return Create(Database.Select("OrderHistory", $"[Id]={id}").FirstOrDefault());
        }

        public static HistoricOrder Create(object[] data)
        {
            return data == null || data.Length != 7 ? null : new HistoricOrder() { Id = (int) data[0], Client = Client.Create(data[1].ToString()), Product = Product.Create((int) data[2]), OrderTime = (DateTime) data[3], Notes = data[4].ToString(), Number = (int) data[5], CollectionTime = (DateTime) data[6] };
        }

        public static HistoricOrder[] Enumerate()
        {
            return Array.ConvertAll(Database.Select("OrderHistory", null), data => Create(data));
        }

        public void Reactivate()
        {
            Database.Add("Orders", new { Id, ClientId = Client.Id, ProductId = Product.Id, OrderTime, Notes, Number, CollectionTime });
            Database.Delete("OrderHistory", $"[Id]={Id}");
        }

        public override bool Equals(object obj)
        {
            return obj is HistoricOrder && (obj as HistoricOrder).Id == Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static implicit operator HistoricOrder(int id)
        {
            return Create(id);
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