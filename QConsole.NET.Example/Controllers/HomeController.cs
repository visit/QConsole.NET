using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QConsole.NET.Example.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Toggle()
        {
            Session["ConsoleAuthorized"] = !((bool?) Session["ConsoleAuthorized"] == true);

            return View("Index");
        }

    }
}
