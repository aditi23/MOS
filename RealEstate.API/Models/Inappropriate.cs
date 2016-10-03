using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using RealEstate.API.Repositories;

namespace RealEstate.API.Models
{
    public class Inappropriate
    {
        public int Id { get; set; }
        [Required]
        public int ReviewId { get; set; }
        public int? UserId { get; set; }
        public string Reason { get; set; }

        public int AddInappropriate(Inappropriate objInappropriate)
        {
            ReviewRepository objReviewRepository = new ReviewRepository();
            int status = objReviewRepository.AddInappropriate(objInappropriate);
            return status;
        }
    }
}