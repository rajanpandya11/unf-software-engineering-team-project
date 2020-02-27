using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication2.Domain;

namespace WebApplication2.Repositories
{
    public interface ITaskRepository
    {
        void Save(ProjectTask tsk);
        void Update(ProjectTask tsk);
        ProjectTask GetById(int id);
    }
}
