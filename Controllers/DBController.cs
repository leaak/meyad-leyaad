using System;
using System.Collections.Generic;
using System.Linq;
//using System.D
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using MeyadLeyaad1.Models;

namespace MeyadLeyaad1.Controllers
{
    public class DBController : Controller
    {

        Database1Entities2 db = new Database1Entities2();

        //
        // GET: /DB/


        public void AddUser(RegisterModel model)
        {
            db.Users.Add(new Users { User_Name = model.User_Name, Email = model.User_Name, Password = model.Password });
            db.SaveChanges();

        }

        public bool isUserExists(String User_Name, String Password)
        {
            return db.Users.Any(u => (u.User_Name == User_Name && u.Password == Password));
        }

        public bool isEmailExists(String Email)
        {
            return db.Users.Any(u => u.Email == Email);
        }

        public void AddDonor(Donor model)
        {
            db.Donor.Add(new Donor { Email = model.Email, First_Name = model.First_Name, Last_Name = model.Last_Name, Building = model.Building, City = model.City, Floor = model.Floor, House = model.House, Street = model.Street, Phone = model.Phone, Fax = model.Fax, Another_Phone = model.Another_Phone, Comments = model.Comments });
            db.SaveChanges();
        }

        public void EditDonor(Donor model)
        {
            Donor d = getDonor(model.Id_Donor);
            d.Email = model.Email;d.First_Name = model.First_Name; d.Last_Name = model.Last_Name; d.Building = model.Building; d.City = model.City; d.Floor = model.Floor; d.House = model.House; d.Street = model.Street; d.Phone = model.Phone; d.Fax = model.Fax; d.Another_Phone = model.Another_Phone; d.Comments = model.Comments;
            db.SaveChanges();
        }

        public void EditDonation(Contribution c)
        {
            Contribution origin = getContribution(c.Id_Contribution);
            c.Modified_Status_Date = DateTime.Now; c.Status = "3";//TODO: change
            db.Entry(origin).CurrentValues.SetValues(c);
            db.SaveChanges();
        }

        public void DeleteDonation(string dd)
        {
            Contribution c = getContribution(Convert.ToInt32(dd));
            db.Contribution.Remove(c);
            db.SaveChanges();

           // db.Donor.InsertOnSubmit(new Donor { Email = model.Email, First_Name = model.First_Name, Last_Name = model.Last_Name, Building = model.Building, City = model.City, Floor = model.Floor, House = model.House, Street = model.Street, Phone = model.Phone, Fax = model.Fax, Another_Phone = model.Another_Phone, Comments = model.Comments });
           // db.SubmitChanges();
        }


        public int AddDonation(Contribution model , string email)
        {
            int id = getIdByEmail(email);
            int newID =  db.Contribution.Add(new Contribution { Id_Donor = id, Category = model.Category, Sub_Category = model.Sub_Category, Position = model.Position, Description = model.Description, Modified_Status_Date = DateTime.Now, Status = model.Status }).Id_Contribution;
            db.SaveChanges();
            return newID;
        }

        public List<String> getStatuses()
        {
            List<String> _s = (from t in db.Statuses
                               select t.Name).ToList();

            return _s;
        }

        public List<String> getCategories()
        {
            List<String> _c = (from t in db.Categories
                               select t.Name).ToList();

            return _c;


            //     List<SelectListItem> items = new List<SelectListItem>();

            //     foreach (var item in _c)
            //{
            //    items.Add(new SelectListItem
            //    {
            //        Text = item.name,
            //    });
            //}
            //     AvailableTypeList = new SelectList(items);

            //     return AvailableTypeList;
            //}

        }

        public List<String> getSubCategories(String category)
        {

            List<String> _c = (from s in db.SubCategories
                               join c in db.Categories
                               on s.Id_Category equals c.Id
                               select s.Name).ToList();

            return _c;
        }

        public int getNextContributionId()
        {
            int count = db.Contribution.Count();
            return count + 1;
        }

        public int getUserType(String email, String password)
        {
            var type = db.Users.Where(u => (u.Email == email && u.Password == password)).Select(u => u.Type);
            // return type.FirstOrDefault();
            return Convert.ToInt32(type.FirstOrDefault());

            // int t = (from u in db.Users where u.User_Name == userName && u.Password == password select u.Type);

        }

        public string getUserPassword(String email)
        {
            return db.Users.Where(u => (u.Email == email)).Select(u => u.Password).ToString();
            // return type.FirstOrDefault();

            // int t = (from u in db.Users where u.User_Name == userName && u.Password == password select u.Type);

        }

