using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.API.Models
{
    public class BuilderProfile
    {
        public int Id { get; set; }
        public int MasterBuilderProfileId { get; set; }
        public string Value { get; set; }
        public int BuilderId { get; set; }

       // public Project Project { get; set; }
        public Builder Builder { get; set; }
    }
}