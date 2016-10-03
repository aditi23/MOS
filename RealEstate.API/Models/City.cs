using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.API.Repositories;

namespace RealEstate.API.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool GetAllCities(out object cityList)
        {
            SearchRepository objSearchRepository = new SearchRepository();
            bool status = objSearchRepository.GetAllCities(out cityList);
            return status;
        }

    }
}