using CABESO.Properties;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CABESO
{
    [Table("Forms")]
    public class Form
    {
        public int Id { get; set; }
        public string Stream { get; set; }
        public int Enrolment { get; set; }
        public Grade GetGrade() => Enrolment;

        public override string ToString()
        {
            return GetGrade().ToString() + (Enrolment > Grade.EF.Enrolment ? Stream : string.Empty);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is Form && (obj as Form).Id == Id;
        }

        public class Grade
        {
            private Grade(string name, int enrolment, int year)
            {
                _name = name;
                Enrolment = enrolment;
                Year = year;
            }

            public int Enrolment { get; private set; }
            public int Year { get; private set; }

            private readonly string _name;
            public override string ToString() => _name;

            public static Grade FOUR => new Grade("4", GetYearStart(DateTime.Now.AddYears(1)), 4);
            public static Grade FIVE => new Grade("5", GetYearStart(DateTime.Now), 5);
            public static Grade SIX => new Grade("6", GetYearStart(DateTime.Now.AddYears(-1)), 6);
            public static Grade SEVEN => new Grade("7", GetYearStart(DateTime.Now.AddYears(-2)), 7);
            public static Grade EIGHT => new Grade("8", GetYearStart(DateTime.Now.AddYears(-3)), 8);
            public static Grade NINE => new Grade("9", GetYearStart(DateTime.Now.AddYears(-4)), 9);
            public static Grade TEN => AdjustG8(new Grade("10", GetYearStart(DateTime.Now.AddYears(-5)), 10), grade => null);
            public static Grade EF => AdjustG8(new Grade("EF", GetYearStart(DateTime.Now.AddYears(-6)), 11), grade => { if (grade.Enrolment == 2017) return null; else { grade.Enrolment++; grade.Year--; return grade; } });
            public static Grade Q1 => AdjustG8(new Grade("Q1", GetYearStart(DateTime.Now.AddYears(-7)), 12), grade => { if (grade.Enrolment == 2017) return null; else { grade.Enrolment++; grade.Year--; return grade; } });
            public static Grade Q2 => AdjustG8(new Grade("Q2", GetYearStart(DateTime.Now.AddYears(-8)), 13), grade => { if (grade.Enrolment == 2017) return null; else { grade.Enrolment++; grade.Year--; return grade; } });
            public static Grade GRADUATE => new Grade(Resources.Graduate, -1, -1);

            public static Grade[] Grades => new[] { FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, EF, Q1, Q2, GRADUATE };

            private static int GetYearStart(DateTime time) => time.Year - (time.Month > 7 ? 0 : 1);
            private static Grade AdjustG8(Grade grade, Func<Grade, Grade> adjustment)
            {
                if (grade.Enrolment <= 2017)
                    return adjustment(grade);
                return grade;
            }

            public static implicit operator Grade(int i)
            {
                if (i >= FOUR.Year && i <= Q2.Year)
                    return Grades.FirstOrDefault(grade => grade != null && grade.Year == i);
                Grade g;
                if ((g = Grades.FirstOrDefault(grade => grade?.Enrolment == i)) != null)
                    return g;
                return GRADUATE;
            }
        }
    }
}