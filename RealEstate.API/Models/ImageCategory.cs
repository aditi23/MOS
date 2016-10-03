using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.API.Repositories;

namespace RealEstate.API.Models
{
    public class ImageCategory
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        //public int ImageCategoryId { get; set; }

        public bool GetAllImageCategory(out object imageCategoryList)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool status = objBuilderRepository.GetAllImageCategory(out imageCategoryList);
            return status;
        }
    }
}