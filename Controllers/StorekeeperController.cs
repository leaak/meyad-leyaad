using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MeyadLeyaad1.Controllers
{
    public class StorekeeperController : Controller
    {
        //
        // GET: /Storekeeper/
        DBController db = new DBController();
   // [HttpPost]
        public ActionResult Storekeeper(string returnUrl)
        {
            //ViewBag.ReturnUrl = returnUrl;
            ViewBag.donation = db.getContributionsInStorehouse();
            return View();
        }

    }
}
