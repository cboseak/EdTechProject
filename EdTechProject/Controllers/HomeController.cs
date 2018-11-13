using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EdTechProject.Controllers
{
 
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Inbox()
        {
            return View();
        }
        public ActionResult Upload()
        {
            return View();
        }
        public ActionResult Compose()
        {
            return View();
        }
        public ActionResult Message(int id)
        {
            return View();
        }
        public ActionResult LessonPlan(int id)
        {
            return View();
        }



    }
}