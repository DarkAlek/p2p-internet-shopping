using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_server.Models.WebApi
{
    public class ServiceChoosedApi
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