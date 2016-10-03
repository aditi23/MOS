using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RealEstate.API.Models;
using System.IO;

namespace RealEstate.API.Controllers
{
    public class BuilderController : ApiController
    {
        #region LandingPage

        /// <summary>
        /// POST: api/Builder/Register
        /// </summary>
        /// <param name="objBuilder"></param>
        /// <returns></returns>
        [ActionName("Register")]
        [HttpPost]                                                           //final-tested
        public IHttpActionResult RegisterBuilder(Builder objBuilder)
        {
            if (objBuilder.CompanyEmailId != null)
            {
                if (ModelState.IsValid == false)
                    return BadRequest(ModelState);
                int builderId;
                int status = objBuilder.RegisterBuilder(objBuilder, out builderId);
                if (status == 1)
                    return Ok(builderId);
                if (status == 2)
                    return Ok("Builder details not added");
                if (status == 3)
                    return Ok("emailId already exist");
                else
                    return BadRequest("Operation failed");
            }
            else
                return BadRequest("Invalid Request");
        }

        /// <summary>
        /// POST : api/Builder/Login
        /// </summary>
        /// <param name="emailId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]                                                                        //final-tested
        public IHttpActionResult Login(Builder objBuilder)
        {
            if (objBuilder.CompanyEmailId != null && objBuilder.Password != null)
            {
                int builderId = 0;
                bool status = true;//objBuilder.Login(objBuilder, out builderId);
                if (status != true)
                    return Ok("Invalid Username or Password");
                return Ok(builderId);
            }
            return BadRequest("Invalid Request");
        }


        #endregion

        #region BuilderProfile

        /// <summary>
        /// GET:api/Builder/BuilderDetails?builderId=
        /// </summary>
        /// <param name="builderId"></param>
        /// <returns></returns>
        [ActionName("BuilderDetails")]
        [HttpGet]                                                               //final-tested
        public IHttpActionResult GetBuilderDetailsByBuilderId(int builderId)
        {
            if (builderId != 0)
            {
                object builderDetails;
                Builder objBuilder = new Builder();
                bool status = objBuilder.GetBuilderDetailsByBuilderId(builderId, out builderDetails);
                if (status != false)
                {
                    if (builderDetails != null)
                        return Ok(builderDetails);
                    return Ok("No data found");
                }
                return BadRequest("Operation Failed");
            }
            return BadRequest("Invalid Request");
        }

        /// <summary>
        /// POST:api/Builder/AddComment
        /// </summary>
        /// <param name="objBuilderComment"></param>
        /// <returns></returns>
        [ActionName("AddComment")]
        [HttpPost]                                                                  //final-tested
        public IHttpActionResult CommentOnReview(BuilderComment objBuilderComment)
        {
            if (objBuilderComment.ProjectId != 0 && objBuilderComment.BuilderId != 0 && objBuilderComment.ReviewId != 0)
            {
                Builder objBuilder = new Builder();
                int status = objBuilder.CommentOnReview(objBuilderComment);

                if (status == 1)
                    return Ok("builder has already commented once");
                if (status == 2)
                    return Ok("comment sent for admin confirmation");
                if (status == 3)
                    return Ok("unable to sent for admin confirmation");
                if (status == 4)
                    return Ok("comment cannot be added");
                return BadRequest("Operation Failed");
            }
            else
                return BadRequest("Invalid Request");
        }

        /// <summary>
        /// PUT:api/Builder/EditProfile
        /// </summary>
        /// <param name="objBuilder"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult EditProfile(Builder objBuilder)
        {
            if (objBuilder.Id != 0)
            {
                int status = objBuilder.EditProfile(objBuilder);
                if (status == 1)
                    return Ok("edited successfully");
                if (status == 2)
                    return Ok("profile cannot be edited");
                if (status == 3)
                    return Ok("builder not found");
                return BadRequest("operation failed");
            }
            return BadRequest("Invalid Request");
        }

