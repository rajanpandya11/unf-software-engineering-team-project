using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REKRDD_DataAccess.Domain
{
    public class User
    {
        // PKEY
        int UserId;

        string FirstName;
        string LastName;
        string Role; // I do not think we should use a string to define roles, yo
        DateTime Birthdate;
        DateTime EmploymentDate;
        int? CompletedTasks;
        int? LateTasks;
        int? CurrentProjects;
        int? LateProjects;
        int? SuccessfulProjects;
        int? ArchivedProjects;
        string PasswdHash;
    }
}
