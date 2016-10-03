using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.API.Repositories;

namespace RealEstate.API.Models
{
    public class ProjectBuilderMapping
    {
        public int Id { get; set; }
        public int BuilderId { get; set; }
        public int ProjectId { get; set; }
        public string BeneficiaryName { get; set; }

        public Project Project { get; set; }

        public bool CreateProjectBuilderMapping(Project objProject)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool status = objBuilderRepository.CreateProjectBuilderMapping(objProject);
            return status;
        }
    }
}