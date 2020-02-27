using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication2.Models
{
    public class UserRoleViewModel
    {
        public IOrderedEnumerable<IdentityRole> Roles { get; set; }
        public IOrderedEnumerable<ApplicationUser> Users { get; set; }
    }
}