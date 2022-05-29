using System;
using System.Collections.Generic;

namespace ADO_Exam.Model
{
    public class Country
    {
        public Country()
        {
            Cities = new List<City>();
        }

        public Country(string name)
        {
            Cities = new List<City>();
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<City> Cities { get; set; }
    }
}
