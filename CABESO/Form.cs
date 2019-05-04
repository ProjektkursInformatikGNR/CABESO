﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CABESO
{
    [Table("Forms")]
    public class Form
    {
        public int Id { get; set; }
        public string Section { get; set; }
        public int Year { get; set; }

        public override string ToString()
        {
            int ef = DateTime.Now.Year - (DateTime.Now.Month > 7 ? 0 : 1) - (Year > 2018 ? 6 : 5);
            switch (ef - Year)
            {
                case int i when i < 0:
                    return (i + 10) + Section;
                case 0:
                    return "EF";
                default:
                    return "Q" + (ef - Year);
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is Form && (obj as Form).Id == Id;
        }
    }
}