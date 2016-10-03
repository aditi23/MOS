using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.API.Models;

namespace RealEstate.API.Repositories
{
    public class DiscussionForumRepository
    {
        public bool GetProjectByPostId(int postId, out object ProjectDetails)
        {
            using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
            {
                try
                {
                    var projectToBeDisplayed = (from post in dbContext.tblPosts
                                                where post.Id == postId
                                                select new { post.ProjectId }).FirstOrDefault();
       
                                                
                    if (projectToBeDisplayed != null)
                    {
                        int projectId = Convert.ToInt32(projectToBeDisplayed);
                        var result = (from p in dbContext.tblProjects
                                      where p.Id == projectId
                                      select new
                                      {   p,
                                          p.tblAmenities,
                                          p.tblApartmentBuildQualities,
                                          p.tblBuilderProfiles,
                                          p.tblConstructionQualityParameters,
                                          p.tblImages,
                                          p.tblIntroductoryVideos,
                                          p.tblInventories,
                                          p.tblLegalClarities,
                                          p.tblLivabilities,
                                          p.tblPosts,
                                          p.tblProjectInformations,
                                          p.tblReviews,
                                          p.tblUpdatedVideos
                                      }).FirstOrDefault();
                        ProjectDetails = result;
                        return true;
                    }
                    else
                    {
                        ProjectDetails = null;
                        return false;
                    }


                }
                catch (Exception ex)
                {
                    ProjectDetails = null;
                    return false;
                }
            }

        }

        public bool AddComment(Comment objComment)
        {
            using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
            {
                try
                {
                    tblComment objTblComment = new tblComment();
                    objTblComment.PostId = objComment.PostId;
                    objTblComment.UserId = objComment.UserId;
                    objTblComment.Text = objComment.Text;
                    objTblComment.Time = DateTime.Now;
                    dbContext.tblComments.AddObject(objTblComment);
                    return dbContext.SaveChanges() > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }

            }
        }

        public bool AddPost(Post objPost)
        {
            using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
            {
                try
                {
                    tblPost objTblPost = new tblPost();
                    objTblPost.Heading = objPost.Heading;
                    objTblPost.UserId = objPost.UserId;
                    objTblPost.Text = objPost.Text;
                    objTblPost.ProjectId = objPost.ProjectId;
                    objTblPost.Time = DateTime.Now;
                    dbContext.tblPosts.AddObject(objTblPost);
                    return dbContext.SaveChanges() > 0;
                }
                catch (Exception ex)
                {

                    return false;
                }
            }
        }

        public bool GetPostDetailsByPostId(int postId,out object postDetails)
        {
            using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
            {
                try
                {
                    var result = (from p in dbContext.tblPosts
                                  where p.Id == postId
                                  select new { p,p.tblComments }).ToList();
                    if(result != null)
                    {
                        postDetails = result;
                        return true;
                    }
                    else
                    {
                        postDetails = null;
                        return false;
                    }
                }
                catch(Exception ex)
                {
                    postDetails = null;
                    return false;
                }
            }
        }
    }
}