        public void AddPicture( int idPicture, string path, int idContribution , string id="")
        {
         
            db.Picture.Add(new Picture {Id_Picture=idPicture,Id_Cotribution=idContribution, Url=path });
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }
            
        }
        public int getNextPictureId()
        {
            int count = db.Picture.Count();
            return count + 1;
        }

        public  List<DisplayDonation> getLatestDonations()
        {
            DateTime thisDay = DateTime.Today;
            DateTime lastWeek = thisDay.AddDays(-(int)thisDay.DayOfWeek - 7);
            List <Contribution>donations= db.Contribution.Where(c => (c.Modified_Status_Date > lastWeek)).ToList();
            List<DisplayDonation> dDonations = new List<DisplayDonation>();
            foreach(Contribution c in donations){
                Picture p = getPicture(c.Id_Contribution);
                string url = p == null ? "" : "..\\" + p.Url;
                DisplayDonation d = new DisplayDonation(c.Category, c.Sub_Category, getDonor(c.Id_Donor).City, url, c.Id_Contribution , getDonorName(getEmailById(c.Id_Donor)) , getStatusName(c.Status));
                dDonations.Add(d);
            }
            return dDonations;
        }


        public string getEmailById(int id)
        {
            var d = db.Donor.FirstOrDefault(c => (c.Id_Donor == id));
            return d.Email;
        }

        public int getIdByEmail(string email)
        {
            var d = db.Donor.FirstOrDefault(c => (c.Email == email));
            return d.Id_Donor;
        }

        public string getDonorName(string email)
        {
            var d = db.Donor.FirstOrDefault(c => (c.Email == email));
            string fullName=d.First_Name + " " + d.Last_Name;
            return fullName;

        }

        public string getStatusName(string id)
        {
            int i = Convert.ToInt32(id);
            var status = db.Statuses.FirstOrDefault(u => (u.Id_Status == i));
            string sName=status.Name;
            // return type.FirstOrDefault();
            return sName;
        }
        public List<Donor> getAllDonors()
        {        
            List<Donor> dDonations = new List<Donor>();
            foreach (Donor d in db.Donor)
            {
                dDonations.Add(d);
            }
            return dDonations;
        }
        public List<DisplayDonation> getDonationsOfDonor(string email)
        {
            int id = getIdByEmail(email);
            List <Contribution>donations= db.Contribution.Where(c => (c.Id_Donor == id)).ToList();
            List<DisplayDonation> dDonations = new List<DisplayDonation>();
            foreach(Contribution c in donations){
                Picture p = getPicture(c.Id_Contribution);
                string url = p == null ? "" : "..\\" + p.Url;
                DisplayDonation d = new DisplayDonation(c.Category, c.Sub_Category, getStatusName(c.Status) , url, c.Id_Contribution);
                dDonations.Add(d);
            }
            return dDonations;
        }
        public Picture getPicture(int idContribution)
         {
             var picture = db.Picture.FirstOrDefault(p => (p.Id_Cotribution == idContribution));
            return picture;
        }

        public Donor getDonor(int idDonor)
        {
            return db.Donor.FirstOrDefault(p => (p.Id_Donor == idDonor));
        }

        public Contribution getContribution(int id)
        {
            return db.Contribution.FirstOrDefault(c => (c.Id_Contribution == id));
        }

        public List<Contribution> getContribution(string category , string subc , string position)
        {
            return db.Contribution.Where(c => (c.Category == category && c.Sub_Category == subc && c.Position == position)).ToList();
        }

      /*  public List<SubCategories> getSubCategoryByIdCotegory(int idCotegory)
        {
            return db.SubCategories.Where(m => (m.Id_Category == idCotegory)).ToList();
        }
        */
        public List<string> getSubCategoryByIdCotegory(int idCotegory)
        {
            List<SubCategories> sc=  db.SubCategories.Where(m => (m.Id_Category == idCotegory)).ToList();   
            List<string> subNames=new List<string>();
            foreach(SubCategories item in sc)
            {
                   subNames.Add(item.Name);
            }
            return subNames;
        }

        public int getSubCategoryIdByCotegoryName(string categoryName)
        {
           return db.Categories.FirstOrDefault(m => (m.Name == categoryName)).Id;
        }



        

 public List<Donor> searchDonors(Donor search)
        {
            return db.Donor.Where(d => (d.City == search.City)).ToList();
        }

        public List<Contribution> serachDontions(Contribution search)
        {
            return db.Contribution.Where(c => (/*c.Status == search.Status &&*/ c.Category == search.Category && c.Sub_Category == search.Sub_Category)).ToList();
        }


    }
}
