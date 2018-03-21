using Microsoft.Owin;
using Owin;

using NLog;

[assembly: OwinStartupAttribute(typeof(JobPosting.Startup))]
namespace JobPosting
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Logger logger = LogManager.GetLogger("Startup");
            logger.Info("Starting application.");

            //app.MapSignalR();
            ConfigureAuth(app);
            
        }
    }
}