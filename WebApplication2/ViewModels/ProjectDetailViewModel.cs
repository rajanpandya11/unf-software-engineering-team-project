using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Domain;

namespace WebApplication2.ViewModels
{
    public class ProjectDetailViewModel
    {
        public Project Project { get; set; }
        public IEnumerable<ProjectTask> ProjectTasks { get; set; }
        public IEnumerable<RoleEmployeeViewModel> Employees { get; set; }    
    }
}