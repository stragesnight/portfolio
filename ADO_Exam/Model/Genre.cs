using System;
using System.Collections.Generic;

namespace ADO_Exam.Model
{
    public class Genre
    {
        public Genre()
        {
            Games = new List<Game>();
        }

        public Genre(string name)
        {
            Games = new List<Game>();
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}
