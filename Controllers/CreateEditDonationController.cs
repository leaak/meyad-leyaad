﻿using System;
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
    public class CreateEditDonationController : Controller

    {
        DBController db = new DBController();
        //
        // GET: /CreateEditDonation/
         [Authorize]

        public ActionResult CreateDonation(int id = -1)
        {
            Contribution c = new Contribution();
            if (id > -1)
                c.Id_Donor = id;
            var tuple = new Tuple<Contribution, Picture>(c, new Picture());
            ViewBag.Categories = new SelectList(db.getCategories()); 
            ViewBag.Statuses = new SelectList(db.getStatuses());
            ViewBag.subc = new SelectList (new[]{" "}); 
            //var subList = new SelectList(db.getSubCategories());
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
            ViewBag.subject = "הוספת תרומה";
            return View("CreateEditDonation", tuple);
        }
         [Authorize]
        public ActionResult EditDonation(int id)
        {
             Contribution c = db.getContribution(id);
            var tuple = new Tuple<Contribution, Picture>(c, db.getPicture(id));
            //var subList = new SelectList(db.getSubCategories());
            ViewBag.Categories = new SelectList(db.getCategories());
            ViewBag.Statuses = new SelectList(db.getStatuses());
            ViewBag.subc = new SelectList(new[] {c.Sub_Category}); 
            if (Session["type"] != null && Session["type"].ToString().Equals("2"))
            {            
                ViewBag.layout = "~/Views/Shared/_LoyoutDonor.cshtml";
                ViewBag.type = "donor";
            }
            else
            {
                ViewBag.layout = "~/Views/Shared/_Layout.cshtml";
                ViewBag.type = "secretary";
            }
            ViewBag.subject = "עריכת תרומה";
            return View("CreateEditDonation", tuple);
        }


        public ActionResult CreateEditDonation([Bind(Prefix = "Item1")] Contribution cmodel, [Bind(Prefix = "Item2")] Picture pmodel, String returnUrl, HttpPostedFileBase file)
        {
            //ViewBag.subCategories = db.getSubCategoryByIdCotegory(id);


            if (ModelState.IsValid /*&& (Session["type"].ToString().Equals("2") || db.isEmailExists(db.getEmailById(cmodel.Id_Donor)))*/)
            {
                int idContribution = cmodel.Id_Contribution;

                if (cmodel.Id_Contribution <= 0)
                {
                    if (Session["type"].ToString().Equals("2"))
                        idContribution = db.AddDonation(cmodel, Session["email"].ToString());
                    else
                        idContribution = db.AddDonation(cmodel , cmodel.Id_Donor);


                    sendEmail(db.getEmailById(cmodel.Id_Donor), "תודה רבה על תרומתך, נבחן את ההצעה וניצור איתך קשר", "תרומתך נקלטה במערכת");
                }
                else
                {
					if(!db.getContribution(cmodel.Id_Contribution).Status.Equals("נמכר") && cmodel.Status.Equals("נמכר"))
                        sendEmail(db.getEmailById(cmodel.Id_Donor) , "תרומתך הועברה לנזקק", "תודה רבה על תרומתך");
                    db.EditDonation(cmodel);
                }

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


                return RedirectToLocal(returnUrl);

            }
            return View(new Tuple<Contribution, Picture>(cmodel , pmodel));

        }


        //Action result for ajax call
        [HttpPost]
        public JsonResult getSubCategoryByCategoryId(string categoryName)
        {
            int categoryId = db.getSubCategoryIdByCotegoryName(categoryName);
            List<string> subCategories = new List<string>();
            subCategories = db.getSubCategoryByIdCotegory(categoryId);
            SelectList subCategories1 = new SelectList(subCategories, "Id", "Name", 0);
            //return Json(subCategories1);
            return Json(subCategories);
        }
     


        public void sendEmail(string email, string msg , string subject)
        {
            var fromAddress = new MailAddress("meyadleyaadoffice@gmail.com", "מיד ליעד");
            string mail = email;
            var toAddress = new MailAddress(mail);
            const string fromPassword = "office123!";
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
                try
                {
                    smtp.Send(message);
                }
                catch (Exception e) { }
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                if(Session["type"] == null)
                    return RedirectToAction("Login", "Account");


                if (Session["type"].ToString().Equals("1"))
                    return RedirectToAction("Index", "Index");
                else
                {
                    ViewBag.msg = "תודה על תרומתך";
                    return RedirectToAction("DonorHomePage", "DonorHomePage", new { msg = "תודה" });
                }
            }
        }

    }
}
