using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Domain
{
    public class ProjectTask
    {
        public int? Tid { get; set; }
        public int? Pid { get; set; }
        public Guid Uid { get; set; }
        public string UidString { get { return Uid.ToString(); } set { Uid = Guid.Parse(value); } }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartedOn { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [DisplayName("Due Date")]
        public DateTime DueOn { get; set; }

        public bool Completed { get; set; }

        public DateTime? CompletedOn { get; set; }
    }
}