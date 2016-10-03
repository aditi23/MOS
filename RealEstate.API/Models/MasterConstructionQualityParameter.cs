using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.API.Repositories;

namespace RealEstate.API.Models
{
    public class MasterConstructionQualityParameter
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool GetAllMasterConstructionQualityParameter(out object MasterConstructionQualityParameterList)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool status = objBuilderRepository.GetAllMasterConstructionQualityParameter(out MasterConstructionQualityParameterList);
            return status;
        }
    }
}