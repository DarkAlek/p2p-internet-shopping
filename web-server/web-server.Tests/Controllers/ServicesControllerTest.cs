using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using web_server.Tests.Mocks;
using web_server.web_api.Controllers;
using System.Web.Http;
using web_server.Models;
using System.Security.Principal;
using System.Linq;
using System.Web;
using Moq;
using System.Net.Http;
using System.Collections.Generic;
using System.Web.Routing;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using System.Threading;
using web_server.Model;
using Microsoft.AspNet.Identity;
using web_server.Models.WebApi;
using WebControllers = web_server.Controllers;
using System.Web.Mvc;

namespace web_server.Tests.Controllers
{
    [TestClass]
    public class ServicesControllerTest
    {
        private DatabaseModelContainer dbContext;
        private ApplicationDbContext appContext;

        private void SetUpTest()
        {
            dbContext = new DatabaseModelContainer();
            appContext = new ApplicationDbContext();
        }

        private ApplicationUser CreateTestProvider()
        {
            var existUser = appContext.Users.FirstOrDefault(u => u.UserName == "TestProvider");

            if (existUser != null)
            {
                appContext.Users.Remove(existUser);
                appContext.SaveChanges();
            }

            var appUser = new ApplicationUser
            {
                Email = "testmail@test.test",
                EmailConfirmed = true,
                UserName = "TestProvider",
                PasswordHash = new PasswordHasher().HashPassword("S3cretPassword$"),
                PhoneNumber = "123456789",
                SecurityStamp = "3236c4f3-b5c1-4f55-aa81-a10fd2dfcece"
            };

            appContext.Users.Add(appUser);
            appContext.SaveChanges();

            var addedUser = new Provider()
            {
                NetUserId = appUser.Id,
                Activated = true,
                FirstName = appUser.UserName,
                SecondName = "Test",
                PhoneNumber = "123456789"
            };

            dbContext.UserSet.Add(addedUser);
            dbContext.SaveChanges();

            return appUser;
        }

        private ApplicationUser CreateTestAdmin()
        {
            var existUser = appContext.Users.FirstOrDefault(u => u.UserName == "TestAdmin");

            if (existUser != null)
            {
                appContext.Users.Remove(existUser);
                appContext.SaveChanges();
            }

            var appUser = new ApplicationUser
            {
                Email = "testmailadmin@test.test",
                EmailConfirmed = true,
                UserName = "TestAdmin",
                PasswordHash = new PasswordHasher().HashPassword("S3cretPassword$"),
                PhoneNumber = "123456789",
                SecurityStamp = "3236c4f3-b5c1-4f55-aa81-a10fd2dfcece"
            };

            appContext.Users.Add(appUser);
            appContext.SaveChanges();

            var addedUser = new Admin()
            {
                NetUserId = appUser.Id,
                Activated = true,
                FirstName = appUser.UserName,
                SecondName = "Test",
            };

            dbContext.UserSet.Add(addedUser);
            dbContext.SaveChanges();

            return appUser;
        }

        private ApplicationUser CreateTestCustomer()
        {
            var existUser = appContext.Users.FirstOrDefault(u => u.UserName == "TestCustomer");

            if (existUser != null)
            {
                appContext.Users.Remove(existUser);
                appContext.SaveChanges();
            }

            var appUser = new ApplicationUser
            {
                Email = "testmailcustomer@test.test",
                EmailConfirmed = true,
                UserName = "TestCustomer",
                PasswordHash = new PasswordHasher().HashPassword("S3cretPassword$"),
                PhoneNumber = "123456789",
                SecurityStamp = "3236c4f3-b5c1-4f55-aa81-a10fd2dfcece"
            };

            appContext.Users.Add(appUser);
            appContext.SaveChanges();

            var addedUser = new Customer()
            {
                NetUserId = appUser.Id,
                Activated = true,
                FirstName = appUser.UserName,
                SecondName = "Test",
                PhoneNumber = "123456789"
            };

            dbContext.UserSet.Add(addedUser);
            dbContext.SaveChanges();

            return appUser;
        }

