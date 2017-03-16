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

        public ActionResult DonationsOfDonor(string email)
        {
            ViewBag.displayDonation = db.getDonationsOfDonor(email);
            return View();
        }

    }
}
