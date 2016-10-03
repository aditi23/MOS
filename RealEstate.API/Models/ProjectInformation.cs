using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.API.Repositories;

namespace RealEstate.API.Models
{
    public class ProjectInformation
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int MasterProjectInformationId { get; set; }
        public string Value { get; set; }
        public bool isIncluded { get; set; }


        public Project Project { get; set; }

        public bool CreateProjectInformation(Project objProject)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool status = objBuilderRepository.CreateProjectInformation(objProject);
            return status;
        }

        public bool EditProjectInformation(Project objProject)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool isEdited = objBuilderRepository.EditProjectInformation(objProject);
            return isEdited;
        }
    }
}