using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JobPosting.Startup))]
namespace JobPosting
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
