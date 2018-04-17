using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using web_server.Models;
using web_server.Models.WebApi;
using web_server.Model;
using web_server.Filters;
using System.Web;
using System.Data.Entity;

namespace web_server.web_api.Controllers
{
    public class ServicesController : ApiController
    {
        DatabaseModelContainer db;
        ApplicationDbContext context;

        public ServicesController() : this(null, null)
        {
            // Default constructor
        }

        public ServicesController(DatabaseModelContainer _db = null, ApplicationDbContext _cntxt = null)
        {
            if (_db == null)
                db = new DatabaseModelContainer();
            else
                db = _db;

            if (_cntxt == null)
                context = new ApplicationDbContext();
            else
                context = _cntxt;
        }

        [ProviderApiAuthentication]
        public bool PostService(ServiceApi service)
        {
            var user = GetCurrentUser();

            try
            {
                var regionId = db.RegionSet.FirstOrDefault(r => r.Name == service.RegionName).Id;

                var new_service = new Service
                {
                    Description = service.Description,
                    Price = service.Price,
                    Title = service.Title,
                    RegionId = regionId,
                    Provider = user as Provider
                };

                db.ServiceSet.Add(new_service);
                db.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        // PUT /api/services/id
        [ProviderApiAuthentication]
        public bool PutService(ServiceApi service)
        {
            try
            {
                var edited_service = db.ServiceSet.FirstOrDefault(u => u.Id == service.Id);

                var regionId = db.RegionSet.FirstOrDefault(r => r.Name == service.RegionName).Id;

                edited_service.Title = service.Title;
                edited_service.Description = service.Description;
                edited_service.Price = service.Price;
                edited_service.RegionId = regionId;

                db.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        // DELETE /api/services/{id}
        [ProviderApiAuthentication]
        public bool DeleteService(int id)
        {
            var service = db.ServiceSet.FirstOrDefault(u => u.Id == id);

            if (service == null)
                return false;

            bool success = false;

            try
            {
                db.ServiceSet.Remove(service);
                db.SaveChanges();
                success = true;
            }
            catch
            {
                success = false;
            }

            return success;
        }

        // GET /api/services
        [ProviderApiAuthentication]
        public ServiceApi[] GetAllServices()
        {
            // implement regions
            var provider = GetCurrentUser() as Provider;

            return db.ServiceSet.Where(s => s.Provider.Id == provider.Id).Select(
                u => new ServiceApi
                {
                    Description = u.Description,
                    Title = u.Title,
                    Price = u.Price,
                    RegionName = u.Region.Name,
                    Id = u.Id
                }
            ).ToArray();
        }

        // GET /api/choosed
        [Route("api/choosed")]
        [ProviderApiAuthentication]
        public ServiceChoosedApi[] GetAllChoosedServices()
        {
            var provider = GetCurrentUser() as Provider;

            return db.ServiceChoosedSet.Where(s => s.Provider.Id == provider.Id).Select(
                u => new ServiceChoosedApi
                {
                    Description = u.Service.Description,
                    Title = u.Service.Title,
                    Price = u.Service.Price,
                    RegionName = "Nazwa Regionu",
                    Accepted = u.Accepted,
                    ClientPhone = u.Customer.PhoneNumber,
                    ClientName = u.Customer.FirstName + " " + u.Customer.SecondName,
                    FinishedByClient = u.FinishedByCustomer,
                    FinishedByProvider = u.FinishedByProvider,
                    Id = u.Id
                }
            ).ToArray();
        }

        // GET /api/choosed/accept
        [Route("api/choosed/accept/{id}")]
        [ProviderApiAuthentication]
        [AcceptVerbs("GET")]
        public bool AcceptService(int id)
        {
            var edited = db.ServiceChoosedSet.FirstOrDefault(u => u.Id == id);

            if (edited == null)
                return false;

            try
            {
                edited.Accepted = true;
                db.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        // PUT /api/choosed/finish
        [Route("api/choosed/finish/{id}")]
        [ProviderApiAuthentication]
        [AcceptVerbs("GET")]
        public bool FinishService(int id)
        {
            var edited = db.ServiceChoosedSet.FirstOrDefault(u => u.Id == id);

            if (edited == null)
                return false;

            try
            {
                edited.FinishedByProvider = true;
                db.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        private User GetCurrentUser()
        {
            ApplicationUser user;

            if (RequestContext.Principal.Identity != null)
            {
                var username = RequestContext.Principal.Identity.GetUserName();
                user = context.Users.FirstOrDefault(u => username == u.UserName);
            }
            else
            {
                var requestContext = Request.Properties["MS_HttpContext"] as HttpContextWrapper;
                user = context.Users.FirstOrDefault(u => requestContext.User.Identity.Name == u.UserName);
            }

            var provider = db.UserSet.FirstOrDefault(u => u.NetUserId == user.Id) as Provider;

            return provider;
        }
    }
}
