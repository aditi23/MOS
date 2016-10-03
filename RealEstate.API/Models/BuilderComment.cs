using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using RealEstate.API.Repositories;

namespace RealEstate.API.Models
{
    public class BuilderComment
    {
        public int Id { get; set; }
        [Required]
        public int ReviewId { get; set; }
       
        public int ProjectBuilderMappingId { get; set; }
        [Required]
        public string Text { get; set; }
        public Nullable<bool> IsConvinced { get; set; }
        public bool IsVerified { get; set; }
        public int BuilderId { get; set; }
        public int ProjectId { get; set; }

        public Builder Builder { get; set; }
        // public Review Review { get; set; }

        public int MarkConvinced(int builderCommentId, int userId)
        {
            ReviewRepository objReviewRepository = new ReviewRepository();
            int status = objReviewRepository.MarkConvinced(builderCommentId, userId);
            return status;
        }

    }
}