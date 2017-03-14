using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MeyadLeyaad1.Models;

namespace MeyadLeyaad1.Controllers
{
    // [Authorize]
    public class SearchDonationController : Controller
    {
        DBController db = new DBController();
        // GET: /SearchDonation/

        public ActionResult SearchDonation(Contribution c = null)
        {
            //var tuple = new Tuple<Donor, Contribution, Statuses>(new Donor(), new Contribution(), new Statuses());
            ViewBag.list = db.serachDontions(c);
            var exemploList = new SelectList(db.getCategories());
            //var subList = new SelectList(db.getSubCategories());
            ViewBag.statuses = new SelectList(db.getStatuses());
            ViewBag.ExemploList = exemploList;
            //return View(tuple);
            return View();
        }


    }
}
