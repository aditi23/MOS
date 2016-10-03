using RealEstate.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RealEstate.API.Controllers
{
    public class DiscussionForumController : ApiController
    {

        /// <summary>
        /// POST :api/DiscussionForum/FollowPost
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="postId"></param>
        /// <returns></returns>                
        [HttpPost]                                                                       //tested
        public IHttpActionResult FollowPost(FollowPost objFollowPost)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (objFollowPost.UserId != 0)
            {
                bool status = objFollowPost.Follow(objFollowPost);
                if (status != true)
                    return BadRequest("Operation Failed");
                return Ok();
            }
            return BadRequest("Sign In Required");                                               //sign In API to be called
        }



        /// <summary>
        /// GET : api/DiscussionForum/GetComments
        /// </summary>
        /// <returns></returns>
        [HttpGet]                                                                        //tested
        public IHttpActionResult GetComments()
        {
            Comment objComment = new Comment();
            Object objComments = new Object();
            bool status = objComment.GetComments(out objComments);
            if (status != true)
                return Ok(objComments);
            return BadRequest("Operation Failed");
        }

        /// <summary>
        /// GET: api/DiscussionForum/GetDiscussion
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="userId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet]                                                                          //tested
        public IHttpActionResult GetDiscussion(int cityId, int? userId)
        {
            if (cityId != 0 || userId != 0)
            {
                User objUser = new User();
                Object objPosts = new object();
                bool status = objUser.GetDiscussion(cityId, userId, out objPosts);
                if (status != true)
                    return BadRequest("Invalid Operation");
                return Ok(objPosts);
            }
            return BadRequest("Invalid Request");
        }
 /// <summary>
        /// GET:api/DiscussionForum/GetProjectByPostId
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetProjectByPostId(int postId)     //tested
        {
            if (postId != 0)
            {
                object projectDetails;
                Project objProject = new Project();
                bool status = objProject.GetProjectByPostId(postId, out projectDetails);
                if (status != false)
                    return Ok(projectDetails);
                else
                    return BadRequest("Operation Failed");

            }
            else
                return BadRequest("Invalid Request");

        }

        /// <summary>
        /// POST: api/DiscussionForum/AddComment
        /// </summary>
        /// <param name="objComment"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddComment(Comment objComment)     //tested
        {
            if (objComment.UserId != 0)
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid Request");
                bool status = objComment.AddComment(objComment);
                if (status != false)
                    return Ok();
                return BadRequest("Operation Failed");
            }
            else
                return BadRequest("SignIn Required");           //call signIn api
        }

        /// <summary>
        /// POST:api/DiscussionForum/AddPost
        /// </summary>
        /// <param name="objPost"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddPost(Post objPost)          //tested
        {
            if (objPost.UserId != 0)
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid Request");
                bool isAdded = objPost.AddPost(objPost);
                if (isAdded != false)
                    return Ok();
                else
                    return BadRequest("Invalid Operation");
            }
            else
                return BadRequest("SignIn Required");       //cal SignIn api
        }

        /// <summary>
        /// GET:api/DiscussionForum/GetPostDetailsByPostId
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetPostDetailsByPostId(int postId)     //tested
        {
            if(postId!= 0)
            {
                object postDetails;
                Post objPost = new Post();
                bool isFound = objPost.GetPostDetailsByPostId(postId, out postDetails);
                if (isFound != false)
                    return Ok(postDetails);
                else
                    return BadRequest("Operation failed");
            }
            return BadRequest("Invalid Request");
        }
    }
}
