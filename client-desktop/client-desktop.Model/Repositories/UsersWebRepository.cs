using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using client_desktop.Model.Models;


namespace client_desktop.Model.Repositories
{
    public class UsersWebRepository : IUsersWebRepository, IDisposable
    {
        private HttpClient context;

        public UsersWebRepository(HttpClient client)
        {
            context = client;
        }

        public async Task<IEnumerable<UserApi>> GetUsersApi()
        {

            try
            {
                HttpResponseMessage response =  context.GetAsync("api/users").Result;
                response.EnsureSuccessStatusCode();

                var usersapi = await response.Content.ReadAsAsync<IEnumerable<UserApi>>();
                return usersapi;

            }
            catch(Exception)
            {
                return null;
                // throw new HttpRequestException();
            }

        }

        public async Task DeleteUserApiById(string userapiId)
        {

            try
            {
                var url = "api/users/" + userapiId.TrimStart();
                HttpResponseMessage response = await context.DeleteAsync(url);
                response.EnsureSuccessStatusCode(); // to check
            }
            catch (Exception)
            {
                // throw new HttpRequestException();
            }
        }

        public async Task InsertUserApi(UserApi userapi)
        {

            try
            {
                HttpResponseMessage response = await context.PostAsJsonAsync("api/users", userapi);
                response.EnsureSuccessStatusCode(); // to check
            }
            catch (Exception)
            {
                // throw new HttpRequestException();
            }
        }

        // TO CHECK
        public async Task UpdateUserApi(UserApi userapi)
        {

            try
            {
                var url = "api/users/" + userapi.Id.TrimStart();
                HttpResponseMessage response = await context.PutAsJsonAsync(url, userapi);
                response.EnsureSuccessStatusCode();  // to check

            }
            catch (Exception)
            {
                // throw new HttpRequestException();
            }
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
