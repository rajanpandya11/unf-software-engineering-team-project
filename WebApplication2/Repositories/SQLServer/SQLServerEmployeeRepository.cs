using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Domain;
using WebApplication2.Repositories;
using Dapper;

namespace WebApplication2.Repositories.SQLServer
{
    public class SQLServerEmployeeRepository : SQLServerBaseRepository<Employee, Guid>, IEmployeeRepository
    {
        public override void Save(Employee obj)
        {
            sv_connection.Execute(@"INSERT INTO rekrdd.dbo.Employee (uid, FirstName, LastName, Birthdate, Joindate) VALUES (@uid, @fn, @ln, @bd, @jd)", new { uid = obj.uid, fn = obj.FirstName, ln = obj.LastName, bd = obj.Birthdate, jd = obj.Joindate });
        }

        public override void Update(Employee obj)
        {
            sv_connection.Execute(@"UPDATE rekrdd.dbo.Employee (FirstName, LastName, Birthdate, Joindate) VALUES (@fn, @ln, @bd, @jd) WHERE uid = @uid", new { uid = obj.uid, fn = obj.FirstName, ln = obj.LastName, bd = obj.Birthdate, jd = obj.Joindate });
        }

        public bool DoesIdExist(Guid uid)
        {
            var Field = sv_connection.Query<Employee>("SELECT uid as uidstring FROM rekrdd.dbo.Employee WHERE uid = @guid", new { guid = uid }).SingleOrDefault();
            if(Field != null)
            {
                return true;
            }
            return false;
        }

        public Employee GetFullEmployeeDataById(Guid uid)
        {
            return sv_connection.Query<Employee>("SELECT emp.uid as uidstring, emp.FirstName, emp.LastName, emp.Birthdate, emp.Joindate, aspu.Email, aspu.UserName FROM [rekrdd].[dbo].[Employee] as emp INNER JOIN [rekrdd].[dbo].AspNetUsers as aspu on emp.uid = aspu.Id WHERE uid = @guid", new { guid = uid }).FirstOrDefault();
        }

        public bool DoesNameExist(string fn, string ln)
        {
            if(sv_connection.Execute("SELECT emp.uid as uidstring FROM rekrdd.dbo.Employee as emp WHERE emp.FirstName = @first AND emp.LastName = @last", new { first = fn, last = ln }) > 0)
            {
                return true;
            }
            return false;
        }

        public Employee GetFullEmployeeByName(string fn, string ln)
        {
            return sv_connection.Query<Employee>("SELECT emp.uid as uidstring, emp.FirstName, emp.LastName, emp.Birthdate, emp.Joindate, aspu.Email, aspu.UserName FROM [rekrdd].[dbo].[Employee] as emp INNER JOIN [rekrdd].[dbo].AspNetUsers as aspu on emp.uid = aspu.Id WHERE emp.FirstName = @fnam AND WHERE emp.LastName = @lnam", new { fnam = fn, lnam = ln }).FirstOrDefault();
        }

        public Employee GetByUsername(string username)
        {
            return sv_connection.Query<Employee>("SELECT emp.uid as uidstring, emp.FirstName, emp.LastName, emp.Birthdate, emp.Joindate, aspu.Email, aspu.UserName FROM [rekrdd].[dbo].[Employee] as emp INNER JOIN [rekrdd].[dbo].AspNetUsers as aspu on emp.uid = aspu.Id WHERE aspu.UserName = @uname", new { uname = username }).FirstOrDefault();
        }

