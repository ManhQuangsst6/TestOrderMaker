using NLog;
using System;
using System.Text;

namespace TestOrderMaker.Common
{
    public class Logger
    {
        // singleton
        private static Logger logger;
        private static readonly NLog.Logger nlog = LogManager.GetCurrentClassLogger();
        private StringBuilder buffer;

        private string output_path;
        private LOG_LEVEL level;

        public enum LOG_LEVEL
        {
            DEBUG = 1,
            INFO = 3,
            WARNING = 6,
            ERROR = 8,
            FATAL = 9,
        };

        private Logger(string output_path, LOG_LEVEL level)
        {
            this.output_path = output_path;
            this.level = level;
            this.buffer = new StringBuilder();
        }

        private static void init(string output_path, LOG_LEVEL level)
        {
            logger = new Logger(output_path, level);
        }

        public static Logger getInstance()
        {
            if (logger == null)
            {
                init("your_log_path_here", LOG_LEVEL.INFO);
            }
            return logger;
        }

        private void writeLog(string level, string text)
        {
            // Use NLog to log the message
            if (level == "【Debug】" && this.level <= LOG_LEVEL.DEBUG)
                nlog.Debug(text);
            else if (level == "【Info】" && this.level <= LOG_LEVEL.INFO)
                nlog.Info(text);
            else if (level == "【Warning】" && this.level <= LOG_LEVEL.WARNING)
                nlog.Warn(text);
            else if (level == "【Error】" && this.level <= LOG_LEVEL.ERROR)
                nlog.Error(text);
            else if (level == "【Fatal】")
                nlog.Fatal(text);

            // You can also choose to append to buffer
            buffer.Append(text + Environment.NewLine);
        }

        public void logDebug(string text)
        {
            writeLog("【Debug】", text);
        }

        public void logInfo(string text)
        {
            writeLog("【Info】", text);
        }

        public void logWarn(string text)
        {
            writeLog("【Warning】", text);
        }

        public void logError(string text)
        {
            writeLog("【Error】", text);
        }

        public void logFatal(string text)
        {
            writeLog("【Fatal】", text);
        }

        public void flush()
        {
            // Make sure to flush NLog as well
            LogManager.Flush();
        }

        public void appendBuffer(string str)
        {
            buffer.Append(str);
        }

        public string getBuffer()
        {
            return buffer.ToString();
        }

        public void clearBuffer()
        {
            buffer.Clear();
        }
    }
}
