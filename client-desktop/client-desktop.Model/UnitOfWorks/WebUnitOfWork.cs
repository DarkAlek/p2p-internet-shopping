using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using client_desktop.Model.Repositories;
using System.Net.Http;
using System.Net.Http.Headers;
using client_desktop;
using client_desktop.Model.Models;

namespace client_desktop.Model.UnitOfWorks
{
    public class WebUnitOfWork : IDisposable, IWebUnitOfWork
    {
        private HttpClient client = new HttpClient();
        private UsersWebRepository usersWebRepository;

        public IUsersWebRepository UserWebRepository
        {
            get
            {
                if (this.usersWebRepository == null)
                {
                    client.BaseAddress = new Uri("http://localhost:50747");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var authValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(DataContainer.Instance.Login.Trim() + ":" + DataContainer.Instance.Password.Trim())));
                    client.DefaultRequestHeaders.Authorization = authValue;

                    this.usersWebRepository = new UsersWebRepository(client);
                }

                return usersWebRepository;
            }
        }

        public void Save()
        {

        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    client.Dispose();
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
