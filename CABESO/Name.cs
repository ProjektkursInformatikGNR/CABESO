using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace CABESO
{
    public class Name : IComparable
    {
        public Name(string mail, bool student)
        {
            if (string.IsNullOrEmpty(mail))
                return;

            Match[] matches = Regex.Matches(mail, @"([^0-9\.])+(?=.*@gnr.wwschool\.de)").ToArray();

            if (matches.Length != 2)
            {
                FirstName = mail;
                return;
            }

            string[] names = Array.ConvertAll(matches, m => m.Value.ToLower());

            if (!student)
                names = names.Reverse().ToArray();

            char[] firstName = names[1].ToCharArray();
            firstName[0] = char.ToUpper(firstName[0]);
            for (int i = 1; i < firstName.Length; i++)
                if (!char.IsLetter(firstName[i - 1]))
                    firstName[i] = char.ToUpper(firstName[i]);

            string[] lastNames = names[0].Split('-');
            lastNames[lastNames.Length - 1] = char.ToUpper(lastNames[lastNames.Length - 1][0]) + (lastNames[lastNames.Length - 1].Length > 1 ? lastNames[lastNames.Length - 1].Substring(1) : "");

            FirstName = new string(firstName);
            LastName = string.Join(' ', lastNames);
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public override string ToString()
        {
            return (FirstName + " " + LastName).Trim();
        }

        public int CompareTo(object obj)
        {
            if (obj is Name)
                return ToString().CompareTo((obj as Name).ToString());
            else if (obj is string)
                return ToString().CompareTo(obj as string);
            else
                return -1;
        }

        public static implicit operator string(Name name)
        {
            return name?.ToString();
        }
    }
}