        /// <summary>
        /// PUT:api/Builder/EditProject
        /// </summary>
        /// <param name="objProject"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult EditProject(Project objProject)
        {
            #region Object for all classes declared.

            Amenity objAmenity = new Amenity();
            ApartmentBuildQuality objApartmentBuildQuality = new ApartmentBuildQuality();
            ConstructionQualityParameter objConstructionQualityParameter = new ConstructionQualityParameter();
            Image objImages = new Image();
            Inventory objInventory = new Inventory();
            LegalClarity objLegalClarity = new LegalClarity();
            Livability objLivability = new Livability();
            Location objLocation = new Location();
            ProjectInformation objProjectInformation = new ProjectInformation();
            #endregion

            bool amenityIsEdited = objAmenity.EditAmenity(objProject);
            bool apartmentBuildQualityIsEdited = objApartmentBuildQuality.EditApartmentBuildQuality(objProject);
            bool constructionQualityParameterIsEdited = objConstructionQualityParameter.EditConstructionQualityParameter(objProject);
            bool imagesIsEdited = objImages.EditImages(objProject);
            bool inventoryIsEdited = objInventory.EditInventory(objProject);
            bool legalClarityIsEdited = objLegalClarity.EditLegalClarity(objProject);
            bool livabilityIsEdited = objLivability.EditLivability(objProject);
            bool locationIsEdited = objLocation.EditLocation(objProject);
            bool projectInformation = objProjectInformation.EditProjectInformation(objProject);
            bool projectIsEdited = objProject.EditProject(objProject);

            if (amenityIsEdited != false || apartmentBuildQualityIsEdited != false || constructionQualityParameterIsEdited != false ||
                imagesIsEdited != false || inventoryIsEdited != false || legalClarityIsEdited != false || livabilityIsEdited != false ||
                locationIsEdited != false || projectInformation != false || projectIsEdited != false)
                return Ok("Edited successfully");
            return BadRequest("Operation failed");
        }

        /// <summary>
        /// GET: api/Builder/GetAllMasterDetails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAllMasterDetails()
        {

            #region Object for all classes declared

            MasterAmenity objMasterAmenity = new MasterAmenity();
            MasterApartmentBuildQuality objMasterApartmentBuildQuality = new MasterApartmentBuildQuality();
            MasterConstructionQualityParameter objMasterConstructionQualityParameter = new MasterConstructionQualityParameter();
            MasterInventory objMasterInventory = new MasterInventory();
            Builder objBuilder = new Builder();
            MasterLegalClarity objMasterLegalClarity = new MasterLegalClarity();
            MasterLivability objMasterLivability = new MasterLivability();
            City objCity = new City();

            #endregion

            object MasterAmenityList;
            object MasterApartmentBuildQualityList;
            object MasterConstructionQualityParameterList;
            object MasterInventoryList;
            object MasterLegalClarityList;
            object MasterLivabilityList;
            object CityList;
            object BuilderList;

            bool status1 = objMasterAmenity.GetAllMasterAmenity(out MasterAmenityList);
            bool status2 = objMasterApartmentBuildQuality.GetAllMasterApartmentBuildQuality(out MasterApartmentBuildQualityList);
            bool status3 = objMasterConstructionQualityParameter.GetAllMasterConstructionQualityParameter(out MasterConstructionQualityParameterList);
            bool status4 = objMasterInventory.GetAllMasterInventory(out MasterInventoryList);
            bool status5 = objMasterLegalClarity.GetAllMasterLegalClarity(out MasterLegalClarityList);
            bool status6 = objMasterLivability.GetAllMasterLivability(out MasterLivabilityList);
            bool status7 = objCity.GetAllCities(out CityList);
            bool status8 = objBuilder.GetAllBuilder(out BuilderList);


            List<object> MasterTableList = new List<object>();
            MasterTableList.Add(MasterAmenityList);
            MasterTableList.Add(MasterApartmentBuildQualityList);
            MasterTableList.Add(MasterConstructionQualityParameterList);
            MasterTableList.Add(MasterInventoryList);
            MasterTableList.Add(MasterLegalClarityList);
            MasterTableList.Add(MasterLivabilityList);
            MasterTableList.Add(CityList);

            if (MasterTableList.Count() != 0)
                return Ok(MasterTableList);
            else
                return BadRequest("Operation Failed");

        }

