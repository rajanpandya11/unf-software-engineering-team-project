using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Domain
{
    public class AspNetUser
    {
        public Guid? uid { get; set; }

        public string SetUidStr { set { uid = Guid.Parse(value); } }


        public string Email { get; set; }
        public string UserName { get; set; }
    }
}