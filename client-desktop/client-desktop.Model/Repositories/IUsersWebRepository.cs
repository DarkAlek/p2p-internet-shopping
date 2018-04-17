using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using client_desktop.Model.Models;

namespace client_desktop.Model.Repositories
{
    public interface IUsersWebRepository : IDisposable
    {
        Task<IEnumerable<UserApi>> GetUsersApi();
        //UserApi GetUserApiById(int userapiId);
        Task InsertUserApi(UserApi userapi);
        Task DeleteUserApiById(string userapiId);
        Task UpdateUserApi(UserApi userapi);
        //void Save();
    }
}
