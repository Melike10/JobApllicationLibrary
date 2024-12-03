using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApllicationLibrary.Models
{
    public class JobApplication
    {
        public Applicant Applicant { get; set; }
        public int YearsOfExperience { get; set; }
        public List<string> TechStackList { get; set; }
        public string OfficeLocation { get; set; }
        public ValidationMode ValidationMode { get; set; }
    }
    public enum ValidationMode
    {
        Detail,
        Quick
    }
}
