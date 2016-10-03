using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using RealEstate.API.Repositories;

namespace RealEstate.API.Models
{
    public class Builder
    {

        #region Properties
        public int Id { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public int CompanyRegistrationNumber { get; set; }
        [Required]
        public int YearOfEstablishment { get; set; }
       
        public int TotalProjectsCount { get; set; }
        [Required]
        public string CompanyEmailId { get; set; }
        [Required]
        public decimal AdminContactNumber { get; set; }
        public bool IsVerified { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string AdminName { get; set; }

        //[Required]
        public List<BuilderDirector> builderDirector { get; set; }
        //[Required]
        public List<BuilderProfile> builderProfile { get; set; }

        public IList<BuilderComment> BuilderComment { get; set; }
        //public BuilderProfile BuilderProfile { get; set; }
        // public IList<BuilderDirector> Bu




        #endregion

        #region functions
        public int RegisterBuilder(Builder objBuilder,out int builderId)    
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            int status = objBuilderRepository.RegisterBuilder(objBuilder ,out builderId);
            return status;
        }

        public bool GetBuilderDetailsByBuilderId(int builderId, out object builderDetails)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool status = objBuilderRepository.GetBuilderDetailsByBuilderId(builderId, out builderDetails);
            return status;
        }

        //public bool Login(Builder objBuilder, out int builderId)
        //{
        //    BuilderRepository objBuilderRepository = new BuilderRepository();
        //    bool status = objBuilderRepository.Login(objBuilder, out builderId);
        //    return status;
        //}


        public int CommentOnReview(BuilderComment objBuilderComment)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            int status = objBuilderRepository.CommentOnReview(objBuilderComment);
            return status;
        }

        public int EditProfile(Builder objBuilder)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            int status = objBuilderRepository.EditProfile(objBuilder);
            return status;
        }

        public bool EditProject(Project objProject)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool status = objBuilderRepository.EditProject(objProject);
            return status;
        }

        public bool GetAllBuilder(out object BuilderList)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool status = objBuilderRepository.GetAllBuilder(out BuilderList);
            return status;
        }
        #endregion
    }
}