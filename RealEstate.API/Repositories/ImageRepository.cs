using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using RealEstate.API.Models;
using System.Web.UI.WebControls;

namespace RealEstate.API.Repositories
{
    public class ImageRepository
    {
        public bool GetImageByImageId(int imageId, out object objImages)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    var objImage = dbContext.tblImages.Where(x => x.Id == imageId).Select(y => y).ToList();

                    if (objImage != null)
                    {

                        objImages = objImage;
                        return true;
                    }
                    else
                    {
                        objImages = null;
                        return false;
                    }

                }
            }
            catch (Exception)
            {
                objImages = null;
                return false;
            }
        }
    }
}