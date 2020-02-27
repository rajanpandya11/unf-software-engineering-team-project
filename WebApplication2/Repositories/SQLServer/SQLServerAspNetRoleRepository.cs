using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Domain;
using WebApplication2.Repositories.SQLServer;

namespace WebApplication2.Repositories.SQLServer
{
    public class SQLServerAspNetRoleRepository : SQLServerBaseRepository<Role, Guid>
    {
        public override void Save(Role obj)
        {
            throw new NotImplementedException("This repo is read only.");
        }

        public override void Update(Role obj)
        {
            throw new NotImplementedException("This repo is read only.");
        }

        public override Role GetById(Guid id)
        {
            return sv_connection.Query<Role>("SELECT Id as ridstring, Name as Name FROM dbo.AspNetRoles WHERE Id = @guid", new { guid = id }).FirstOrDefault();
        }

        public Role GetByUserId(Guid id)
        {
            return sv_connection.Query<Role>("SELECT rol.Id as ridstring, rol.Name as Name FROM dbo.AspNetRoles as rol INNER JOIN dbo.AspNetUserRoles as usrrol on usrrol.RoleId = rol.Id INNER JOIN dbo.AspNetUsers as usr on usr.Id = usrrol.UserId WHERE usr.Id = @uid", new { uid = id }).SingleOrDefault();
        }

        public Role GetRoleByUsername(string uname)
        {
            return sv_connection.Query<Role>("Select rol.Id as ridstring, rol.Name as Name FROM dbo.AspNetRoles as rol INNER JOIN dbo.AspNetUserRoles as usrrol ON usrrol.RoleId = rol.Id INNER JOIN dbo.AspNetUsers as usr ON usr.Id = usrrol.UserId WHERE UserName = @uname", new { unam = uname }).FirstOrDefault();
        }
    }
}