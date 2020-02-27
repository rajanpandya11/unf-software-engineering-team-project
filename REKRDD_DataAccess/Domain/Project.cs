using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REKRDD_DataAccess.Domain
{
    public class Project
    {
        int ProjectId;

        string ProjName;
        string ProjDesc;
        bool Completed;
        DateTime DueOn;
        bool Archived;
        double Budget;
        bool Late;
        bool Overbudget;

        DateTime Started;
    }
}
