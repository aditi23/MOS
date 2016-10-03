using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RealEstate.API.Models
{
    public class ReviewRatingDetail
    {
        public int Id { get; set; }
        public int ReviewId { get; set; }
        [Required]
        public int MasterReviewId { get; set; }
        [Required]
        public double Value { get; set; }

    }
}