using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using api_client_provider;

namespace api_client_provider.Controllers
{
    public class ApiClient
    {
        public HttpClient client;

        public ApiClient()
        {
            client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:50747");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var authValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes("testprovider@mail.pl:S3cretPassword$")));
            client.DefaultRequestHeaders.Authorization = authValue;
        }
    }
}
