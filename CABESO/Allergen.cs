using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CABESO
{
    [Table("Allergens")]
    public class Allergen
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return Description;
        }

        public override bool Equals(object obj)
        {
            return obj is Allergen && (obj as Allergen).Id == Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}