using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Domain
{
    public class Project
    {
        public int pid { get; set; }
        public Guid Owner { get; set; }
        public string OwnerString { get { return Owner.ToString(); } set { Owner = Guid.Parse(value); } }

        [DisplayName("Project Name")]
        public string Name { get; set; }
        public string Description { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString ="{0:d}")]
        [DisplayName("Project Start Date")]
        public DateTime StartedOn { get; set; }
        public string ParseStartedOn { set { StartedOn = DateTime.Parse(value); } }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [DisplayName("Project Due Date")]
        public DateTime DueDate { get; set; }

        public string ParseDueDate { set { DueDate = DateTime.Parse(value); } }

        public bool Completed { get; set; }
        public bool Archived { get; set; }

        public int CurrentSpent { get; set; }
        public int Budget { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [DisplayName("Project Completion Date")]
        [DataType(DataType.DateTime)]
        public DateTime? CompletedOn { get; set; }
        public string ParseCompletedOn { set { CompletedOn = DateTime.Parse(value); } }

        public bool Approved { get; set; }

    }
}