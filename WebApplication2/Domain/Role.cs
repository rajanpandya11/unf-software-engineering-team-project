using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Domain
{
    public class Role
    {
        public Guid? rid;
        public string ridstring { get { return rid.ToString(); } set { rid = Guid.Parse(value); } }

        public string Name;
    }
}