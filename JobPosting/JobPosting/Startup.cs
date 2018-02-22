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
            // NLog demo
            Logger logger = LogManager.GetLogger("DemoLogger");
            logger.Trace("Sample trace message");
            logger.Debug("Sample debug message");
            logger.Info("Sample informational message");
            logger.Warn("Sample warning message");
            logger.Error("Sample error message");
            logger.Fatal("Sample fatal error message");
            // ...
            ConfigureAuth(app);
        }
    }
}
