using System.Web.Mvc;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SportLigue.MainApp.Startup))]
namespace SportLigue.MainApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
