using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.API.Repositories;

namespace RealEstate.API.Models
{
    public class MasterInventory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool GetAllMasterInventory(out object MasterInventoryList)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool status = objBuilderRepository.GetAllMasterInventory(out MasterInventoryList);
            return status;
        }
    }
}