using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.API.Repositories;


namespace RealEstate.API.Models
{
    public class FollowProject
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }

        public int FollowProperty(FollowProject objFollowProject)
        {
            UserRepository objUserRepository = new UserRepository();
            int status = objUserRepository.FollowProject(objFollowProject);
            return status;
        }
        public bool UnFollowProject(FollowProject objFollowProject)
        {
            UserRepository objUserRepository = new UserRepository();
            bool status = objUserRepository.UnFollowProject(objFollowProject);
            return status;
        }

        public bool UnfollowAll(int userId)
        {
            UserRepository objUserRepository = new UserRepository();
            bool status = objUserRepository.UnfollowAll(userId);
            return status;
        }
    }
}