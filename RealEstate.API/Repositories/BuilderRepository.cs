using System;
using System.Collections.Generic;
using System.Linq;
using RealEstate.API.Models;
using System.IO;
using System.Data.Entity;


namespace RealEstate.API.Repositories
{
    public class BuilderRepository
    {
        #region LandingPage

        public int RegisterBuilder(Builder objBuilder, out int builderId)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    bool DoEmailExist = (((dbContext.tblBuilders.Where(u => u.CompanyEmailId == objBuilder.CompanyEmailId).Count()) == 1) ? true : false);
                    if (DoEmailExist == false)
                    {
                        tblBuilder objtblBuilder = new tblBuilder();
                        objtblBuilder.CompanyName = objBuilder.CompanyName;
                        objtblBuilder.CompanyRegistrationNumber = objBuilder.CompanyRegistrationNumber;
                        objtblBuilder.YearOfEstablishment = objBuilder.YearOfEstablishment;
                        objtblBuilder.TotalProjectsCount = 0;
                        objtblBuilder.CompanyEmailId = objBuilder.CompanyEmailId;
                        objtblBuilder.AdminContactNumber = objBuilder.AdminContactNumber;
                        objtblBuilder.IsVerified = false;
                        objtblBuilder.Password = objBuilder.Password;
                        objtblBuilder.AdminName = objBuilder.AdminName;
                        dbContext.tblBuilders.AddObject(objtblBuilder);
                        bool status = dbContext.SaveChanges() > 0;
                        builderId = objtblBuilder.Id;
                        if (status != false)
                        {
                            return 1;                               //successful
                        }
                        else
                        {
                            builderId = 0;                          //builder not added
                            return 2;
                        }

                    }
                    else
                    {
                        builderId = 0;                          //emailId already exist
                        return 3;
                    }

                }
            }
            catch (Exception ex)
            {
                builderId = 0;
                return 0;
            }
        }

        #endregion

        #region BuilderProfile

        #region GetAllMasterDetails
        public bool GetAllBuilder(out object BuilderList)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    BuilderList = (from c in dbContext.tblBuilders
                                   orderby c.CompanyName
                                   select new { c.Id, c.CompanyName }).ToList();
                    return true;
                }
            }
            catch (Exception ex)
            {
                BuilderList = null;
                return false;
            }
        }

        public bool GetAllMasterLivability(out object MasterLivabilityList)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    MasterLivabilityList = (from c in dbContext.tblMasterLivabilities
                                            orderby c.Name
                                            select new { c.Id, c.Name }).ToList();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MasterLivabilityList = null;
                return false;
            }
        }

        public bool GetAllMasterAmenity(out object MasterAmenityList)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    MasterAmenityList = (from c in dbContext.tblMasterAmenities
                                         orderby c.Name
                                         select new { c.Id, c.Name }).ToList();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MasterAmenityList = null;
                return false;
            }
        }

        public bool GetAllMasterLegalClarity(out object MasterLegalClarityList)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    MasterLegalClarityList = (from c in dbContext.tblMasterLegalClarities
                                              orderby c.Name
                                              select new { c.Id, c.Name }).ToList();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MasterLegalClarityList = null;
                return false;
            }
        }

        public bool GetAllMasterInventory(out object MasterInventoryList)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    MasterInventoryList = (from c in dbContext.tblMasterInventories
                                           orderby c.Name
                                           select new { c.Id, c.Name }).ToList();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MasterInventoryList = null;
                return false;
            }
        }

        public bool GetAllMasterConstructionQualityParameter(out object MasterConstructionQualityParameterList)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    MasterConstructionQualityParameterList = (from c in dbContext.tblMasterConstructionQualityParameters
                                                              orderby c.Name
                                                              select new { c.Id, c.Name }).ToList();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MasterConstructionQualityParameterList = null;
                return false;
            }
        }

        public bool GetAllMasterApartmentBuildQuality(out object MasterApartmentBuildQualityList)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    MasterApartmentBuildQualityList = (from c in dbContext.tblMasterApartmentBuildQualities
                                                       orderby c.Name
                                                       select new { c.Id, c.Name }).ToList();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MasterApartmentBuildQualityList = null;
                return false;
            }
        }

        #endregion

        public bool GetBuilderDetailsByBuilderId(int builderId, out object builderDetails)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    builderDetails = (from b in dbContext.tblBuilders
                                      where b.Id == builderId
                                      select new
                                      {
                                          b.CompanyName,
                                          b.YearOfEstablishment,
                                          b.TotalProjectsCount,
                                          b.CompanyEmailId,
                                          b.AdminContactNumber,
                                          Directors = (from bd in dbContext.tblBuilderDirectors
                                                       where bd.BuilderId == builderId
                                                       select new { bd.NameOfDirector }).ToList(),
                                          Projects = (from p in dbContext.tblProjects
                                                      join pbm in dbContext.tblProjectBuilderMappings
                                                      on p.Id equals pbm.ProjectId
                                                      join i in dbContext.tblImages
                                                      on p.Id equals i.ProjectId
                                                      join ic in dbContext.tblImageCategories
                                                      on i.ImageCategoryId equals ic.Id
                                                      where pbm.BuilderId == builderId && ic.Id == 1
                                                      select new { p.Id, p.Name, i.ImageUrl }).ToList(),
                                          Reviews = (from r in dbContext.tblReviews
                                                     join pbm in dbContext.tblProjectBuilderMappings
                                                     on r.ProjectId equals pbm.ProjectId
                                                     join p in dbContext.tblProjects
                                                     on pbm.ProjectId equals p.Id
                                                     join u in dbContext.tblUsers
                                                     on r.UserId equals u.Id
                                                     where pbm.BuilderId == builderId && r.IsReviewed == true
                                                     orderby r.Time descending
                                                     select new { r.Id, u.FirstName, u.ProfilePictureUrl, u.LevelId, r.Text, p.Name }).ToList()
                                      }).FirstOrDefault();
                    return true;
                }
            }
            catch (Exception ex)
            {
                builderDetails = null;
                return false;
            }
        }


        #region AddNewProject

        public bool CreateProjectBuilderMapping(Project objProject)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    tblProjectBuilderMapping objtblProjectBuilderMapping = new tblProjectBuilderMapping();
                    objtblProjectBuilderMapping.ProjectId = objProject.Id;
                    objtblProjectBuilderMapping.BuilderId = objProject.ProjectBuilderMapping.BuilderId;
                    objtblProjectBuilderMapping.BeneficiaryName = objProject.ProjectBuilderMapping.BeneficiaryName;
                    dbContext.tblProjectBuilderMappings.AddObject(objtblProjectBuilderMapping);
                    bool status = dbContext.SaveChanges() > 0;
                    return status;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CreateProject(Project objProject, out int projectId)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    tblProject objtblProject = new tblProject();
                    objtblProject.Name = objProject.Name;
                    objtblProject.Possession = objProject.Possession;
                    objtblProject.Pricing = objProject.Pricing;

                    //using (var binaryReader = new BinaryReader(objProject.ProfilePicture.InputStream))
                        //objtblProject.ProfilePicture = binaryReader.ReadBytes(objProject.ProfilePicture.ContentLength);
                    dbContext.tblProjects.AddObject(objtblProject);
                    dbContext.SaveChanges();
                    projectId = objtblProject.Id;
                    if (projectId != 0)
                        return true;
                    return false;
                }
            }
            catch (Exception ex)
            {
                projectId = 0;
                return false;
            }


        }

        public bool CreateAmenity(Project objProject)
        {

            try
            {
                using (dbRealEstateEntities db = new dbRealEstateEntities())
                {
                    for (var i = 0; i < objProject.Amenities.Count; i++)
                    {
                        if (objProject.Amenities[i].isIncluded)
                        {
                            tblAmenity objtblAmenity = new tblAmenity()
                            {
                                ProjectId = objProject.Id,
                                MasterAmenityId = objProject.Amenities[i].MasterAmenityId,
                                Value = objProject.Amenities[i].Value,
                            };
                            db.tblAmenities.AddObject(objtblAmenity);
                            db.SaveChanges();
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CreateApartmentBuildQuality(Project objProject)
        {

            try
            {
                using (dbRealEstateEntities db = new dbRealEstateEntities())
                {

                    for (var i = 0; i < objProject.ApartmentBuildQualities.Count; i++)
                    {
                        if (objProject.ApartmentBuildQualities[i].isIncluded)
                        {
                            tblApartmentBuildQuality objtblApartmentBuildQuality = new tblApartmentBuildQuality()
                            {
                                ProjectId = objProject.Id,
                                MasterApartmentBuildQualityId = objProject.ApartmentBuildQualities[i].MasterApartmentBuildQualityId,
                                Name = objProject.ApartmentBuildQualities[i].Name,
                                Value = objProject.ApartmentBuildQualities[i].Value,
                            };
                            db.tblApartmentBuildQualities.AddObject(objtblApartmentBuildQuality);
                            db.SaveChanges();
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CreateConstructionQualityParameter(Project objProject)
        {

            try
            {
                using (dbRealEstateEntities db = new dbRealEstateEntities())
                {
                    for (var i = 0; i < objProject.ConstructionQualityParameters.Count; i++)
                    {
                        if (objProject.ConstructionQualityParameters[i].isIncluded)
                        {
                            tblConstructionQualityParameter objtblConstructionQualityParameter = new tblConstructionQualityParameter()
                            {
                                ProjectId = objProject.Id,
                                MasterConstructionQualityParameterId = objProject.ConstructionQualityParameters[i].MasterConstructionQualityParameterId,
                                Value = objProject.ConstructionQualityParameters[i].Value,
                            };
                            db.tblConstructionQualityParameters.AddObject(objtblConstructionQualityParameter);
                            db.SaveChanges();
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CreateImage(Project objProject)
        {

            try
            {
                using (dbRealEstateEntities db = new dbRealEstateEntities())
                {
                    for (var i = 0; i < objProject.Images.Count; i++)
                    {
                        tblImage objtblImage = new tblImage();
                        objtblImage.ProjectId = objProject.Id;
                        objtblImage.ImageCategoryId = objProject.Images[i].ImageCategoryId;
                        objtblImage.ImageUrl = objProject.Images[i].ImageUrl;

                        //using (Stream inputStream = objProject.ProjectImagesDetails[i].InputStream)
                        //{
                        //    MemoryStream memoryStream = inputStream as MemoryStream;
                        //    if (memoryStream == null)
                        //    {
                        //        memoryStream = new MemoryStream();
                        //        inputStream.CopyTo(memoryStream);
                        //    }

                        //    objtblImage.ImageData = memoryStream.ToArray();
                        //}
                        db.tblImages.AddObject(objtblImage);
                        db.SaveChanges();

                        //string physicalPathProjectImage = System.Web.HttpContext.Current.Server.MapPath("~\\" + ("ProjectImages") + "\\" + objProject.ProjectImagesDetails[i].FileName);
                        //objProject.ProjectImagesDetails[i].SaveAs(physicalPathProjectImage);

                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CreateInventory(Project objProject)
        {
            try
            {
                using (dbRealEstateEntities db = new dbRealEstateEntities())
                {

                    for (var i = 0; i < objProject.Inventories.Count; i++)
                    {
                        if (objProject.Inventories[i].isIncluded)
                        {
                            tblInventory objtblInventory = new tblInventory()
                            {
                                ProjectId = objProject.Id,
                                MasterInventoryId = objProject.Inventories[i].MasterInventoryId,
                                Value = objProject.Inventories[i].Value,
                            };
                            db.tblInventories.AddObject(objtblInventory);
                            db.SaveChanges();
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CreateLegalClarity(Project objProject)
        {

            try
            {
                using (dbRealEstateEntities db = new dbRealEstateEntities())
                {
                    for (var i = 0; i < objProject.LegalClarities.Count; i++)
                    {
                        if (objProject.LegalClarities[i].isIncluded)
                        {
                            tblLegalClarity objtblLegalClarity = new tblLegalClarity()
                            {
                                ProjectId = objProject.Id,
                                MasterLegalClarityId = objProject.LegalClarities[i].MasterLegalClarityId,
                                Value = objProject.LegalClarities[i].Value,
                            };
                            db.tblLegalClarities.AddObject(objtblLegalClarity);
                            db.SaveChanges();
                        }
                    }
                    return true;
                }
            }

            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CreateLivability(Project objProject)
        {

            try
            {
                using (dbRealEstateEntities db = new dbRealEstateEntities())
                {
                    for (var i = 0; i < objProject.Livabilities.Count; i++)
                    {
                        if (objProject.Livabilities[i].isIncluded)
                        {
                            tblLivability objtblLivability = new tblLivability()
                            {
                                ProjectId = objProject.Id,
                                MasterLivabilityId = objProject.Livabilities[i].MasterLivabilityId,
                                Name = objProject.Livabilities[i].Name,
                                Value = objProject.Livabilities[i].Value,
                            };
                            db.tblLivabilities.AddObject(objtblLivability);
                            db.SaveChanges();
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CreateLocation(Project objProject)
        {

            try
            {
                using (dbRealEstateEntities db = new dbRealEstateEntities())
                {
                    tblLocation objtblLocation = new tblLocation()
                    {
                        ProjectId = objProject.Id,
                        AddressLine1 = objProject.Locations.AddressLine1,
                        AddressLine2 = objProject.Locations.AddressLine2,
                        CityId = objProject.Locations.CityId,
                    };
                    db.tblLocations.AddObject(objtblLocation);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;

            }
        }

        public bool CreateProjectInformation(Project objProject)
        {

            try
            {
                using (dbRealEstateEntities db = new dbRealEstateEntities())
                {
                    foreach (var projInfo in objProject.ProjectInformations)
                    {
                        if (projInfo.isIncluded)
                        {
                            tblProjectInformation objtblProjectInformation = (new tblProjectInformation()
                            {
                                ProjectId = objProject.Id,
                                MasterProjectInformationId = projInfo.MasterProjectInformationId,
                                Value = projInfo.Value,
                            });
                            db.tblProjectInformations.AddObject(objtblProjectInformation);
                            db.SaveChanges();
                        }
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        #endregion

        public bool GetAllImageCategory(out object imageCategoryList)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    imageCategoryList = (from c in dbContext.tblImageCategories
                                 orderby c.Id
                                 select new { c.Id, c.CategoryName }).ToList();
                    return true;
                }
            }
            catch (Exception ex)
            {
                imageCategoryList = null;
                return false;
            }
        }


        #region EditProject

        public bool EditAmenity(Project objProject)
        {
            using (dbRealEstateEntities db = new dbRealEstateEntities())
            {
                try
                {
                    for (var i = 0; i < objProject.Amenities.Count; i++)
                    {
                        tblAmenity objtblAmenity = new tblAmenity()
                        {
                            Id = objProject.Amenities[i].Id,
                            ProjectId = objProject.Amenities[i].ProjectId,
                            MasterAmenityId = objProject.Amenities[i].MasterAmenityId,
                            Value = objProject.Amenities[i].Value,
                        };
                        if (objtblAmenity != null)
                        {
                            db.tblAmenities.Attach(objtblAmenity);
                            db.ObjectStateManager.ChangeObjectState(objtblAmenity, EntityState.Modified);
                            db.SaveChanges();
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool EditApartmentBuildQuality(Project objProject)
        {
            using (dbRealEstateEntities db = new dbRealEstateEntities())
            {
                try
                {
                    for (var i = 0; i < objProject.ApartmentBuildQualities.Count; i++)
                    {
                        tblApartmentBuildQuality objtblApartmentBuildQuality = new tblApartmentBuildQuality()
                        {
                            Id = objProject.ApartmentBuildQualities[i].Id,
                            ProjectId = objProject.ApartmentBuildQualities[i].ProjectId,
                            MasterApartmentBuildQualityId = objProject.ApartmentBuildQualities[i].MasterApartmentBuildQualityId,
                            Name = objProject.ApartmentBuildQualities[i].Name,
                            Value = objProject.ApartmentBuildQualities[i].Value,
                        };
                        if (objtblApartmentBuildQuality != null)
                        {
                            db.tblApartmentBuildQualities.Attach(objtblApartmentBuildQuality);
                            db.ObjectStateManager.ChangeObjectState(objtblApartmentBuildQuality, EntityState.Modified);
                            db.SaveChanges();
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool EditConstructionQualityParameter(Project objProject)
        {
            using (dbRealEstateEntities db = new dbRealEstateEntities())
            {
                try
                {
                    for (var i = 0; i < objProject.ConstructionQualityParameters.Count; i++)
                    {
                        tblConstructionQualityParameter objtblConstructionQualityParameter = new tblConstructionQualityParameter()
                        {
                            Id = objProject.ConstructionQualityParameters[i].Id,
                            ProjectId = objProject.ConstructionQualityParameters[i].ProjectId,
                            MasterConstructionQualityParameterId = objProject.ConstructionQualityParameters[i].MasterConstructionQualityParameterId,
                            Value = objProject.ConstructionQualityParameters[i].Value,
                        };
                        if (objtblConstructionQualityParameter != null)
                        {
                            db.tblConstructionQualityParameters.Attach(objtblConstructionQualityParameter);
                            db.ObjectStateManager.ChangeObjectState(objtblConstructionQualityParameter, EntityState.Modified);
                            db.SaveChanges();
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool EditImages(Project objProject)
        {
            using (dbRealEstateEntities db = new dbRealEstateEntities())
            {
                try
                {
                    for (var j = 0; j < objProject.ImageToBeDeleted.Count; j++)
                    {
                        if (objProject.ImageToBeDeleted[j].deleted)
                        {
                            int imageIdToBeDeleted = objProject.ImageToBeDeleted[j].imageId;
                            tblImage objtblImage = new tblImage();
                            objtblImage = db.tblImages.FirstOrDefault(data => data.Id == imageIdToBeDeleted);
                            //var imgToBeDeleted = Convert.ToBase64String(objtblImage.ImageData);
                            //objProject.Images[j].ImageData = String.Format("data:image/gif;base64,{0}", imgToBeDeleted);
                            //string physicalPathImageDelete = System.Web.HttpContext.Current.Server.MapPath("~\\" + ("ProjectImages") + "\\" + objProject.Images[j].Image);
                            //System.IO.File.Delete(physicalPathImageDelete);
                            db.tblImages.DeleteObject(objtblImage);
                            db.SaveChanges();
                        }
                    }


                    for (var i = 0; i < objProject.Images.Count; i++)
                    {
                        tblImage objtblImage = new tblImage();

                        if (objProject.Images[i] != null)
                        {
                            objtblImage.Id = objProject.Images[i].Id;
                            objtblImage.ProjectId = objProject.Images[i].ProjectId;
                            objtblImage.ImageCategoryId = objProject.Images[i].ImageCategoryId;
                            objtblImage.ImageUrl = objProject.Images[i].ImageUrl;
                            //using (var binaryReader = new BinaryReader(objProject.ProjectImagesDetails[i].InputStream))
                            //    objtblImage.ImageData = binaryReader.ReadBytes(objProject.ProjectImagesDetails[i].ContentLength);

                            db.tblImages.Attach(objtblImage);
                            db.tblImages.AddObject(objtblImage);
                            db.SaveChanges();
                            //string physicalPath = System.Web.HttpContext.Current.Server.MapPath("~\\" + ("ProjectImages") + "\\" + objProject.ProjectImagesDetails[i].FileName);
                            //objProject.ProjectImagesDetails[i].SaveAs(physicalPath);
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

        }

        public bool EditInventory(Project objProject)
        {
            using (dbRealEstateEntities db = new dbRealEstateEntities())
            {
                try
                {
                    for (var i = 0; i < objProject.Inventories.Count; i++)
                    {
                        tblInventory objtblInventory = new tblInventory()
                        {
                            Id = objProject.Inventories[i].Id,
                            ProjectId = objProject.Id,
                            MasterInventoryId = objProject.Inventories[i].MasterInventoryId,
                            Value = objProject.Inventories[i].Value,
                        };
                        if (objtblInventory != null)
                        {
                            db.tblInventories.Attach(objtblInventory);
                            db.ObjectStateManager.ChangeObjectState(objtblInventory, EntityState.Modified);
                            db.SaveChanges();
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool EditLegalClarity(Project objProject)
        {
            using (dbRealEstateEntities db = new dbRealEstateEntities())
            {
                try
                {

                    for (var i = 0; i < objProject.LegalClarities.Count; i++)
                    {
                        tblLegalClarity objtblLegalClarity = new tblLegalClarity()
                        {
                            Id = objProject.LegalClarities[i].Id,
                            ProjectId = objProject.LegalClarities[i].ProjectId,
                            MasterLegalClarityId = objProject.LegalClarities[i].MasterLegalClarityId,
                            Value = objProject.LegalClarities[i].Value,
                        };
                        if (objtblLegalClarity != null)
                        {
                            db.tblLegalClarities.Attach(objtblLegalClarity);
                            db.ObjectStateManager.ChangeObjectState(objtblLegalClarity, EntityState.Modified);
                            db.SaveChanges();
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool EditLivability(Project objProject)
        {
            using (dbRealEstateEntities db = new dbRealEstateEntities())
            {
                try
                {
                    for (var i = 0; i < objProject.Livabilities.Count; i++)
                    {
                        tblLivability objtblLivability = new tblLivability()
                        {
                            Id = objProject.Livabilities[i].Id,
                            ProjectId = objProject.Livabilities[i].ProjectId,
                            MasterLivabilityId = objProject.Livabilities[i].MasterLivabilityId,
                            Name = objProject.Livabilities[i].Name,
                            Value = objProject.Livabilities[i].Value,
                        };
                        if (objtblLivability != null)
                        {
                            db.tblLivabilities.Attach(objtblLivability);
                            db.ObjectStateManager.ChangeObjectState(objtblLivability, EntityState.Modified);
                            db.SaveChanges();
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool EditLocation(Project objProject)
        {
            using (dbRealEstateEntities db = new dbRealEstateEntities())
            {
                try
                {
                    tblLocation objtblLocation = new tblLocation()
                    {
                        Id = objProject.Locations.Id,
                        AddressLine1 = objProject.Locations.AddressLine1,
                        AddressLine2 = objProject.Locations.AddressLine2,
                        CityId = objProject.Locations.CityId,
                        ProjectId = objProject.Locations.ProjectId,
                    };
                    if (objtblLocation != null)
                    {
                        db.tblLocations.Attach(objtblLocation);
                        db.ObjectStateManager.ChangeObjectState(objtblLocation, EntityState.Modified);
                        db.SaveChanges();
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool EditProjectInformation(Project objProject)
        {
            using (dbRealEstateEntities db = new dbRealEstateEntities())
            {
                try
                {
                    for (var i = 0; i < objProject.ProjectInformations.Count; i++)
                    {
                        tblProjectInformation objtblProjectInformation = new tblProjectInformation()
                        {
                            Id = objProject.ProjectInformations[i].Id,
                            ProjectId = objProject.ProjectInformations[i].ProjectId,
                            MasterProjectInformationId = objProject.ProjectInformations[i].MasterProjectInformationId,
                            Value = objProject.ProjectInformations[i].Value,
                        };
                        if (objtblProjectInformation != null)
                        {
                            db.tblProjectInformations.Attach(objtblProjectInformation);
                            db.ObjectStateManager.ChangeObjectState(objtblProjectInformation, EntityState.Modified);
                            db.SaveChanges();
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool EditProject(Project objProject)
        {
            using (dbRealEstateEntities db = new dbRealEstateEntities())
            {
                try
                {
                    tblProject objtblProject = new tblProject();
                    objtblProject.Id = objProject.Id;
                    objtblProject.Name = objProject.Name;
                    objtblProject.Possession = objProject.Possession;
                    objtblProject.Pricing = objProject.Pricing;
                    if (objProject.ProfilePicture != null)

                    {
                        string physicalPathProfilePicture = System.Web.HttpContext.Current.Server.MapPath("~\\" + ("ProjectProfilePicture") + "\\" + objProject.ProfilePicture.FileName);
                        objProject.ProfilePicture.SaveAs(physicalPathProfilePicture);
                    }


                    db.tblProjects.Attach(objtblProject);
                    db.ObjectStateManager.ChangeObjectState(objtblProject, EntityState.Modified);
                    db.SaveChanges();



                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        #endregion

        public int CommentOnReview(BuilderComment objBuilderComment)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    int alreadyCommented = (from bc in dbContext.tblBuilderComments
                                            where bc.tblProjectBuilderMapping.BuilderId == objBuilderComment.BuilderId 
                                            && bc.tblProjectBuilderMapping.ProjectId == objBuilderComment.ProjectId
                                            && bc.ReviewId == objBuilderComment.ReviewId
                                            select new { bc.Id }).Count();
                    if (alreadyCommented != 0)
                        return 1;                               //builder has already commented once
                    else
                    {
                        int pbmId = (from pbm in dbContext.tblProjectBuilderMappings
                                     where pbm.ProjectId == objBuilderComment.ProjectId && pbm.BuilderId == objBuilderComment.BuilderId
                                     select new { pbm.Id}).First().Id;

                        tblBuilderComment objtblBuilderComment = new tblBuilderComment();
                        objtblBuilderComment.ReviewId = objBuilderComment.ReviewId;
                        objtblBuilderComment.ProjectBuilderMappingId = Convert.ToInt32(pbmId);
                        objtblBuilderComment.Text = objBuilderComment.Text;
                        objtblBuilderComment.IsVerified = false;
                        objtblBuilderComment.IsConvinced = false;
                        dbContext.tblBuilderComments.AddObject(objtblBuilderComment);
                        bool status = dbContext.SaveChanges() > 0;
                        int bcId = objtblBuilderComment.Id;
                        if (status != false)
                        {
                            string mailSubject = "New Comment By Builder";
                            string mailBody = "<b>Hello Admin,</b><br/><br/>";
                            mailBody += "A builder has added a comment on a review. <br/>Click on link below to confirm-<br/>";
                            mailBody += "<p><a href=#>Click here</a></p>";
                            mailBody += "builderComment id is = " + bcId;

                            Notification objNotification = new Notification();
                            bool mailSent = objNotification.SendMail("sukankshi.jain@gmail.com", mailSubject, mailBody);

                            if (mailSent != false)
                            {
                                return 2;                                                           //comment sent for admin confirmation
                            }
                            return 3;                                     //unable to sent for admin confirmation
                        }
                        return 4;                                    //comment cannot be added
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;                                           //operation failed
            }
        }

        public int EditProfile(Builder objBuilder)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    tblBuilder objtblBuilder = dbContext.tblBuilders.SingleOrDefault(b => b.Id == objBuilder.Id);
                    if (objtblBuilder != null)
                    {
                        objtblBuilder.CompanyName = objBuilder.CompanyName;
                        objtblBuilder.CompanyRegistrationNumber = objBuilder.CompanyRegistrationNumber;
                        objtblBuilder.YearOfEstablishment = objBuilder.YearOfEstablishment;
                        objtblBuilder.TotalProjectsCount = 0;
                        objtblBuilder.CompanyEmailId = objBuilder.CompanyEmailId;
                        objtblBuilder.AdminContactNumber = objBuilder.AdminContactNumber;
                        objtblBuilder.Password = objBuilder.Password;
                        objtblBuilder.AdminName = objBuilder.AdminName;
                      
                        bool status = dbContext.SaveChanges() > 0;
                        if (status != false)
                        {
                            List<tblBuilderDirector> objBuilderDirector = (from bd in dbContext.tblBuilderDirectors
                                                                           where bd.BuilderId == objBuilder.Id
                                                                           select bd).ToList();
                            foreach (tblBuilderDirector obd in objBuilderDirector)
                            {
                                int i = 0;
                                obd.NameOfDirector = objBuilder.builderDirector[i].NameOfDirector;
                                dbContext.SaveChanges();
                                i++;
                            }
                            List<tblBuilderProfile> objBuilderProfile = (from bp in dbContext.tblBuilderProfiles
                                                                         where bp.BuilderId == objBuilder.Id
                                                                         select bp).ToList();
                            foreach (tblBuilderProfile obp in objBuilderProfile)
                            {
                                int i = 0;
                                obp.MasterBuilderProfileId = objBuilder.builderProfile[i].MasterBuilderProfileId;
                                obp.Value = objBuilder.builderProfile[i].Value;
                                dbContext.SaveChanges();
                                i++;
                            }
                            return 1;                                       //edited successfully
                        }
                        return 2;                                    //profile cannot be edited

                    }
                    return 3;                               //builder not found
                }
            }
            catch (Exception ex)
            {
                return 0;                                   //operation failed
            }
        }

        public void EditProject(Project objProject, int builderId)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    var projectToUpdate = dbContext.tblProjectBuilderMappings.SingleOrDefault(pbm => pbm.ProjectId == objProject.Id && pbm.BuilderId == builderId);
                    if (projectToUpdate != null)
                    {
                        tblProject objtblProject = new tblProject();
                        objtblProject.Possession = objProject.Possession;
                        objtblProject.Pricing = objProject.Pricing;
                       // objtblProject.ProfilePicture = objProject.ProfilePicture;
                        // objtblProject.isLatest = false;
                        bool status = dbContext.SaveChanges() > 0;
                        if (status == true)
                        {
                            Notification objNotification = new Notification();
                            string mailSubject = "Project Details Updation Request";
                            string mailBody = "Following project details updation have been done.<br/>";
                            mailBody += "<b>Possession<b>" + objProject.Possession + "<br/>";
                            mailBody += "<b>Pricing<b>" + objProject.Pricing + "<br/>";
                            mailBody += "<b>ProfilePicture<b>" + objProject.ProfilePicture + "<br/>";

                            #region NotInUse

                            //public bool Login(Builder objBuilder, out int builderId)
                            //{
                            //    try
                            //    {
                            //        using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                            //        {
                            //            builderId = dbContext.tblBuilders.Where(u => u.EmailId == objBuilder.EmailId && u.Password == objBuilder.Password).Select(u => u.Id).FirstOrDefault();
                            //            if (builderId != 0)
                            //                return true;
                            //            else
                            //            {
                            //                builderId = 0;
                            //                return false;
                            //            }

                            //        }
                            //    }
                            //    catch (Exception)
                            //    {
                            //        builderId = 0;
                            //        return false;
                            //    }
                            //}
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
    #endregion
}