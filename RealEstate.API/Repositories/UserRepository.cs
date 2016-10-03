using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.API.Models;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Web.Http;

namespace RealEstate.API.Repositories
{
    enum baseValue
    {
        sayThanks=3,
        helpful=2,
        Review=5
    } 

    public class UserRepository
    {

        #region UserProfile

        public bool AddRecentlyViewed(RecentlyViewed objRecentlyViewed)
        {
            using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
            {
                try
                {
                    tblRecentlyViewed objTblRecentlyViewed = new tblRecentlyViewed();
                    objTblRecentlyViewed.UserId = objRecentlyViewed.UserId;
                    objTblRecentlyViewed.ProjectId = objRecentlyViewed.ProjectId;
                    objTblRecentlyViewed.Time = DateTime.Now;

                    dbContext.tblRecentlyVieweds.AddObject(objTblRecentlyViewed);
                    return dbContext.SaveChanges() > 0;
                }
                catch(Exception ex)
                {
                    return false;
                }
            }
        }


        public bool GetUserDetailsByUserId(int userId, out object UserDetails)
        {
            using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
            {
                try
                {

                    UserDetails = (from u in dbContext.tblUsers
                                   where u.Id == userId && u.IsDeleted == 0
                                   select new
                                   {
                                       u.FirstName,
                                       u.LastName,
                                       u.ProfilePictureUrl,
                                       u.EmailId,
                                       u.Contact,
                                       u.LevelId,
                                       u.City,
                                       u.Points,
                                       userType = (from ut in dbContext.tblUserTypes
                                                   where u.UserTypeId == ut.Id
                                                   select new { ut.Id, ut.UserTypeName}).FirstOrDefault(),
                                       reviewCount = u.tblReviews.Count(),
                                       helpfulCount = dbContext.tblReviews.Where(r => r.UserId == userId).Select(r => (int?)r.Helpful).Sum()?? 0,
                                       sayThanksCount = dbContext.tblReviews.Where(r => r.UserId == userId).Select(r => (int?)r.SayThanks).Sum()?? 0,
                                       reviewList = (from r in dbContext.tblReviews
                                                 
                                                     where r.UserId == userId && r.IsReviewed == true
                                                     orderby r.Time descending
                                                     select new
                                                     {
                                                         r.Helpful,
                                                         r.Text,
                                                         r.Time,
                                                         r.SayThanks,
                                                         r.lastVisited,
                                                         //duration = ReviewRepository.CalculateLastVisitedDuration(lv.MonthId, lv.tblYear.YearName),
                                                         r.Heading,
                                                         BuilderComment = (from bc in dbContext.tblBuilderComments
                                                                           where bc.ReviewId == r.Id && bc.IsVerified == true
                                                                           select new { bc.Text, bc.IsConvinced }).FirstOrDefault(),
                                                     }).ToList(),
                                       recentlyViewed = (from rv in dbContext.tblRecentlyVieweds
                                                         where rv.UserId == userId
                                                         orderby rv.Time descending
                                                         select new
                                                         {
                                                             rv.ProjectId,
                                                             rv.tblProject.Name,
                                                             rv.tblProject.AverageRating,
                                                             profilePicture = (from i in dbContext.tblImages
                                                                               where i.ProjectId == rv.ProjectId && i.ImageCategoryId == 1
                                                                               select new { i.ImageUrl}).FirstOrDefault(),
                                                             AddressLine1 = (from l in dbContext.tblLocations
                                                                             join c in dbContext.tblCities
                                                                             on l.CityId equals c.Id
                                                                             where l.ProjectId == rv.ProjectId
                                                                             select new
                                                                             {
                                                                                 l.AddressLine1,
                                                                                 l.Id
                                                                             }).FirstOrDefault(),
                                                             AddressLine2 = (from l in dbContext.tblLocations
                                                                             join c in dbContext.tblCities
                                                                             on l.CityId equals c.Id
                                                                             where l.ProjectId == rv.ProjectId
                                                                             select new
                                                                             {
                                                                                 l.AddressLine2,
                                                                                 l.Id
                                                                             }).FirstOrDefault(),
                                                             City = (from l in dbContext.tblLocations
                                                                     join c in dbContext.tblCities
                                                                     on l.CityId equals c.Id
                                                                     where l.ProjectId == rv.ProjectId
                                                                     select new
                                                                     {
                                                                         c.Name,
                                                                         c.Id
                                                                     }).FirstOrDefault(),
                                                         }).Take(5).ToList(),
                                       followedProperty = (from fp in dbContext.tblFollowProjects
                                                           where fp.UserId == userId
                                                           select new
                                                           {
                                                               fp.ProjectId,
                                                               fp.tblProject.Name,
                                                               ProfilePicture = (from i in dbContext.tblImages
                                                                                 where i.ProjectId == fp.ProjectId && i.ImageCategoryId == 1
                                                                                 select new { i.ImageUrl}).FirstOrDefault(),
                                                               AddressLine1 = (from l in dbContext.tblLocations
                                                                               join c in dbContext.tblCities
                                                                               on l.CityId equals c.Id
                                                                               where l.ProjectId == fp.ProjectId
                                                                               select new
                                                                               {
                                                                                   l.AddressLine1,
                                                                                   l.Id
                                                                               }).FirstOrDefault(),
                                                               AddressLine2 = (from l in dbContext.tblLocations
                                                                               join c in dbContext.tblCities
                                                                               on l.CityId equals c.Id
                                                                               where l.ProjectId == fp.ProjectId
                                                                               select new
                                                                               {
                                                                                   l.AddressLine2,
                                                                                   l.Id
                                                                               }).FirstOrDefault(),
                                                               City = (from l in dbContext.tblLocations
                                                                       join c in dbContext.tblCities
                                                                       on l.CityId equals c.Id
                                                                       where l.ProjectId == fp.ProjectId
                                                                       select new
                                                                       {
                                                                           c.Name,
                                                                           c.Id
                                                                       }).FirstOrDefault(),
                                                           }).ToList()
                                   }).FirstOrDefault();
                    return true;
                }
                catch (Exception ex)
                {
                    UserDetails = null;
                    return false;
                }
            }
        }

