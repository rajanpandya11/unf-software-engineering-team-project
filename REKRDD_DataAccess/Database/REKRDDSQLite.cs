using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SQLite;

namespace REKRDD_DataAccess.Database
{
    public class REKRDDSQLite : IREKRDDDatabase
    {
        SQLiteConnection dbconn;

        public REKRDDSQLite(SQLiteConnection sqlconn)
        {
            // Insert checking code here.

            dbconn = sqlconn;

        }

        // Return repositories below.

    }
}
