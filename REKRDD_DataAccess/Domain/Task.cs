using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REKRDD_DataAccess.Domain
{
    public class Task
    {
        int TaskId;
        int ProjectId;

        int UserId;
        string ProjectName;
        string ProjectDescription;
        bool Completed;
        bool Assigned;
        int CompletedBy;
        int AssignedTo;
        bool Late;
        DateTime Datetime;

    }
}
