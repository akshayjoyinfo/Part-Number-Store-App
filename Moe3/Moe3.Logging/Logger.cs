using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Moe3.Logging
{
    public enum LogLevel : int
    {
        DEBUG = 0,
        INFO = 1,
        WARN = 2,
        ERROR = 3,
        FATAL = 4
    }

    public enum LogModes : int
    {
        UI = 0,
        REPO = 1,
        ERROR = 2
    }

    public class Logger
    {
        public void LogMsg(LogModes mode, LogLevel level, object msg)
        {
            string strLogFileName = String.Empty;

            switch (mode)
            {
                case LogModes.UI:
                    strLogFileName = "Moe3.UI.log";
                    break;

                case LogModes.REPO:
                    strLogFileName = "Moe3.Repository.log";
                    break;

                case LogModes.ERROR:
                    strLogFileName = "Moe3.Error.log";
                    break;
            }

            log4net.GlobalContext.Properties["LogFileName"] = strLogFileName;
            log4net.Config.XmlConfigurator.Configure();

            ILog log = LogManager.GetLogger(typeof(Logger));

            switch (level)
            {
                case LogLevel.DEBUG:
                    log.Debug(msg);
                    break;

                case LogLevel.INFO:
                    log.Info(msg);
                    break;

                case LogLevel.WARN:
                    log.Warn(msg);
                    break;

                case LogLevel.ERROR:
                    log.Error(msg);
                    break;

                case LogLevel.FATAL:
                    log.Fatal(msg);
                    break;
            }
        }
    }
}
