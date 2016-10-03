using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using RealEstate.API.Repositories;

namespace RealEstate.API.Models
{
    public class Review
    {

        public int Id { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public int UserId { get; set; }
        public double AverageValue { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public string Heading { get; set; }
        public int Helpful { get; set; }
        public System.DateTime Time { get; set; }
        public bool IsReviewed { get; set; }
        public int SayThanks { get; set; }
        [Required]
        public DateTime lastVisited { get; set; }
       
       // public int LastVisitedId { get; set; }

        public Project Project { get; set; }
        public User User { get; set; }
        //public Year Year { get; set; }
        public List<ReviewRatingDetail> reviewRatingDetails { get; set; }

        //public BuilderComment BuilderComment { get; set; }

        public int AddReview(Review objReview)
        {
            ReviewRepository objReviewRepository = new ReviewRepository();
            int status = objReviewRepository.AddReview(objReview);
            return status;
        }

        public int RecentReviewsByProjectId(int projectId, out object recentReviews)
        {
            ReviewRepository objReviewRepository = new ReviewRepository();
            int status = objReviewRepository.RecentReviewsByProjectId(projectId, out recentReviews);
            return status;
        }

        #region SearchRelated
        public bool GetReviewsByPropertyId(int propId, out object obj)
        {
            SearchRepository objSearchRepository = new SearchRepository();
            bool status = objSearchRepository.GetReviewsByPropertyId(propId, out obj);
            return status;
        }
        #endregion
    }
}