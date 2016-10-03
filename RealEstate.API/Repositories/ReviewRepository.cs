using RealEstate.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.API.Repositories
{
    public class ReviewRepository
    {

        #region PropertyPage

        public int MarkConvinced(int builderCommentId, int userId)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    
                    tblBuilderComment objTblBuilderComment = dbContext.tblBuilderComments.Where(bc => bc.Id == builderCommentId).FirstOrDefault();
                    int User = dbContext.tblReviews.Where(r => r.Id == objTblBuilderComment.ReviewId).Select(r => r.UserId).FirstOrDefault();
                    if (User != userId)
                        return 1;                           //user not authorized to mark convinced
                    objTblBuilderComment.IsConvinced = true;
                    if (dbContext.SaveChanges() > 0)
                    {
                        
                        return 2;                               //marked convinced
                    }
                    return 3;                                   //unable to mark convinced
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int AddInappropriate(Inappropriate objInappropriate)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    //user which has posted review cannot mark Inappropriate
                    int IsReviewedBySameUser = dbContext.tblReviews.Where(r => r.Id == objInappropriate.ReviewId && r.UserId == objInappropriate.UserId).Count();
                    if (IsReviewedBySameUser > 0)
                        return 0;
                    //user which has marked helpful or say thanks cannot mark Inappropriate
                    int IsReviewMarked = dbContext.tblReviewDetails.Where(rd => rd.ReviewId == objInappropriate.ReviewId && rd.ReviewedUserId == objInappropriate.UserId).Count();
                    if (IsReviewMarked > 0)
                        return 1;
                    //user which has already marked Inappropriate cannot mark again
                    int IsInappropriateMarked = dbContext.tblInappropriates.Where(i => i.ReviewId == objInappropriate.ReviewId && i.UserId == objInappropriate.UserId).Count();
                    if (IsInappropriateMarked > 0)
                        return 2;
                    //Mark Inappropriate
                    tblInappropriate objTblInappropriate = new tblInappropriate();
                    objTblInappropriate.ReviewId = objInappropriate.ReviewId;
                    objTblInappropriate.UserId = Convert.ToInt32(objInappropriate.UserId);
                    objTblInappropriate.Reason = objInappropriate.Reason;

                    dbContext.tblInappropriates.AddObject(objTblInappropriate);
                    dbContext.SaveChanges();

                    
                    return 3;
                }
            }
            catch (Exception)
            {
                //Return false
                return 4;
            }
        }

        public int MarkHelpful(ReviewDetail objReviewDetail)       //calculation of level to be added
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    int reviewerId = dbContext.tblReviews.Where(r => r.Id == objReviewDetail.ReviewId).Select(r=>r.Id).FirstOrDefault();
                   
                    
                    if ( reviewerId != objReviewDetail.ReviewedUserId)
                    {
                        int AlreadyMarkedHelpful = dbContext.tblReviewDetails.Where(rd => rd.ReviewedUserId == objReviewDetail.ReviewedUserId && rd.ReviewId == objReviewDetail.ReviewId && rd.Helpful == true).Count();
                        
                        if (AlreadyMarkedHelpful == 0)
                        {
                            int AlreadyMarkedInappropriate = dbContext.tblInappropriates.Where(i => i.ReviewId == objReviewDetail.ReviewId && i.UserId == objReviewDetail.ReviewedUserId).Count();
                            
                            if (AlreadyMarkedInappropriate == 0)
                            {
                                tblReviewDetail objTblReviewDetail = new tblReviewDetail();
                                objTblReviewDetail.ReviewId = objReviewDetail.ReviewId;
                                objTblReviewDetail.ReviewedUserId = Convert.ToInt32(objReviewDetail.ReviewedUserId);
                                objTblReviewDetail.Helpful = true;
                                dbContext.tblReviewDetails.AddObject(objTblReviewDetail);
                                dbContext.SaveChanges();

                                
                                tblReview objTblReview = dbContext.tblReviews.Where(r => r.Id == objReviewDetail.ReviewId).First();
                                objTblReview.Helpful = objTblReview.Helpful + 1;
                                dbContext.SaveChanges();

                                var user = (from u in dbContext.tblUsers
                                            where u.Id == reviewerId
                                            select (u)).FirstOrDefault();
                                string mailSubject = "Review Marked Helpful";
                                string mailBody = "Your review has been marked helpful.<br/>";
                                Notification objNotification = new Notification();
                                bool status = objNotification.SendMail(user.EmailId, mailSubject, mailBody);
                                return 3;                 //successful
                            }
                            return 2;                       //user has already marked Inappropriate
                        }
                        return 1;                            //user has already marked helpful
                    }
                    return 0;                              //user is same as who posted it
                }
            }
                   
            catch (Exception ex)
                    {
                return 4;                               //unsuccessful
            }
                    }

        public int MarkSayThanks(ReviewDetail objReviewDetail)       //calculation of level to be added
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    int reviewerId = dbContext.tblReviews.Where(r => r.Id == objReviewDetail.ReviewId).Select(r => r.UserId).FirstOrDefault();

                    if (reviewerId != objReviewDetail.ReviewedUserId)
                    {
                        int AlreadySaidThanks = dbContext.tblReviewDetails.Where(rd => rd.ReviewedUserId == objReviewDetail.ReviewedUserId && rd.ReviewId == objReviewDetail.ReviewId && rd.SayThanks == true).Count();

                        if (AlreadySaidThanks == 0)
                        {
                            int AlreadyMarkedInappropriate = dbContext.tblInappropriates.Where(i => i.ReviewId == objReviewDetail.ReviewId && i.UserId == objReviewDetail.ReviewedUserId).Count();

                            if (AlreadyMarkedInappropriate == 0)
                            {
                    tblReviewDetail objTblReviewDetail = new tblReviewDetail();
                    objTblReviewDetail.ReviewId = objReviewDetail.ReviewId;
                    //changed
                    objTblReviewDetail.ReviewedUserId = Convert.ToInt32(objReviewDetail.ReviewedUserId);
                                objTblReviewDetail.SayThanks = true;
                    dbContext.tblReviewDetails.AddObject(objTblReviewDetail);
                    dbContext.SaveChanges();

                    tblReview objTblReview = dbContext.tblReviews.Where(r => r.Id == objReviewDetail.ReviewId).First();
                                objTblReview.SayThanks = objTblReview.SayThanks + 1;
                                dbContext.SaveChanges();

                                var user = (from u in dbContext.tblUsers
                                            where u.Id == reviewerId
                                            select (u)).FirstOrDefault();
                                string mailSubject = "Review Marked Helpful";
                                string mailBody = "Your review has been marked helpful.<br/>";
                                Notification objNotification = new Notification();
                                bool status = objNotification.SendMail(user.EmailId, mailSubject, mailBody);

                                return 3;                 //successful
                            }
                            return 2;                       //user has already marked Inappropriate
                        }
                        return 1;                            //user has already said thanks
                    }
                    return 0;                              //user is same as who posted it
                }
            }

            catch (Exception ex)
            {
                return 4;                               //unsuccessful
            }
        }

        #endregion

        #region AddReviewPage

        public int RecentReviewsByProjectId(int projectId, out object recentReviews)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    bool isProjectPresent = dbContext.tblProjects.Where(p => p.Id == projectId).Count() == 1 ? true : false;
                    if (isProjectPresent == true)
                    {
                        var Reviews = (from r in dbContext.tblReviews
                                    join u in dbContext.tblUsers
                                    on r.UserId equals u.Id
                                       where r.ProjectId == projectId && r.IsReviewed == true
                                       orderby r.Time descending
                                         select new { u.FirstName, u.LastName, u.LevelId, r.Text }).ToList().Take(5);
                        if (Reviews.Count() != 0)
                        {
                            recentReviews = Reviews;
                            return 1;    
                                                                                           //successful
                        }
                        recentReviews = Reviews;
                        return 2;                                                                   //no reviews
                    }
                    else
                    {
                        recentReviews = null;
                        return 3;                                                                   //project not present
                    }
                }
            }
            catch (Exception ex)
            {
                recentReviews = null;
                return 0;
            }
        }

        public int AddReview(Review objReview)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                        double sum = 0;

                        tblReview objTblReview = new tblReview();
                        objTblReview.ProjectId = objReview.ProjectId;
                        objTblReview.UserId = Convert.ToInt32(objReview.UserId);
                        objTblReview.Text = objReview.Text;
                        objTblReview.Time = System.DateTime.Now;
                        objTblReview.Heading = objReview.Heading;
                        objTblReview.IsReviewed = true;
                        objTblReview.SayThanks = 0;
                        objTblReview.Helpful = 0;
                    objTblReview.lastVisited = objReview.lastVisited;
                        dbContext.tblReviews.AddObject(objTblReview);
                        if (dbContext.SaveChanges() > 0)
                        {
                            for (int i = 0; i < objReview.reviewRatingDetails.Count; i++)
                            {
                                tblReviewRatingDetail objTblReviewRatingDetail = new tblReviewRatingDetail();
                                objTblReviewRatingDetail.ReviewId = objTblReview.Id;
                                objTblReviewRatingDetail.MasterReviewId = objReview.reviewRatingDetails[i].MasterReviewId;
                                objTblReviewRatingDetail.Value = objReview.reviewRatingDetails[i].Value;
                                sum += objReview.reviewRatingDetails[i].Value;
                                dbContext.tblReviewRatingDetails.AddObject(objTblReviewRatingDetail);
                                dbContext.SaveChanges();
                            }
                            objTblReview.AverageValue = sum / (objReview.reviewRatingDetails.Count());
                            int reviewId = objTblReview.Id;
                            dbContext.SaveChanges();
                            

                        //tblLastVisited objTblLastVisited = new tblLastVisited();
                        //objTblLastVisited.MonthId = objReview.MonthId;
                        //objTblLastVisited.YearId = objReview.YearId;
                        //objTblLastVisited.ReviewId = reviewId;
                        //dbContext.tblLastVisiteds.AddObject(objTblLastVisited);
                        dbContext.SaveChanges();
                        //int lastVisitedId = objTblLastVisited.Id;


                                string mailSubject = "New Review Added";
                                string mailBody = "<b>Hello Admin,</b><br/><br/>";
                                mailBody += "A new review has been added. <br/>Click on link below to confirm-<br/>";
                                mailBody += "<p><a href=#>Click here</a></p>";
                                mailBody += "review id is = " + reviewId;

                                Notification objNotification = new Notification();
                                bool mailSent = objNotification.SendMail("sukankshi.jain@gmail.com", mailSubject, mailBody);

                                if (mailSent != false)
                                {
                                    return 1;                                                           //review sent for admin confirmation
                                }
                                return 2;                                     //unable to sent for admin confirmation
                        }
                        return 3;                               //review cannot be addded
                }
            }
            catch (Exception ex)
            {
                return 0;                                   //Operation Failed
            }
        }

        //public bool GetAllMonth(out object monthList)
        //{
        //    try
        //    {
        //        using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
        //        {
        //            monthList = (from c in dbContext.tblMonths
        //                        orderby c.Id
        //                        select new { c.Id, c.MonthName }).ToList();
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        monthList = null;
        //        return false;
        //    }
        //}

        //public bool GetAllYear(out object yearList)
        //{
        //    try
        //    {
        //        using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
        //        {
        //            yearList = (from c in dbContext.tblYears
        //                         orderby c.Id
        //                         select new { c.Id, c.YearName }).ToList();
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        yearList = null;
        //        return false;
        //    }
        //}


        //public static List<int> CalculateLastVisitedDuration(int month, int year)
        //{
        //    DateTime toCompare = new DateTime(year, month, 1, 12, 00, 00);
        //    DateTime objPresentDateTime;
        //    objPresentDateTime = System.DateTime.Now;
        //    System.TimeSpan diff = objPresentDateTime.Subtract(toCompare);
        //    int totalDays = Convert.ToInt32(diff.TotalDays);
        //    int totalYears = Convert.ToInt32(totalDays / 365);
        //    int totalMonths = Convert.ToInt32((totalDays % 365) / 30);
        //    int remainingDays = Convert.ToInt32((totalDays % 365) % 30);
        //    List<int> duration = new List<int>();
        //    duration.Add(totalYears);
        //    duration.Add(totalMonths);
        //    duration.Add(totalDays);
        //    return duration;
        //}

        #endregion

        #region NotInUse

        public bool GetReviews(int cityId, int? userId, out object objReviews)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    if (userId != null)
                    {
                        var listOfProjectByCity = (from l in dbContext.tblLocations
                                                   join p in dbContext.tblProjects
                                                   on l.ProjectId equals p.Id
                                                   orderby p.AverageRating
                                                   where l.CityId == cityId
                                                   orderby p.AverageRating descending
                                                   select new { p }).ToList();

                        var listOfProjectByFollow = (from l in dbContext.tblLocations
                                                     join p in dbContext.tblProjects
                                                     on l.ProjectId equals p.Id
                                                     join r in dbContext.tblReviews
                                                     on l.ProjectId equals r.ProjectId
                                                     orderby p.AverageRating descending
                                                     where r.UserId == userId && l.CityId == cityId
                                                     select new { p }).ToList();
                        objReviews = listOfProjectByFollow.Concat(listOfProjectByCity.Except(listOfProjectByFollow)).ToList();
                    }
                    else
                    {
                        var listOfPropertyByCity = (from l in dbContext.tblLocations
                                                    join p in dbContext.tblProjects
                                                    on l.ProjectId equals p.Id
                                                    orderby p.AverageRating descending
                                                    where l.CityId == cityId
                                                    select new { p }).ToList();

                        var listOfAllProperty = (from p in dbContext.tblProjects
                                                 orderby p.AverageRating descending
                                                 select new { p }).ToList();
                        objReviews = listOfPropertyByCity.Concat(listOfAllProperty.Except(listOfPropertyByCity)).ToList();
                    }
                    return true;
                }
            }
            
            catch (Exception ex)
            {
                objReviews = null;
                return false;
            }
        }

        #endregion

    }
}