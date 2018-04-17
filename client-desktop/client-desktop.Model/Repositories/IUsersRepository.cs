using System;
using System.Collections.Generic;
using client_desktop.Model;

namespace client_desktop.Model.Repositories
{
    public interface IUsersRepository : IDisposable
    {
        IEnumerable<User> GetUsers();
        User GetUserById(int userId);
        void InsertUser(User user);
        void DeleteUserById(int userId);
        void UpdateUser(User user);
        void Save();
    }
}
