using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.ViewModels
{
    public class ProjectBudget
    {
        public string Name { get; set; }
        public long Budget { get; set; }
        public long Spent { get; set; }
    }
    public class TimeCompletion
    {
        public string Name { get; set; }
        public DateTime CompletedOn { get; set; }
        public bool Completed { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class Successes
    {
        public bool CompletedOn { get; set; }

    }
}