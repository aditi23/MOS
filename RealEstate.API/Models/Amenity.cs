using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.API.Repositories;

namespace RealEstate.API.Models
{
    public class Amenity
    {

        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int MasterAmenityId { get; set; }
        public string Value { get; set; }
        public bool isIncluded { get; set; }


        public Project Project { get; set; }

        public bool CreateAmenity(Project objProject)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool status = objBuilderRepository.CreateAmenity(objProject);
            return status;
        }

        public bool EditAmenity(Project objProject)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool isEdited = objBuilderRepository.EditAmenity(objProject);
            return isEdited;
        }
    }
}