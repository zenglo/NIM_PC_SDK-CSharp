using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace NimUtility
{
    public enum LogLevel
    {
        Verbose,
        Info,
        Error
    }

    public static class Log
    {
        private static string _filePath = null;
        private const string LogFileName = "nim_unity_cs.log";
        private const int MaxLogFileLegth = 2 * 1024 * 1024; //2M
        private static object _lockObj = new object(); 

        static Log()
        {
#if DEBUGVERSION
            CreateLogFile();
            AppendLogHeader();
#endif
        }

        static void AppendLogHeader()
        {
            var head = string.Format("{0} {1}:Nim Started {2}{3}", new string('-', 40), DateTime.Now.ToString("MM-dd hh:mm:ss"), new string('-', 40), System.Environment.NewLine);
            head = System.Environment.NewLine + head;
            OutPutLog(head);
        }

        static string CreateLogFile()
        {
#if UNITY
            string targetDir = UnityEngine.Application.persistentDataPath;
            if (UnityEngine.Application.platform == UnityEngine.RuntimePlatform.Android)
            {
                if (Directory.Exists("/sdcard"))
                    targetDir = "/sdcard";
            }
            if(UnityEngine.Application.platform == UnityEngine.RuntimePlatform.WindowsPlayer)
            {
                targetDir = System.Environment.CurrentDirectory;
            }
            _filePath = Path.Combine(targetDir, LogFileName);
#else
            _filePath = Path.Combine(System.Environment.CurrentDirectory, LogFileName);
#endif
            var stream = File.Open(_filePath, FileMode.OpenOrCreate,FileAccess.ReadWrite, FileShare.Write);
            if (stream.Length > MaxLogFileLegth)
            {
                if (stream.CanWrite && stream.CanSeek)
                    stream.SetLength(0);
                else
                    stream = File.Open(_filePath, FileMode.CreateNew,FileAccess.ReadWrite,FileShare.Write);
            }
            stream.Close();
            return _filePath;
        }

        static void WriteFile(string log)
        {
            lock(_lockObj)
            {
                if (!string.IsNullOrEmpty(log))
                    File.AppendAllText(_filePath, log);
            }
        }

        static string Format(LogLevel level, string log)
        {

            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("[{0}:{1}] {2}:{3} ", DateTime.Now.ToString("MM-dd hh:mm:ss"), Thread.CurrentThread.ManagedThreadId,
                level.ToString(), log);
            builder.AppendLine();
            return builder.ToString();
        }

        static void WriteLine(LogLevel level, string format, params object[] args)
        {
            string log;
            try
            {
                log = string.Format(format, args);
                log = log.Trim().TrimEnd(new char[] { '\r', '\n' });
                log = Format(level, log);
            }
            catch (Exception e)
            {
                log = e.ToString();
                log = string.Format("Log.WriteLine Error:{0}{1}", System.Environment.NewLine, log);
            }
            OutPutLog(log);
        }

        static void OutPutLog(string log)
        {
            if (!log.EndsWith(System.Environment.NewLine))
                log += System.Environment.NewLine;
#if UNITY
            UnityEngine.Debug.Log(log);
#else
            System.Diagnostics.Debug.WriteLine(log);
#endif
            WriteFile(log);
        }

        [Conditional("DEBUGVERSION")]
        public static void InfoFormat(string format, params object[] args)
        {
            WriteLine(LogLevel.Info, format, args);
        }

        [Conditional("DEBUGVERSION")]
        public static void Info(string log)
        {
            OutPutLog(log);
        }

        [Conditional("DEBUGVERSION")]
        public static void Error(string format, params object[] args)
        {
            StringBuilder builder = new StringBuilder();
            StackTrace st = new StackTrace(true);
            var frames = st.GetFrames();
            builder.AppendLine(new string('*', 20) + "Error:");
            foreach(var sf in frames)
            {
                builder.AppendFormat("File: {0} Line:{1}", sf.GetFileName(), sf.GetFileLineNumber());
                builder.AppendLine();
                builder.AppendFormat("Method:{0}", sf.GetMethod().Name);
                builder.AppendLine();
            }
            
            var trace = builder.ToString();
            var log = string.Format(format, args);
            WriteLine(LogLevel.Error, log + trace);
        }
    }
}
