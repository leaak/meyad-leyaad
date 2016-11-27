using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MeyadLeyaad1.Models;
using MeyadLeyaad1.Controllers;

namespace MeyadLeyaad1.Controllers
{
    public class CreateProfileController : Controller
    {
        //
        // GET: /CreateProfile/
        DBController db = new DBController();

        public ActionResult CreateProfile()
        {
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
            return View();
        }

        public ActionResult EditProfile(Donor d)
        {
            ViewBag.donor = d;
            return View();
        }

        public ActionResult EditProfile()
        {
            ViewBag.donor = db.getDonor(Session["email"].ToString());
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CreateProfile([Bind(Prefix = "Item1")] Donor dmodel, [Bind(Prefix = "Item2")] Schedule smodel, String returnUrl)
        {
            if (ModelState.IsValid)
            {

                // Insert a new user into the database


                // Donor user = db.Donor.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                // Check if user already exists
                // if (user == null)
                //{
                // Insert name into the profile table
                // db.Donor.Add(new Donor { Email = model.Email, First_Name = model.First_Name, Last_Name = model.Last_Name, Building = model.Building, City = model.City, Floor = model.Floor, House = model.House, Street = model.Street, Phone = model.Phone , Fax = model.Fax , Another_Phone = model.Another_Phone , Comments = model.Comments  , Password = model.Password});
                //db.Donor.Add(new Donor { Email = "model.Email", First_Name = "model.First_Name", Last_Name = "model.Last_Name", Building = 45, City = "model.City", Floor = 9, House = 12, Street = "model.Street", Phone = "model.Phone", Fax = "model.Fax", Another_Phone = "model.Another_Phone", Comments = "model.Comments" });


                // OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                // OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                if (Session["type"].ToString().Equals("1")  || db.isEmailExists(dmodel.Email))
                {
                    db.AddDonor(dmodel);

                    return RedirectToLocal(returnUrl);
                }
                else
                    ModelState.AddModelError("", "כתובת דואר אלקטרוני אינה מוכרת");
                //}




                // Attempt to register the user
                try
                {
                    // WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    //WebSecurity.Login(model.UserName, model.Password);

                    return RedirectToAction("CreateProfile", "CreateProfile");
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


