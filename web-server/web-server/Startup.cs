using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(web_server.Startup))]
namespace web_server
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
