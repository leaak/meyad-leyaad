using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MeyadLeyaad1.Controllers
{
    public class DonationsOfDonorController : Controller
    {
        //
        // GET: /DonationsOfDonor/
        DBController db = new DBController();

        public ActionResult DonationsOfDonor(int id)
        {
            ViewBag.displayDonation = db.getDonationsOfDonor(db.getEmailById(id));
            ViewBag.id = id;
            return View();
        }

    }
}
