using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.API.Repositories;

namespace RealEstate.API.Models
{
    public class Livability
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int MasterLivabilityId { get; set; }
        public string Name { get; set; }
        //public double Value { get; set; }
        public bool isIncluded { get; set; }


        public Project Project { get; set; }

        public bool CreateLivability(Project objProject)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool status = objBuilderRepository.CreateLivability(objProject);
            return status;
        }

        public bool EditLivability(Project objProject)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool isEdited = objBuilderRepository.EditLivability(objProject);
            return isEdited;
        }

    }
}