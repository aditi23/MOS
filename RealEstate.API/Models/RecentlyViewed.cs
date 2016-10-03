using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.API.Models
{
    public class RecentlyViewed
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public System.DateTime Time { get; set; }
    }
}