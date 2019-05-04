using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CABESO
{
    [Table("Products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        public static Product GetProductById(int id)
        {
            return Database.Context.Products.Find(id);
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
    }
}