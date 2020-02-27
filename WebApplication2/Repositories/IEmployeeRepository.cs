using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication2.Domain;

namespace WebApplication2.Repositories
{
    public interface IEmployeeRepository
    {
        Employee GetById(Guid uid);
        void Save(Employee emp);
        void Update(Employee emp);


    }
}
