using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_server.Model;

namespace web_server.Helpers
{
    public class ProviderRateHelper
    {
        public static double? GetRate(Provider provider)
        {
            DatabaseModelContainer db = new DatabaseModelContainer();

            var avg = db.ServiceChoosedSet.Where(s => s.FinishedByCustomer && s.FinishedByProvider && s.Rate != null).Select(s => (int)s.Rate.Mark).Average();

            return avg;
        }

        public static int GetCount(Provider provider)
        {
            DatabaseModelContainer db = new DatabaseModelContainer();

            var count = db.ServiceChoosedSet.Where(s => s.FinishedByCustomer && s.FinishedByProvider && s.Rate != null).Select(s => (int)s.Rate.Mark).Count();

            return count;
        }
    }
}