        /// <summary>
        /// POST: api/Builder/AddNewProject
        /// </summary>
        /// <param name="objProject"></param>
        /// <param name="builderId"></param>
        /// <param name="beneficiaryName"></param>
        /// <returns></returns>
        [ActionName("AddNewProject")]
        [HttpPost]
        public IHttpActionResult AddProjectByBuilder(Project objProject)
        {
            int iUploadedCnt = 0;
            string sPath = "";
            sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Views/");
            System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
            {
                System.Web.HttpPostedFile hpf = hfc[iCnt];
                if (hpf.ContentLength > 0)
                {
                    if (!File.Exists(sPath + Path.GetFileName(hpf.FileName)))
                    {
                        hpf.SaveAs(sPath + Path.GetFileName(hpf.FileName));
                        iUploadedCnt = iUploadedCnt + 1;
                    }
                }
            }
            #region Object for all classes declared.

            Amenity objAmenity = new Amenity();
            ApartmentBuildQuality objApartmentBuildQuality = new ApartmentBuildQuality();
            Builder objBuilder = new Builder();
            ConstructionQualityParameter objConstructionQualityParameter = new ConstructionQualityParameter();
            Image objImages = new Image();
            Inventory objInventory = new Inventory();
            LegalClarity objLegalClarity = new LegalClarity();
            Livability objLivability = new Livability();
            Location objLocation = new Location();
            ProjectInformation objProjectInformation = new ProjectInformation();
            ProjectBuilderMapping objProjectBuilderMapping = new ProjectBuilderMapping();
            City objCity = new City();
            #endregion

            if (objProject.Name != null)
            {
                int projectId;
                bool projectIsFilled = objProject.CreateProject(objProject, out projectId);
                // objProject.Id = projectId;
                if (projectId != 0)
                {
                    bool amenityIsFilled = objAmenity.CreateAmenity(objProject);
                    bool apartmentBuildQualityIsFilled = objApartmentBuildQuality.CreateApartmentBuildQuality(objProject);
                    bool constructionQualityParameterIsFilled = objConstructionQualityParameter.CreateConstructionQualityParameter(objProject);
                    bool imagesIsFilled = objImages.CreateImage(objProject);
                    bool inventoryIsFilled = objInventory.CreateInventory(objProject);
                    bool legalClarityIsFilled = objLegalClarity.CreateLegalClarity(objProject);
                    bool livabilityIsFilled = objLivability.CreateLivability(objProject);
                    bool locationIsFilled = objLocation.CreateLocation(objProject);
                    bool projectInformationIsFilled = objProjectInformation.CreateProjectInformation(objProject);
                    //objProjectBuilderMapping.BuilderId = 1;
                    objProjectBuilderMapping.ProjectId = projectId;
                    //objProjectBuilderMapping.BeneficiaryName = "ghghjg";
                    bool projectBuilderMappingCreated = objProjectBuilderMapping.CreateProjectBuilderMapping(objProject);
                    return Ok(projectId);
                }
                return BadRequest("Operation Failed.");
            }
            return BadRequest("Invalid Request");
        }

        /// <summary>
        /// GET:api/Builder/ImageCategory
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ImageCategory")]
        public IHttpActionResult GetAllImageCategory()
        {
            object imageCategoryList;
            ImageCategory objImageCategory = new ImageCategory();
            bool status = objImageCategory.GetAllImageCategory(out imageCategoryList);
            if (status != false)
                return Ok(imageCategoryList);
            return BadRequest("No image categories found");
        }

        #endregion

        #region NotInUse

        ///// <summary>
        ///// POST : api/Builder/Login
        ///// </summary>
        ///// <param name="emailId"></param>
        ///// <param name="password"></param>
        ///// <returns></returns>
        //[HttpPost]                                                                        //final-tested
        //public IHttpActionResult Login(Builder objBuilder)
        //{
        //    if (objBuilder.EmailId != null && objBuilder.Password != null)
        //    {
        //        int builderId = 0;
        //        bool status = objBuilder.Login(objBuilder, out builderId);
        //        if (status != true)
        //            return Ok("Invalid Username or Password");
        //        return Ok(builderId);
        //    }
        //    return BadRequest("Invalid Request");
        //}


        #endregion
    }
}

