using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.API.Repositories;

namespace RealEstate.API.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int MasterInventoryId { get; set; }
        public double Value { get; set; }
        public bool isIncluded { get; set; }


        public Project Project { get; set; }

        public bool CreateInventory(Project objProject)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool status = objBuilderRepository.CreateInventory(objProject);
            return status;
        }

        public bool EditInventory(Project objProject)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool isEdited = objBuilderRepository.EditInventory(objProject);
            return isEdited;
        }
    }
}