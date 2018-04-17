using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using web_server.Tests.Mocks;
using web_server.Controllers;
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
using web_server.web_api.Controllers;

namespace web_server.Tests.Controllers
{
    [TestClass]
    public class UsersControllerTest
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

        private void CleanupUser(string id)
        {
            var user = dbContext.UserSet.FirstOrDefault(u => u.NetUserId == id);
            dbContext.UserSet.Remove(user);
            dbContext.SaveChanges();

            var userApp = appContext.Users.FirstOrDefault(u => u.Id == id);
            appContext.Users.Remove(userApp);
            appContext.SaveChanges();
        }

        [TestMethod]
        public void GetAllUsersTest()
        {
            SetUpTest();

            var appUser = CreateTestAdmin();
            var username = appUser.UserName;

            var usersController = new UsersController(dbContext, appContext);
            usersController.RequestContext.Principal = new GenericPrincipal(new GenericIdentity(username), null);

            var usersApp = appContext.Users.ToArray();
            var usersDb = dbContext.UserSet.ToArray();
            var complete_users = from usera in usersApp
                                 join userd in usersDb on usera.Id equals userd.NetUserId
                                 select new UserApi
                                 {
                                     Id = usera.Id,
                                     UserName = usera.UserName,
                                     Email = usera.Email,
                                     PhoneNumber = usera.PhoneNumber,
                                 };

            var result = usersController.GetAllUsers();

            Assert.IsNotNull(result);
            Assert.AreEqual(complete_users.Count(), result.Count());
        }

        [TestMethod]
        public void GetUserByIdTest()
        {
            SetUpTest();

            var appUser = CreateTestAdmin();
            var username = appUser.UserName;

            var usersController = new UsersController(dbContext, appContext);
            usersController.RequestContext.Principal = new GenericPrincipal(new GenericIdentity(username), null);
            var result = usersController.GetUserById(appUser.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.UserName, username);

            CleanupUser(appUser.Id);
        }

        [TestMethod]
        public void DeleteUserTest()
        {
            SetUpTest();

            var appUser = CreateTestAdmin();
            var username = appUser.UserName;

            var usersController = new UsersController(dbContext, appContext);
            usersController.RequestContext.Principal = new GenericPrincipal(new GenericIdentity(username), null);

            var count_before = dbContext.UserSet.Count();
            var result = usersController.DeleteUser(appUser.Id);
            var count_after = dbContext.UserSet.Count();

            Assert.IsNotNull(result);
            Assert.AreEqual(count_before - 1, count_after);
        }

        [TestMethod]
        public void PostUserCustomerTest()
        {
            SetUpTest();

            var appUser = CreateTestAdmin();
            var username = appUser.UserName;

            var usersController = new UsersController(dbContext, appContext);
            usersController.RequestContext.Principal = new GenericPrincipal(new GenericIdentity(username), null);

            var countBefore = dbContext.UserSet.Count();

            var newUser = new UserApi()
            {
                Email = "testmethodemail@email.email",
                Password = "S3cretPasswod$",
                PhoneNumber = "127127127",
                UserName = "VeryTestUsernameMethod",
                Type = 0
            };

            var result = usersController.PostUser(newUser);
            var countAfter = dbContext.UserSet.Count();

            Assert.IsNotNull(result);
            Assert.AreEqual(countBefore + 1, countAfter);

            CleanupUser(appUser.Id);

            var appNewUser = appContext.Users.FirstOrDefault(u => u.UserName == newUser.UserName);
            CleanupUser(appNewUser.Id);
        }

        [TestMethod]
        public void PostUserProviderTest()
        {
            SetUpTest();

            var appUser = CreateTestAdmin();
            var username = appUser.UserName;

            var usersController = new UsersController(dbContext, appContext);
            usersController.RequestContext.Principal = new GenericPrincipal(new GenericIdentity(username), null);

            var countBefore = dbContext.UserSet.Count();

            var newUser = new UserApi()
            {
                Email = "testmethodemail@email.email",
                Password = "S3cretPasswod$",
                PhoneNumber = "127127127",
                UserName = "VeryTestUsernameMethod",
                Type = 1
            };

            var result = usersController.PostUser(newUser);
            var countAfter = dbContext.UserSet.Count();

            Assert.IsNotNull(result);
            Assert.AreEqual(countBefore + 1, countAfter);

            CleanupUser(appUser.Id);

            var appNewUser = appContext.Users.FirstOrDefault(u => u.UserName == newUser.UserName);
            CleanupUser(appNewUser.Id);
        }

        [TestMethod]
        public void PostUserAdminTest()
        {
            SetUpTest();

            var appUser = CreateTestAdmin();
            var username = appUser.UserName;

            var usersController = new UsersController(dbContext, appContext);
            usersController.RequestContext.Principal = new GenericPrincipal(new GenericIdentity(username), null);

            var countBefore = dbContext.UserSet.Count();

            var newUser = new UserApi()
            {
                Email = "testmethodemail@email.email",
                Password = "S3cretPasswod$",
                PhoneNumber = "127127127",
                UserName = "VeryTestUsernameMethod",
                Type = 2
            };

            var result = usersController.PostUser(newUser);
            var countAfter = dbContext.UserSet.Count();

            Assert.IsNotNull(result);
            Assert.AreEqual(countBefore + 1, countAfter);

            CleanupUser(appUser.Id);

            var appNewUser = appContext.Users.FirstOrDefault(u => u.UserName == newUser.UserName);
            CleanupUser(appNewUser.Id);
        }

        [TestMethod]
        public void PutUserTest()
        {
            SetUpTest();

            var appUser = CreateTestAdmin();
            var username = appUser.UserName;

            var usersController = new UsersController(dbContext, appContext);
            usersController.RequestContext.Principal = new GenericPrincipal(new GenericIdentity(username), null);

            var editedUser = new UserApi()
            {
                Id = appUser.Id,
                Email = appUser.Email,
                PhoneNumber = appUser.PhoneNumber,
                UserName = "TomaszProvider"
            };

            var result = usersController.PutUser(editedUser);
            var user = appContext.Users.FirstOrDefault(u => u.Id == editedUser.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(editedUser.UserName, user.UserName);

            CleanupUser(appUser.Id);
        }
    }
}
