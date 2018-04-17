using System;
using System.Data.Entity;
using web_server.Model;

namespace web_server.Tests.Mocks
{
    public class TestDbContext : DatabaseModelContainer
    {
        public TestDbContext()
        {
            this.Services = new TestServiceDbSet();
        }

        public DbSet<Service> Services { get; set; }

        public override int SaveChanges()
        {
            return 0;
        }

        public void MarkAsModified(Service item) { }
        public new void Dispose() { }
    }
}