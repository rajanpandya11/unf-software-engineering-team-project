using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using WebApplication2.Models;
using WebApplication2.Repositories.SQLServer;
using WebApplication2.Domain;

namespace WebApplication2.Controllers
{    
    public class RoleController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private SQLServerEmployeeRepository _emprepo;
        private SQLServerAspNetUserRepository _asprepo;
        private SQLServerUserRequestRepository _request;

        ApplicationDbContext _context;

        public RoleController()
        {
            _context = new ApplicationDbContext();
            _emprepo = new SQLServerEmployeeRepository();
            _asprepo = new SQLServerAspNetUserRepository();
            _request = new SQLServerUserRequestRepository();
        }

        /// <summary>
        /// Get All Roles
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {                
                if (!isAdminUser())
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            var roles = _context.Roles.ToList().OrderBy(x=>x.Name);
            var users = _context.Users.ToList().OrderBy(x => x.UserName);
            //foreach (var user in users)
            //{//"dc8e73f1-3e8a-4d9a-9645-61408bd3b1db"
            //    var roleId = user.Roles.FirstOrDefault().RoleId;
            //    var role = roles.FirstOrDefault(x => x.Id == roleId);
            //    var roleName = role.Name;
            //}
            var viewModel = new UserRoleViewModel
            {
                Roles = roles,
                Users = users
            };
            return View(viewModel);
        }

        //
        // GET: /Account/Register
        [Authorize(Roles = "Admin")]
        public ActionResult NewRegister(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (isAdminUser())
                {
                    ViewBag.Name = new SelectList(_context.Roles
                                            .ToList(), "Name", "Name");
                    return View();
                }
            }
            ViewBag.ReturnURL = returnUrl;
            return View();
            //return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewRegister(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email};
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);


                    var aspuser = _asprepo.GetByUsername(user.UserName);

                    var emp = new Employee();

                    emp.uid = aspuser.uid;
                    emp.FirstName = model.FirstName;
                    emp.LastName = model.LastName;
                    emp.Birthdate = model.BirthDate;
                    emp.Joindate = DateTime.Now;

                    _emprepo.Save(emp);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    await this.UserManager.AddToRoleAsync(user.Id, model.UserRoles);

                    return RedirectToAction("Index", "Role");
                }
                ViewBag.Name = new SelectList(_context.Roles
                                          .ToList(), "Name", "Name");
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [Authorize(Roles = "Department Manager")]
        public ActionResult NewAccountRequest(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Name = new SelectList(_context.Roles.Where(u => !u.Name.Contains("Admin"))
                    .ToList(), "Name", "Name");
                return View();
             
            }
            ViewBag.ReturnURL = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ApproveUser(int reqid)
        {
            if (ModelState.IsValid)
            {
                var req = _request.GetById(reqid);


                var user = new ApplicationUser { UserName = req.Username, Email = req.Email };

                // Generate a new password here.

                req.Password = "A$dfghjkl11";

                var result = await UserManager.CreateAsync(user, req.Password);
                if (result.Succeeded)
                {
                    // await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);


                    var aspuser = _asprepo.GetByUsername(user.UserName);

                    var emp = new Employee();

                    emp.uid = aspuser.uid;
                    emp.FirstName = req.FirstName;
                    emp.LastName = req.LastName;
                    emp.Birthdate = req.BirthDate;
                    emp.Joindate = DateTime.Now;

                    _emprepo.Save(emp);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    await this.UserManager.AddToRoleAsync(user.Id, req.UserRoles);

                    // If we were able to do everything successfully, then remove the request from the database.

                    _request.Delete((int)req.Id);

                    return RedirectToAction("Index", "Role");
                }
                ViewBag.Name = new SelectList(_context.Roles.Where(u => !u.Name.Contains("Admin"))
                                          .ToList(), "Name", "Name");
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View();
        }

        [HttpPost]
        public ActionResult RejectUser(int reqid)
        {
            _request.Delete(reqid);
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewAccountRequest(UserRequest model)
        {
            if (ModelState.IsValid)
            {
                _request.Save(model);
                return RedirectToAction("Index", "Projects");
            }
            return View(model);
        }

        public ActionResult Requests()
        {
            var allRequests = _request.GetAllRequests();
            return View(allRequests);
        }

        public Boolean isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Create  a New role
        /// </summary>
        /// <returns></returns>         
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!isAdminUser())
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            var Role = new IdentityRole();
            return View(Role);
        }

        /// <summary>
        /// Create a New Role
        /// </summary>
        /// <param name="Role"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(IdentityRole Role)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!isAdminUser())
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            _context.Roles.Add(Role);
            _context.SaveChanges();
            return RedirectToAction("Index");
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
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}