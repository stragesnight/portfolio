using System;

namespace ADO_Exam.Model
{
    public class CompanyBranch
    {
        public CompanyBranch() { }

        public CompanyBranch(int companyId, int cityId)
        {
            CompanyId = companyId;
            CityId = cityId;
        }

        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int CityId { get; set; }

        public virtual Company Company { get; set; }
        public virtual City City { get; set; }
    }
}
