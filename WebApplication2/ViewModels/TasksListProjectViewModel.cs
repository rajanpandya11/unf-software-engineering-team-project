using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Domain;

namespace WebApplication2.ViewModels
{
    public class TasksListProjectViewModel
    {
        public List<ProjectTask> OpenTasks { get; set; }
        public List<ProjectTask> ClosedTasks { get; set; }
        public List<ProjectTask> UserTasks { get; set; }
        public List<ProjectTask> OtherTasks { get; set; }
        public int pid { get; set; }
    }
}