using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RealEstate.API.Models;
using System.Web.Http.Cors;
using System.IO;
using System.Data;
using System.Drawing;

namespace RealEstate.API.Controllers
{
     public class MemoryPostedFile : System.Web.HttpPostedFileBase
    {
        private byte[] fileBytes;
        private Stream inputStream;

        public MemoryPostedFile(byte[] fileBytes, string fileName = null)
        {
            this.fileBytes = fileBytes;
            this.FileName = fileName;
            this.InputStream = new MemoryStream(fileBytes);
        }

        public override int ContentLength => fileBytes.Length;

        public override string FileName { get; }

        public override Stream InputStream { get; }
    }
    public class UserController : ApiController
    {
        #region UserProfile

        /// <summary>
        /// POST:api/User/AddRecentlyViewed
        /// </summary>
        /// <param name="objRecentlyViewed"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddRecentlyViewed(RecentlyViewed objRecentlyViewed)
        {
            if(objRecentlyViewed.ProjectId !=0 && objRecentlyViewed.UserId!=0)
            {
                User objUser = new User();
                bool status = objUser.AddRecentlyViewed(objRecentlyViewed);
                if (status == true)
                    return Ok("Recently viewed added.");
                return Ok("Operation Failed");

            }
            return BadRequest("Invalid Request");
        }

        /// <summary>
        /// GET:api/User/UserProfile?userId=
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [ActionName("UserProfile")]
        [HttpGet]                                                                    //final-tested
        public IHttpActionResult GetUserDetailsByUserId(int userId)
        {
            if (userId != 0)
            {
                object userDetails;
                User objUser = new User();
                bool isFound = objUser.GetUserDetailsByUserId(userId, out userDetails);
                if (isFound != true)
                    return BadRequest("Operation Failed");
                if (userDetails != null)
                    return Ok(userDetails);
                return Ok("Data Not Found");
            }
            else
                return BadRequest("Invalid Request");
        }

        /// <summary>
        /// PUT : api/User/EditProfile
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns></returns>
        [ActionName("EditProfile")]
        [HttpPut]                                                                       //final-tested
        public IHttpActionResult EditProfileByUserId(User objUser)
        {
            if (objUser.Id != 0)
            {
                int status = objUser.EditProfileByUserId(objUser);
                if (status == 1)
                    return Ok("profile edited");
                if (status == 2)
                    return Ok("user not found");
                return BadRequest("operation failed");
            }
            return BadRequest("Invalid Request");
        }

        /// <summary>
        /// PUT:api/User/ForgotPassword?userEmail=
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        [ActionName("ForgotPassword")]
        [HttpPut]                                                                     //final-tested
        public IHttpActionResult UserForgotPassword(string userEmail)
        {
            if (!userEmail.Equals(null))
            {
                User objUser = new User();
                int status = objUser.UserForgotPassword(userEmail);
                if (status == 1)
                    return Ok("Check your mail for new password");
                if (status == 2)
                    return Ok("operation failed");
                if (status == 3)
                    return Ok("emailId not registered");
                else
                    return BadRequest("operation failed");
            }
            return BadRequest("invalid request");
        }

        /// <summary>
        /// PUT:api/User/ResetPassword?userId=&&oldPassword=&&newPassword=
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [ActionName("ResetPassword")]
        [HttpPost]                                                                              //final-tested
        public IHttpActionResult UserResetPassword(int userId, string oldPassword, string newPassword)
        {
            if (userId != 0 && !oldPassword.Equals(null) && !newPassword.Equals(null))
            {
                User objUser = new User();
                int status = objUser.UserResetPassword(userId, oldPassword, newPassword);
                if (status == 1)
                    return Ok("password reset successfully");
                if (status == 2)
                    return Ok("invalid credentials");
                else BadRequest("operation failed");
            }
            return BadRequest("Invalid Request");
        }

        /// <summary>
        /// GET:api/User/GetAllUserType
        /// </summary>
        /// <returns></returns>
        [HttpGet]                                                                 //final-tested
        public IHttpActionResult GetAllUserType()
        {
            object userTypeList;
            UserType objUserType = new UserType();
            bool status = objUserType.GetAllUserType(out userTypeList);
            if (status != false)
                return Ok(userTypeList);
            return BadRequest("No user type found");
        }

