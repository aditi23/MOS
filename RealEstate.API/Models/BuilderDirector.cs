using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.API.Models
{
    public class BuilderDirector
    {
        public int Id { get; set; }
        public int BuilderId { get; set; }
        public string NameOfDirector { get; set; }

        public Builder Builder { get; set; }
    }
}