using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Domain;

namespace WebApplication2.ViewModels
{
    public class AddMemberProjectViewModel
    {
        public int ProjectId { get; set; }
        public List<Employee> Employees { get; set; }
    }
}