        public Employee GetByEmail(string email)
        {
            return sv_connection.Query<Employee>(@"SELECT emp.uid as uidstring, emp.FirstName, emp.LastName, emp.Birthdate, emp.Joindate, aspu.Email, aspu.UserName 
                                                   FROM [rekrdd].[dbo].[Employee] as emp INNER JOIN [rekrdd].[dbo].AspNetUsers as aspu 
                                                   on emp.uid = aspu.Id WHERE aspu.Email = @eml", new { eml = email }).FirstOrDefault();
        }

        public override Employee GetById(Guid uid)
        {
            return sv_connection.Query<Employee>("SELECT uid as uidstring, FirstName as FirstName, LastName as LastName, Birthdate as Birthdate, Joindate as Joindate FROM rekrdd.dbo.Employee WHERE uid = @id ", new { id = uid }).FirstOrDefault();
        }

        public List<Employee> GetAllEmployees()
        {
            return sv_connection.Query<Employee>("SELECT Emp.uid as uidstring, Emp.FirstName as FirstName, Emp.LastName as LastName, Emp.Birthdate as Birthdate, Emp.Joindate as Joindate FROM rekrdd.dbo.Employee as Emp INNER JOIN rekrdd.dbo.AspNetUsers as asp ON Emp.uid = asp.Id").AsList();
        }

        public List<Employee> GetAllEmployeesForProject(int pid)
        {
            return sv_connection.Query<Employee>(@"SELECT emp.uid as uidstring, emp.FirstName as FirstName, emp.LastName as LastName, emp.Birthdate as Birthdate, emp.Joindate as Joindate FROM rekrdd.dbo.Employee as emp INNER JOIN rekrdd.dbo.AspNetUsers as asp ON asp.Id = emp.uid INNER JOIN rekrdd.dbo.UserProject as uproj ON uproj.uid = emp.uid WHERE uproj.pid = @project", new {project = pid}).ToList();
        }

        public List<Employee> GetAllEmployeesByRoleId(Guid rid)
        {
            return sv_connection.Query<Employee>(@"SELECT emp.uid as uidstring, emp.FirstName as FirstName, emp.LastName as LastName, emp.Birthdate as Birthdate, emp.Joindate as Joindate FROM rekrdd.dbo.Employee as emp INNER JOIN rekrdd.dbo.AspNetUsers as asp ON asp.Id = emp.uid INNER JOIN rekrdd.dbo.AspNetUserRoles as urol ON urol.UserId = emp.uid WHERE urol.RoleId = @rolid", new { rolid = rid }).ToList();
        }

        public List<Employee> GetAllEmployeesByRoleName(string rnam)
        {
            return sv_connection.Query<Employee>(@"SELECT emp.uid as uidstring, emp.FirstName as FirstName, emp.LastName as LastName, emp.Birthdate as Birthdate, emp.Joindate as Joindate FROM rekrdd.dbo.Employee as emp INNER JOIN rekrdd.dbo.AspNetUsers as asp ON asp.Id = emp.uid INNER JOIN rekrdd.dbo.AspNetUserRoles as urol ON urol.UserId = emp.uid INNER JOIN rekrdd.dbo.AspNetRoles as rols ON urol.RoleId = rols.Id WHERE rols.Name  = @rolnam", new { rolnam = rnam }).ToList();
        }

        public List<Employee> GetAllEmployeesNotAssignedToProject(int pid)
        {
            return sv_connection.Query<Employee>(@"SELECT emp.uid as uidstring, emp.FirstName as FirstName, emp.LastName as LastName, emp.Birthdate as Birthdate, emp.Joindate as Joindate
                                                   FROM rekrdd.dbo.Employee as emp 
                                                   WHERE NOT EXISTS (SELECT uproj.uid FROM rekrdd.dbo.UserProject as uproj WHERE uproj.pid = @pid AND uproj.uid = emp.uid)", new { project = pid }).ToList();
        }

        public List<Employee> GetAllEmployeesInRoleNotAssignedToProject(int pid, string rname)
        {
            return sv_connection.Query<Employee>(@"SELECT emp.uid as uidstring, emp.FirstName as FirstName, emp.LastName as LastName, emp.Birthdate as Birthdate, emp.Joindate as Joindate
                                                    , rol.Name AS Role FROM rekrdd.dbo.Employee as emp
                                                      INNER JOIN rekrdd.dbo.AspNetUserRoles as urol ON urol.UserId = emp.uid
                                                      INNER JOIN rekrdd.dbo.AspNetRoles as rol ON urol.RoleId = rol.Id
                                                      WHERE NOT EXISTS (SELECT uproj.uid FROM rekrdd.dbo.UserProject as uproj WHERE uproj.pid = @project AND emp.uid = uproj.uid)
                                                      AND rol.Name = @rnam", new { project = pid, rnam = rname }).ToList();
        }
    }
}