        public int EditProfileByUserId(User objUser)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    tblUser objTblUser = dbContext.tblUsers.Where(u => u.Id == objUser.Id).FirstOrDefault();
                    if (objTblUser != null)
                    {
                        objTblUser.City = objUser.City;
                        objTblUser.Contact = objUser.Contact;
                        objTblUser.FirstName = objUser.FirstName;
                        objTblUser.LastName = objUser.LastName;
                        objTblUser.UserTypeId = objUser.UserTypeId;
                        objTblUser.ProfilePictureUrl = objUser.ProfilePictureUrl;

                        dbContext.SaveChanges();
                        return 1;                                       //profile edited
                    }
                    return 2;                                           //user not found
                }
            }
            catch (Exception ex)
            {
                return 0;                                               //operation failed
            }
        }

        public int UserForgotPassword(string userEmail)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    var userDetails = (from u in dbContext.tblUsers
                                       where u.EmailId.Equals(userEmail)
                                       select new { u, u.FirstName, u.LastName, u.EmailId }).FirstOrDefault();
                    if (userDetails != null)
                    {
                        string mailSubject = "New Password";
                        string newPassword = RandomPassword();
                        string mailBody = "<b>Hello </b>" + userDetails.FirstName + userDetails.LastName + ",<br/><br/>";
                        mailBody += "<b>Your new pasword is: </b>" + newPassword + "<br/><br/>";
                        mailBody += "Kindly login with this and then reset your password";

                        Notification objNotification = new Notification();
                        bool mailSent = objNotification.SendMail(userDetails.EmailId, mailSubject, mailBody);

                        if (mailSent != false)
                        {
                            userDetails.u.Password = newPassword;
                            dbContext.SaveChanges();
                            return 1;                                                           //successful new password mail sent
                        }
                        return 2;                                                               //operation failed
                    }
                    return 3;                                                                  //email-id not regestered
                }
            }
            catch (Exception ex)
            {
                return 0;                                                                       //exception
            }

        }

        public int UserResetPassword(int userId, string oldPassword, string newPassword)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    tblUser objtblUser = dbContext.tblUsers.SingleOrDefault(u => u.Id == userId && u.Password.Equals(oldPassword));
                    if (objtblUser != null)
                    {
                        objtblUser.Password = newPassword;
                        dbContext.SaveChanges();
                        return 1;                                                   //password reset successfully
                    }
                    return 2;                                                        //invalid credentials
                }
            }
            catch (Exception ex)
            {
                return 0;                                                             //operation failed
            }
        }

        #endregion

        #region Functions

        public static string RandomPassword()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var stringChars = new char[8];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var finalString = new String(stringChars);
            return finalString;
        }

        public bool CalculateLevel(int userId,int reviewId, int baseValue) {

            using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
            {
                var reviewedUserInfo = (from u in dbContext.tblUsers
                            join r in dbContext.tblReviews
                            on u.Id equals r.UserId
                            where r.Id == reviewId
                            join l in dbContext.tblLevels
                            on u.LevelId equals l.Id
                            select new {UserId=r.UserId, LevelId = u.LevelId, l.Points,UserPoints=u.Points }
                          ).FirstOrDefault();
                var reviewerUserInfo = (from u in dbContext.tblUsers
                                        where u.Id == userId
                                        select new { LevelId = u.LevelId }).FirstOrDefault();
                int points = 0;
                if(reviewerUserInfo.LevelId>reviewedUserInfo.LevelId)               //y>x  //2*(y-x)*baseprice
                {
                   points = 2 * (reviewerUserInfo.LevelId - reviewedUserInfo.LevelId) * baseValue;
                }
                else if(reviewerUserInfo.LevelId <= reviewedUserInfo.LevelId)
                {
                    points = baseValue;
                }

                points = reviewedUserInfo.UserPoints + points;
                int userLevel;
                if (points > reviewedUserInfo.Points)
                {
                    userLevel = reviewedUserInfo.LevelId + 1;
                }
                else
                    userLevel = reviewedUserInfo.LevelId;
                tblUser objTblUser = dbContext.tblUsers.Where(u => u.Id == reviewedUserInfo.UserId).FirstOrDefault();
                objTblUser.LevelId = userLevel;
                objTblUser.Points = points;
                if (dbContext.SaveChanges() > 0)
                    return true;
                return false;
            }
        }

        #endregion


        #region PropertyPage

        public int FollowProject(FollowProject objFollowProject)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    tblFollowProject objTblFollowProject = new tblFollowProject();
                    objTblFollowProject.UserId = objFollowProject.UserId;
                    objTblFollowProject.ProjectId = objFollowProject.ProjectId;
                    dbContext.tblFollowProjects.AddObject(objTblFollowProject);
                    bool status = dbContext.SaveChanges() > 0;
                    if (status != false)
                    {
                        var userDetails = (from u in dbContext.tblUsers
                                           where u.Id == objFollowProject.UserId
                                           select new { u, u.FirstName, u.LastName, u.EmailId }).FirstOrDefault();
                        var projectDetails = (from p in dbContext.tblProjects
                                           where p.Id == objFollowProject.ProjectId
                                           select new { p.Name }).FirstOrDefault();
                        string mailSubject = "New Project Followed";
                        string mailBody = "<b>Hello </b>" + userDetails.FirstName + userDetails.LastName + ",<br/><br/>";
                        mailBody += "<b>Your have followed project </b>" + projectDetails.Name + "<br/><br/>";
                        mailBody += "Stay tuned for the updates.";

                        Notification objNotification = new Notification();
                        bool mailSent = objNotification.SendMail(userDetails.EmailId, mailSubject, mailBody);

                        if (mailSent != false)
                        {
                            return 1;                                                           //followed project mail sent
                        }
                        return 2;                                                               //followed project mail not sent
                    }
                    return 3;                                                                   //unable to follow
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool UnFollowProject(FollowProject objFollowProject)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    tblFollowProject objtblFollowProject = dbContext.tblFollowProjects.Where(fp => fp.UserId == objFollowProject.UserId && fp.ProjectId == objFollowProject.ProjectId).FirstOrDefault();
                    dbContext.DeleteObject(objtblFollowProject);
                    return dbContext.SaveChanges() > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UnfollowAll(int userId)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    var followedProjects = (from fp in dbContext.tblFollowProjects
                                           where fp.UserId == userId
                                           select (fp)).ToList();
                    foreach(var fProject in followedProjects)
                    {
                        dbContext.tblFollowProjects.DeleteObject(fProject);
                    }
                    bool status = dbContext.SaveChanges() > 0;
                    return status;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region LandingPage

        public int RegisterUser(User objUser, out List<object> Person)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    bool DoEmailExist = (((dbContext.tblUsers.Where(u => u.EmailId == objUser.EmailId).Count()) == 1) ? true : false);
                    //int registeredMode = (dbContext.tblUsers.Where(u => u.EmailId == objUser.EmailId).Select(u => u.UserRegMode).FirstOrDefault());

                    var RegisteredMode = (from u in dbContext.tblUsers
                                          where u.EmailId == objUser.EmailId
                                          select new
                                          {
                                              u.Id,
                                              u.UserRegMode
                                          }).FirstOrDefault();

                    if (DoEmailExist == true && RegisteredMode.UserRegMode ==false)
                    {
                        List<object> loginPerson = new List<object>();
                        string type = "User";
                        loginPerson.Add(RegisteredMode.Id);
                        loginPerson.Add(RegisteredMode.UserRegMode);
                        loginPerson.Add(type);
                        Person = loginPerson;
                        return 4;                                           //already social signed up

                    }

                    if (DoEmailExist != true)
                    {
                        tblUser objTblUser = new tblUser();
                        objTblUser.City = objUser.City;
                        objTblUser.Contact = objUser.Contact;
                        objTblUser.EmailId = objUser.EmailId;
                        objTblUser.FirstName = objUser.FirstName;
                        objTblUser.LastName = objUser.LastName;
                        objTblUser.LevelId = 2;                                 //change to 1
                        objTblUser.IsDeleted = 0;
                        objTblUser.Points = 5;
                        objTblUser.Password = objUser.Password;
                        objTblUser.UserTypeId = objUser.UserTypeId;
                        objTblUser.RegisteredDate = DateTime.Now;
                      
                        objTblUser.ProfilePictureUrl = objUser.ProfilePictureUrl;
                        objUser.UserRegMode = objUser.UserRegMode;
                        dbContext.tblUsers.AddObject(objTblUser);

                        //string physicalPathProjectImage = System.Web.HttpContext.Current.Server.MapPath("~\\" + ("UserImages") + "\\" + objUser.displayPicture.FileName);
                        //objUser.displayPicture.SaveAs(physicalPathProjectImage);

                        bool status = dbContext.SaveChanges() > 0;
                        int userId = objTblUser.Id;
                        
                        if (userId != 0)
                        {
                            List<object> loginPerson = new List<object>();
                            string type = "User";
                            loginPerson.Add(userId);
                            loginPerson.Add(1);
                            loginPerson.Add(type);
                            Person = loginPerson;
                            return 1;                                        //user registered
                        }
                        Person = null; 
                        return 2;                                              //unsucccessful
                    }
                    else
                    {
                        Person = null;
                        return 3;                                               //EMAIL ID already exist
                    }
                }
            }
            catch (Exception ex)
            {
                Person = null;
                return 0;                                                       //operation failed
            }
        }


        public bool Login(User objUser, out List<object> Person)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    //var user = dbContext.tblUsers.Where(u => u.EmailId == objUser.EmailId && u.Password == objUser.Password).Select(u => u.Id).FirstOrDefault();
                    var user = (from u in dbContext.tblUsers
                                where u.EmailId == objUser.EmailId && u.Password == objUser.Password
                                select new { u.Id, u.UserRegMode }).FirstOrDefault(); 
                    if (user != null)
                    {
                        List<object> loginPerson = new List<object>();
                        string type = "User";
                        loginPerson.Add(user.Id);
                        loginPerson.Add(user.UserRegMode);
                        loginPerson.Add(type);
                        Person = loginPerson;
                        return true;
                    }
                    else
                    {
                        int builderId = dbContext.tblBuilders.Where(u => u.CompanyEmailId == objUser.EmailId && u.Password == objUser.Password).Select(u => u.Id).FirstOrDefault();
                        if (builderId != 0)
                        {
                            List<object> loginPerson = new List<object>();
                            string type = "Builder";
                            loginPerson.Add(builderId);
                            loginPerson.Add("Web");
                            loginPerson.Add(type);
                            Person = loginPerson;
                            return true;
                        }
                        else
                        {
                            Person = null;
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Person = null;
                return false;
            }
        }

        public bool GetAllUserType(out object userTypeList)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    userTypeList = (from c in dbContext.tblUserTypes
                                    orderby c.UserTypeName
                                    select new { c.Id, c.UserTypeName }).ToList();
                    return true;
                }
            }
            catch (Exception ex)
            {
                userTypeList = null;
                return false;
            }
        }


        #endregion

        #region NotInUse

        public bool GetUserFirstName(int userId, out string Name)
        {

            List<object> objUser = new List<object>();
            using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
            {
                try
                {
                    var result = (from u in dbContext.tblUsers
                                  where u.Id == userId
                                  select new { u.FirstName }).FirstOrDefault();
                    if (result != null)
                    {

                        Name = result.FirstName;

                        return true;
                    }
                    else
                    {
                        Name = null;
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Name = null;
                    return false;
                }
            }
        }

        #endregion

    }
}

