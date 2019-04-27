using System.Linq;

namespace CABESO
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal? Sale { get; set; }
        public string Picture { get; set; }
        public string[] Allergens { get; set; }
        public bool Vegetarian { get; set; }
        public bool Vegan { get; set; }
        public string Size { get; set; }
        public decimal? Deposit { get; set; }
        public string Information { get; set; }

        public static Product Create(int id)
        {
            return Database.Create<Product>($"[Id]={id}");
        }

        public static Product Create(object[] data)
        {
            return data == null ? null : new Product() { Id = (int) data[0], Name = data[1].ToString(), Price = (decimal) data[2], Sale = data[3] as decimal?, Picture = data[4].ToString(), Allergens = data[5]?.ToString().Split('|'), Vegetarian = (bool) data[6], Vegan = (bool) data[7], Size = data[8].ToString(), Deposit = data[9] as decimal?, Information = data[10].ToString() };
        }

        public static Product[] Enumerate()
        {
            return Database.Enumerate<Product>().ToArray();
        }

        public override bool Equals(object obj)
        {
            return obj is Product && (obj as Product).Id == Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}{1} - {2}", Name, string.IsNullOrEmpty(Size) ? string.Empty : string.Format(" ({0})", Size), string.Format("{0:C2}", Price - (Sale ?? 0m)) + (Deposit.HasValue ? string.Format(" (+ {0:C2} Pfand)", Deposit) : string.Empty)); 
        }

        public static implicit operator Product(int id)
        {
            return Create(id);
        }
    }
}