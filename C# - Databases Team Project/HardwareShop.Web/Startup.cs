using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HardwareShop.Web.Startup))]

namespace HardwareShop.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}