using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WebApplication2.Domain;
using WebApplication2.Models;
using WebApplication2.Repositories.SQLServer;

namespace WebApplication2.Controllers
{
    [Authorize(Roles ="Admin")]
    public class DataImportController : Controller
    {
        private ApplicationUserManager _userManager;
        private SQLServerEmployeeRepository empdb;
        private SQLServerProjectRepository projdb;
        private SQLServerAspNetUserRepository aspuserdb;
        private SQLServerTaskRepository taskdb;

        public DataImportController()
        {
            empdb = new SQLServerEmployeeRepository();
            projdb = new SQLServerProjectRepository();
            aspuserdb = new SQLServerAspNetUserRepository();
            taskdb = new SQLServerTaskRepository();
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public DataImportController(ApplicationUserManager userManager) : this()
        {
            _userManager = userManager;
        }

        public ActionResult Result()
        {
            return View(TempData["resultlist"]);
        }

        // GET: DataImport
        public ActionResult Index(string err)
        {
            return View(err);
        }

        [HttpPost]
        public async Task<ActionResult> Index(HttpPostedFileBase file)
        {
            ImportStatus results = new ImportStatus();
            if (ModelState.IsValid)
            {
                if (file == null)
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                }
                else if (file.ContentLength > 0)
                {
                    int MaxContentLength = 1024 * 1024 * 3; //3 MB
                    string[] AllowedFileExtensions = new string[] { ".xml" };

                    if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                    {
                        ModelState.AddModelError("File", "Please upload file of type: " + string.Join(", ", AllowedFileExtensions));
                    }

                    else if (file.ContentLength > MaxContentLength)
                    {
                        ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + MaxContentLength + " MB");
                    }
                    else
                    {


                        //TO:DO
                        XDocument doc = new XDocument();
                        doc = XDocument.Load(file.InputStream);

                        IEnumerable<UserXML> UserList = from c in doc.Descendants("User")
                                                        select new UserXML()
                                                        {
                                                            Email = (string)c.Element("email").Value,
                                                            UserName = (string)c.Element("username").Value,
                                                            FirstName = (string)c.Element("firstname").Value,
                                                            LastName = (string)c.Element("lastname").Value,
                                                            BirthDate = (string)c.Element("birthdate").Value,
                                                            Role = (string)c.Attribute("role").Value
                                                        };

                        IEnumerable<ProjectXML> ProjectImportList = from c in doc.Descendants("Project")
                                                                    select new ProjectXML()
                                                                    {
                                                                        Owner = (string)c.Attribute("owner").Value,
                                                                        Name = (string)c.Element("name").Value,
                                                                        Description = (string)c.Element("description").Value,
                                                                        StartDate = DateTime.Parse((string)c.Element("startdate").Value),
                                                                        Completed = bool.Parse((string)c.Element("completed").Value),
                                                                        CompletedOn = DateTime.Parse((string)c.Element("completedon").Value),
                                                                        Archived = bool.Parse((string)c.Element("archived").Value),
                                                                        Budget = int.Parse((string)c.Element("budget").Value),
                                                                        Spent = int.Parse((string)c.Element("spent").Value),
                                                                        DueDate = DateTime.Parse((string)c.Element("duedate").Value)
                                                                    };

                        IEnumerable<TaskXML> TaskImportList = from c in doc.Descendants("Task")
                                                              select new TaskXML()
                                                              {
                                                                  ProjectName = (string)c.Attribute("ProjectName").Value,
                                                                  Name = (string)c.Element("name").Value,
                                                                  Description = (string)c.Element("description").Value,
                                                                  Started = DateTime.Parse(c.Element("started").Value),
                                                                  Due = DateTime.Parse(c.Element("due").Value),
                                                                  Completed = bool.Parse(c.Element("completed").Value),
                                                                  CompletedOn = DateTime.Parse(c.Element("completedon").Value)
                                                              };


                        // We have read the XML. Now we analyze the data, starting with the User data first.

                        try
                        {
                            foreach (UserXML udat in UserList)
                            {
                                bool UserAdded = false;

                                // Before putting something into the database, we need to ensure that what we are putting in
                                // is not a duplicate e-mail address or Username. Users can have the same name.
                                var address = new MailAddress(udat.Email);
                                if (empdb.GetByEmail(address.ToString()) != null)
                                {
                                    // Email already exists, cannot add new address.

                                    results.Users.Add(new ImportResult
                                    {
                                        Succeeded = false,
                                        Name = udat.UserName,
                                        FailReason = "This email is already registered."
                                    });
                                    continue;
                                }

                                if (!udat.Role.ToLower().Equals("employee") && !udat.Role.ToLower().Equals("manager"))
                                {
                                    results.Users.Add(new ImportResult
                                    {
                                        Succeeded = false,
                                        Name = udat.UserName,
                                        FailReason = "The provided user role is not valid."
                                    });
                                    continue;
                                }

                                if (empdb.GetByUsername(udat.UserName) != null)
                                {
                                    results.Users.Add(new ImportResult
                                    {
                                        Succeeded = false,
                                        Name = udat.UserName,
                                        FailReason = "The username is invalid."
                                    });
                                    continue;
                                }

                                if (string.IsNullOrEmpty(udat.FirstName) || string.IsNullOrEmpty(udat.LastName))
                                {
                                    results.Users.Add(new ImportResult
                                    {
                                        Succeeded = false,
                                        Name = udat.UserName,
                                        FailReason = "The first or last name is invalid."
                                    });
                                    continue;
                                }

                                if (udat.BirthDate == null)
                                {
                                    results.Users.Add(new ImportResult
                                    {
                                        Succeeded = false,
                                        Name = udat.UserName,
                                        FailReason = "The birthdate is invalid or malformed."
                                    });
                                    continue;
                                }




                                // If these succeed, generate a password. First three charactrs of Firstname, first three characters of Lastname, year

                                var passwdbuilder = new StringBuilder();

                                if (udat.FirstName.Length > 3)
                                {
                                    passwdbuilder.Append(udat.FirstName.ToUpper().Substring(0, 3));
                                }
                                else
                                {
                                    passwdbuilder.Append(udat.FirstName.Substring(0, 1).ToUpper());
                                }

                                if (udat.LastName.Length > 3)
                                {
                                    passwdbuilder.Append(udat.LastName.ToLower().Substring(0, 3));
                                }
                                else
                                {
                                    passwdbuilder.Append(udat.LastName.ToLower().Substring(0, 3));
                                }

                                passwdbuilder.Append("$" + DateTime.Now.Month + DateTime.Now.Year);

                                var newuser = new ApplicationUser
                                {
                                    UserName = udat.UserName,
                                    Email = address.ToString()

                                };

                                var result = await UserManager.CreateAsync(newuser, passwdbuilder.ToString());

                                if (!result.Succeeded)
                                {
                                    results.Users.Add(new ImportResult
                                    {
                                        Succeeded = false,
                                        Name = udat.UserName,
                                        FailReason = "The birthdate is invalid or malformed."
                                    });
                                }

                                string role = null;

                                if (udat.Role.ToLower().Equals("employee"))
                                {
                                    role = "Employee";
                                }
                                else if (udat.Role.ToLower().Equals("manager"))
                                {
                                    role = "Manager";
                                }
                                else if(udat.Role.ToLower().Equals("department manager"))
                                {
                                    role = "Department Manager";
                                }
                                else
                                {
                                    await UserManager.DeleteAsync(newuser);
                                    results.Users.Add(new ImportResult
                                    {
                                        Succeeded = false,
                                        Name = udat.UserName,
                                        FailReason = "The role was invalid."
                                    });
                                    continue;
                                }

                                var result2 = await UserManager.AddToRoleAsync(newuser.Id, role);

                                if (!result2.Succeeded)
                                {
                                    await UserManager.DeleteAsync(newuser);
                                    results.Users.Add(new ImportResult
                                    {
                                        Succeeded = false,
                                        Name = udat.UserName,
                                        FailReason = "Could not add the user to the specified role. (Does it exist?)"
                                    });
                                    continue;
                                }

                                var aspuser = aspuserdb.GetByUsername(newuser.UserName);

                                var emp = new Employee();

                                emp.uid = aspuser.uid;
                                emp.FirstName = udat.FirstName;
                                emp.LastName = udat.LastName;
                                emp.Birthdate = DateTime.Parse(udat.BirthDate);
                                emp.Joindate = DateTime.Now;

                                empdb.Save(emp);
                                results.Users.Add(new ImportResult
                                {
                                    Name = udat.UserName,
                                    Succeeded = true,
                                    FailReason = null
                                });
                            }
                        }
                        catch(Exception ex)
                        {
                            results.Users.Add(new ImportResult
                            {
                                Name = "No users added",
                                Succeeded = false,
                                FailReason = "No Users were found in your XML or there was an error in the XML file: " + ex.Message
                            });
                        }

                        try
                        {
                            foreach (ProjectXML proj in ProjectImportList)
                            {
                                if (string.IsNullOrEmpty(proj.Name) || projdb.GetProjectByName(proj.Name) != null)
                                {
                                    // Name is empty or already exists.
                                    results.Projects.Add(new ImportResult
                                    {
                                        Succeeded = false,
                                        Name = proj.Name,
                                        FailReason = "Project was invalid or already exists."
                                    });
                                    continue;
                                }

                                var owner = empdb.GetByUsername(proj.Owner);

                                if (owner == null)
                                {
                                    results.Projects.Add(new ImportResult
                                    {
                                        Succeeded = false,
                                        Name = proj.Name,
                                        FailReason = "The specified owner name could not be found."
                                    });
                                    continue;
                                }



                                if (proj.Budget <= 0)
                                {
                                    // Invalid budget.
                                    results.Projects.Add(new ImportResult
                                    {
                                        Succeeded = false,
                                        Name = proj.Name,
                                        FailReason = "The provided budget was not within the valid range. (Greater then 0)"
                                    });
                                    continue;
                                }

                                if (proj.Spent < 0)
                                {
                                    results.Projects.Add(new ImportResult
                                    {
                                        Succeeded = false,
                                        Name = proj.Name,
                                        FailReason = "The provided spent column was not within the valid range. (Greater then 0)"
                                    });
                                    continue;
                                }

                                if (proj.DueDate == DateTime.MinValue)
                                {
                                    results.Projects.Add(new ImportResult
                                    {
                                        Succeeded = false,
                                        Name = proj.Name,
                                        FailReason = "The provided Due Date was invalid or malformed."
                                    });
                                    continue;
                                }

                                if (proj.StartDate == DateTime.MinValue)
                                {
                                    proj.StartDate = DateTime.Now;
                                }

                                if (proj.Completed && proj.CompletedOn == DateTime.MinValue)
                                {
                                    proj.CompletedOn = DateTime.Now;
                                }

                                if(!proj.Completed)
                                {
                                    proj.CompletedOn = null;
                                }

                                var NewProj = new Domain.Project
                                {
                                    Name = proj.Name,
                                    Description = proj.Description,
                                    Owner = (Guid)owner.uid,
                                    StartedOn = proj.StartDate,
                                    Completed = proj.Completed,
                                    CompletedOn = proj.CompletedOn,
                                    Archived = proj.Archived,
                                    Budget = proj.Budget,
                                    CurrentSpent = proj.Spent,
                                    DueDate = proj.DueDate
                                };

                                projdb.Save(NewProj);

                                // Remember to assign the owner to the project so it doesn't disappear or something.

                                // Get the project...
                                var refproj = projdb.GetProjectByName(NewProj.Name);
                                projdb.AddEmployeeToProject((Guid)owner.uid, refproj.pid);

                                results.Projects.Add(new ImportResult
                                {
                                    Name = proj.Name,
                                    Succeeded = true,
                                    FailReason = null
                                });
                                // Add to Success list.
                            }
                        }
                        catch (Exception ex)
                        {
                            results.Projects.Add(new ImportResult
                            {
                                Name = "Project Exception",
                                Succeeded = false,
                                FailReason = "No Projects were found in your XML or there was an error in the XML file: " + ex.Message
                            });
                        }

                        try
                        {
                            foreach (TaskXML task in TaskImportList)
                            {
                                if (string.IsNullOrEmpty(task.Name))
                                {
                                    // Invalid task name.
                                    results.Tasks.Add(new ImportResult
                                    {
                                        Succeeded = false,
                                        Name = task.Name,
                                        FailReason = "The task name could not be found."
                                    });
                                    continue;
                                }

                                if (task.Started == DateTime.MinValue)
                                {
                                    // Invalid start date.
                                    results.Tasks.Add(new ImportResult
                                    {
                                        Succeeded = false,
                                        Name = task.Name,
                                        FailReason = "The Startdate was invalid or malformed."
                                    });
                                    continue;
                                }

                                if (task.Due == DateTime.MinValue)
                                {
                                    // Invalid Duedate.
                                    continue;
                                }

                                if (task.Completed && task.CompletedOn == DateTime.MinValue)
                                {
                                    // Invalid completedon date.
                                    results.Tasks.Add(new ImportResult
                                    {
                                        Succeeded = false,
                                        Name = task.Name,
                                        FailReason = "The Completion Date was invalid or malformed."
                                    });
                                    continue;
                                }

                                var attachproj = projdb.GetProjectByName(task.ProjectName);

                                if (attachproj == null)
                                {
                                    results.Tasks.Add(new ImportResult
                                    {
                                        Succeeded = false,
                                        Name = task.Name,
                                        FailReason = "The project this task is supposed to be attached to could not be found."
                                    });
                                    continue;
                                }

                                var taskexists = taskdb.GetByProjectAndName(attachproj.pid, task.Name);

                                if(taskexists != null)
                                {
                                    results.Tasks.Add(new ImportResult
                                    {
                                        Succeeded = false,
                                        Name = task.Name,
                                        FailReason = "A task with this name is already attached to this project."
                                    });
                                    continue;
                                }

                                var NewTask = new ProjectTask
                                {
                                    Name = task.Name,
                                    Description = task.Description,
                                    StartedOn = task.Started,
                                    DueOn = task.Due,
                                    Completed = task.Completed,
                                    CompletedOn = task.CompletedOn,
                                    Pid = attachproj.pid
                                };

                                taskdb.Save(NewTask);

                                results.Tasks.Add(new ImportResult
                                {
                                    Succeeded = true,
                                    Name = task.Name,
                                    FailReason = null
                                });
                                // Add to success list
                            }
                        }
                        catch (Exception ex)
                        {
                            results.Tasks.Add(new ImportResult
                            {
                                Name = "Task Exception",
                                Succeeded = false,
                                FailReason = "No Tasks were found in your XML or there was an error in your XML: " + ex.Message
                            });
                        }
                        TempData["resultlist"] = results;
                        return RedirectToAction("Result");
                    }
                }
            }


            // Return model errors
            return View();
        }
    }
}