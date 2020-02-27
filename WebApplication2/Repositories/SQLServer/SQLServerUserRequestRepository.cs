using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Domain;

namespace WebApplication2.Repositories.SQLServer
{
    public class SQLServerUserRequestRepository : SQLServerBaseRepository<UserRequest, int>
    {

        public override void Save(UserRequest obj)
        {
            sv_connection.Execute("INSERT INTO rekrdd.dbo.UserRequest (Email, FirstName, LastName, BirthDate, Username, rid) VALUES ( @email, @fn, @ln, @bdate, @uname, @urol )", new { email = obj.Email, fn = obj.FirstName, ln = obj.LastName, bdate = obj.BirthDate, uname = obj.Username, urol = obj.UserRoles });
        }

        public override void Update(UserRequest obj)
        {
            sv_connection.Execute("UPDATE rekrdd.dbo.UserRequest (Email, FirstName, LastName, BirthDate, Username, rid) VALUES ( @email, @fn, @ln, @bdate, @uname, @rol ) WHERE Id = @id", new { email = obj.Email, fn = obj.FirstName, ln = obj.LastName, bdate = obj.BirthDate, uname = obj.Username, id = obj.Id, rol = obj.UserRoles });
        }

        public void Delete(int id)
        {
            sv_connection.Execute("DELETE FROM rekrdd.dbo.UserRequest WHERE Id = @rid", new { rid = id });
        }

        public override UserRequest GetById(int id)
        {
            return sv_connection.Query<UserRequest>("SELECT Id as Id, FirstName as FirstName, LastName as LastName, BirthDate as BirthDate, UserName as UserName, Email as Email, rid as UserRoles FROM rekrdd.dbo.UserRequest WHERE Id = @rid", new { rid = id }).SingleOrDefault();
        }

        public List<UserRequest> GetAllRequests()
        {
            return sv_connection.Query<UserRequest>("SELECT Id as Id, FirstName as FirstName, LastName as LastName, BirthDate as BirthDate, UserName as UserName, rid as UserRoles FROM rekrdd.dbo.UserRequest").ToList();
        }
    }
}