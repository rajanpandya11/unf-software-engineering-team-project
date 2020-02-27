using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebApplication2.Domain;
using WebApplication2.Models;
using WebApplication2.Repositories.SQLServer;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    [Authorize(Roles = "Employee, Manager, Department Manager")]
    public class ProjectTasksController : Controller
    {
        private SQLServerTaskRepository db;
        private SQLServerEmployeeRepository empdb;
        private SQLServerAspNetUserRepository aspuserdb;

        public ProjectTasksController()
        {
            db = new SQLServerTaskRepository();
            empdb = new SQLServerEmployeeRepository();
            aspuserdb = new SQLServerAspNetUserRepository();
        }

        // GET: ProjectTasks
        public ActionResult Index(int id)
        {
            var allEmployees = empdb.GetAllEmployeesForProject(id);
            var empGuid = Guid.Parse(User.Identity.GetUserId());
            //var emp = empdb.GetById(empGuid);
            var found = allEmployees.Find(x => x.uid == empGuid);
            if (found!=null)
            {
                var openTasks = db.GetAllOpenTasksForProject(id);
                var closedTasks = db.GetAllClosedTasksForProject(id);
                var forUser = db.GetAllOpenTasksForUserByProject(empGuid, id);
                var notForUser = db.GetAllTasksNotForUserByProject(empGuid, id); //new List<ProjectTask>(); 
                var viewModel = new TasksListProjectViewModel
                {
                    pid = id,
                    OpenTasks = openTasks,
                    ClosedTasks = closedTasks,
                    UserTasks = forUser,
                    OtherTasks = notForUser
                };
                return View("NewIndex", viewModel);
            }
            return HttpNotFound("No task found");
        }    

        // GET: ProjectTasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = db.GetById((int)id);
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            return View(projectTask);
        }

        // GET: ProjectTasks/Create
        public ActionResult Create(int pid, Guid guid)
        {
            var task = new ProjectTask();
            task.Pid = pid;
            task.Uid = guid;
            task.DueOn = DateTime.Now;
            return View(task);
        }

        // POST: ProjectTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectTask projectTask)
        {
            if (ModelState.IsValid)
            {
                var user = aspuserdb.GetByUsername(User.Identity.Name);
                projectTask.Uid = (Guid)user.uid;
                projectTask.StartedOn = System.DateTime.Now;              
                db.Save(projectTask);
                return RedirectToAction("Index", new { id = projectTask.Pid });
            }
            return View(projectTask);
        }

        [HttpPost]
        public void FinishTask(int tid)
        {
            var task = db.GetById(tid);
            if (task != null)
            {
                task.Completed = true;
                task.CompletedOn = DateTime.Now;
                db.Update(task);
            }
        }

        // GET: ProjectTasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = db.GetById((int)id);
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            return View(projectTask);
        }

        // POST: ProjectTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartedOn,DueDate,IsCompleted,IsAssigned,IsLate,CompletedBy,CurrentAssignee")] ProjectTask projectTask)
        {
            if (ModelState.IsValid)
            {
                db.Update(projectTask);
                return RedirectToAction("Index");
            }
            return View(projectTask);
        }

        // GET: ProjectTasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = db.GetById((int)id);
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            return View(projectTask);
        }

        // POST: ProjectTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //ProjectTask projectTask = db.ProjectTasks.Find(id);
            //db.ProjectTasks.Remove(projectTask);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
