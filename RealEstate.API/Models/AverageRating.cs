using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.API.Models
{
    public class AverageRating
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public double Amenity { get; set; }
        public double ApartmentBuildQuality { get; set; }
        public double BuilderProfile { get; set; }
        public double ConstructionQualityParameter { get; set; }
        public double Inventory { get; set; }
        public double LegalClarity { get; set; }
        public double Livability { get; set; }
    }
}