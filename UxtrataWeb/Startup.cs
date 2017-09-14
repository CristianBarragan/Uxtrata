using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UxtrataWeb.Startup))]
namespace UxtrataWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
