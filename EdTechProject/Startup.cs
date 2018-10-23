using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EdTechProject.Startup))]
namespace EdTechProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
