using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication2.Domain;

namespace WebApplication2.Repositories
{
    public interface IProjectRepository {
    
        void Save(Project proj);
        void Update(Project proj);
        Project GetById(int pid);
    }
}
