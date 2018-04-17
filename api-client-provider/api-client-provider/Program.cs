using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using api_client_provider.Controllers;
using api_client_provider.Models;

// This application is example how to use API for provider
// Uses example user

namespace api_client_provider
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new ServicesApi();

            var rand = new Random();
            var num = rand.Next();
            
            // Adding service
            client.AddService(new Service()
            {
                Title = "Title of the service " + num.ToString(),
                Description = "Short Description of service",
                Price = 29.99M,
                RegionName = "Warsaw"
            });

            // List all services
            var services = ListServices(client);

            // UpdateService
            var edited_service = services[0];
            edited_service.Title = "Edited service";
            client.UpdateService(edited_service);

            ListServices(client);

            // Delete service
            client.DeleteService(services[0].Id);

            ListServices(client);
           
            var choosed = ListChoosed(client);

            client.AcceptService(choosed[0].Id);
            client.FinishService(choosed[0].Id);

            Console.ReadKey();
        }

        private static Service[] ListServices(ServicesApi client)
        {
            var services = client.GetServices();
            Console.WriteLine("My services");
            foreach (var srv in services)
            {
                Console.WriteLine(srv.Title);
                Console.WriteLine(srv.Description);
                Console.WriteLine(srv.Price.ToString());
                Console.WriteLine(srv.RegionName);
                Console.WriteLine();
            }

            return services;
        }

        private static ServiceChoosed[] ListChoosed(ServicesApi client)
        {
            var services = client.GetChoosedServices();
            Console.WriteLine("Active services");
            foreach (var srv in services)
            {
                Console.WriteLine(srv.Title);
                Console.WriteLine(srv.Description);
                Console.WriteLine(srv.Price);
                Console.WriteLine(srv.RegionName);
                Console.WriteLine(srv.ClientName);
                Console.WriteLine(srv.ClientPhone);
                Console.WriteLine(srv.Accepted);
                Console.WriteLine(srv.FinishedByProvider);
                Console.WriteLine(srv.FinishedByClient);
                Console.WriteLine();
            }

            return services;
        }
    }
}
