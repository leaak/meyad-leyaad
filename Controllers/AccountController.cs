using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MeyadLeyaad1.Models;
using MeyadLeyaad1.Controllers;
using System.IO;
using System.Net.Mail;
using System.Net;

namespace MeyadLeyyad1.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login
        DBController db = new DBController();

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.displayDonation = db.getLatestDonations();
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && db.isUserExists(model.User_Name , model.Password))
            {
                System.Web.HttpContext.Current.Session["email"] = model.User_Name;
                System.Web.HttpContext.Current.Session["password"] = model.Password;
                int type = db.getUserType(model.User_Name, model.Password);
                System.Web.HttpContext.Current.Session["type"] = type;
                //if (context.MyEntity.Any(o => o.Id == idToMatch))
                //secritary is 1 donor is 2
                return RedirectToLocal(returnUrl , false,type);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "שם משתמש או סיסמה שגויה");
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.displayDonation = db.getLatestDonations();
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateInput(false)]
        public ActionResult remindPassword(string email , string returnURL)
        {
            ValidateRequest = false;
            var fromAddress = new MailAddress("rivkirom4@gmail.com", "מיד ליעד");
            string mail = email;
            var toAddress = new MailAddress(mail);
            const string fromPassword = "rivki308090";
            const string subject = "שחזור סיסמה";
            //string msg = "Hi " + d.get_client_name_according_to_user_name(UserName.Text) + ",\nyour password is " + d.getPassword(UserName.Text);
            string body = "סיסמתך היא:" + db.getUserPassword(email);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
           // ViewBag.ReturnUrl = returnUrl;
           // ViewBag.displayDonation = db.getLatestDonations();
           return View();
        }


        //
        // POST: /Account/LogOff

        [HttpPost]
        public ActionResult LogOff()
        {
          //  WebSecurity.Logout();

            return RedirectToAction("Account", "Login");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(RegisterModel model ,  String returnUrl)
        {
            if(ModelState.IsValid)
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
                if (!db.isEmailExists(model.User_Name) && !db.isUserExists(model.User_Name , model.Password))
                {

                    System.Web.HttpContext.Current.Session["email"] = model.User_Name;
                    System.Web.HttpContext.Current.Session["password"] = model.Password;
                    System.Web.HttpContext.Current.Session["email"] = model.User_Name;
                    System.Web.HttpContext.Current.Session["type"] = "2";

                    db.AddUser(model);
                    
                    return RedirectToLocal(returnUrl , true , 2);
                }
                else
                    ModelState.AddModelError("", "משתמש קיים כבר");
                    //}
                   
                
                
                
                // Attempt to register the user
                try
                {
                   // WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    //WebSecurity.Login(model.UserName, model.Password);
                    return RedirectToAction("Register", "Account");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        

        //
        // POST: /Account/Disassociate



        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl , bool newUser, int type)
        {
             
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                    ViewBag.UserName = Session["UserName"];
                    ViewBag.Password = Session["Password"];
                if(type==1)
                    return RedirectToAction("Index", "Index");
                else
                {
                    if (newUser)
                        return RedirectToAction("CreateProfile", "CreateProfile");
                    else
                        return RedirectToAction("DonorHomePage", "DonorHomePage");
                }
                
                   // return RedirectToAction("Index", "Account");
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

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
