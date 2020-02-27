using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REKRDD_DataAccess.Repositories
{
    public interface ITaskRepository
    {
        void SaveOrUpdate(Task tsk);
        Task GetById(int id);
    }
}
