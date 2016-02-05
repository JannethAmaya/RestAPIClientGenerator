using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RestAPIClientGenerator.Startup))]
namespace RestAPIClientGenerator
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
