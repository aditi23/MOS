using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.API.Repositories;

namespace RealEstate.API.Models
{
    public class LegalClarity
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int MasterLegalClarityId { get; set; }
        public string Value { get; set; }
        public bool isIncluded { get; set; }


        public Project Project { get; set; }

        public bool CreateLegalClarity(Project objProject)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool status = objBuilderRepository.CreateLegalClarity(objProject);
            return status;
        }

        public bool EditLegalClarity(Project objProject)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool isEdited = objBuilderRepository.EditLegalClarity(objProject);
            return isEdited;
        }


    }
}