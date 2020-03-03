using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnlineRetail.WebUI.Startup))]
namespace OnlineRetail.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
