using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.API.Repositories;

namespace RealEstate.API.Models
{
    public class MasterLivability
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool GetAllMasterLivability(out object MasterLivabilityList)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool status = objBuilderRepository.GetAllMasterLivability(out MasterLivabilityList);
            return status;
        }
    }
}