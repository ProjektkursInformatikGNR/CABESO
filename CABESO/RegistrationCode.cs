using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CABESO
{
    [Table("Codes")]
    public class RegistrationCode
    {
        [Key]
        public string Code { get; set; }
        public DateTime CreationTime { get; set; }
        public string Role { get; set; }

        public static RegistrationCode GetCodeByCode(string code)
        {
            return Database.Context.Codes.Find(code);
        }

        public override bool Equals(object obj)
        {
            return obj is RegistrationCode && (obj as RegistrationCode).Code.Equals(Code);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}