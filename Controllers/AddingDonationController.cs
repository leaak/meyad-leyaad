using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MeyadLeyaad1.Models;
using System.IO;
using System.Net.Mail;
using System.Net;

namespace MeyadLeyaad1.Controllers
{
    [Authorize]
    public class AddingDonationController : Controller
    {
        
        // GET: /Adding_Donation/
        DBController db = new DBController();
       

        public ActionResult AddingDonation()
        {
            var tuple = new Tuple<Contribution, Picture>(new Contribution(), new Picture());
            var exemploList = new SelectList(db.getCategories());
            //var subList = new SelectList(db.getSubCategories());
            ViewBag.ExemploList = exemploList;
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
            return View(tuple);
        }

        public ActionResult AddingDonationForDonor()
        {
            var tuple = new Tuple<Contribution, Picture>(new Contribution(), new Picture());

            var exemploList = new SelectList(db.getCategories());
            //var subList = new SelectList(db.getSubCategories());
            ViewBag.ExemploList = exemploList;
            return View(tuple);
        }

        public ActionResult EditingDonation(int id)
        {
            var tuple = new Tuple<Contribution, Picture>(new Contribution(), new Picture());

            var exemploList = new SelectList(db.getCategories());
            //var subList = new SelectList(db.getSubCategories());
            ViewBag.contribution = db.getContribution(id);
            ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";
            ViewBag.ExemploList = exemploList;
            return View(tuple);
        }





        [HttpPost]
        [AllowAnonymous]
        public ActionResult AddingDonation([Bind(Prefix = "Item1")] Contribution cmodel, [Bind(Prefix = "Item2")] Picture pmodel, String returnUrl, HttpPostedFileBase file)
        {

            if (ModelState.IsValid && (Session["type"].ToString().Equals("2") || db.isEmailExists(db.getEmailById(cmodel.Id_Donor))))
            {
                int idContribution = -1;
                if(Session["type"].ToString().Equals("2"))
                    idContribution = db.AddDonation(cmodel, Session["email"].ToString());
                else
                    idContribution = db.AddDonation(cmodel, Session["email"].ToString());
                
                    for (var i=0;i<Request.Files.Count;i++)
                    {
                        var newFile = Request.Files[i];
                        if (newFile != null && newFile.ContentLength > 0)
                        {
                            int idPicture = db.getNextPictureId();
;                           var fileName = Path.GetFileName(newFile.FileName);
                            var path = Path.Combine(Server.MapPath("~/DonationImages/"), idContribution + "_" + idPicture+".jpg");
                            
                            newFile.SaveAs(path);
                            path = path.Substring(path.IndexOf("DonationImages"));
                            db.AddPicture(idPicture, path,idContribution);
                        }

                    }


                    //  sendEmail(cmodel.Id_Donor.ToString(), "Thank you");
                    sendEmail("rivkirom4@gmail.com", "תודה רבה על תרומתך, נבחן את ההצעה וניצור איתך קשר");
                

                    return RedirectToLocal(returnUrl);
                
            
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
            return View(cmodel);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult EditingDonation([Bind(Prefix = "Item1")] Contribution cmodel, [Bind(Prefix = "Item2")] Picture pmodel,  String returnUrl, HttpPostedFileBase file)
        {
            if (ModelState.IsValid && db.isEmailExists(db.getEmailById(cmodel.Id_Donor)))
            {

                db.EditDonation(cmodel);

                for (var i = 0; i < Request.Files.Count; i++)
                {
                    var newFile = Request.Files[i];
                    if (newFile != null && newFile.ContentLength > 0)
                    {
                        int idPicture = db.getNextPictureId();
                         var fileName = Path.GetFileName(newFile.FileName);
                        //var path = Path.Combine(Server.MapPath("~/DonationImages/"), idContribution + "_" + idPicture + ".jpg");

                        //newFile.SaveAs(path);
                        //path = path.Substring(path.IndexOf("DonationImages"));
                        //db.AddPicture(idPicture, path, idContribution);
                    }

                }


                //  sendEmail(cmodel.Id_Donor.ToString(), "Thank you");
              


                return RedirectToLocal(returnUrl);

            }

            // If we got this far, something failed, redisplay form
            return View(cmodel);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AddingDonationForDonor([Bind(Prefix = "Item1")] Contribution cmodel, [Bind(Prefix = "Item2")] Picture pmodel, String returnUrl, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {

                int idContribution = db.AddDonation(cmodel , Session["email"].ToString());

                for (var i = 0; i < Request.Files.Count; i++)
                {
                    var newFile = Request.Files[i];
                    if (newFile != null && newFile.ContentLength > 0)
                    {
                        int idPicture = db.getNextPictureId();
                        var fileName = Path.GetFileName(newFile.FileName);
                        var path = Path.Combine(Server.MapPath("~/DonationImages/"), idContribution + "_" + idPicture + ".jpg");

                        newFile.SaveAs(path);
                        path = path.Substring(path.IndexOf("DonationImages"));
                        db.AddPicture(idPicture, path, idContribution);
                    }

                }
                sendEmail("rivkirom4@gmail.com", "Thank you");
                TempData["Message"] = "thanks you";
                

                return RedirectToLocal(returnUrl);


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
            return View(cmodel);
        }

       

        public void sendEmail(string email , string msg)
        {
            var fromAddress = new MailAddress("meyadleyaadoffice@gmail.com", "מיד ליעד");
            string mail = email;
            var toAddress = new MailAddress(mail);
            const string fromPassword = "office123!";
            const string subject = "תרומתך נקלטה במערכת";
            //string msg = "Hi " + d.get_client_name_according_to_user_name(UserName.Text) + ",\nyour password is " + d.getPassword(UserName.Text);
            string body = msg;

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
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                file.SaveAs(path);
            }

            return RedirectToAction("UploadDocument");
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
                {
                    ViewBag.msg ="תודה על תרומתך";
                    return RedirectToAction("DonorHomePage", "DonorHomePage", new { msg = "תודה" });
                }
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
