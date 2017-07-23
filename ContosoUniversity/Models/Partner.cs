using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Partner
    {
        public int PartnerID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        //public virtual ICollection<Course> Courses { get; set; }
    }
}