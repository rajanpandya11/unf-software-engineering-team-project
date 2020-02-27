using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Domain
{
    public class Employee
    {
        [Key]
        public Guid? uid;
        public string uidstring { get { return uid.ToString(); } set { uid = Guid.Parse(value); } }

        public string FirstName;

        public string LastName;

        public DateTime Birthdate;
        public DateTime Joindate;
    }

}