using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Domain
{
    public class ProjectIndexModel
    {
        public List<Project> YourProjects { get; set; }
        public List<Project> UnapprovedProject { get; set; }

    }
}