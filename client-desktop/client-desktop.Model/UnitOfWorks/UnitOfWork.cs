using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using client_desktop.Model;
using client_desktop.Model.Repositories;

namespace client_desktop.Model.UnitOfWorks
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private DatabaseModelContainer context = new DatabaseModelContainer();
        private UsersRepository usersRepository;

        public IUsersRepository UserRepository
        {
            get
            {
                if (this.usersRepository == null)
                {
                    this.usersRepository = new UsersRepository(context);
                }

                return usersRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}