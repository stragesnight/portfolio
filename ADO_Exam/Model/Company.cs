using System;
using System.Collections.Generic;

namespace ADO_Exam.Model
{
    public class Company
    {
        public Company()
        {
            CompanyBranches = new List<CompanyBranch>();
            Games = new List<Game>();
        }

        public Company(string name)
        {
            CompanyBranches = new List<CompanyBranch>();
            Games = new List<Game>();
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<CompanyBranch> CompanyBranches { get; set; }
        public ICollection<Game> Games { get; set; }
    }
}
