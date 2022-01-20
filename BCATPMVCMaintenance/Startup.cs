using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BCATPMVCMaintenance.Startup))]
namespace BCATPMVCMaintenance
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
//test