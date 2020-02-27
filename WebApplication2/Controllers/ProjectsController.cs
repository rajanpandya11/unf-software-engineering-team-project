using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.Security;
using WebApplication2.Domain;
using WebApplication2.Repositories.SQLServer;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    [Authorize(Roles = "Employee,Manager,Department Manager")]
    public class ProjectsController : Controller
    {
        private SQLServerProjectRepository projectdb;
        private SQLServerAspNetUserRepository aspuserdb;
        private SQLServerEmployeeRepository empdb;
        private SQLServerTaskRepository taskdb;
        private SQLServerAspNetRoleRepository roledb;

        public ProjectsController()
        {
            projectdb = new SQLServerProjectRepository();
            aspuserdb = new SQLServerAspNetUserRepository();
            empdb = new SQLServerEmployeeRepository();
            taskdb = new SQLServerTaskRepository();
            roledb = new SQLServerAspNetRoleRepository();
        }

        // GET: Projects
        public ActionResult Index()
        {
            ProjectIndexModel pjidx = new ProjectIndexModel();
            Guid guid = Guid.Parse(User.Identity.GetUserId());
            pjidx.YourProjects = projectdb.GetAllApprovedProjectsForUser(guid);
            pjidx.UnapprovedProject = projectdb.GetAllUnapprovedProjectsForUser(guid);
            
            return View(pjidx);
        }

        // GET: Projects/Details/5
        public ActionResult Details(int id)
        {
            var project = projectdb.GetById(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            var employeeList = empdb.GetAllEmployeesForProject((int)id);
            var roleEmployeeList = new List<RoleEmployeeViewModel>();
            foreach (var employee in employeeList)
            {
                if (employee.uid == null) continue;
                var role = roledb.GetByUserId((Guid)employee.uid);
                var rolename = "";
                if (role != null) rolename = role.Name;
                var vm = new RoleEmployeeViewModel
                {
                    Firstname = employee.FirstName,
                    Lastname = employee.LastName,
                    Rolename = rolename,
                    Guid = (Guid)employee.uid
                };
                roleEmployeeList.Add(vm);
            }
            var taskList = taskdb.GetAllByProjectId(id);
            var viewModel = new ProjectDetailViewModel
            {
                Project = project,
                Employees = roleEmployeeList,
                ProjectTasks = taskList
            };
            return View(viewModel);
        }

        [Authorize(Roles = "Manager, Department Manager")]
        public ActionResult AddMember(int pid)
        {
            var employeeList = empdb.GetAllEmployeesInRoleNotAssignedToProject(pid, "Employee");
            var viewModel = new AddMemberProjectViewModel
            {
                Employees = employeeList,
                ProjectId = pid
            };
            return View(viewModel);
        }

        [HttpPost]
        public void AddMember(int pid, Guid guid)
        {
            projectdb.AddEmployeeToProject(guid, pid);
        }

        [HttpPost]
        public void RemoveMember(int pid, Guid guid)
        {
            projectdb.RemoveEmployeeFromProject(guid, pid);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Department Manager")]
        public ActionResult Create()
        {
            var project = new Project();
            project.DueDate = DateTime.Now;
            project.StartedOn = DateTime.Now;
            return View(project);
        }

        [HttpPost]
        public void ApproveProject(int pid)
        {
            var item = projectdb.GetById(pid);
            item.Approved = true;
            projectdb.Update(item);
        }

        [HttpPost]
        public void RejectProject(int pid)
        {
            projectdb.Delete(pid);
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description,StartedOn,DueDate,Completed,Archived,Budget,CurrentSpent")] Project project)
        {
            if (ModelState.IsValid)
            {
                var uid = aspuserdb.GetByUsername(User.Identity.Name);

                if(uid == null)
                {
                    throw new Exception("Unknown owner error occurred.");
                }

                project.Owner = (Guid)uid.uid;
                project.StartedOn = DateTime.Now;
                project.Approved = true;

                projectdb.Save(project);
                var nameof = projectdb.GetProjectByName(project.Name);
                projectdb.AddEmployeeToProject(project.Owner, nameof.pid);
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Manager, Department Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Project project = db.Projects.Find(id);
            Project project = projectdb.GetById((int)id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "pid,Owner,Name,Description,StartedOn,DueDate,Completed,IsArchived,IsLate,IsOverbudgeted,Budget,CurrentSpent")] Project prjct)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(project).State = EntityState.Modified;
                //db.SaveChanges();
                if(prjct.Completed)
                {
                    if (prjct.CompletedOn == null)
                    {
                        prjct.CompletedOn = DateTime.Now;
                    }
                }
                else
                {
                    prjct.CompletedOn = null;
                }
                projectdb.Update(prjct);
                return RedirectToAction("Index");
            }
            return View(prjct);
        }
        
        [Authorize(Roles = "Manager")]
        public ActionResult Request()
        {
            var project = new Project();
            project.DueDate = DateTime.Now;
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Request([Bind(Include = "Name,Description,StartedOn,DueDate,Completed,Archived,Budget,CurrentSpent")] Project project)
        {
            if (ModelState.IsValid)
            {
                var uid = aspuserdb.GetByUsername(User.Identity.Name);

                project.Owner = (Guid)uid.uid;
                project.StartedOn = DateTime.Now;                
                projectdb.Save(project);

                // Add the owner to the Projects database just in case.
                var nameof = projectdb.GetProjectByName(project.Name);
                projectdb.AddEmployeeToProject(project.Owner, nameof.pid);
                return RedirectToAction("Index");
            }
            return View(project);
        }

        [Authorize(Roles = "Department Manager")]
        public ActionResult ProjectRequests()
        {
            var allRequests = projectdb.GetAllUnapprovedProjects();
            return View(allRequests);
        }


        // GET: Projects/Delete/5
        [Authorize(Roles = "Department Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = projectdb.GetById((int)id);
            //Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost]
        
        public void DeleteConfirmed(int id)
        {
            //Project project = db.Projects.Find(id);
            //db.Projects.Remove(project);
            //db.SaveChanges();

            //return RedirectToAction("Index");
            
            projectdb.Delete(id);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                projectdb.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
