﻿using Microsoft.AspNetCore.Identity;
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
    }

    [Table("Orders")]
    public class CurrentOrder : Order
    {
    }
}