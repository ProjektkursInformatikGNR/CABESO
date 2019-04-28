using System;
using System.Linq;
using System.Reflection;

namespace CABESO
{
    public class Order
    {
        public int Id { get; set; }
        public Client Client { get; set; }
        public Product Product { get; set; }
        public DateTime OrderTime { get; set; }
        public string Notes { get; set; }
        public int Number { get; set; }
        public DateTime CollectionTime { get; set; }

        public static Order Create(int id)
        {
            return Database.Create<Order>($"[Id] = '{id}'", ("Client", "ClientId"), ("Product", "ProductId"));
        }

        public static Order Create(object[] data)
        {
            return data == null || data.Length != 7 ? null : new Order() { Id = (int) data[0], Client = Client.Create(data[1].ToString()), Product = Product.Create((int) data[2]), OrderTime = (DateTime) data[3], Notes = data[4].ToString(), Number = (int) data[5], CollectionTime = (DateTime) data[6] };
        }

        public static Order[] Enumerate()
        {
            return Database.Enumerate<Order>(("Client", "ClientId"), ("Product", "ProductId")).ToArray();
        }

        public void Archive()
        {
            Database.Add("OrderHistory", new { Id, ClientId = Client.Id, ProductId = Product.Id, OrderTime, Notes, Number, CollectionTime });
            Database.Delete("Orders", $"[Id] = '{Id}'");
        }

        public override bool Equals(object obj)
        {
            return obj is Order && (obj as Order).Id == Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static implicit operator Order(int id)
        {
            return Create(id);
        }
        public static implicit operator Order(HistoricOrder historicOrder)
        {
            Order order = new Order();
            foreach (PropertyInfo property in typeof(Order).GetProperties())
                property.SetValue(order, typeof(HistoricOrder).GetProperty(property.Name).GetValue(historicOrder));
            return order;
        }
    }
}