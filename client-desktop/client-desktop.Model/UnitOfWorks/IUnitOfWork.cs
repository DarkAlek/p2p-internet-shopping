using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using client_desktop.Model;
using client_desktop.Model.Repositories;

namespace client_desktop.Model.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        IUsersRepository UserRepository { get; }
        void Save();
    }
}