        #endregion


        #region PropertyPage

        /// <summary>
        /// POST:api/User/FollowProject
        /// </summary>
        /// <param name="objFollowProject"></param>
        /// <returns></returns>
        [HttpPost]                                                                         //final-tested
        public IHttpActionResult FollowProject(FollowProject objFollowProject)
        {
            if (objFollowProject.ProjectId != 0 && objFollowProject.UserId != 0)
            {
                int status = objFollowProject.FollowProperty(objFollowProject);
                if (status == 1)
                    return Ok("followed project mail sent");
                if (status == 2)
                    return Ok("followed project mail not sent");
                if (status == 3)
                    return Ok("unable to follow");
                return BadRequest("Operation Failed");
            }
            return
                BadRequest("Invalid Request");
        }

        /// <summary>
        /// POST : api/User/Query
        /// </summary>
        /// <param name="objQueryUs"></param>
        /// <returns></returns>
        [ActionName("Query")]
        [HttpPut]                                                                        //final-tested
        public IHttpActionResult SendQuery(QueryUs objQueryUs)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Request");
            bool status = objQueryUs.SendQuery(objQueryUs);
            if (status != true)
                return BadRequest("Operation Failed");
            return Ok("submitted successfully");
        }

        /// <summary>
        /// PUT: api/User/UnfollowProject
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpPut]                                                                           //final-tested
        public IHttpActionResult UnfollowProject(FollowProject objFollowProject)
        {
            if (objFollowProject.UserId != 0 && objFollowProject.ProjectId != 0)
            {
                bool status = objFollowProject.UnFollowProject(objFollowProject);
                if (status != true)
                    return BadRequest("Operation Failed");
                return Ok();
            }
            return BadRequest("Invalid Request");
        }

        /// <summary>
        /// PUT: api/User/UnsubscribeMail?userId=
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [ActionName("UnsubscribeMail")]
        [HttpPut]                                                                           //final-tested
        public IHttpActionResult UnfollowAll(int userId)
        {
            if (userId != 0)
            {
                FollowProject objFollowProject = new FollowProject();
                bool status = objFollowProject.UnfollowAll(userId);
                if (status != false)
                    return Ok("Successfully Unsubscribed from updates");
                return Ok("Operation failed.");
            }
            return BadRequest("Invalid Request");
        }

        #endregion


        #region LandingPage

        /// <summary>
        /// POST : api/User/RegisterUser
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns></returns>
        [HttpPost]                                                                         //final-tested
        public IHttpActionResult RegisterUser(User objUser)
            {
            if (objUser.Password != null)
            {

                //int iUploadedCnt = 0;


                string sPath = "";
                sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Views/");
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Views/") + objUser.FirstName + ".jpg";



                //if (!ModelState.IsValid)
                //    return BadRequest(ModelState);

                List<object> Person = new List<object>();
                int status = objUser.RegisterUser(objUser, out Person);
                if (status == 1)
                    return Ok(Person);
                if (status == 2)
                    return Ok("reg unsucccessful");
                if (status == 3)
                    return Ok("EMAIL ID already exist");
                if (status == 4)
                    return Ok(Person);
                return Ok("operation failed");
            }
            return BadRequest("Invalid Request");
        }

        public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
                using (var ms = new MemoryStream(byteArrayIn))
                {
                    return System.Drawing.Image.FromStream(ms);
                }
        }


        /// <summary>
        /// POST : api/User/Login
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns></returns>
        [HttpPost]                                                                        //final-tested
        public IHttpActionResult Login(User objUser)
        {
            if (objUser.EmailId != null && objUser.Password != null)
            {
                List<object> Person = new List<object>();
                bool status = objUser.Login(objUser, out Person);
                if (status != true)
                    return Ok("Invalid Username or Password");
                return Ok(Person);
            }
            return BadRequest("Invalid Request");
        }

        #endregion


        #region NotInUse

        ///// <summary>
        ///// GET :api/User/GetUserFirstName
        ///// </summary>
        ///// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetUserFirstName(int userId)
        {

            string userName;
            User objUser = new User();
            bool isFound = objUser.GetUserFirstName(userId, out userName);
            if (isFound != false)
                return Ok(userName);
            return Ok("Operation Failed");

        }

        #endregion

    }
}
