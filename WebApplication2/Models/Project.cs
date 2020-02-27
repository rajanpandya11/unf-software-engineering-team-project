using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Project Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Date Started")]
        [DisplayFormat(DataFormatString = "{0:MMMM-dd-yyyy, hh.mm tt}", ApplyFormatInEditMode = true)]
        public DateTime StartedOn { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of Completion")]
        public DateTime DueDate { get; set; }

        [Required]
        [Display(Name = "Is finished?")]
        public bool IsCompleted { get; set; }

        [Required]
        [Display(Name = "Is archived?")]
        public bool IsArchived { get; set; }

        [Required]
        [Display(Name = "Is late?")]
        public bool IsLate { get; set; }

        public bool IsOverbudgeted { get; set; }

        public bool IsRequest { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double Budget { get; set; }

       // public IEnumerable<ProjectTask> Tasks { get; set; }

        //public IEnumerable<Employee> Employees { get; set; }
    }
}