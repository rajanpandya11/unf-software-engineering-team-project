using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Domain;
using WebApplication2.Repositories.SQLServer;

namespace WebApplication2.Repositories.SQLServer
{
    public class SQLServerAspNetUserRepository : SQLServerBaseRepository<AspNetUser, Guid>
    {
        public override void Save(AspNetUser obj)
        {
            throw new NotImplementedException("This repo is read only.");
        }

        public override void Update(AspNetUser obj)
        {
            throw new NotImplementedException("This repo is read only.");
        }

        public override AspNetUser GetById(Guid id)
        {
            return sv_connection.Query<AspNetUser>("SELECT Id, Email, UserName FROM dbo.AspNetUsers WHERE Id = @guid", new { guid = id }).FirstOrDefault();
        }

        public AspNetUser GetByUsername(string uname)
        {
            return sv_connection.Query <AspNetUser>("Select Id as SetUidStr, Email, Username FROM dbo.AspNetUsers WHERE UserName = @unam", new { unam = uname }).FirstOrDefault();
        }
    }
}