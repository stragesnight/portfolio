using System;
using System.Collections.Generic;

namespace ADO_Exam.Model
{
    public class City
    {
        public City()
        {
            CompanyBranches = new List<CompanyBranch>();
        }

        public City(int countryId, string name)
        {
            CompanyBranches = new List<CompanyBranch>();
            CountryId = countryId;
            Name = name;
        }

        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<CompanyBranch> CompanyBranches { get; set; }
    }
}
