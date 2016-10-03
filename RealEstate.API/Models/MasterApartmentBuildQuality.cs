using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.API.Repositories;

namespace RealEstate.API.Models
{
    public class MasterApartmentBuildQuality
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool GetAllMasterApartmentBuildQuality(out object MasterApartmentBuildQualityList)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool status = objBuilderRepository.GetAllMasterApartmentBuildQuality(out MasterApartmentBuildQualityList);
            return status;
        }
    }
}