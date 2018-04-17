using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using web_server.Models;
using web_server.Models.WebApi;
using web_server.Model;
using web_server.Filters;

namespace web_server.web_api.Controllers
{
    public class UsersController : ApiController
    {
        DatabaseModelContainer db;
        ApplicationDbContext context;

        public UsersController() : this(null, null)
        {
            // Default constructor
        }

        public UsersController(DatabaseModelContainer _db = null, ApplicationDbContext _cntxt = null)
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

        // POST /api/users/
        [AdminApiAuthentication]
        public bool PostUser(UserApi user)
        {
            try
            {
                var appUser = new ApplicationUser
                {
                    Email = user.Email,
                    EmailConfirmed = true,
                    UserName = user.UserName,
                    PasswordHash = new PasswordHasher().HashPassword(user.Password),
                    PhoneNumber = user.PhoneNumber,
                    SecurityStamp = "3236c4f3-b5c1-4f55-aa81-a10fd2dfcece"  // should be randomized
                };

                context.Users.Add(appUser);
                context.SaveChanges();

                if (user.Type == 0)
                {
                    var newCustomer = new Customer()
                    {
                        NetUserId = appUser.Id,
                        Activated = true,
                        PhoneNumber = "127127127",
                        FirstName = "",
                        SecondName = ""
                    };

                    db.UserSet.Add(newCustomer);
                }
                else if (user.Type == 1)
                {
                    var newProvider = new Provider()
                    {
                        NetUserId = appUser.Id,
                        Activated = true,
                        PhoneNumber = "127127127",
                        FirstName = "",
                        SecondName = ""
                    };

                    db.UserSet.Add(newProvider);
                }
                else if (user.Type == 2)
                {
                    var newAdmin = new Admin()
                    {
                        NetUserId = appUser.Id,
                        Activated = true,
                        FirstName = "",
                        SecondName = ""
                    };

                    db.UserSet.Add(newAdmin);
                }

            }
            catch
            {
                return false;
            }

            db.SaveChanges();

            return true;
        }

        // PUT /api/users/id
        [AdminApiAuthentication]
        public bool PutUser(UserApi user)
        {
            try
            {
                var appUser = context.Users.FirstOrDefault(u => u.Id == user.Id);
                var appUser2 = db.UserSet.FirstOrDefault(u => u.NetUserId == appUser.Id);

                appUser.Email = user.Email;
                appUser.EmailConfirmed = true;
                appUser.UserName = user.UserName;
                appUser.PhoneNumber = user.PhoneNumber;

                context.SaveChanges();

                if (appUser2 is Provider)
                    (appUser2 as Provider).PhoneNumber = user.PhoneNumber;
                else if (appUser2 is Customer)
                    (appUser2 as Customer).PhoneNumber = user.PhoneNumber;

                db.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        // DELETE /api/users/d02dcd10-2c5d-4c6a-a5ad-5b79679755ad
        [AdminApiAuthentication]
        public bool DeleteUser(string id)
        {
            var user = context.Users.FirstOrDefault(u => u.Id == id);
            var userApp = db.UserSet.FirstOrDefault(u => u.NetUserId == user.Id);

            if (user == null || userApp == null)
                return false;

            bool success = false;
            
            try
            {
                user.Roles.Clear();
                context.Users.Remove(user);
                context.Entry(user).State = System.Data.Entity.EntityState.Deleted; // should be removed later... i think
                context.SaveChanges();

                db.UserSet.Remove(userApp);
                db.SaveChanges();

                success = true;
            }
            catch
            {
                success = false;
            }

            return success;
        }

        // GET /api/users/d02dcd10-2c5d-4c6a-a5ad-5b79679755ad
        [AdminApiAuthentication]
        public UserApi GetUserById(string id)
        {
            var user = context.Users.FirstOrDefault(u => u.Id == id);
            var userApp = db.UserSet.FirstOrDefault(u => u.NetUserId == user.Id);

            var userApi = new UserApi
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Type = GetTypeByUser(userApp)
            };

            return userApi;
        }

        // GET /api/users
        [AdminApiAuthentication]
        public UserApi[] GetAllUsers()
        {
            var users = context.Users.Select(
                u => new UserApi
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                }
            ).ToArray();

            var types = db.UserSet.AsEnumerable().Select(
                u => new UserApi
                {
                    Type = GetTypeByUser(u),
                    Id = u.NetUserId
                }
            ).ToArray();

            var complete_users = from user in users
                                 join type in types on user.Id equals type.Id
                                 select new UserApi
                                 {
                                     Id = user.Id,
                                     UserName = user.UserName,
                                     Email = user.Email,
                                     PhoneNumber = user.PhoneNumber,
                                     Type = type.Type
                                 };

            return complete_users.ToArray();
        }

        private int GetTypeByUser(User user)
        {
            if (user is Customer)
                return 0;
            else if (user is Provider)
                return 1;
            else if (user is Admin)
                return 2;

            return -1;
        }
    }
}
