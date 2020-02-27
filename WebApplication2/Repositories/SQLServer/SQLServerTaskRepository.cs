using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Domain;
using WebApplication2.Models;
using WebApplication2.Repositories;
using Dapper;

namespace WebApplication2.Repositories.SQLServer
{
    public class SQLServerTaskRepository : SQLServerBaseRepository<ProjectTask, int>, ITaskRepository
    {
        public override ProjectTask GetById(int id) // Get By ProjectId
        {
            return sv_connection.Query<ProjectTask>(@"SELECT TID as Tid, PID as Pid, UID as UidString, Name as Name, Description as Description, StartedOn as StartedOn,
                                                     DueOn as DueOn, Completed as Completed, CompletedOn as CompletedOn FROM rekrdd.dbo.Task WHERE TID = @taskid", new { taskid = id }).SingleOrDefault();
        }

        public List<ProjectTask> GetProjectTasksForEmployee(Guid empid)
        {
            return sv_connection.Query<ProjectTask>(@"SELECT tsk.TID as Tid, tsk.PID as Pid, tsk.UID as UidString, tsk.Name as Name, tsk.Description as Description, tsk.Startedon as Startedon,
                                                     tsk.DueOn as DueOn, tsk.Completed as Completed, tsk.CompletedOn as CompletedOn FROM [rekrdd].[dbo].Task as tsk INNER JOIN [dbo].TaskUser AS tskuser ON 
                                                     tsk.UID = tskuser.uid WHERE tsk.uid = @eid", new { eid = empid  }).ToList();
        }

        public List<ProjectTask> GetAllByProjectId(int pid)
        {
            return sv_connection.Query<ProjectTask>(@"SELECT TID as Tid, PID as Pid, UID as UidString, Name as Name, Description as Description, StartedOn as StartedOn,
                                                     DueOn as DueOn, Completed as Completed, CompletedOn as CompletedOn FROM rekrdd.dbo.Task WHERE PID = @projid", new { projid = pid }).ToList();
        }

        public ProjectTask GetByProjectAndName(int pid, string tnam)
        {
            return sv_connection.Query<ProjectTask>(@"SELECT TID as Tid, PID as Pid, UID as UidString, Name as Name, Description as Description, StartedOn as StartedOn,
                                                     DueOn as DueOn, Completed as Completed, CompletedOn as CompletedOn FROM rekrdd.dbo.Task WHERE PID = @projid AND Name = @nam", new { projid = pid, nam = tnam }).SingleOrDefault();
        }

        public List<ProjectTask> GetAllByUserId(Guid uid)
        {
            return sv_connection.Query<ProjectTask>(@"SELECT TID as Tid, PID as Pid, UID as UidString, Name as Name, Description as Description, StartedOn as StartedOn,
                                                     DueOn as DueOn, Completed as Completed, CompletedOn as CompletedOn FROM rekrdd.dbo.Task WHERE UID = @userid", new { userid = uid }).ToList();
        }

        public List<ProjectTask> GetAllByUserAndProjectId(Guid uid, int pid)
        {
            return sv_connection.Query<ProjectTask>(@"SELECT TID as Tid, PID as Pid, UID as UidString, Name as Name, Description as Description, StartedOn as StartedOn,
                                                     DueOn as DueOn, Completed as Completed, CompletedOn as CompletedOn FROM rekrdd.dbo.Task WHERE UID = @userid AND WHERE PID = @projid", new { userid = uid, projid = pid}).ToList();
        }

        public override void Save(ProjectTask obj)
        {
            sv_connection.Execute(@"INSERT INTO [rekrdd].[dbo].Task (PID, UID, Name, Description, StartedOn, DueOn, Completed, CompletedOn) 
            VALUES (@projid, @assigneeid, @taskname, @taskdesc, @taskstart, @taskDue, @taskComplete, @taskCompleteDate)", 
                new { projid = obj.Pid, assigneeid = obj.Uid, taskname = obj.Name, taskdesc = obj.Description, taskstart = obj.StartedOn, taskDue = obj.DueOn, taskComplete = obj.Completed, taskCompleteDate = obj.CompletedOn  });
        }

        public override void Update(ProjectTask obj)
        {
            sv_connection.Execute(@"UPDATE [rekrdd].[dbo].Task SET PID = @projid, UID = @assigneeid, Name = @taskname, Description = @taskdesc, StartedOn = @taskstart, DueOn = @taskDue, Completed = @taskComplete, CompletedOn = @taskCompleteDate WHERE TID = @task",
                new { projid = obj.Pid, assigneeid = obj.Uid, taskname = obj.Name, taskdesc = obj.Description, taskstart = obj.StartedOn, taskDue = obj.DueOn, taskComplete = obj.Completed, taskCompleteDate = obj.CompletedOn, task = obj.Tid });
        }

        public List<ProjectTask> GetAllTasks()
        {
            return sv_connection.Query<ProjectTask>(@"SELECT TID as Tid, PID as Pid, UID as Uid, Name as Name, Description as Description, StartedOn as StartedOn,
                                                     DueOn as DueOn, Completed as Completed, CompletedOn as CompletedOn FROM rekrdd.dbo.Task").ToList();
        }

        public List<ProjectTask> GetAllTasksForUserByProject(Guid uid, int pid)
        {
            return sv_connection.Query<ProjectTask>(@"SELECT TID as Tid, PID as Pid, UID as UidString, Name as Name, Description as Description, StartedOn as StartedOn, DueOn as DueOn, Completed as Completed, CompletedOn as CompletedOn
                                                      FROM rekrdd.dbo.Task
                                                      WHERE PID = @project AND UID = @user", new { project = pid, user = uid }).ToList();
        }

        public List<ProjectTask> GetAllOpenTasksForUserByProject(Guid uid, int pid)
        {
            return sv_connection.Query<ProjectTask>(@"SELECT TID as Tid, PID as Pid, UID as UidString, Name as Name, Description as Description, StartedOn as StartedOn, DueOn as DueOn, Completed as Completed, CompletedOn as CompletedOn
                                                      FROM rekrdd.dbo.Task
                                                      WHERE PID = @project AND Completed = 0 AND UID = @user", new { project = pid, user = uid }).ToList();
        }


        public List<ProjectTask> GetAllTasksNotForUserByProject(Guid uid, int pid)
        {
            return sv_connection.Query<ProjectTask>(@"SELECT TID as Tid, PID as Pid, UID as UidString, Name as Name, Description as Description, StartedOn as StartedOn, DueOn as DueOn, Completed as Completed, CompletedOn as CompletedOn
                                                      FROM rekrdd.dbo.Task
                                                      WHERE PID = @project AND UID != @user", new { project = pid, user = uid }).ToList();
        }

        public List<ProjectTask> GetAllOpenTasksForProject(int pid)
        {
            return sv_connection.Query<ProjectTask>(@"SELECT TID as Tid, PID as Pid, UID as UidString, Name as Name, Description as Description, StartedOn as StartedOn, DueOn as DueOn, Completed as Completed, CompletedOn as CompletedOn
                                                      FROM rekrdd.dbo.Task
                                                      WHERE PID = @project AND Completed = 0", new { project = pid }).ToList();
        }

        public List<ProjectTask> GetAllClosedTasksForProject(int pid)
        {
            return sv_connection.Query<ProjectTask>(@"SELECT TID as Tid, PID as Pid, UID as UidString, Name as Name, Description as Description, StartedOn as StartedOn, DueOn as DueOn, Completed as Completed, CompletedOn as CompletedOn
                                                      FROM rekrdd.dbo.Task
                                                      WHERE PID = @project AND Completed = 1", new { project = pid }).ToList();
        }

        public List<ProjectTask> GetAllUnassignedTasksForProject(int pid)
        {
            return sv_connection.Query<ProjectTask>(@"SELECT TID as Tid, PID as Pid, UID as UidString, Name as Name, Description as Description, StartedOn as StartedOn, DueOn as DueOn, Completed as Completed, CompletedOn as CompletedOn
                                                      FROM rekrdd.dbo.Task
                                                      WHERE UID = NULL AND PID = @project", new { project = pid }).ToList();
        }

    }
}