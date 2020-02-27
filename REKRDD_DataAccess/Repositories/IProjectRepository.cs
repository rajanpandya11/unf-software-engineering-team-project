using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using REKRDD_DataAccess.Domain;

namespace REKRDD_DataAccess.Repositories
{
    public interface IProjectRepository
    {
        void SaveOrUpdate(Project proj);
        Project GetById(int pid);
    }
}
