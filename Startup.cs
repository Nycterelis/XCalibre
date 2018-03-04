using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(XCalibre.Startup))]
namespace XCalibre
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