        private Region CreateTestRegion()
        {
            var region = dbContext.RegionSet.Add(new Region()
            {
                Name = "TestRegion",
            });

            dbContext.SaveChanges();

            return region;
        }

        private Service CreateTestService(Region region, ApplicationUser appUser)
        {
            var user = dbContext.UserSet.FirstOrDefault(u => u.NetUserId == appUser.Id);

            var service = dbContext.ServiceSet.Add(new Service()
            {
                Title = "Test Service Title",
                Description = "Test Service Description",
                Price = 40.00M,
                Provider = user as Provider,
                Region = region,
            });

            dbContext.SaveChanges();

            return service;
        }

        private ServiceChoosed CreateTestServiceChoosed(Service service, ApplicationUser customer)
        {
            var customerApp = dbContext.UserSet.FirstOrDefault(c => c.NetUserId == customer.Id) as Customer;

            var serviceChoosed = new ServiceChoosed()
            {
                Customer = customerApp,
                Accepted = false,
                CustomerNote = "Buy me coffeeee!",
                Provider = service.Provider,
                Service = service,
                FinishedByCustomer = false,
                FinishedByProvider = false
            };

            dbContext.ServiceChoosedSet.Add(serviceChoosed);
            dbContext.SaveChanges();

            return serviceChoosed;
        }

        private void CleanupServiceChoosed(int id)
        {
            var serviceChoosed = dbContext.ServiceChoosedSet.FirstOrDefault(s => s.Id == id);

            dbContext.ServiceChoosedSet.Remove(serviceChoosed);
            dbContext.SaveChanges();
        }

        private void CleanupUser(string id)
        {
            var user = dbContext.UserSet.FirstOrDefault(u => u.NetUserId == id);
            dbContext.UserSet.Remove(user);
            dbContext.SaveChanges();

            var userApp = appContext.Users.FirstOrDefault(u => u.Id == id);
            appContext.Users.Remove(userApp);
            appContext.SaveChanges();
        }

        private void CleanupService(int id)
        {
            var service = dbContext.ServiceSet.FirstOrDefault(s => s.Id == id);

            dbContext.ServiceSet.Remove(service);
            dbContext.SaveChanges();
        }

        private void CleanupRegion(int id)
        {
            var region = dbContext.RegionSet.FirstOrDefault(r => r.Id == id);

            dbContext.RegionSet.Remove(region);
            dbContext.SaveChanges();
        }

        [TestMethod]
        public void GetAllServicesMoreThanZeroTest()
        {
            SetUpTest();

            var appUser = CreateTestProvider();
            var region = CreateTestRegion();
            var service = CreateTestService(region, appUser);
            var username = appUser.UserName;

            var serviceController = new ServicesController(dbContext, appContext);
            
            serviceController.RequestContext.Principal = new GenericPrincipal(new GenericIdentity(username), null);

            var result = serviceController.GetAllServices();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());

            CleanupService(service.Id);
            CleanupRegion(region.Id);
            CleanupUser(appUser.Id);
        }

        [TestMethod]
        public void GetAllServicesByUserWhenZeroTest()
        {
            SetUpTest();

            var appUser = CreateTestProvider();
            var username = appUser.UserName;

            var serviceController = new ServicesController(dbContext, appContext);

            serviceController.RequestContext.Principal = new GenericPrincipal(new GenericIdentity(username), null);

            var result = serviceController.GetAllServices();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());

