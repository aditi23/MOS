using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.API.Models
{
    public class Level
    {
        public int Id { get; set; }
        public int Points { get; set; }
        public int ReviewCount { get; set; }
        public int ThanksCount { get; set; }
        public int HelpfulCount { get; set; }

    }
}