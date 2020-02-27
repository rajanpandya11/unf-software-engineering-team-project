using REKRDD_DataAccess.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REKRDD_DataAccess.Repositories
{
    public interface IUserRepository
    {
        User GetById(int uid);
        void SaveOrUpdate(User usr);


    }
}
