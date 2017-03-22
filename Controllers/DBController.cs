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

        Database1Entities4 db = new Database1Entities4();

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

        public void AddDonor(Donor dmodel, Schedule smodel)
        {
            Donor d = db.Donor.Add(new Donor { Email = dmodel.Email, First_Name = dmodel.First_Name, Last_Name = dmodel.Last_Name, Building = dmodel.Building, City = dmodel.City, Floor = dmodel.Floor, House = dmodel.House, Street = dmodel.Street, Phone = dmodel.Phone, Fax = dmodel.Fax, Another_Phone = dmodel.Another_Phone, Comments = dmodel.Comments });
            db.SaveChanges();
            int id = d.Id_Donor;
            db.Schedule.Add(new Schedule { Day = smodel.Day, Id_User = id, End_Time = smodel.End_Time, Start_Time = smodel.Start_Time });
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
            c.Modified_Status_Date = DateTime.Now;
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


        public int AddDonation(Contribution model , int id)
        {
            //int id = getIdByEmail(email);
            Contribution c =  db.Contribution.Add(new Contribution { Id_Donor = id, Category = model.Category, Sub_Category = model.Sub_Category, Position = model.Position, Description = model.Description, Modified_Status_Date = DateTime.Now, Status = "לפני סינון"  , years = model.years});
            db.SaveChanges();
            return c.Id_Contribution;
        }

        public int AddDonation(Contribution model, string email)
        {
            return AddDonation(model, getIdByEmail(email));
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

        public int getUserId(String email, String password)
        {
            var type = db.Users.Where(u => (u.Email == email && u.Password == password)).Select(u => u.Id_Donor);
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
                DisplayDonation d = new DisplayDonation(c.Category, c.Sub_Category, getDonor(c.Id_Donor).City, url, c.Id_Contribution , getDonorName(getEmailById(c.Id_Donor)) , c.Status);
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

        public List<string> getAllCities()
        {

            return  db.Donor.Select(s=>s.City).Distinct().ToList();
              
        }

        public List<DisplayDonation> getDonationsOfDonor(string email)
        {
            int id = getIdByEmail(email);
            //List <Contribution>donations= db.Contribution.Where(c => (c.Id_Donor == id)).ToList();
            List<Contribution> donations =
            (from c in db.Contribution 
            join s in db.Statuses 
            on c.Status equals s.Name 
            where (c.Id_Donor == id && s.Taken == false)
            select c).ToList();

            List<DisplayDonation> dDonations = new List<DisplayDonation>();
            foreach(Contribution c in donations){
                Picture p = getPicture(c.Id_Contribution);
                string url = p == null ? "" : "..\\" + p.Url;
                DisplayDonation d = new DisplayDonation(c.Category, c.Sub_Category, c.Status , url, c.Id_Contribution);
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

        public List<DisplayDonation> getContributionsInStorehouse()
        {
            List<Contribution> con =  db.Contribution.Where(c => (c.Status == "במחסן")).ToList();
            List<DisplayDonation> dDonations = new List<DisplayDonation>();
            foreach (Contribution c in con)
            {
                Picture p = getPicture(c.Id_Contribution);
                string url = p == null ? "" : "..\\" + p.Url;
                DisplayDonation d = new DisplayDonation(c.Category, c.Sub_Category, c.Status, url, c.Id_Contribution);
                dDonations.Add(d);
            }
            return dDonations;
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
            if (categoryName.Equals(""))
                return -1;
           return db.Categories.FirstOrDefault(m => (m.Name == categoryName)).Id;
        }



        

        public List<Donor> searchDonors(Donor search)
        {
            return db.Donor.Where(d => (d.City == search.City || d.First_Name == search.First_Name || d.Last_Name == search.Last_Name)).ToList();
        }

      
       

        public List<Contribution> serachDontions(Contribution search)
        {
            /*return db.Contribution.Where(c=>( /*search.Category != null  && !search.Category.Equals("") &&  c.Category == search.Category))
                 .Where(c => (/* search.Sub_Category != null &&!search.Sub_Category.Equals("") &&  c.Sub_Category == search.Sub_Category.Trim()))
                 .Where(c => (/*search.Status != null &&  !search.Status.Equals("") && c.Status == search.Status))
                 .Where(c => (/*search.Position != null && !search.Position.Equals("") && c.Position == search.Position)).ToList();*/
             List<Contribution> result = (from c in db.Contribution
                                          where (/*search.Status != null &&*/ search.Status == c.Status)
                                          where (/*search.Category != null && */search.Category == c.Category)
                                          where (/*search.Sub_Category != null &&*/ search.Sub_Category == c.Sub_Category)
                                          where (/*search.Position != null &&*/ search.Position == c.Position)
                                          select c).ToList();

            //List<Contribution> donationPerDay =
            //(from c in db.Contribution 
            //join s in db.Schedule on c.Id_Donor equals s.Id_User 
            //where s.Day == day
            //select c).ToList();

             return result;
        }
  

        //calculate the route - help functions
        public List<Contribution> getContributionByStatus(string status)
        {
            return db.Contribution.Where(c => (c.Status == status)).ToList();
       
        }

        public Donor getDonorOfDonation(int /*string*/contributionId)
        {
            int idDonor;
            idDonor = db.Contribution.FirstOrDefault(c => (c.Id_Contribution == contributionId)).Id_Donor;
            return db.Donor.FirstOrDefault(d => (d.Id_Donor == idDonor));
        }

        public List<string> getAddressOfDonor(int idDonor)
        {
            Donor donor= db.Donor.FirstOrDefault(d => (d.Id_Donor == idDonor));
            List<string> address = new List<string>();
            address.Add(donor.City);
            address.Add(donor.Street);
            address.Add(donor.Building.ToString());
            address.Add(donor.House.ToString());
            address.Add(donor.Floor.ToString());

            return address;
        }
          
        public List<Contribution> getDonationByDay(string day)
        {

            List<Contribution> donationPerDay = new List<Contribution>();
            List<Contribution> donationsPerDonor = new List<Contribution>();
            List<int> id_donors =   (from d in db.Donor
                                    join s in db.Schedule
                                    on d.Id_Donor equals s.Id_User
                                    where s.Day == day
                                    orderby d.City,s.End_Time 
                                    select d.Id_Donor).ToList();

            foreach (int id in id_donors)
            {
                donationsPerDonor = getDonationsOfDonor(id).ToList();
                foreach(var c in donationsPerDonor)
                {
                    donationPerDay.Add(c);
                }
            }
            return donationsPerDonor;
        }

        public List<Contribution> getDonationsOfDonor(int id_donor)
        {
            //int id = getIdByEmail(email);
            return db.Contribution.Where(c => (c.Id_Donor == id_donor)).ToList();

        }

        public List<Schedule> getScheduleForDonor(int idDonor)
        {
                      
              return db.Schedule.Where(s => (s.Id_User == idDonor)).ToList();           

        }
      

        //public Dictionary<string, Dictionary<string, List<Contribution>>> getDonationByDayAndCity(string day)
        //{

        //    Dictionary<string, List<Contribution>> dictContributionInCity = new Dictionary<string, List<Contribution>>();
        //    Dictionary<string, Dictionary<string, List<Contribution>>> dictDayAndCityDonation = new Dictionary<string, Dictionary<string, List<Contribution>>>();
        //    //get all the contribution from specific day,
        //    List<Contribution> donationPerDay = getDonationByDay(day);
        //    //dictDayAndCityDonation[day]
        //    //sort by city
        //    List<Contribution> con = new List<Contribution>();
        //    List<string> cities = getAllCities();
        //    Donor donor;
        //    foreach (var city in cities)
        //    {
        //        dictContributionInCity[city] = con;
        //        foreach (var donation in donationPerDay)
        //        {
        //            donor = db.Donor.FirstOrDefault(d => (d.Id_Donor == donation.Id_Donor));
        //            if (donor.City == city)
        //                dictContributionInCity[city].Add(donation);
        //        }
        //    }

        //    dictDayAndCityDonation[day] = dictContributionInCity;
        //    return dictDayAndCityDonation;

        //}




        //public List<Contribution> getDonationByCityAndDay(List<Contribution> c, string city, string day )
        //{

        //}
        public Donor getDonorByContribution(int idDonor)
        {
            Donor donor = db.Donor.FirstOrDefault(d => (d.Id_Donor == idDonor));
            return donor;
        }
                    
        
    }
}
