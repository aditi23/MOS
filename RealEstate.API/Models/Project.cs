using RealEstate.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace RealEstate.API.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Possession { get; set; }
        public string Pricing { get; set; }
        public double AverageRating { get; set; }
        public DateTime Date { get; set; }
        public HttpPostedFileBase ProfilePicture { get; set; }
        public List<HttpPostedFileBase> ProjectImagesDetails { get; set; }


        public ProjectBuilderMapping ProjectBuilderMapping { get; set; }
        public BuilderProfile BuilderProfile { get; set; }
        public List<Review> Review { get; set; }
        public List<Amenity> Amenities { get; set; }
        public List<ApartmentBuildQuality> ApartmentBuildQualities { get; set; }
        public List<ConstructionQualityParameter> ConstructionQualityParameters { get; set; }
        public List<Image> Images { get; set; }
        public List<Inventory> Inventories { get; set; }
        public List<LegalClarity> LegalClarities { get; set; }
        public List<Livability> Livabilities { get; set; }
        public Location Locations { get; set; }
        public City Cities { get; set; }
        
        public List<ProjectInformation> ProjectInformations { get; set; }
        public List<ChooseImageToBeDeleted> ImageToBeDeleted { get; set; }


        public bool EditProject(Project objProject)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool isEdited = objBuilderRepository.EditProject(objProject);
            return isEdited;
        }

        public bool GetAllProjects(out object projectList)
        {
            SearchRepository objSearchRepository = new SearchRepository();
            bool status = objSearchRepository.GetAllProjects(out projectList);
            return status;
        }


        //public bool GetProjectByPostId(int postId, out object projectDetails)
        //{
        //    object result;
        //    DiscussionForumRepository objDiscussionForumRepository = new DiscussionForumRepository();
        //    bool status = objDiscussionForumRepository.GetProjectByPostId(postId, out result);
        //    if (status != false)
        //    {
        //        projectDetails = result;
        //        return true;
        //    }
        //    else
        //    {
        //        projectDetails = null;
        //        return false;
        //    }
        //}

        public bool CreateProject(Project objProject, out int projectId)
        {
            BuilderRepository objBuilderRepository = new BuilderRepository();
            bool status = objBuilderRepository.CreateProject(objProject, out projectId);
            return status;
        }

        #region SearchRelated
        public int GetProjectDetailstByProjectId(int projectId,int? userId, out object projectDetails)
        {
            SearchRepository objReviewRepository = new SearchRepository();
            int status = objReviewRepository.GetProjectDetailstByProjectId(projectId , userId, out projectDetails);
            return status;
        }

        public bool GetProjectListByCityId(int cityId, out object objProjectList)
        {
            SearchRepository objSearchRepository = new SearchRepository();
            bool status = objSearchRepository.GetProjectListByCityId(cityId,out objProjectList);
            return status;
        }

        public int GetSimilarProjectsByProjectId(int projectId, out object similarProjects)
        {
            SearchRepository objSearchRepository = new SearchRepository();
            int status = objSearchRepository.GetSimilarProjectsByProjectId(projectId, out similarProjects);
            return status;
        }

        public bool GetProjectListByBuilderId(int builderId, out object objProjectList)
        {
            SearchRepository objSearchRepository = new SearchRepository();
            bool status = objSearchRepository.GetProjectListByBuilderId(builderId, out objProjectList);
            return status;
        }


        public bool GetProjectListByLocationId(int locationId, out object projectDetails)
        {
            SearchRepository objSearchRepository = new SearchRepository();
            bool status = objSearchRepository.GetProjectListByLocationId(locationId, out projectDetails);
            return status;
        }

        public bool AutocompleteForMainSearch(string stringToLook,int cityId, out object listResult)
        {
            SearchRepository objSearchRepository = new SearchRepository();
            bool status = objSearchRepository.AutocompleteForMainSearch(stringToLook,cityId, out listResult);
            return status;
        }

        public bool FinalAutoComplete(out object finalList)
        {
            SearchRepository objSearchRepository = new SearchRepository();
            bool status = objSearchRepository.FinalAutoComplete(out finalList);
            return status;
        }

        public bool FinalAutoCompleteByCityId(int cityId,out object finalList)
        {
            SearchRepository objSearchRepository = new SearchRepository();
            bool status = objSearchRepository.FinalAutoCompleteByCityId(cityId, out finalList);
            return status;
        }

        public bool AutocompleteForReviewSearch(string stringToLook, out object listResult)
        {
            SearchRepository objSearchRepository = new SearchRepository();
            bool status = objSearchRepository.AutocompleteForReviewSearch(stringToLook, out listResult);
            return status;
        }

        public bool GetTrendingReviews(out object trendingReviews)
        {
            SearchRepository objSearchRepository = new SearchRepository();
            bool status = objSearchRepository.GetTrendingReviews(out trendingReviews);
            return status;
        }


             public bool GetSiteSatistics(out object stats)
        {
            SearchRepository objSearchRepository = new SearchRepository();
            bool status = objSearchRepository.GetSiteSatistics(out stats);
            return status;
        }

        #endregion
    }
}