using REKRDD_DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REKRDD_DataAccess.Database
{
    public interface IREKRDDDatabase
    {
        // Add repositories here.

        IProjectRepository REKRDDProjectRepository();
        ITaskRepository REKRDDTaskRepository();
        IUserRepository REKRDDUserRepository();
    }
}
