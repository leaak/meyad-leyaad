using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MeyadLeyaad1.Models;
using MeyadLeyaad1.Controllers;

namespace MeyadLeyaad1.Controllers
{
   // [Authorize]
    public class CreateEditProfileController : Controller
    {
        
        // GET: /CreateProfile/
        DBController db = new DBController();

        public ActionResult CreateProfile()
        {
            var tuple = new Tuple<Donor, Schedule , Users>(new Donor(), new Schedule() , new Users());
            ViewBag.UserName = Session["email"];
            if (Session["type"].ToString().Equals("1"))
            {
                ViewBag.layout = "~/Views/Shared/_Layout.cshtml";
                ViewBag.type = "secretary";
            }
            else
            {
                ViewBag.layout = "~/Views/Shared/_LoyoutDonor.cshtml";
                ViewBag.type = "donor";
            }
            return View("CreateEditProfile", tuple);
        }

        public ActionResult EditProfile(int id = -1)
        {
            if (id == -1)
                id = db.getUserId(Session["email"].ToString(), Session["password"].ToString());
            var tuple = new Tuple<Donor, Schedule, Users>(db.getDonor(id), db.getScheduleForDonor(id).FirstOrDefault(), new Users());

            ViewBag.UserName = Session["email"];
            if (Session["type"].ToString().Equals("1"))
            {
                ViewBag.layout = "~/Views/Shared/_Layout.cshtml";
                ViewBag.type = "secretary";
            }
            else
            {
                ViewBag.layout = "~/Views/Shared/_LoyoutDonor.cshtml";
                ViewBag.type = "donor";
            }
            return View("CreateEditProfile", tuple);
        }

       

        [HttpPost]
        public ActionResult CreateEditProfile([Bind(Prefix = "Item1")] Donor dmodel, [Bind(Prefix = "Item2")] Schedule smodel, [Bind(Prefix = "Item3")] Users umodel, String returnUrl)
        {
            if (ModelState.IsValid)
            {
                if(dmodel.Id_Donor<=0)
                {
                    if (Session["type"].ToString().Equals("1") || db.isEmailExists(dmodel.Email))
                    {
                        db.AddDonor(dmodel , smodel);
                        return RedirectToLocal(returnUrl);
                    }
                    else
                        ModelState.AddModelError("", "כתובת דואר אלקטרוני אינה מוכרת");
                }
                else
                {
                    db.EditDonor(dmodel);
                }
                
                //}




                // Attempt to register the user
                try
                {
                    // WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    //WebSecurity.Login(model.UserName, model.Password);

                    return RedirectToLocal(returnUrl);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(dmodel);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                if (Session["type"].ToString().Equals("1"))
                    return RedirectToAction("Index", "Index");
                else
                    return RedirectToAction("DonorHomePage", "DonorHomePage");
                
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                // OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        //private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        //{
        //    // See http://go.microsoft.com/fwlink/?LinkID=177550 for
        //    // a full list of status codes.
        //    switch (createStatus)
        //    {
        //        case MembershipCreateStatus.DuplicateUserName:
        //            return "User name already exists. Please enter a different user name.";

        //        case MembershipCreateStatus.DuplicateEmail:
        //            return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

        //        case MembershipCreateStatus.InvalidPassword:
        //            return "The password provided is invalid. Please enter a valid password value.";

        //        case MembershipCreateStatus.InvalidEmail:
        //            return "The e-mail address provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.InvalidAnswer:
        //            return "The password retrieval answer provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.InvalidQuestion:
        //            return "The password retrieval question provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.InvalidUserName:
        //            return "The user name provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.ProviderError:
        //            return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

        //        case MembershipCreateStatus.UserRejected:
        //            return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

        //        default:
        //            return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
        //    }
      //  }
        #endregion

    }


}


