using System;
using System.Linq;
using web_server.Model;

namespace web_server.Tests.Mocks
{
    class TestServiceDbSet : TestDbSet<Service>
    {
        public override Service Find(params object[] keyValues)
        {
            return this.SingleOrDefault(product => product.Id == (int)keyValues.Single());
        }
    }
}