using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Domain;
using Dapper;
using WebApplication2.Repositories;

namespace WebApplication2.Repositories.SQLServer
{
    public class SQLServerProjectRepository : SQLServerBaseRepository<Project, int>, IProjectRepository
    {
        public override Project GetById(int id)
        {
            return sv_connection.Query<Project>("SELECT pid as pid, Name as Name, Description as Description, StartedOn as StartedOn, DueDate as DueDate, Completed as Completed, CompletedOn as CompletedOn, Archived as Archived, Budget as Budget, CurrentSpent as CurrentSpent, uid as OwnerString FROM dbo.Project WHERE pid = @projid", new { projid = id }).SingleOrDefault();
        }

        public override void Save(Project obj)
        {
            sv_connection.Execute("INSERT INTO dbo.Project (Name, Description, StartedOn, DueDate, Completed, CompletedOn, Archived, Budget, CurrentSpent, uid, Approved) VALUES (@nam, @desc, @son, @ddt, @comp, @compon, @arch, @budg, @spent, @own, @appr )",
                new { nam = obj.Name, desc = obj.Description, son = obj.StartedOn, ddt = obj.DueDate, comp = obj.Completed, compon = obj.CompletedOn, arch = obj.Archived, budg = obj.Budget, spent = obj.CurrentSpent, own = obj.Owner.ToString(), appr = obj.Approved});
        }

        public override void Update(Project obj)
        {
            sv_connection.Execute("UPDATE dbo.Project SET Name = @nam, Description = @desc, StartedOn = @son, DueDate = @ddt, Completed = @comp, CompletedOn = @compon, Archived = @arch, Budget = @budg, CurrentSpent = @spent, uid = @own, Approved = @appr WHERE pid = @id",
            new { nam = obj.Name, desc = obj.Description, son = obj.StartedOn, ddt = obj.DueDate, comp = obj.Completed, compon = obj.CompletedOn, arch = obj.Archived, budg = obj.Budget, spent = obj.CurrentSpent, own = obj.Owner, id = obj.pid, appr = obj.Approved });
        }

        public void Delete(int id)
        {
            sv_connection.Execute("DELETE FROM dbo.Project WHERE pid = @projid", new { projid = id });
        }

        public List<Project> GetAllProjects()
        {
            return sv_connection.Query<Project>("SELECT pid as pid, Name as Name, Description as Description, StartedOn as StartedOn, DueDate as DueDate, Completed as Completed, CompletedOn as CompletedOn, Archived as Archived, Budget as Budget, CurrentSpent as CurrentSpent, uid as OwnerString, Approved as Approved FROM dbo.Project").ToList();
        }

        public Project GetProjectByName(string projnam)
        {
            return sv_connection.Query<Project>("SELECT pid as pid, Name as Name, Description as Description, StartedOn as StartedOn, DueDate as DueDate, Completed as Completed, CompletedOn as CompletedOn, Archived as Archived, Budget as Budget, CurrentSpent as CurrentSpent, uid as OwnerString, Approved as Approved FROM dbo.Project WHERE Name = @nam", new { nam = projnam }).SingleOrDefault();
        }

        public void AddEmployeeToProject(Guid uid, int pid)
        {
            sv_connection.Execute("INSERT INTO dbo.UserProject (pid, uid) VALUES (@project, @user)", new { project = pid, user = uid });
        }

        public void RemoveEmployeeFromProject(Guid uid, int pid)
        {
            sv_connection.Execute("DELETE FROM dbo.UserProject WHERE pid = @project AND uid = @user", new { project = pid, user = uid });
        }

        public List<Project> GetAllProjectsForUser(Guid uid)
        {
            return sv_connection.Query<Project>("SELECT proj.pid as pid, Name as Name, Description as Description, StartedOn as StartedOn, DueDate as DueDate, Completed as Completed, CompletedOn as CompletedOn, Archived as Archived, Budget as Budget, CurrentSpent as CurrentSpent, proj.uid as OwnerString FROM dbo.Project as proj INNER JOIN dbo.UserProject as uproj ON proj.pid = uproj.pid WHERE uproj.uid = @user", new { user = uid}).ToList();
        }

        public List<Project> GetAllUnapprovedProjects()
        {
            return sv_connection.Query<Project>("SELECT proj.pid as pid, Name as Name, Description as Description, StartedOn as StartedOn, DueDate as DueDate, Completed as Completed, CompletedOn as CompletedOn, Archived as Archived, Budget as Budget, CurrentSpent as CurrentSpent, proj.uid as OwnerString FROM dbo.Project as proj WHERE proj.Approved = 0").ToList();
        }

        public List<Project> GetAllApprovedProjectsForUser(Guid uid)
        {
            return sv_connection.Query<Project>("SELECT proj.pid as pid, Name as Name, Description as Description, StartedOn as StartedOn, DueDate as DueDate, Completed as Completed, CompletedOn as CompletedOn, Archived as Archived, Budget as Budget, CurrentSpent as CurrentSpent, proj.uid as OwnerString FROM rekrdd.dbo.Project as proj INNER JOIN rekrdd.dbo.UserProject as uproj ON proj.pid = uproj.pid WHERE proj.uid = @user AND proj.Approved = 1", new { user = uid }).ToList();
        }

        public List<Project> GetAllUnapprovedProjectsForUser(Guid uid)
        {
            return sv_connection.Query<Project>("SELECT proj.pid as pid, Name as Name, Description as Description, StartedOn as StartedOn, DueDate as DueDate, Completed as Completed, CompletedOn as CompletedOn, Archived as Archived, Budget as Budget, CurrentSpent as CurrentSpent, proj.uid as OwnerString FROM rekrdd.dbo.Project as proj INNER JOIN rekrdd.dbo.UserProject as uproj ON proj.pid = uproj.pid WHERE proj.uid = @user AND proj.Approved = 0", new { user = uid }).ToList();
        }
    }
}