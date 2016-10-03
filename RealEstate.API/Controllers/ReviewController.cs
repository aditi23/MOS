using RealEstate.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RealEstate.API.Controllers
{
    public class ReviewController : ApiController
    {

        #region PropertyPage

     
        /// <summary>
        /// POST : api/Review/AddInappropriate
        /// </summary>
        [HttpPost]                                                                   //final-tested
        public IHttpActionResult AddInappropriate(Inappropriate objInappropriate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (objInappropriate.UserId != null)
            {
                int status = objInappropriate.AddInappropriate(objInappropriate);
                if (status == 4)
                    return BadRequest("Operation Failed");     //return false
                if (status == 0)
                    return Ok("User cannot vote his own review");
                if (status == 1)
                    return Ok("This review is already voted");
                if (status == 2)
                    return Ok("ALready Marked Inappropriate");
                if (status == 3)
                    return Ok("Send for admin review");
            }
            return BadRequest("Sign In Required");
        }

        /// <summary>
        /// POST:api/Review/MarkHelpful
        /// </summary>
        /// <param name="objReviewDetail"></param>
        /// <returns></returns>
        [HttpPost]                                                              //final-tested
        public IHttpActionResult MarkHelpful(ReviewDetail objReviewDetail)      //add level calculation
        {
            if (objReviewDetail.ReviewedUserId != 0)
            {
                if (ModelState.IsValid == false)
                    BadRequest(ModelState);
                int status = objReviewDetail.MarkHelpful(objReviewDetail);
                if (status == 0)
                    return Ok(" user is same as who posted it");
                if (status == 1)
                    return Ok("user has already marked helpful");
                if (status == 2)
                    return Ok("user has already marked Inappropriate");
                if (status == 3)
                    return Ok("successful");
                else 
                    return Ok("unsuccessful");
            }
            else
                return BadRequest("SignIn Required");
        }

        /// <summary>
        /// PUT : api/Review/MarkConvinced?builderCommentId=&&userId=
        /// </summary>
        /// <param name="builderCommentId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPut]                                                                               //final-tested
        public IHttpActionResult MarkConvinced(int builderCommentId,int userId)
        {
            if (builderCommentId != 0 && userId != 0)
            {
                BuilderComment objBuilderComment = new BuilderComment();
                int status = objBuilderComment.MarkConvinced(builderCommentId, userId);
                if (status == 1)
                    return BadRequest("user not authorized to mark convinced");
                if (status == 2)
                    return Ok("marked convinced");
                if (status == 3)
                    return Ok("unable to mark convinced");
                else
                    return Ok("Operation failed");
            }
            return BadRequest("Invalid Request");
        }

        /// <summary>
        /// POST:api/Review/MarkSayThanks
        /// </summary>
        /// <param name="objReviewDetail"></param>
        /// <returns></returns>
        [HttpPost]                                                              //final-tested
        public IHttpActionResult MarkSayThanks(ReviewDetail objReviewDetail)      //add level calculation
        {
            if (objReviewDetail.ReviewedUserId != 0)
            {
                if (ModelState.IsValid == false)
                    BadRequest(ModelState);
                int status = objReviewDetail.MarkSayThanks(objReviewDetail);
                if (status == 0)
                    return Ok(" user is same as who posted it");
                if (status == 1)
                    return Ok("user has already said thanks");
                if (status == 2)
                    return Ok("user has already marked Inappropriate");
                if (status == 3)
                    return Ok("successful");
                else
                    return Ok("unsuccessful");
            }
            else
                return BadRequest("SignIn Required");
        }
      
        #endregion

        #region AddReviewPage

        /// <summary>
        /// POST : api/Review/AddReview
        /// </summary>
        /// <param name="objReview"></param>
        /// <returns></returns>
        [HttpPost]                                                                    //final-tested
        public IHttpActionResult AddReview(Review objReview)
        {
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (objReview.UserId != 0)
            {
                int status = objReview.AddReview(objReview);
                if (status == 1)
                    return Ok("review sent for admin confirmation");
                if (status == 2)
                    return Ok("unable to sent mail for admin confirmation");
                if (status == 3)
                    return Ok("review cannot be addded");
                return BadRequest("Operation Failed");
            }
            return BadRequest("Sign In Required");
        }

        /// <summary>
        /// GET:api/Review/RecentReviews?projectId=
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [ActionName("RecentReviews")]
        [HttpGet]                                                               //final-tested
        public IHttpActionResult RecentReviewsByProjectId(int projectId)
        {
            if (projectId != 0)
            {
                object recentReviews;
                Review objReview = new Review();
                int status = objReview.RecentReviewsByProjectId(projectId, out recentReviews);
                if (status ==1)
                    return Ok(recentReviews);
                if (status == 2)
                    return Ok("no reviews");
                if (status == 3)
                    return Ok("project not present");
                return BadRequest("Operation Failed");
            }
            return BadRequest("Invalid Request");
        }

        ///// <summary>
        ///// GET:api/Review/Month
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]                                                                 //final-tested
        //[ActionName("Month")]
        //public IHttpActionResult GetAllMonth()
        //{
        //    object monthList;
        //        LastVisited objLastVisited = new LastVisited();
        //    bool status = objLastVisited.GetAllMonth(out monthList);
        //        if (status != false)
        //        return Ok(monthList);
        //    return BadRequest("No month found");
        //}

        ///// <summary>
        ///// GET:api/Review/Year
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]                                                                 //final-tested
        //[ActionName("Year")]
        //public IHttpActionResult GetAllYear()
        //{
        //    object yearList;
        //    LastVisited objLastVisited = new LastVisited();
        //    bool status = objLastVisited.GetAllYear(out yearList);
        //    if (status != false)
        //        return Ok(yearList);
        //    return BadRequest("No year found");
        //}


        #endregion

        #region NotInUse

        /// <summary>
        /// POST: api/Review/VoteReview
        /// </summary>
        /// <param name="objReviewDetail"></param>
        /// <returns></returns>
        //[HttpPost]                                                                  //tested
        //public IHttpActionResult VoteReview(ReviewDetail objReviewDetail)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    if (objReviewDetail.ReviewedUserId != null)
        //    {
        //        bool status = objReviewDetail.VoteReview(objReviewDetail);
        //        if (status != true)
        //            return BadRequest("Operation Failed");
        //        return Ok();
        //    }
        //    return BadRequest("Sign In Required");
        //}


        ///// <summary>
        ///// GET : api/Review/GetReviews
        ///// </summary>
        ///// <param name="cityId"></param>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //[HttpGet]                                                                   //tested
        //public IHttpActionResult GetReviews(int cityId, int? userId)
        //{
        //    if (cityId != 0 || userId != 0)
        //    {
        //        User objUser = new User();
        //        Object objReviews = new object();
        //        bool status = objUser.GetReviews(cityId, userId, out objReviews);
        //        if (status != true)
        //            return BadRequest("Invalid Operation");
        //        return Ok(objReviews);
        //    }
        //    return BadRequest("Invalid Request");
        //}

        ///// <summary>
        ///// POST: api/Review/VoteReview
        ///// </summary>
        ///// <param name="objReviewDetail"></param>
        ///// <returns></returns>
        //[HttpPost]                                                                  //tested
        //public IHttpActionResult VoteReview(ReviewDetail objReviewDetail)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    if (objReviewDetail.ReviewedUserId != null)
        //    {
        //        bool status = objReviewDetail.VoteReview(objReviewDetail);
        //        if (status != true)
        //            return BadRequest("Operation Failed");
        //        return Ok();
        //    }
        //    return BadRequest("Sign In Required");
        //}

        #endregion

    }
}

