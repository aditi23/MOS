using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.API.Models
{
    public class ChooseImageToBeDeleted
    {
        public int imageId { get; set; }
        public bool deleted { get; set; }
    }
}