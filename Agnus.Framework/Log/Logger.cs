using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Framework.Log
{
    public class Logger
    {
        public string Pattern { get; set; }
        public bool AppendToFile { get; set; }
        public string FilePath { get; set; }        
        public int MaxSizeRollBackups { get; set; }
        public string MaximumFileSize { get; set; }
        public Log4netLevelEnum Level { get; set; }

        ILog _log;
        
        
        public void Setup()
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();            

            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = this.Pattern;
            patternLayout.ActivateOptions();

            RollingFileAppender roller = new RollingFileAppender();
            roller.AppendToFile = this.AppendToFile;
            roller.File = this.FilePath;
            roller.Layout = patternLayout;
            roller.MaxSizeRollBackups = this.MaxSizeRollBackups;
            roller.MaximumFileSize = this.MaximumFileSize;
            roller.RollingStyle = RollingFileAppender.RollingMode.Size;
            roller.StaticLogFileName = true;
            roller.ActivateOptions();            
            hierarchy.Root.AddAppender(roller);

            MemoryAppender memory = new MemoryAppender();
            memory.ActivateOptions();
            hierarchy.Root.AddAppender(memory);

            hierarchy.Root.Level = this.GetLevel();
            hierarchy.Configured = true;

            _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            //_log = (ILog)hierarchy.GetLogger("QSLOG");
            Logger.AppLog = this;
        }

        private log4net.Core.Level GetLevel()
        {
            switch (this.Level)
            {
                case Log4netLevelEnum.Alert: return log4net.Core.Level.Alert;
                case Log4netLevelEnum.All: return log4net.Core.Level.All;
                case Log4netLevelEnum.Critical: return log4net.Core.Level.Critical;
                case Log4netLevelEnum.Debug: return log4net.Core.Level.Debug;
                case Log4netLevelEnum.Emergency: return log4net.Core.Level.Emergency;
                case Log4netLevelEnum.Error: return log4net.Core.Level.Error;
                case Log4netLevelEnum.Fatal: return log4net.Core.Level.Fatal;
                case Log4netLevelEnum.Fine: return log4net.Core.Level.Fine;
                case Log4netLevelEnum.Finer: return log4net.Core.Level.Finer;                    
                case Log4netLevelEnum.Finest: return log4net.Core.Level.Alert;
                case Log4netLevelEnum.Info: return log4net.Core.Level.Info;
                case Log4netLevelEnum.Log4Net_Debug: return log4net.Core.Level.Log4Net_Debug;
                case Log4netLevelEnum.Notice: return log4net.Core.Level.Notice;
                case Log4netLevelEnum.Off: return log4net.Core.Level.Off;
                case Log4netLevelEnum.Severe: return log4net.Core.Level.Severe;
                case Log4netLevelEnum.Trace: return log4net.Core.Level.Trace;
                case Log4netLevelEnum.Verbose: return log4net.Core.Level.Verbose;
                case Log4netLevelEnum.Warn: return log4net.Core.Level.Warn;
                default: return log4net.Core.Level.All;
            }
        }

        public void LogException(Exception ex)
        {
            _log.Error(this.ConcatenarInnerExeptions(ex));
        }

        private string ConcatenarInnerExeptions(Exception ex)
        {
            if (ex.InnerException == null)
                return ex.Message;
            else
                return string.Format("{0}####INNER####{1}", ex.Message, this.ConcatenarInnerExeptions(ex.InnerException));
        }

        public static Logger AppLog { get; private set; }
    }
}
