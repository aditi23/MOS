using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.API.Repositories;

namespace RealEstate.API.Models
{
    public class ApartmentBuildQuality
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int MasterApartmentBuildQualityId { get; set; }
        public string Name { get; set; }
       // public string Value { get; set; }
        public bool isIncluded { get; set; }


        public Project Project { get; set; }

        public bool CreateApartmentBuildQuality(Project objProject)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool status = objBuilderRepository.CreateApartmentBuildQuality(objProject);
            return status;
        }

        public bool EditApartmentBuildQuality(Project objProject)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool isEdited = objBuilderRepository.EditApartmentBuildQuality(objProject);
            return isEdited;
        }
    }
}