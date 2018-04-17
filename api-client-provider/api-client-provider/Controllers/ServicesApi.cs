using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using api_client_provider.Models;

namespace api_client_provider.Controllers
{
    public class ServicesApi
    {
        private ApiClient apiClient;

        public ServicesApi()
        {
            apiClient = new ApiClient();
        }

        public Service[] GetServices()
        {
            try
            {
                HttpResponseMessage response = apiClient.client.GetAsync("api/services").Result;
                response.EnsureSuccessStatusCode();

                var services = response.Content.ReadAsAsync<IEnumerable<Service>>().Result.ToArray();

                return services;
            }
            catch (Exception)
            {
                throw new HttpRequestException();
            }
        }

        public bool DeleteService(int id)
        {
            try
            {
                HttpResponseMessage response = apiClient.client.DeleteAsync("api/services/" + id.ToString()).Result;
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception)
            {
                throw new HttpRequestException();
            }
        }

        public bool AddService(Service service)
        {
            try
            {
                HttpResponseMessage response = apiClient.client.PostAsJsonAsync("api/services", service).Result;
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception)
            {
                throw new HttpRequestException();
            }
        }

        public bool UpdateService(Service service)
        {
            try
            {
                var url = "api/services/" + service.Id.ToString();
                HttpResponseMessage response = apiClient.client.PutAsJsonAsync(url, service).Result;
                response.EnsureSuccessStatusCode();  // to check

                return true;
            }
            catch (Exception)
            {
                throw new HttpRequestException();
            }
        }

        public ServiceChoosed[] GetChoosedServices()
        {
            try
            {
                HttpResponseMessage response = apiClient.client.GetAsync("api/choosed").Result;
                response.EnsureSuccessStatusCode();

                var services = response.Content.ReadAsAsync<IEnumerable<ServiceChoosed>>().Result.ToArray();

                return services;
            }
            catch (Exception)
            {
                throw new HttpRequestException();
            }
        }

        public bool AcceptService(int id)
        {
            try
            {
                HttpResponseMessage response = apiClient.client.GetAsync("api/choosed/accept/" + id.ToString()).Result;
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception)
            {
                throw new HttpRequestException();
            }
        }

        public bool FinishService(int id)
        {
            try
            {
                HttpResponseMessage response = apiClient.client.GetAsync("api/choosed/finish/" + id.ToString()).Result;
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception)
            {
                throw new HttpRequestException();
            }
        }
    }
}
