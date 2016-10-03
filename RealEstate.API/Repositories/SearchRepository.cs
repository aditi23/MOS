using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealEstate.API.Models;
using RealEstate.API.Repositories;

namespace RealEstate.API.Repositories
{
    public class SearchRepository
    {


        #region ListingPage

        public bool GetProjectListByCityId(int cityId, out object objProjectList)
        {
            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    var projectList = (from p in dbContext.tblProjects
                                       join l in dbContext.tblLocations
                                       on p.Id equals l.ProjectId
                                       join pbm in dbContext.tblProjectBuilderMappings
                                       on p.Id equals pbm.ProjectId
                                       orderby p.AverageRating descending
                                       where l.CityId == cityId
                                       select new
                                       {
                                           p.Id,
                                           p.Name,
                                           p.Possession,
                                           p.Pricing,
                                           p.AverageRating,
                                           profilePicture = (from i in dbContext.tblImages
                                                             where i.ProjectId == p.Id && i.ImageCategoryId == 1
                                                             select new { i.ImageUrl}).FirstOrDefault(),
                                           AddressLine1 = (from l in dbContext.tblLocations
                                                           join c in dbContext.tblCities
                                                           on l.CityId equals c.Id
                                                           where l.ProjectId == p.Id
                                                           select new
                                                           {
                                                               l.AddressLine1,
                                                               l.Id
                                                           }).FirstOrDefault(),
                                           AddressLine2 = (from l in dbContext.tblLocations
                                                           join c in dbContext.tblCities
                                                           on l.CityId equals c.Id
                                                           where l.ProjectId == p.Id
                                                           select new
                                                           {
                                                               l.AddressLine2,
                                                               l.Id
                                                           }).FirstOrDefault(),
                                           City = (from l in dbContext.tblLocations
                                                   join c in dbContext.tblCities
                                                   on l.CityId equals c.Id
                                                   where l.ProjectId == p.Id
                                                   select new
                                                   {
                                                       c.Name,
                                                       c.Id
                                                   }).FirstOrDefault(),
                                           Builder = (from b in dbContext.tblBuilders
                                                      join pbm in dbContext.tblProjectBuilderMappings
                                                      on b.Id equals pbm.BuilderId
                                                      where pbm.ProjectId == p.Id
                                                      select new { b.CompanyName }).FirstOrDefault(),
                                           Images = (from i in dbContext.tblImages
                                                     where i.ProjectId == p.Id && i.ImageCategoryId == 1
                                                     select new { i.ImageUrl }).FirstOrDefault(),
                                           ReviewCount = (from r in dbContext.tblReviews
                                                          join u in dbContext.tblUsers
                                                          on r.UserId equals u.Id
                                                          join ut in dbContext.tblUserTypes
                                                          on u.UserTypeId equals ut.Id
                                                          where r.ProjectId == p.Id && r.IsReviewed == true
                                                          select new
                                                          {
                                                              r
                                                          } ).Count()
                                       }).ToList();
                
                if (projectList.Count == 0)
                    objProjectList = null;
                else
                    objProjectList = projectList;
                return true;
            }
            }
            catch (Exception ex)
            {
                objProjectList = null;
                return false;
            }
}

public bool GetProjectListByBuilderId(int builderId, out object objProjectList)
{
    try
    {
        using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
        {
            var projectList = (from p in dbContext.tblProjects
                               join pbm in dbContext.tblProjectBuilderMappings
                               on p.Id equals pbm.ProjectId
                               join l in dbContext.tblLocations
                               on p.Id equals l.ProjectId
                               orderby p.AverageRating descending
                               where pbm.BuilderId == builderId
                               select new
                               {
                                   p.Id,
                                   p.Name,
                                   p.Possession,
                                   p.Pricing,
                                   p.AverageRating,
                                   AddressLine1 = (from l in dbContext.tblLocations
                                                   join c in dbContext.tblCities
                                                   on l.CityId equals c.Id
                                                   where l.ProjectId == p.Id
                                                   select new
                                                   {
                                                       l.AddressLine1,
                                                       l.Id
                                                   }).FirstOrDefault(),
                                   AddressLine2 = (from l in dbContext.tblLocations
                                                   join c in dbContext.tblCities
                                                   on l.CityId equals c.Id
                                                   where l.ProjectId == p.Id
                                                   select new
                                                   {
                                                       l.AddressLine2,
                                                       l.Id
                                                   }).FirstOrDefault(),
                                   City = (from l in dbContext.tblLocations
                                           join c in dbContext.tblCities
                                           on l.CityId equals c.Id
                                           where l.ProjectId == p.Id
                                           select new
                                           {
                                               c.Name,
                                               c.Id
                                           }).FirstOrDefault(),
                                   Builder = (from b in dbContext.tblBuilders
                                              join pbm in dbContext.tblProjectBuilderMappings
                                              on b.Id equals pbm.BuilderId
                                              where pbm.ProjectId == p.Id
                                              select new { b.CompanyName }).FirstOrDefault(),
                                   Images = (from i in dbContext.tblImages
                                             where i.ProjectId == p.Id && i.ImageCategoryId == 1
                                             select new { i.ImageUrl}).FirstOrDefault(),
                                   ReviewCount = (from r in dbContext.tblReviews
                                                  join u in dbContext.tblUsers
                                                  on r.UserId equals u.Id
                                                  join ut in dbContext.tblUserTypes
                                                  on u.UserTypeId equals ut.Id
                                                  where r.ProjectId == p.Id && r.IsReviewed == true
                                                  select new
                                                  {
                                                      r
                                                  }).Count()
                               }).ToList();
            if (projectList.Count == 0)
                objProjectList = null;
            else
                objProjectList = projectList;
            return true;

        }
    }
    catch (Exception)
    {
        objProjectList = null;
        return false;
    }
}

