using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.API.Repositories;

namespace RealEstate.API.Models
{
    public class Image
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string ImageUrl { get; set; }
        public int Likes { get; set; }
        public int ImageCategoryId { get; set; }

        public Project Project { get; set; }

        public bool CreateImage(Project objProject)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool status = objBuilderRepository.CreateImage(objProject);
            return status;
        }

        public bool EditImages(Project objProject)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool isEdited = objBuilderRepository.EditImages(objProject);
            return isEdited;
        }
    }
}