using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace REKRDD_DataAccess.Repositories.SQLITE
{
    public class SQLiteUserRepository : IUserRepository
    {
        SQLiteConnection sqlconn;

        SQLiteUserRepository(SQLiteConnection sqlc)
        {
            sqlconn = sqlc;
        }



    }
}
