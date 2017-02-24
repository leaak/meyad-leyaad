using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MeyadLeyaad1.Models;

namespace MeyadLeyaad1.Controllers
{
    [Authorize]
    public class SearchDonorController : Controller
    {

        DBController db = new DBController();
        
        // GET: /SearchDonor/

       /* public ActionResult SearchDonor(){
            return View();
        }*/

        public ActionResult SearchDonor(Donor d = null)
        {
            ViewBag.list = db.searchDonors(d);
            return View();
        }

    }
}
