using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Domain
{
    public class UserRequest
    {
        public int? Id { get; set; }
        [Required]
        [Display(Name = "User Role")]
        public string UserRoles { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        [Required]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}