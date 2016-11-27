using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MeyadLeyaad1.Controllers;

namespace MeyadLeyaad1.Models
{
    public class DisplayDonation
    {
        DBController db = new DBController();
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string City { get; set; }
        public string PictureURL { get; set; }
        public string DonorName { get; set; }
        public string StatusName { get; set; }
        public int ID_Contribution { get; set; }

        public DisplayDonation(string Category, string SubCategory, string City, string PictureURL, int ID_Contribution , string DonorName , string statusName)
        {
            this.Category = Category;
            this.SubCategory = SubCategory;
            this.City = City;
            this.PictureURL = PictureURL;
            this.ID_Contribution = ID_Contribution;
            this.DonorName = DonorName;
            this.StatusName = statusName;
        }

        public DisplayDonation(string Category, string SubCategory, string status, string PictureURL, int ID_Contribution)
        {
            this.Category = Category;
            this.SubCategory = SubCategory;
            this.StatusName = status;
            this.PictureURL = PictureURL;
            this.ID_Contribution = ID_Contribution;
        }
       
    }
}