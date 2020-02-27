using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REKRDD_DataAccess.Repositories
{
    public abstract class SQLiteBaseRepository<T> where T : class
    {
        protected readonly SQLiteConnection Connection;

        public )
        {
            Connection = conn;
        }

        public abstract T GetById(int id);
        public abstract void SaveOrUpdate(T obj);
    }
}
