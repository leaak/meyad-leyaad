using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MeyadLeyaad1.Models;
using System.IO;
using System.Net.Mail;
using System.Net;
//using System.Net.Http;
//using System.Web.Http;

namespace MeyadLeyaad1.Controllers
{
    public class SchedulerController : Controller
    {
        //
        // GET: /Scheduler/

        DBController db = new DBController();
        [HttpPost]

        public ActionResult Scheduler(String day = "")
        {
            List<Dictionary<string, string>> lst = new List<Dictionary<string,string>>();
            if(day != null && !day.Equals(""))
                lst = getDonationByDayAndCity(day);
            ViewBag.dict = lst;
            return View();
        }


        public ActionResult Scheduler()
        {
            List<Dictionary<string, string>> lst = new List<Dictionary<string, string>>();
            ViewBag.dict = lst;
            return View();
        }

        public List<Dictionary<string,string>> getDonationByDayAndCity(string day)
        {

            List<Dictionary<string,string>> dictContributionInCity = new List<Dictionary<string,string>>();
             //get all the contribution from specific day,
            List<Contribution> donationPerDay = db.getDonationByDay(day);
            
            Dictionary<string,string> details = new Dictionary<string,string>();
            Donor donor;
          
            foreach (var donation in donationPerDay)
            {
                List<Dictionary<string,string>> lst= new List<Dictionary<string,string>>();
                donor = db.getDonorByContribution(donation.Id_Donor);
                string address = donor.Street + ' ' + donor.Building + '/' + donor.House+" קומה "+ donor.Floor;
                string donorName = donor.First_Name + ' ' + donor.Last_Name;
                string donorPhone = donor.Phone;

                foreach (Schedule s in db.getSchedulfordonor(donor.Id_Donor))
                {
                    details = new Dictionary<string, string>();
                    details["בין השעות"] = s.Start_Time + " ל " + s.End_Time;
                    details["שם התורם"] = donorName;
                    details["טלפון"] = donorPhone;
                    details["כתובת"] = address;
                    details["עיר"] = donor.City;
                    details["המוצר"] = donation.Sub_Category;
                    details["מזהה"] = ""+donation.Id_Contribution;
                   
                }
                dictContributionInCity.Add(details);
            }
            return dictContributionInCity;
        }


    }
}
