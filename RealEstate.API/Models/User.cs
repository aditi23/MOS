using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.API.Repositories;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.API.Models
{
    public class User
    {
        #region properties
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string EmailId { get; set; }
        public string ProfilePictureUrl { get; set; }
        public Nullable<decimal> Contact { get; set; }
        [Required]
        public string Password { get; set; }
        public string City { get; set; }
        public int LevelId { get; set; }
        public int IsDeleted { get; set; }
        public bool UserRegMode { get; set; }
        public DateTime RegisteredDate { get; set; }
        public int Points { get; set; }
        [Required]
        public int UserTypeId { get; set; }
        public HttpPostedFileBase displayPicture { get; set; }

        public IList<Review> Review { get; set; }
        public string image { get; set; }
        #endregion

        #region functions
        public bool GetReviews(int cityId, int? userId, out object objReviews)
        {
            ReviewRepository objReviewRepository = new ReviewRepository();
            bool status = objReviewRepository.GetReviews(cityId, userId, out objReviews);
            return status;
        }

        public int EditProfileByUserId(User objUser)
        {
            UserRepository objUserRepository = new UserRepository();
            int status = objUserRepository.EditProfileByUserId(objUser);
            return status;
        }

        //public bool GetDiscussion(int cityId, int? userId, out object objPosts)
        //{
        //    DiscussionForumRepository objDiscussionForumRepository = new DiscussionForumRepository();
        //    bool status = objDiscussionForumRepository.GetDiscussion(cityId, userId, out objPosts);
        //    return status;
        //}

            public bool AddRecentlyViewed(RecentlyViewed objRecentlyViewed)
        {
            UserRepository objUserRepository = new UserRepository();
            bool status = objUserRepository.AddRecentlyViewed(objRecentlyViewed);
            return status;
        }

        public bool GetUserDetailsByUserId(int userId, out object userDetails)
        {
            UserRepository objUserRepository = new UserRepository();
            bool status = objUserRepository.GetUserDetailsByUserId(userId, out userDetails);
            return status;
        }

        public bool GetUserFirstName(int userId,out string userName)
        {
            string result;
            UserRepository objUserRepository = new UserRepository();
            bool status = objUserRepository.GetUserFirstName(userId,out result);
            if (status != false)
            {
                userName = result;
                return true;
            }
            else
            {
                userName = null;
                return false;
            }
        }


        public int RegisterUser(User objUser, out List<object> Person)
        {
            UserRepository objUserRepository = new UserRepository();
            int status = objUserRepository.RegisterUser(objUser,out Person);
            return status;
        }

        public bool Login(User objUser, out List<object> Person)
        {
            UserRepository objUserRepository = new UserRepository();
            bool status = objUserRepository.Login(objUser,out Person);
            return status;
        }

        public int UserForgotPassword(string userEmail)
        {
            UserRepository objUserRepository = new UserRepository();
            int status = objUserRepository.UserForgotPassword(userEmail);
            return status;
        }

        public int UserResetPassword(int userId, string oldPassword, string newPassword)
        {
            UserRepository objUserRepository = new UserRepository();
            int status = objUserRepository.UserResetPassword(userId, oldPassword, newPassword);
            return status;
        }

        #endregion
    }
}