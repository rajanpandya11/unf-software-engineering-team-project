using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication2.Repositories.SQLServer
{
    public abstract class SQLServerBaseRepository<T, IDType> : IDisposable 
            where T : class
            where IDType : struct
    {
        protected SqlConnection sv_connection;

        protected SQLServerBaseRepository()
        {
            string constr = ConfigurationManager.ConnectionStrings["TestServerConnection"].ToString();
            sv_connection = new SqlConnection(constr);
        }

        public abstract T GetById(IDType id);
        public abstract void Update(T obj);
        public abstract void Save(T obj);

        public void Dispose()
        {
            sv_connection.Dispose();
        }

    }
}