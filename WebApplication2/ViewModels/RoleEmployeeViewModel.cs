using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.ViewModels
{
    public class RoleEmployeeViewModel
    {
        public String Rolename { get; set; }
        public String Firstname { get; set; }
        public String Lastname { get; set; }

        public Guid Guid { get; set; }

    }
}