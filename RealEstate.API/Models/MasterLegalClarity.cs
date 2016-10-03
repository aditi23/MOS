using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.API.Repositories;

namespace RealEstate.API.Models
{
    public class MasterLegalClarity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool GetAllMasterLegalClarity(out object MasterLegalClarityList)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool status = objBuilderRepository.GetAllMasterLegalClarity(out MasterLegalClarityList);
            return status;
        }
    }
}