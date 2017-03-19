using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MeyadLeyaad1.Models;

namespace MeyadLeyaad1.Controllers
{
    [Authorize]
    public class DonorHomePageController : Controller
    {
        
        // GET: /DonorHomePage/
        DBController db = new DBController();

        public ActionResult DonorHomePage(string returnUrl, string dd = "")
        {
            String msg="";
            if (!dd.Equals(""))
            {
                db.DeleteDonation(dd);
                msg = "תודה על תרומתך";
            }
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.displayDonation = db.getDonationsOfDonor(Session["email"].ToString());
            ViewBag.msg = msg;
            return View();
        }

        public JsonResult DeleteDonation(string dd)
        {
           // ViewBag.displayDonation = db.getDonationsOfDonor(Session["email"].ToString());
            db.DeleteDonation(dd);
            //ViewBag.ReturnUrl = returnUrl;
            //ViewBag.displayDonation = db.getDonationsOfDonor(Session["email"].ToString());
            //return View();
    //        var redirectUrl = new UrlHelper(Request.RequestContext).Action("DonorHomePage", "DonorHomePage");
    //        return Json(new { Url = redirectUrl });
         
            //ViewBag.displayDonation = db.getDonationsOfDonor(Session["email"].ToString());
            //return View("DonorHomePage");
            return Json("success");

           
        }

    }
}
