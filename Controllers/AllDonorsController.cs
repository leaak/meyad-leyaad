using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MeyadLeyaad1.Models;
using MeyadLeyaad1.Controllers;

namespace MeyadLeyaad1.Controllers
{
    public class AllDonorsController : Controller
    {
        //
        // GET: /AllDonors/
        DBController db = new DBController();

        public ActionResult AllDonors()
        {
            ViewBag.list = db.getAllDonors();
            return View();
        }

    }
}
