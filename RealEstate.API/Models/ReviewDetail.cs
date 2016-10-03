using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.API.Repositories;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.API.Models
{
    public class ReviewDetail
    {
        public int Id { get; set; }
        [Required]
        public int ReviewId { get; set; }
        [Required]
        public int ReviewedUserId { get; set; }
        public bool Helpful { get; set; }
        public bool SayThanks { get; set; }


        //public bool VoteReview(ReviewDetail objReviewDetail)
        //{
        //    ReviewRepository objReviewRepository = new ReviewRepository();
        //    bool status = objReviewRepository.VoteReview(objReviewDetail);
        //    return status;
        //}

        public int MarkHelpful(ReviewDetail objReviewDetail)
        {
            ReviewRepository objReviewRepository = new ReviewRepository();
                int status = objReviewRepository.MarkHelpful(objReviewDetail);
                return status;
        }

        public int MarkSayThanks(ReviewDetail objReviewDetail)
        {
            ReviewRepository objReviewRepository = new ReviewRepository();
            int status = objReviewRepository.MarkSayThanks(objReviewDetail);
            return status;
        }
    }
}