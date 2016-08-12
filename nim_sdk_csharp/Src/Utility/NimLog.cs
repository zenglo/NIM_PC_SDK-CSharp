using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NimUtility
{
    public class NimLog
    {
        private readonly log4net.ILog _log;

        public NimLog(string loggerName)
        {
            _log = log4net.LogManager.GetLogger(loggerName);
        }

        public void Debug(string msg)
        {
            if (_log.IsDebugEnabled)
                _log.Debug(msg);
        }

        public void Warn(string msg)
        {
            if (_log.IsWarnEnabled)
                _log.Warn(msg);
        }

        public void Error(string msg)
        {
            if (_log.IsErrorEnabled)
                _log.Error(msg);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            if (_log.IsErrorEnabled)
                _log.ErrorFormat(format, args);
        }

        public void Info(string msg)
        {
            if (_log.IsInfoEnabled)
                _log.Info(msg);
        }

        public void InfoFormat(string format,params object[] args)
        {
            if (_log.IsInfoEnabled)
                _log.InfoFormat(format, args);
        }
    }

    public class NimLogManager
    {
        static NimLogManager()
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            var stream = asm.GetManifestResourceStream("NimUtility.log4net.xml");
            if (stream != null)
                log4net.Config.XmlConfigurator.Configure(stream);
        }

        public static readonly NimLog NimCoreLog = new NimLog("NimCoreLogger");

        public static readonly NimLog DefaultLog = new NimLog("DefaultLogger");
    }
}
