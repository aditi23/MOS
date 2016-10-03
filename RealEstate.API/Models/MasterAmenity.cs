using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.API.Repositories;

namespace RealEstate.API.Models
{
    public class MasterAmenity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool GetAllMasterAmenity(out object MasterAmenityList)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool status = objBuilderRepository.GetAllMasterAmenity(out MasterAmenityList);
            return status;
        }
    }
}