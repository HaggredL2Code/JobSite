using Microsoft.Owin;
using Owin;

using NLog;

[assembly: OwinStartupAttribute(typeof(JobPosting.Startup))]
namespace JobPosting
{
    public partial class Startup
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        public void Configuration(IAppBuilder app)
        {
            logger.Info("Starting application.");
            //app.MapSignalR();
            ConfigureAuth(app);
            
        }
    }
}