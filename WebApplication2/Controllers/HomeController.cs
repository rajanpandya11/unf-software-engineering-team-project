using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Role");
            }          
            return RedirectToAction("Index", "Projects");
        }

        public ActionResult About()
        { 
            return View();
        }
    }
}