public bool GetProjectListByLocationId(int locationId, out object objProjectDetails)
{
    using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
    {
        try
        {
            var projectDetails = (from p in dbContext.tblProjects
                                  join l in dbContext.tblLocations
                                  on p.Id equals l.ProjectId
                                  join pbm in dbContext.tblProjectBuilderMappings
                                  on p.Id equals pbm.ProjectId
                                  join c in dbContext.tblCities
                                  on l.CityId equals c.Id
                                  where l.Id == locationId
                                  orderby p.AverageRating descending
                                  select new
                                  {
                                      p.Id,
                                      p.Name,
                                      p.Possession,
                                      p.Pricing,
                                      p.AverageRating,
                                      AddressLine1 = (from l in dbContext.tblLocations
                                                      join c in dbContext.tblCities
                                                      on l.CityId equals c.Id
                                                      where l.ProjectId == p.Id
                                                      select new
                                                      {
                                                          l.AddressLine1,
                                                          l.Id
                                                      }).FirstOrDefault(),
                                      AddressLine2 = (from l in dbContext.tblLocations
                                                      join c in dbContext.tblCities
                                                      on l.CityId equals c.Id
                                                      where l.ProjectId == p.Id
                                                      select new
                                                      {
                                                          l.AddressLine2,
                                                          l.Id
                                                      }).FirstOrDefault(),
                                      City = (from l in dbContext.tblLocations
                                              join c in dbContext.tblCities
                                              on l.CityId equals c.Id
                                              where l.ProjectId == p.Id
                                              select new
                                              {
                                                  c.Name,
                                                  c.Id
                                              }).FirstOrDefault(),
                                      Builder = (from b in dbContext.tblBuilders
                                                 join pbm in dbContext.tblProjectBuilderMappings
                                                 on b.Id equals pbm.BuilderId
                                                 where pbm.ProjectId == p.Id
                                                 select new { b.CompanyName }).FirstOrDefault(),
                                      Images = (from i in dbContext.tblImages
                                                where i.ProjectId == p.Id && i.ImageCategoryId == 1
                                                select new { i.ImageUrl }).FirstOrDefault(),
                                      ReviewCount = (from r in dbContext.tblReviews
                                                     join u in dbContext.tblUsers
                                                     on r.UserId equals u.Id
                                                     join ut in dbContext.tblUserTypes
                                                     on u.UserTypeId equals ut.Id
                                                     where r.ProjectId == p.Id && r.IsReviewed == true
                                                     select new
                                                     {
                                                         r
                                                     }).Count()
                                  }
                           ).ToList();
            if (projectDetails.Count == 0)
                objProjectDetails = null;
            else
                objProjectDetails = projectDetails;
            return true;
        }
        catch (Exception ex)
        {
            objProjectDetails = null;
            return false;
        }
    }

}


#endregion

#region LandingPage

