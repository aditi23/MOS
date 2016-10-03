using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.API.Repositories;

namespace RealEstate.API.Models
{
    public class ConstructionQualityParameter
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int MasterConstructionQualityParameterId { get; set; }
        public string Value { get; set; }
        public bool isIncluded { get; set; }


        public Project Project { get; set; }

        public bool CreateConstructionQualityParameter(Project objProject)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool status = objBuilderRepository.CreateConstructionQualityParameter(objProject);
            return status;
        }

        public bool EditConstructionQualityParameter(Project objProject)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool isEdited = objBuilderRepository.EditConstructionQualityParameter(objProject);
            return isEdited;
        }
    }
}