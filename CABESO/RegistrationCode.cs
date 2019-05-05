using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CABESO
{
    [Table("Codes")]
    public class RegistrationCode
    {
        [Key]
        public string Code { get; set; }
        public DateTime CreationTime { get; set; }
        public IdentityRole Role { get; set; }

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