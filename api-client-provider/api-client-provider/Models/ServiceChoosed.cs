using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api_client_provider.Models
{
    public class ServiceChoosed
    {
        public int Id;
        public string Description;
        public string Title;
        public decimal Price;
        public string RegionName;
        public bool Accepted;
        public string Mark;
        public string ClientName;
        public string ClientPhone;
        public bool FinishedByClient;
        public bool FinishedByProvider;
    }
}