public bool GetAllProjects(out object projectList)
{
    using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
    {
        try
        {
           projectList = (from p in dbContext.tblProjects
                           select new
                           {
                               ID = p.Id,
                               p.Name,
                               p.Possession,
                               p.AverageRating,
                               p.Pricing,
                               AddressLine1 = (from l in dbContext.tblLocations
                                               join c in dbContext.tblCities
                                               on l.CityId equals c.Id
                                               where l.ProjectId == p.Id
                                               select new
                                               {
                                                   l.AddressLine1,
                                                   l.Id
                                               }).FirstOrDefault(),
                               AddressLine2 = (from l in dbContext.tblLocations
                                               join c in dbContext.tblCities
                                               on l.CityId equals c.Id
                                               where l.ProjectId == p.Id
                                               select new
                                               {
                                                   l.AddressLine2,
                                                   l.Id
                                               }).FirstOrDefault(),
                               City = (from l in dbContext.tblLocations
                                       join c in dbContext.tblCities
                                       on l.CityId equals c.Id
                                       where l.ProjectId == p.Id
                                       select new
                                       {
                                           c.Name,
                                           c.Id
                                       }).FirstOrDefault(),
                               Amenities = (from a in dbContext.tblAmenities
                                            join m in dbContext.tblMasterAmenities
                                            on a.MasterAmenityId equals m.Id
                                            where a.ProjectId == p.Id
                                            select new
                                            {
                                                m.Name,
                                                a.Value
                                            }).ToList(),
                               ApartmentBuildQuality = (from a in dbContext.tblApartmentBuildQualities
                                                        where a.ProjectId == p.Id
                                                        select new
                                                        {
                                                            a.MasterApartmentBuildQualityId,
                                                            a.Name,
                                                            a.Value
                                                        }).ToList(),
                               AverageRaitings = (from a in dbContext.tblAverageRatings
                                                  where a.ProjectId == p.Id
                                                  select new
                                                  {
                                                      a.Amenity,
                                                      a.ApartmentBuildQuality,
                                                      a.BuilderProfile,
                                                      a.ConstructionQualityParameter,
                                                      a.Inventory,
                                                      a.LegalClarity,
                                                      a.Livability
                                                  }).FirstOrDefault(),
                               Builder = (from b in dbContext.tblBuilders
                                          join pbm in dbContext.tblProjectBuilderMappings
                                          on b.Id equals pbm.BuilderId
                                          where pbm.ProjectId == p.Id
                                          select new { b.CompanyName }).FirstOrDefault(),
                               ConstructionQualityParameter = (from c in dbContext.tblConstructionQualityParameters
                                                               join m in dbContext.tblMasterConstructionQualityParameters
                                                               on c.MasterConstructionQualityParameterId equals m.Id
                                                               where c.ProjectId == p.Id
                                                               select new
                                                               {
                                                                   m.Name,
                                                                   c.Value
                                                               }).ToList(),
                               Images = (from i in dbContext.tblImages
                                         join ic in dbContext.tblImageCategories
                                         on i.ImageCategoryId equals ic.Id
                                         where i.ProjectId == p.Id
                                         select new { i.ImageUrl, i.Likes, ic.CategoryName }).ToList(),
                               Inventory = (from i in dbContext.tblInventories
                                            join m in dbContext.tblMasterInventories
                                            on i.MasterInventoryId equals m.Id
                                            where i.ProjectId == p.Id
                                            select new { m.Name, i.Value }).ToList(),
                               LegalClarity = (from l in dbContext.tblLegalClarities
                                               join m in dbContext.tblMasterLegalClarities
                                               on l.MasterLegalClarityId equals m.Id
                                               where l.ProjectId == p.Id
                                               select new { m.Name, l.Value }).ToList(),
                               Livability = (from l in dbContext.tblLivabilities
                                             join m in dbContext.tblMasterLivabilities
                                             on l.MasterLivabilityId equals m.Id
                                             where l.ProjectId == p.Id
                                             select new { m.Name, l.Value }).ToList(),
                               ProjectInformation = (from pi in dbContext.tblProjectInformations
                                                     join m in dbContext.tblMasterProjectInformations
                                                     on pi.MasterProjectInformationId equals m.Id
                                                     where pi.ProjectId == p.Id
                                                     select new { m.Name, pi.Value }).ToList(),
                               Reviews = (from r in dbContext.tblReviews
                                          
                                          join u in dbContext.tblUsers
                                          on r.UserId equals u.Id
                                          join ut in dbContext.tblUserTypes
                                          on u.UserTypeId equals ut.Id
                                          where r.ProjectId == p.Id && r.IsReviewed == true
                                          select new
                                          {
                                              reviewId = r.Id,
                                              r.Heading,
                                              userId = u.Id,
                                              ut.UserTypeName,
                                              u.FirstName,
                                              u.LastName,
                                              u.ProfilePictureUrl,
                                              u.LevelId,
                                              u.EmailId,
                                              r.Text,
                                              r.AverageValue,
                                              r.Time,
                                              r.lastVisited,
                                              
                                              BuilderComment = (from bc in dbContext.tblBuilderComments
                                                                where bc.ReviewId == r.Id && bc.IsVerified == true
                                                                select new { bc.Text, bc.IsConvinced }).FirstOrDefault(),
                                              HelpfulCount = r.Helpful,
                                              SayThanksCount = r.SayThanks
                                          }
                                             ).ToList(),
                               ReviewCount = (from r in dbContext.tblReviews
                                              join u in dbContext.tblUsers
                                              on r.UserId equals u.Id
                                              join ut in dbContext.tblUserTypes
                                              on u.UserTypeId equals ut.Id
                                              where r.ProjectId == p.Id && r.IsReviewed == true
                                              select new
                                              {
                                                  r
                                              }
                                             ).Count()
                           }).ToList();

                    return true;
        }
        catch (Exception ex)
        {
            projectList = null;
            return false;
        }
    }
}


