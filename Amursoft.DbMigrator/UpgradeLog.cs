using DbUp.Engine.Output;
using Microsoft.Extensions.Logging;

namespace Amursoft.DbMigrator
{
    public class UpgradeLog:IUpgradeLog
    {
        private readonly ILogger logger;

        public UpgradeLog(ILogger logger)
        {
            this.logger = logger;
        }
        
        public void WriteInformation(string format, params object[] args)
        {
            logger.LogInformation(format,args);
        }

        public void WriteError(string format, params object[] args)
        {
            logger.LogError(format, args);
        }

        public void WriteWarning(string format, params object[] args)
        {
            logger.LogWarning(format, args);
        }
    }
}