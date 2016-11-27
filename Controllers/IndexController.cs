using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MeyadLeyaad1.Models;

namespace MeyadLeyaad1.Controllers
{
    public class IndexController : Controller
    {
        DBController db = new DBController();
        //
        // GET: /Index/

        public ActionResult Index()
        {
            List<DisplayDonation> list = db.getLatestDonations();
            ViewBag.list = list;
            return View();
        }

       

    }
}
