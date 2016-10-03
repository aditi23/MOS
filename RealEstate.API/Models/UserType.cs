using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.API.Repositories;

namespace RealEstate.API.Models
{
    public class UserType
    {
        public int Id { get; set; }
        public string UserTypeName { get; set; }

        public bool GetAllUserType(out object userTypeList)
        {
            UserRepository objUserRepository = new UserRepository();
            bool status = objUserRepository.GetAllUserType(out userTypeList);
            return status;
        }
    }
}