using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using client_desktop.Model.Models;
using client_desktop.Model.UnitOfWorks;
using Moq;


namespace UnitTests
{
    [TestFixture]
    public class UserVewModelTests
    {
        UsersWebViewModel viewContext;
        Mock<IWebUnitOfWork> databaseMock;

        private List<UserApi> Init()
        {
            databaseMock = MockWebUnitOfWork();

            List<UserApi> users = new List<UserApi>();

            viewContext = new UsersWebViewModel(databaseMock.Object);

            // user updating
            databaseMock.Setup(x => x.UserWebRepository.UpdateUserApi(It.IsAny<UserApi>()))
                .Callback((UserApi user) => users[users.FindIndex(u => u.Id == user.Id)] = user);

            // user inserting
            databaseMock.Setup(x => x.UserWebRepository.InsertUserApi(It.IsAny<UserApi>()))
                .Callback((UserApi user) => users.Add(user));

            // list users
            databaseMock.Setup(x => x.UserWebRepository.GetUsersApi()).Returns(Task.FromResult(users as IEnumerable<UserApi>));

            // delete user
            databaseMock.Setup(x => x.UserWebRepository.DeleteUserApiById(It.IsAny<string>()))
                .Callback((string id) => users.RemoveAll(u => int.Parse(u.Id) == int.Parse(id)));

            return users;
        }

        private Mock<IWebUnitOfWork> MockWebUnitOfWork()
        {
            Mock<IWebUnitOfWork> mock = new Mock<IWebUnitOfWork>();

            return mock;
        }

        [Test]
        public void UsersReadTest()
        {
            var users = Init();

            users.Add(new UserApi()
            {
                Id = "3",
                Email = "fakestudent@student.mini.pw.edu.pl",
                Password = "S3cretPassword$"
            });

            Assert.IsTrue(databaseMock.Object.UserWebRepository.GetUsersApi().Result.ToList().Count() == 1);
        }

        [Test]
        public void AddUserTest()
        {
            Init();
            List<UserApi> users = new List<UserApi>();

            viewContext = new UsersWebViewModel(databaseMock.Object);
            databaseMock.Setup(x => x.UserWebRepository.UpdateUserApi(It.IsAny<UserApi>()))
                .Callback((UserApi user) => users.Add(user));
            databaseMock.Setup(x => x.UserWebRepository.GetUsersApi()).Returns(Task.FromResult(users as IEnumerable<UserApi>));
            viewContext.SelectedUser = new UserApi();

            viewContext.SaveUser();

            Assert.IsTrue(databaseMock.Object.UserWebRepository.GetUsersApi().Result.ToList().Count() == 1);
        }

        [Test]
        public void UpdateUserDataTest()
        {
            var users = Init();

            users.Add(new UserApi() {
                Id = "3",
                Email = "fakestudent@student.mini.pw.edu.pl",
                Password = "S3cretPassword$"
            });

            var userUpdated = new UserApi()
            {
                Id = "3",
                Email = "fakestudent@student.mini.pw.edu.pl",
                Password = "S3cretPassword$"
            };

            viewContext.SelectedUser = userUpdated;

            userUpdated.Email = "fakestudent1@student.mini.pw.edu.pl";
            viewContext.SaveUser();
            Assert.IsTrue(databaseMock.Object.UserWebRepository.GetUsersApi().Result.ToList().First().Email == "fakestudent1@student.mini.pw.edu.pl");

            userUpdated.Password = "S3cretPassword";
            viewContext.SaveUser();
            Assert.IsTrue(databaseMock.Object.UserWebRepository.GetUsersApi().Result.ToList().First().Password == "S3cretPassword");

        }

        [Test]
        public void DeleteUserTest()
        {
            var users = Init();
            users.Add(new UserApi()
            {
                Id = "3",
                Email = "fakestudent@student.mini.pw.edu.pl",
                Password = "S3cretPassword$"
            });

            var userToDelete = users[0];

            viewContext.SelectedUser = userToDelete;
            viewContext.DeleteUser();

            Assert.IsTrue(databaseMock.Object.UserWebRepository.GetUsersApi().Result.ToList().Count() == 0);
        }
    }
}
