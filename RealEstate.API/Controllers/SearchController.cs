using RealEstate.API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RealEstate.API.Controllers
{
    public class SearchController : ApiController
    {
        [HttpPost()]
        public string UploadFiles()
        {
            int iUploadedCnt = 0;
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            string name = "user" + r + ".jpg";

            // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
            //string sPath = "http://mosrealestate.silive.in\rest\UserImages/";
           string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/UserImages/");

            System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

            // CHECK THE FILE COUNT.
            for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
            {
                System.Web.HttpPostedFile hpf = hfc[iCnt];

                if (hpf.ContentLength > 0)
                {
                    // CHECK IF THE SELECTED FILE(S) ALREADY EXISTS IN FOLDER. (AVOID DUPLICATE)
                    if (!File.Exists(sPath + name))
                    {
                        // SAVE THE FILES IN THE FOLDER.
                        hpf.SaveAs(Path.Combine(sPath + name));
                        iUploadedCnt = iUploadedCnt + 1;
                    }
                }
                var a = hpf.InputStream;
            }

            // RETURN A MESSAGE (OPTIONAL).
            if (iUploadedCnt > 0)
            {
                return sPath+name;
            }
            else
            {
                return "Upload Failed";
            }
        }

        #region PropertyPage

        /// <summary>
        /// GET:api/Search/ProjectDetails?projectId=&&userId=
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [ActionName("ProjectDetails")]
        [HttpGet]                                                                     //final tested                          
        public IHttpActionResult GetProjectDetailstByProjectId(int projectId, int? userId)
        {
            if (projectId != 0)
            {
                object projectDetails = new object();
                Project objProject = new Project();
                int isFound = objProject.GetProjectDetailstByProjectId(projectId, userId, out projectDetails);
                if (isFound == 1)
                    return Ok("project not found");
                if (isFound == 2)
                    return Ok(projectDetails);
                return BadRequest("Operation Failed");
            }
            else
                return BadRequest("Invalid Request");
        }

        /// <summary>
        /// GET:api/Search/SimilarProjects?projectId=
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [ActionName("SimilarProjects")]
        [HttpGet]                                                                   //final tested
        public IHttpActionResult GetSimilarProjectsByProjectId(int projectId)
        {
            if (projectId != 0)
            {
                object similarProjects;
                Project objProject = new Project();
                int status = objProject.GetSimilarProjectsByProjectId(projectId, out similarProjects);
                if (status == 1)
                    return Ok(similarProjects);
                if (status == 2)
                    return Ok("no similar projects found");
                if (status == 3)
                    return Ok("projectId not found");
                else
                    return BadRequest("unsuccessful");
            }
            return BadRequest("Invalid Request");
        }


        #endregion

        #region ListingPage

        /// <summary>
        /// GET :api/Search/ProjectByCity?cityId=
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [ActionName("ProjectByCity")]
        [HttpGet]                                                                       //final-tested
        public IHttpActionResult GetProjectListByCityId(int cityId)
        {
            if (cityId != 0)
            {
                Project objProject = new Project();
                Object objProjectList = new Object();
                bool status = objProject.GetProjectListByCityId(cityId, out objProjectList);
                if (status != true)
                    return BadRequest("Operation Failed");
                if (objProjectList != null)
                    return Ok(objProjectList);
                return Ok("No Data Found");
            }
            return BadRequest("Invalid Request");
        }

        /// <summary>
        /// GET: api/Search/ProjectByBuilder?builderId=
        /// </summary>
        /// <param name="builderId"></param>
        /// <returns></returns>
        [ActionName("ProjectByBuilder")]
        [HttpGet]                                                                       //final-tested
        public IHttpActionResult GetProjectListByBuilderId(int builderId)
        {
            if (builderId != 0)
            {
                Project objProject = new Project();
                Object objProjectList = new Object();
                bool status = objProject.GetProjectListByBuilderId(builderId, out objProjectList);
                if (status != true)
                    return BadRequest("Operation Failed");
                if (objProjectList != null)
                    return Ok(objProjectList);
                return Ok("No Data Found");
            }
            return BadRequest("Invalid Request");
        }

        /// <summary>
        /// GET:api/Search/ProjectByLocation?locationId=
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        [ActionName("ProjectByLocation")]
        [HttpGet]                                                                       //final-tested
        public IHttpActionResult GetProjectListByLocationId(int locationId)
        {
            if (locationId != 0)
            {
                object projectDetails;
                Project objProject = new Project();
                bool isFound = objProject.GetProjectListByLocationId(locationId, out projectDetails);
                if (isFound != true)
                    return BadRequest("Operation Failed");
                if (projectDetails != null)
                    return Ok(projectDetails);
                return Ok("No Data Found");
            }
            else
                return BadRequest("Invalid Request");
        }


        #endregion

        [HttpGet]
        public void Filter(FilterProjects obj)
        {
            dbRealEstateEntities db = new dbRealEstateEntities();
            //if (obj != null)
            //{
            //    Project objProject = new Project();
            //    Object objProjectList = new Object();
            //    bool status = objProject.GetProjectListByBuilderId(builderId, out objProjectList);
            //    objProject = db.tblProjects.Where(
            //        x => x.tblAmenities.
            //        )
            //    if (status != true)
            //        return BadRequest("Operation Failed");
            //    if (objProjectList != null)
            //        return Ok(objProjectList);
            //    return Ok("No Data Found");

            //}
            //return BadRequest("Invalid Request");
        }

        #region LandingPage

        /// <summary>
        /// GET:api/Search/SiteStats
        /// </summary>
        /// <returns></returns>
        [ActionName("SiteStats")]
        [HttpGet]                                                               //final-tested
        public IHttpActionResult GetSiteSatistics()
        {
           object stats;
            Project objProject = new Project();
            bool status = objProject.GetSiteSatistics(out stats);
            if (status != false)
            {
                if (stats != null)
                    return Ok(stats);
                return Ok("No data");
            }
            return BadRequest("Invalid Request ");
        }


        /// <summary>
        /// GET:api/Search/TrendingReviews
        /// </summary>
        /// <returns></returns>
        [ActionName("TrendingReviews")]
        [HttpGet]                                                               //final-tested
        public IHttpActionResult GetTrendingReviews()
        {
            object trendingReviews;
            Project objProject = new Project();
            bool status = objProject.GetTrendingReviews(out trendingReviews);
            if (status != false)
            {
                if (trendingReviews != null)
                    return Ok(trendingReviews);
                return Ok("No data");
            }
            return BadRequest("Invalid Request ");

        }

        /// <summary>
        /// GET: api/Search/MainSearch?stringToLook=&&cityId=
        /// </summary>
        /// <param name="stringToLook"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [ActionName("MainSearch")]
        [HttpGet]                                                                   //final-tested 
        public IHttpActionResult AutocompleteForMainSearch(string stringToLook, int cityId = 0)
        {
            if (stringToLook == null)
            {
                return Ok();
            }
            if ((!stringToLook.Equals(null) && stringToLook.Length >= 3) || (cityId != 0))
            {
                object listResult;
                Project objProject = new Project();
                bool status = objProject.AutocompleteForMainSearch(stringToLook, cityId, out listResult);
                if (status != false)
                {
                    if (listResult != null)
                        return Ok(listResult);
                    else
                        return Ok("No data found.");
                }
                else
                    return BadRequest("Operation Failed");
            }
            return BadRequest("Invalid request");
        }

        /// <summary>
        /// GET: api/Search/FinalAutoComplete
        /// </summary>
        /// <returns></returns>
        [HttpGet]                                                                   
        public IHttpActionResult FinalAutoComplete()
        {
                object listResult;
                Project objProject = new Project();
                bool status = objProject.FinalAutoComplete(out listResult);
                if (status != false)
                {
                    if (listResult != null)
                        return Ok(listResult);
                    else
                        return Ok("No data found.");
                }
                else
                    return BadRequest("Operation Failed");
         }

        /// <summary>
        /// GET: api/Search/SearchByCity?cityId=
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [ActionName("SearchByCity")]
        [HttpGet]
        public IHttpActionResult FinalAutoCompleteByCityId(int cityId)
        {
            object listResult;
            Project objProject = new Project();
            bool status = objProject.FinalAutoCompleteByCityId(cityId,out listResult);
            if (status != false)
            {
                if (listResult != null)
                    return Ok(listResult);
                else
                    return Ok("No data found.");
            }
            else
                return BadRequest("Operation Failed");
        }


        /// <summary>
        /// GET:api/Search/GetAllCities
        /// </summary>
        /// <returns></returns>
        [HttpGet]                                                                 //final-tested
        public IHttpActionResult GetAllCities()
        {
            object cityList;
            City objCity = new City();
            bool status = objCity.GetAllCities(out cityList);
            if (status != false)
                return Ok(cityList);
            return BadRequest("No city found");
        }

        /// <summary>
        /// GET:api/Search/GetAllProjects
        /// </summary>
        /// <returns></returns>
        [HttpGet]                                                                 //final-tested
        public IHttpActionResult GetAllProjects()
        {
            object projectList;
            Project objProject = new Project();
            bool status = objProject.GetAllProjects(out projectList);
            if (status != false)
                return Ok(projectList);
            return BadRequest("No project found");
        }


        #endregion

        #region NotInUse

        ///// <summary>
        ///// GET : api/Search/Reviews?propId=
        ///// </summary>
        ///// <returns></returns>
        //[ActionName("Reviews")]
        //[HttpGet]                                                                       //tested
        //public IHttpActionResult GetReviewsByPropertyId(int propId)
        //{
        //    if (propId != 0)
        //    {
        //        Review objReview = new Review();
        //        Object obj = new Object();
        //        bool status = objReview.GetReviewsByPropertyId(propId, out obj);
        //        if (status != true)
        //            return BadRequest("Operation Failed");
        //        return Ok(obj);
        //    }
        //    return BadRequest("Invalid Request");
        //}

        ///// <summary>
        ///// GET:api/Search/ReviewSearch
        ///// </summary>
        ///// <param name="stringToLook"></param>
        ///// <returns></returns>
        //[ActionName("ReviewSearch")]
        //[HttpGet]
        //public IHttpActionResult AutocompleteForReviewSearch(string stringToLook)         //tested
        //{
        //    if (stringToLook != null)
        //    {
        //        object listResult;
        //        Project objProject = new Project();
        //        bool status = objProject.AutocompleteForReviewSearch(stringToLook, out listResult);
        //        if (status != false)
        //            return Ok(listResult);
        //        else
        //            return BadRequest("Operation Failed");
        //    }
        //    return BadRequest("Invalid request");
        //}


        #endregion

    }
}
