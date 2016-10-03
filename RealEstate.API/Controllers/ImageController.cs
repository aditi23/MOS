using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RealEstate.API.Models;

namespace RealEstate.API.Controllers
{
    public class ImageController : ApiController
    {
        /// <summary>
        /// GET:api/Image/GetImageByImageId
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetImageByImageId(int imageId)
        {
            if (imageId != 0)
            {
                object objImages;
                Image objImage = new Image();
                bool isFound = objImage.GetImageByImageId(imageId, out objImages);
                if (isFound != false)
                    return Ok(objImages);
                return BadRequest("Operation Failed");
            }
            else
            {
                return BadRequest("Invalid Request");
            }
        }
    }
}