public bool GetTrendingReviews(out object trendingReviews)
{
    using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
    {
        try
        {
            trendingReviews = (from r in dbContext.tblReviews
                               join p in dbContext.tblProjects
                               on r.ProjectId equals p.Id
                               join u in dbContext.tblUsers
                               on r.UserId equals u.Id
                               join l in dbContext.tblLocations
                               on p.Id equals l.ProjectId
                               join c in dbContext.tblCities
                               on l.CityId equals c.Id
                               orderby r.Time descending
                               where r.IsReviewed == true
                               select new
                               {
                                   p.Name,
                                   city = c.Name,
                                   r.Id,
                                   r.Heading,
                                   r.AverageValue,
                                   u.FirstName,
                                   u.LastName,
                                   u.ProfilePictureUrl,
                                   u.LevelId,
                                   ReviewCount = dbContext.tblReviews.Where(rev => rev.UserId == u.Id && rev.IsReviewed == true).Count(),
                                   SayThanksCount = dbContext.tblReviews.Where(re => re.UserId == u.Id && re.IsReviewed == true).Select(re => re.SayThanks).Sum()
                                                                 }).ToList().Take(6);
            return true;

        }
        catch (Exception ex)
        {
            trendingReviews = null;
            return false;
        }
    }

}

        public bool GetSiteSatistics(out object stats)
        {

            try
            {
                using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
                {
                    stats = (from p in dbContext.tblProjects
                                  select new
                                  {
                                      projectCount = (from pr in dbContext.tblProjects
                                                      select new
                                                      { pr }).Count(),

                                      userCount = (from u in dbContext.tblUsers
                                                   where u.IsDeleted == 0
                                                   select new { u }).Count(),
                                      reviewCount = (from r in dbContext.tblReviews
                                                     where r.IsReviewed == true
                                                     select new { r }).Count(),
                                      builderCount = (from b in dbContext.tblBuilders
                                                      where b.IsVerified == true
                                                      select new { b }).Count()
                                  }).FirstOrDefault();
                    
                 
                    return true;
                }
            }
            catch(Exception ex)
            {
                stats = null;
                return false;
            }
        }

        public bool FinalAutoComplete(out object finalList)
        {
            using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
            {
                try
                {
                    var resultByProjectName = (from p in dbContext.tblProjects
                                               join c in dbContext.tblLocations
                                               on p.Id equals c.ProjectId
                                               select new { Id=p.Id, Name = p.Name , Type = "Project",CityId = c.CityId }).ToList();
                    var resultByCity = (from c in dbContext.tblCities
                                        select new { Id=c.Id, Name = c.Name, Type = "City" , CityId = c.Id }).ToList();
                    var resultByLocation = (from l in dbContext.tblLocations
                                            join c in dbContext.tblCities
                                            on l.CityId equals c.Id
                                            select new {Id= l.Id, Name = l.AddressLine1 + "," + l.AddressLine2 + "," + c.Name, Type = "Location", CityId = l.CityId }).ToList();
                    var resultByBuilderName = (from b in dbContext.tblBuilders
                                               join pb in dbContext.tblProjectBuilderMappings
                                               on b.Id equals pb.BuilderId
                                               select new { Id = b.Id, Name = b.CompanyName, Type = "Builder", CityId = 1 }).ToList();

                    if (resultByProjectName.Count == 0 && resultByCity.Count == 0 && resultByLocation.Count == 0 && resultByBuilderName.Count == 0)
                        finalList = null;
                    else
                        finalList = resultByProjectName.Concat(resultByCity).Concat(resultByLocation).Concat(resultByBuilderName);
                        return true;
                }
                catch(Exception ex)
                {
                    finalList = null;
                    return false;
                }
            }
        }

        public bool FinalAutoCompleteByCityId(int cityId,out object finalList)
        {
            using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
            {
                try
                {
                    var resultByProjectName = (from p in dbContext.tblProjects
                                               join c in dbContext.tblLocations
                                               on p.Id equals c.ProjectId
                                               where c.CityId == cityId
                                               select new { Id = p.Id, Name = p.Name, Type = "Project" }).ToList();
                    var resultByLocation = (from l in dbContext.tblLocations
                                            join c in dbContext.tblCities
                                            on l.CityId equals c.Id
                                            where c.Id == cityId
                                            select new { Id = l.Id, Name = l.AddressLine1 + "," + l.AddressLine2 + "," + c.Name, Type = "Location" }).ToList();
                    var resultByBuilderName = (from b in dbContext.tblBuilders
                                               join pb in dbContext.tblProjectBuilderMappings
                                               on b.Id equals pb.BuilderId
                                               join p in dbContext.tblProjects
                                               on pb.ProjectId  equals p.Id
                                               join l in dbContext.tblLocations
                                              on p.Id equals l.ProjectId
                                               where l.CityId == cityId
                                               select new { Id = b.Id, Name = b.CompanyName, Type = "Builder" }).ToList();

                    if (resultByProjectName.Count == 0 && resultByLocation.Count == 0 && resultByBuilderName.Count == 0)
                        finalList = null;
                    else
                        finalList = resultByProjectName.Concat(resultByLocation).Concat(resultByBuilderName);
                    return true;
                }
                catch (Exception ex)
                {
                    finalList = null;
                    return false;
                }
            }
        }


        public bool AutocompleteForMainSearch(string stringToLook, int cityId, out object listResult)
{
    using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
    {
        try
        {
            #region OnlyStringToLookNoCity
            if (cityId == 0)
            {
                var resultByProjectName = (from p in dbContext.tblProjects
                                           join l in dbContext.tblLocations
                                           on p.Id equals l.ProjectId
                                           join c in dbContext.tblCities
                                           on l.CityId equals c.Id
                                           where p.Name.StartsWith(stringToLook)
                                           select new { p.Id, Result = p.Name + "," + l.AddressLine1 + "," + l.AddressLine2 + "," + c.Name, Type = "Project" }).ToList();
                var resultByCity = (from c in dbContext.tblCities
                                    where c.Name.StartsWith(stringToLook)
                                    select new { c.Id, Result = c.Name, Type = "City" }).ToList();
                var resultByLocation = (from l in dbContext.tblLocations
                                        join c in dbContext.tblCities
                                        on l.CityId equals c.Id
                                        where l.AddressLine1.Contains(stringToLook) || l.AddressLine2.Contains(stringToLook)
                                        select new { l.Id, Result = l.AddressLine1 + "," + l.AddressLine2 + "," + c.Name, Type = "Location" }).ToList();
                var resultByBuilderName = (from b in dbContext.tblBuilders
                                           where b.CompanyName.StartsWith(stringToLook) && b.IsVerified == true
                                           select new { b.Id, Result = b.CompanyName, Type = "Builder" }).ToList();
                if (resultByProjectName.Count == 0 && resultByCity.Count == 0 && resultByLocation.Count == 0 && resultByBuilderName.Count == 0)
                    listResult = null;
                else
                    listResult = resultByProjectName.Concat(resultByCity).Concat(resultByLocation).Concat(resultByBuilderName);
            }
            #endregion
            else
            {
                #region CitySelectedWithStringToLook
                if (!stringToLook.Equals(null))
                {
                    var resultByProjectName = (from p in dbContext.tblProjects
                                               join l in dbContext.tblLocations
                                               on p.Id equals l.ProjectId
                                               join c in dbContext.tblCities
                                               on l.CityId equals c.Id
                                               where p.Name.StartsWith(stringToLook) && l.CityId == cityId
                                               select new { p.Id, Result = p.Name + "," + l.AddressLine1 + "," + l.AddressLine2 + "," + c.Name, Type = "Project" }).ToList();

                    var resultByLocation = (from l in dbContext.tblLocations
                                            join c in dbContext.tblCities
                                            on l.CityId equals c.Id
                                            where (l.AddressLine1.StartsWith(stringToLook) || l.AddressLine2.StartsWith(stringToLook)) && c.Id == cityId
                                            select new { l.Id, Result = l.AddressLine1 + "," + l.AddressLine2 + "," + c.Name, Type = "Location" }).ToList();
                    var resultByBuilderName = (from b in dbContext.tblBuilders
                                               where b.CompanyName.StartsWith(stringToLook) && b.IsVerified == false
                                               select new { b.Id, Result = b.CompanyName, Type = "Builder" }).ToList();
                    if (resultByProjectName.Count == 0 && resultByLocation.Count == 0 && resultByBuilderName.Count == 0)
                        listResult = null;
                    else
                        listResult = resultByProjectName.Concat(resultByLocation).Concat(resultByBuilderName);
                    #endregion
                }
                else
                {
                    #region OnlyCitySelected
                    var resultByCity = (from p in dbContext.tblProjects
                                        join l in dbContext.tblLocations
                                        on p.Id equals l.ProjectId
                                        join c in dbContext.tblCities
                                        on l.CityId equals c.Id
                                        where l.CityId == cityId
                                        select new { p.Id, Result = p.Name + "," + l.AddressLine1 + "," + l.AddressLine2 + "," + c.Name, Type = "Project" }).ToList();
                    if (resultByCity.Count == 0)
                        listResult = null;
                    else
                        listResult = resultByCity;
                    #endregion
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            listResult = null;
            return false;

        }
    }
}

public bool GetAllCities(out object cityList)
{
    try
    {
        using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
        {
            cityList = (from c in dbContext.tblCities
                        orderby c.Name
                        select new { c.Id, c.Name }).ToList();
            return true;
        }
    }
    catch (Exception ex)
    {
        cityList = null;
        return false;
    }
}

#endregion

#region PropertyPage

public int GetProjectDetailstByProjectId(int projectId, int? userId, out object projectDetails)
{
    using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
    {
        try
        {
            bool IsProjectPresent = ((dbContext.tblProjects.Where(proj => proj.Id == projectId).Count() == 1) ? true : false);
            if (IsProjectPresent == false)
            {
                projectDetails = null;
                return 1;                                                  //project not found
            }
            else
            {
                if (userId != null)
                {
                            #region LoggedInUser
                            projectDetails = (from p in dbContext.tblProjects
                                           where p.Id == projectId
                                           select new
                                           {
                                               Project = p.Name,
                                               p.Possession,
                                               p.Pricing,
                                               p.AverageRating,
                                               AddressLine1 = (from l in dbContext.tblLocations
                                                               join c in dbContext.tblCities
                                                               on l.CityId equals c.Id
                                                               where l.ProjectId == projectId
                                                               select new
                                                               {
                                                                   l.AddressLine1,
                                                                   l.Id
                                                               }).FirstOrDefault(),
                                               AddressLine2 = (from l in dbContext.tblLocations
                                                               join c in dbContext.tblCities
                                                               on l.CityId equals c.Id
                                                               where l.ProjectId == projectId
                                                               select new
                                                               {
                                                                   l.AddressLine2,
                                                                   l.Id
                                                               }).FirstOrDefault(),
                                               City = (from l in dbContext.tblLocations
                                                       join c in dbContext.tblCities
                                                       on l.CityId equals c.Id
                                                       where l.ProjectId == projectId
                                                       select new
                                                       {
                                                           c.Name,
                                                           c.Id
                                                       }).FirstOrDefault(),
                                               Amenities = (from a in dbContext.tblAmenities
                                                            join m in dbContext.tblMasterAmenities
                                                            on a.MasterAmenityId equals m.Id
                                                            where a.ProjectId == projectId
                                                            select new
                                                            {
                                                                m.Name,
                                                                a.Value
                                                            }).ToList(),
                                               ApartmentBuildQuality = (from a in dbContext.tblApartmentBuildQualities
                                                                        where a.ProjectId == projectId
                                                                        select new
                                                                        {
                                                                            a.MasterApartmentBuildQualityId,
                                                                            a.Name,
                                                                            a.Value
                                                                        }).ToList(),
                                               AverageRaitings = (from a in dbContext.tblAverageRatings
                                                                  where a.ProjectId == projectId
                                                                  select new
                                                                  {
                                                                      a.Amenity,
                                                                      a.ApartmentBuildQuality,
                                                                      a.BuilderProfile,
                                                                      a.ConstructionQualityParameter,
                                                                      a.Inventory,
                                                                      a.LegalClarity,
                                                                      a.Livability
                                                                  }).FirstOrDefault(),
                                               Builder = (from b in dbContext.tblBuilders
                                                          join pbm in dbContext.tblProjectBuilderMappings
                                                          on b.Id equals pbm.BuilderId
                                                          where pbm.ProjectId == projectId
                                                          select new { b.CompanyName }).FirstOrDefault(),
                                               ConstructionQualityParameter = (from c in dbContext.tblConstructionQualityParameters
                                                                               join m in dbContext.tblMasterConstructionQualityParameters
                                                                               on c.MasterConstructionQualityParameterId equals m.Id
                                                                               where c.ProjectId == projectId
                                                                               select new
                                                                               {
                                                                                   m.Name,
                                                                                   c.Value
                                                                               }).ToList(),
                                               Images = (from i in dbContext.tblImages
                                                         join ic in dbContext.tblImageCategories
                                                         on i.ImageCategoryId equals ic.Id
                                                         where i.ProjectId == projectId
                                                         select new { i.ImageUrl, i.Likes, ic.CategoryName }).ToList(),
                                               Inventory = (from i in dbContext.tblInventories
                                                            join m in dbContext.tblMasterInventories
                                                            on i.MasterInventoryId equals m.Id
                                                            where i.ProjectId == projectId
                                                            select new { m.Name, i.Value }).ToList(),
                                               LegalClarity = (from l in dbContext.tblLegalClarities
                                                               join m in dbContext.tblMasterLegalClarities
                                                               on l.MasterLegalClarityId equals m.Id
                                                               where l.ProjectId == projectId
                                                               select new { m.Name, l.Value }).ToList(),
                                               Livability = (from l in dbContext.tblLivabilities
                                                             join m in dbContext.tblMasterLivabilities
                                                             on l.MasterLivabilityId equals m.Id
                                                             where l.ProjectId == projectId
                                                             select new { m.Name, l.Value }).ToList(),
                                               ProjectInformation = (from pi in dbContext.tblProjectInformations
                                                                     join m in dbContext.tblMasterProjectInformations
                                                                     on pi.MasterProjectInformationId equals m.Id
                                                                     where pi.ProjectId == projectId
                                                                     select new { m.Name, pi.Value }).ToList(),
                                               Reviews = (from r in dbContext.tblReviews.AsEnumerable()
                                                          
                                                          join u in dbContext.tblUsers
                                                          on r.UserId equals u.Id
                                                          join ut in dbContext.tblUserTypes
                                                          on u.UserTypeId equals ut.Id
                                                          where r.ProjectId == projectId && r.IsReviewed == true
                                                          select new
                                                          {
                                                              reviewId = r.Id,
                                                              r.Heading,
                                                              userId = u.Id,
                                                              ut.UserTypeName,
                                                              u.FirstName,
                                                              u.LastName,
                                                              u.ProfilePictureUrl,
                                                              u.LevelId,
                                                              u.EmailId,
                                                              r.Text,
                                                              r.AverageValue,
                                                              r.Time,
                                                              r.lastVisited,
                                                              BuilderComment = (from bc in dbContext.tblBuilderComments
                                                                                where bc.ReviewId == r.Id && bc.IsVerified == true
                                                                                select new { bc.Text, bc.IsConvinced }).FirstOrDefault(),
                                                              HelpfulCount = r.Helpful,
                                                              SayThanksCount = r.SayThanks
                                                          }
                                                     ).ToList(),
                                               ReviewCount = (from r in dbContext.tblReviews
                                                              join u in dbContext.tblUsers
                                                              on r.UserId equals u.Id
                                                              join ut in dbContext.tblUserTypes
                                                              on u.UserTypeId equals ut.Id
                                                              where r.ProjectId == projectId && r.IsReviewed == true
                                                              select new
                                                              {
                                                                  r
                                                              }
                                                     ).Count(),
                                               IsFollowed = (((dbContext.tblFollowProjects.Where(fp => fp.ProjectId == projectId && fp.UserId == userId).Count()) == 1) ? true : false),

                                           }).FirstOrDefault();
                      
                            //foreach(var p in Details.Reviews)
                            //{
                            //   IEnumerable<int> duration= ReviewRepository.CalculateLastVisitedDuration(p.mid, p.yname);
                            //    Details.Reviews.Add(duration);
                            //    Details.Reviews.Concat(duration);
                            //    Details.Reviews.Insert(1,duration);
                            //    Details.Reviews.Union(duration);
                            //    Details.Reviews.AddRange(duration);
                            //    Details.Reviews.
                            //}
                    #endregion
                }
                        else
                {
                    #region BuilderOrGuestUser
                    projectDetails = (from p in dbContext.tblProjects
                                      where p.Id == projectId
                                      select new
                                      {
                                          Project = p.Name,
                                          p.Possession,
                                          p.Pricing,
                                          p.AverageRating,
                                          AddressLine1 = (from l in dbContext.tblLocations
                                                          join c in dbContext.tblCities
                                                          on l.CityId equals c.Id
                                                          where l.ProjectId == projectId
                                                          select new
                                                          {
                                                              l.AddressLine1,
                                                              l.Id
                                                          }).FirstOrDefault(),
                                          AddressLine2 = (from l in dbContext.tblLocations
                                                          join c in dbContext.tblCities
                                                          on l.CityId equals c.Id
                                                          where l.ProjectId == projectId
                                                          select new
                                                          {
                                                              l.AddressLine2,
                                                              l.Id
                                                          }).FirstOrDefault(),
                                          City = (from l in dbContext.tblLocations
                                                  join c in dbContext.tblCities
                                                  on l.CityId equals c.Id
                                                  where l.ProjectId == projectId
                                                  select new
                                                  {
                                                      c.Name,
                                                      c.Id
                                                  }).FirstOrDefault(),
                                          Amenities = (from a in dbContext.tblAmenities
                                                       join m in dbContext.tblMasterAmenities
                                                       on a.MasterAmenityId equals m.Id
                                                       where a.ProjectId == projectId
                                                       select new
                                                       {
                                                           m.Name,
                                                           a.Value
                                                       }).ToList(),
                                          ApartmentBuildQuality = (from a in dbContext.tblApartmentBuildQualities
                                                                   where a.ProjectId == projectId
                                                                   select new
                                                                   {
                                                                       a.MasterApartmentBuildQualityId,
                                                                       a.Name,
                                                                       a.Value
                                                                   }).ToList(),
                                          AverageRaitings = (from a in dbContext.tblAverageRatings
                                                             where a.ProjectId == projectId
                                                             select new
                                                             {
                                                                 a.Amenity,
                                                                 a.ApartmentBuildQuality,
                                                                 a.BuilderProfile,
                                                                 a.ConstructionQualityParameter,
                                                                 a.Inventory,
                                                                 a.LegalClarity,
                                                                 a.Livability
                                                             }).FirstOrDefault(),
                                          Builder = (from b in dbContext.tblBuilders
                                                     join pbm in dbContext.tblProjectBuilderMappings
                                                     on b.Id equals pbm.BuilderId
                                                     where pbm.ProjectId == projectId
                                                     select new { b.CompanyName }).FirstOrDefault(),
                                          ConstructionQualityParameter = (from c in dbContext.tblConstructionQualityParameters
                                                                          join m in dbContext.tblMasterConstructionQualityParameters
                                                                          on c.MasterConstructionQualityParameterId equals m.Id
                                                                          where c.ProjectId == projectId
                                                                          select new
                                                                          {
                                                                              m.Name,
                                                                              c.Value
                                                                          }).ToList(),
                                          Image = (from i in dbContext.tblImages
                                                   join ic in dbContext.tblImageCategories
                                                   on i.ImageCategoryId equals ic.Id
                                                   where i.ProjectId == projectId
                                                   select new { i.ImageUrl, i.Likes, ic.CategoryName }).ToList(),
                                          Inventory = (from i in dbContext.tblInventories
                                                       join m in dbContext.tblMasterInventories
                                                       on i.MasterInventoryId equals m.Id
                                                       where i.ProjectId == projectId
                                                       select new { m.Name, i.Value }).ToList(),
                                          LegalClarity = (from l in dbContext.tblLegalClarities
                                                          join m in dbContext.tblMasterLegalClarities
                                                          on l.MasterLegalClarityId equals m.Id
                                                          where l.ProjectId == projectId
                                                          select new { m.Name, l.Value }).ToList(),
                                          Livability = (from l in dbContext.tblLivabilities
                                                        join m in dbContext.tblMasterLivabilities
                                                        on l.MasterLivabilityId equals m.Id
                                                        where l.ProjectId == projectId
                                                        select new { m.Name, l.Value }).ToList(),
                                          ProjectInformation = (from pi in dbContext.tblProjectInformations
                                                                join m in dbContext.tblMasterProjectInformations
                                                                on pi.MasterProjectInformationId equals m.Id
                                                                where pi.ProjectId == projectId
                                                                select new { m.Name, pi.Value }).ToList(),
                                          Reviews = (from r in dbContext.tblReviews
                                                     
                                                     join u in dbContext.tblUsers
                                                     on r.UserId equals u.Id
                                                     join ut in dbContext.tblUserTypes
                                                     on u.UserTypeId equals ut.Id
                                                     where r.ProjectId == projectId && r.IsReviewed == true
                                                     select new
                                                     {
                                                         reviewId = r.Id,
                                                         r.Heading,
                                                         userId = u.Id,
                                                         ut.UserTypeName,
                                                         u.FirstName,
                                                         u.LastName,
                                                         u.ProfilePictureUrl,
                                                         u.LevelId,
                                                         u.EmailId,
                                                         r.Text,
                                                         r.AverageValue,
                                                         r.Helpful,
                                                         r.lastVisited,
                                                         //mid=lv.MonthId,
                                                         //yname=lv.tblYear.YearName,
                                                         //duration = ReviewRepository.CalculateLastVisitedDuration(lv.MonthId, lv.tblYear.YearName),
                                                         BuilderComment = (from bc in dbContext.tblBuilderComments
                                                                           where bc.ReviewId == r.Id && bc.IsVerified == true
                                                                           select new { bc.Text, bc.IsConvinced }).FirstOrDefault(),
                                                         HelpfulCount = r.Helpful,
                                                         SayThanksCount = r.SayThanks
                                                     }
                                                     ).ToList(),
                                      }).FirstOrDefault();
                    #endregion
                }

                return 2;                                               //successful
            }
        }
        catch (Exception ex)
        {
            projectDetails = null;
            return 0;                                                   //operation failed
        }
    }
}

public int GetSimilarProjectsByProjectId(int projectId, out object similarProjects)
{
    try
    {
        using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
        {
            var currentProject = (from p in dbContext.tblProjects
                                  join l in dbContext.tblLocations
                                  on p.Id equals l.ProjectId
                                  where p.Id == projectId
                                  select new { p.Pricing, l.AddressLine1, l.AddressLine2, l.CityId }).FirstOrDefault();
            if (currentProject != null)
            {
                similarProjects = (from pr in dbContext.tblProjects
                                   join l in dbContext.tblLocations
                                   on pr.Id equals l.ProjectId
                                   join i in dbContext.tblImages
                                   on pr.Id equals i.ProjectId
                                   where pr.Pricing >= (currentProject.Pricing - 0.1 * currentProject.Pricing) 
                                   && pr.Pricing <= (currentProject.Pricing + 0.1 * currentProject.Pricing)
                                   && l.CityId == currentProject.CityId && l.AddressLine2.Equals(currentProject.AddressLine2) 
                                   && pr.Id != projectId && i.ImageCategoryId == 1
                                   select new
                                   {
                                       pr.Id,
                                       pr.Name,
                                       i.ImageUrl,
                                       pr.AverageRating,
                                       ReviewCount = dbContext.tblReviews.Where(r => r.ProjectId == pr.Id && r.IsReviewed == true).Count()
                                   }).ToList().Take(5);
                if (similarProjects != null)
                    return 1;                                           //succcessful
                return 2;                                               //no similar projects found
            }
            else
            {
                similarProjects = null;
                return 3;                                               //projectId not found
            }
        }
    }
    catch (Exception ex)
    {
        similarProjects = null;
        return 0;                                                           //unsuccessful
    }
}

#endregion

#region NotInUse

public bool GetReviewsByPropertyId(int propId, out object obj)
{
    try
    {
        using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
        {
            obj = (from r in dbContext.tblReviews
                   orderby r.AverageValue descending
                   where r.ProjectId == propId
                   select new { r.ProjectId, r.UserId, r.Time, r.AverageValue, r.Helpful, r.Text, r.tblUser.FirstName, r.tblUser.LastName,r.lastVisited }).ToList();
            if (obj != null)
                return true;
            return false;
        }
    }
    catch (Exception)
    {
        obj = null;
        return false;
    }
}

public bool AutocompleteForReviewSearch(string stringToLook, out object listResult)
{
    using (dbRealEstateEntities dbContext = new dbRealEstateEntities())
    {
        try
        {
            var resultByProjectName = (from p in dbContext.tblProjects
                                       join l in dbContext.tblLocations
                                       on p.Id equals l.ProjectId
                                       join c in dbContext.tblCities
                                       on l.CityId equals c.Id
                                       where p.Name.StartsWith(stringToLook)
                                       select new { p.Id, Result = p.Name + "," + l.AddressLine1 + "," + l.AddressLine2 + "," + c.Name, Type = "Project" }).ToList();
            var resultByCity = (from c in dbContext.tblCities
                                where c.Name.StartsWith(stringToLook)
                                select new { c.Id, Result = c.Name, Type = "City" }).ToList();
            var resultByLocation = (from l in dbContext.tblLocations
                                    join c in dbContext.tblCities
                                    on l.CityId equals c.Id
                                    where l.AddressLine1.StartsWith(stringToLook) || l.AddressLine2.StartsWith(stringToLook)
                                    select new { l.Id, Result = l.AddressLine1 + "," + l.AddressLine2 + "," + c.Name, Type = "Location" }).ToList();
            listResult = resultByProjectName.Concat(resultByCity).Concat(resultByLocation);
            if (listResult != null)
                return true;
            else
                return false;
        }
        catch (Exception ex)
        {
            listResult = null;
            return false;
        }
    }
}

        #endregion

    }
}