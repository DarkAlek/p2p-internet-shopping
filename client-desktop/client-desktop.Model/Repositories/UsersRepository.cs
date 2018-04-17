using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using client_desktop.Model;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace client_desktop.Model.Repositories
{
    public class UsersRepository : IUsersRepository, IDisposable
    {
        private DatabaseModelContainer context;

        public UsersRepository(DatabaseModelContainer dbContainer)
        {
            context = dbContainer;
        }

        public IEnumerable<User> GetUsers()
        {
            return context.UserSet.ToList();
        }

        public User GetUserById(int userId)
        {
            return context.UserSet.Find(userId);
        }

        public void InsertUser(User user)
        {
            context.UserSet.Add(user);
        }

        public void DeleteUserById(int userId)
        {
            User user = context.UserSet.Find(userId);
            context.UserSet.Remove(user);
        }

        public void UpdateUser(User user)
        {
            context.Entry(user).State = EntityState.Modified;
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