            CleanupUser(appUser.Id);
        }

        [TestMethod]
        public void DeleteServiceTest()
        {
            SetUpTest();

            var appUser = CreateTestProvider();
            var region = CreateTestRegion();
            var service = CreateTestService(region, appUser);
            var username = appUser.UserName;

            var serviceController = new ServicesController(dbContext, appContext);

            serviceController.RequestContext.Principal = new GenericPrincipal(new GenericIdentity(username), null);

            var count = dbContext.ServiceSet.Count(s => s.Provider.NetUserId == appUser.Id);
            Assert.AreEqual(count, 1);

            var result = serviceController.DeleteService(service.Id);

            count = dbContext.ServiceSet.Count(s => s.Provider.NetUserId == appUser.Id);
            Assert.AreEqual(count, 0);

            CleanupRegion(region.Id);
            CleanupUser(appUser.Id);
        }

        [TestMethod]
        public void UpdateServiceTest()
        {
            SetUpTest();

            var appUser = CreateTestProvider();
            var region = CreateTestRegion();
            var service = CreateTestService(region, appUser);
            var username = appUser.UserName;

            var serviceController = new ServicesController(dbContext, appContext);

            serviceController.RequestContext.Principal = new GenericPrincipal(new GenericIdentity(username), null);

            var newDescription = "This is different description";
            var newTitle = "This is new title";

            var editedService = new ServiceApi()
            {
                Description = newDescription,
                Title = newTitle,
                Price = service.Price,
                RegionName = service.Region.Name,
                Id = service.Id
            };

            var result = serviceController.PutService(editedService);

            service = dbContext.ServiceSet.FirstOrDefault(s => s.Id == service.Id);

            Assert.AreEqual(service.Description, newDescription);
            Assert.AreEqual(service.Title, newTitle);

            CleanupService(service.Id);
            CleanupRegion(region.Id);
            CleanupUser(appUser.Id);
        }

        [TestMethod]
        public void AddServiceTest()
        {
            SetUpTest();

            var appUser = CreateTestProvider();
            var username = appUser.UserName;
            var region = CreateTestRegion();

            var serviceController = new ServicesController(dbContext, appContext);

            serviceController.RequestContext.Principal = new GenericPrincipal(new GenericIdentity(username), null);

            var service = new ServiceApi()
            {
                Title = "This is title",
                Description = "This is description",
                Price = 40.00M,
                RegionName = region.Name
            };

            var result = serviceController.PostService(service);

            var count = dbContext.ServiceSet.Count(s => s.Provider.NetUserId == appUser.Id);
            var serviceDb = dbContext.ServiceSet.Where(s => s.Provider.NetUserId == appUser.Id).ToArray()[0];

            Assert.AreEqual(count, 1);
            Assert.AreEqual(serviceDb.Title, service.Title);
            Assert.AreEqual(serviceDb.Description, service.Description);
            Assert.AreEqual(serviceDb.Price, service.Price);
            Assert.AreEqual(serviceDb.Region.Name, service.RegionName);
            Assert.AreEqual(serviceDb.Provider.NetUserId, appUser.Id);

            CleanupService(serviceDb.Id);
            CleanupRegion(region.Id);
            CleanupUser(appUser.Id);
        }

        [TestMethod]
        public void AcceptService()
        {
            SetUpTest();

            var appUser = CreateTestProvider();
            var customer = CreateTestCustomer();
            var region = CreateTestRegion();
            var service = CreateTestService(region, appUser);
            var serviceChoosed = CreateTestServiceChoosed(service, customer);
            var username = appUser.UserName;

            var serviceController = new ServicesController(dbContext, appContext);

            serviceController.RequestContext.Principal = new GenericPrincipal(new GenericIdentity(username), null);

            Assert.AreEqual(serviceChoosed.Accepted, false);

            var result = serviceController.AcceptService(serviceChoosed.Id);

            serviceChoosed = dbContext.ServiceChoosedSet.FirstOrDefault(s => s.Id == serviceChoosed.Id);

            Assert.AreEqual(serviceChoosed.Accepted, true);

            CleanupServiceChoosed(serviceChoosed.Id);
            CleanupService(service.Id);
            CleanupRegion(region.Id);
            CleanupUser(appUser.Id);
            CleanupUser(customer.Id);
        }

        [TestMethod]
        public void FinishService()
        {
            SetUpTest();

            var appUser = CreateTestProvider();
            var customer = CreateTestCustomer();
            var region = CreateTestRegion();
            var service = CreateTestService(region, appUser);
            var serviceChoosed = CreateTestServiceChoosed(service, customer);
            var username = appUser.UserName;

            var serviceController = new ServicesController(dbContext, appContext);

            serviceController.RequestContext.Principal = new GenericPrincipal(new GenericIdentity(username), null);

            Assert.AreEqual(serviceChoosed.FinishedByProvider, false);

            var result = serviceController.FinishService(serviceChoosed.Id);

            serviceChoosed = dbContext.ServiceChoosedSet.FirstOrDefault(s => s.Id == serviceChoosed.Id);

            Assert.AreEqual(serviceChoosed.FinishedByProvider, true);

            CleanupServiceChoosed(serviceChoosed.Id);
            CleanupService(service.Id);
            CleanupRegion(region.Id);
            CleanupUser(appUser.Id);
            CleanupUser(customer.Id);
        }

        [TestMethod]
        public void GetAllChoosedServicesdMoreThanZeroTest()
        {
            SetUpTest();

            var appUser = CreateTestProvider();
            var customer = CreateTestCustomer();
            var region = CreateTestRegion();
            var service = CreateTestService(region, appUser);
            var serviceChoosed = CreateTestServiceChoosed(service, customer);
            var username = appUser.UserName;

            var serviceController = new ServicesController(dbContext, appContext);

            serviceController.RequestContext.Principal = new GenericPrincipal(new GenericIdentity(username), null);

            var result = serviceController.GetAllChoosedServices();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());

            CleanupServiceChoosed(serviceChoosed.Id);
            CleanupService(service.Id);
            CleanupRegion(region.Id);
            CleanupUser(appUser.Id);
            CleanupUser(customer.Id);
        }

        [TestMethod]
        public void GetAllChoosedServicesWhenZeroTest()
        {
            SetUpTest();

            var appUser = CreateTestProvider();
            var username = appUser.UserName;

            var serviceController = new ServicesController(dbContext, appContext);

            serviceController.RequestContext.Principal = new GenericPrincipal(new GenericIdentity(username), null);

            var result = serviceController.GetAllChoosedServices();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());

            CleanupUser(appUser.Id);
        }

        // Website tests

        [TestMethod]
        public void GetAllServicesWebsiteTest()
        {
            SetUpTest();

            var appUser = CreateTestProvider();
            var region = CreateTestRegion();
            var service = CreateTestService(region, appUser);
            var username = appUser.UserName;

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);
            var serviceController = new WebControllers.ServicesController(dbContext, appContext);

            var result = serviceController.Index() as ViewResult;
            var count = dbContext.ServiceSet.Count();

            Assert.IsNotNull(result);
            Assert.AreEqual((result.ViewData.Model as IEnumerable<Service>).Count(), Math.Min(count, 4));

            CleanupService(service.Id);
            CleanupRegion(region.Id);
            CleanupUser(appUser.Id);
        }

        [TestMethod]
        public void GetServicesWebsiteTest()
        {
            SetUpTest();

            var appUser = CreateTestProvider();
            var region = CreateTestRegion();
            var service = CreateTestService(region, appUser);
            var username = appUser.UserName;

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);
            var serviceController = new WebControllers.ServicesController(dbContext, appContext);

            var result = serviceController.Service(service.Id) as ViewResult;
            var retService = result.ViewData.Model as Service;

            Assert.IsNotNull(result);
            Assert.AreEqual(retService.Price, service.Price);

            CleanupService(service.Id);
            CleanupRegion(region.Id);
            CleanupUser(appUser.Id);
        }

        [TestMethod]
        public void OrderSummaryServicesWebsiteTest()
        {
            SetUpTest();

            var appUser = CreateTestProvider();
            var customer = CreateTestCustomer();
            var region = CreateTestRegion();
            var service = CreateTestService(region, appUser);
            var username = appUser.UserName;

            var serviceController = new WebControllers.ServicesController(dbContext, appContext);
            var note = "This is sample note";

            var result = serviceController.OrderSummary(service.Id, note, new GenericIdentity(customer.UserName)) as JsonResult;
            dynamic dresult = result.Data;

            Assert.IsNotNull(result);
            Assert.AreEqual("TestProvider", service.Provider.FirstName);
            Assert.AreEqual("Test", service.Provider.SecondName);
            Assert.AreEqual("TestRegion", service.Region.Name);

            CleanupService(service.Id);
            CleanupRegion(region.Id);
            CleanupUser(appUser.Id);
            CleanupUser(customer.Id);
        }
    }
}