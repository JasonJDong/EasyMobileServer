using System;
using System.IO;
using System.Text;

namespace DMobile.Server.Utilities
{
    public class LogRecorder : IDisposable
    {
        public const string DefaultLogFileName = "ServerLogFile.log";

        private static FileStream LogFileStream;
        private static TextWriter LogWriter;

        public LogRecorder(string logFileName, string logFileDirectory)
        {
            LogFileName = logFileName;
            LogFileDirectory = logFileDirectory;

            //InitializeLogFileForLogging();
        }

        public LogRecorder()
        {
            //InitializeLogFileForLogging();
        }

        public string LogFileName { get; set; }
        public string LogFileDirectory { get; set; }

        public string LogFileFullPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(LogFileName))
                {
                    LogFileName = DefaultLogFileName;
                }
                if (string.IsNullOrWhiteSpace(LogFileDirectory))
                {
                    LogFileDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }
                return Path.Combine(LogFileDirectory, LogFileName);
            }
        }

        public void RecordLog(string msg)
        {
            //ThreadPool.QueueUserWorkItem(AsyncRecordLog, msg);
        }

        private void AsyncRecordLog(object state)
        {
            lock (LogWriter)
            {
                LogWriter.WriteLine("{0} : {1}", DateTime.Now.ToString(), state);
                LogWriter.Flush();
            }
        }

        private void InitializeLogFileForLogging()
        {
            if (!File.Exists(LogFileFullPath))
            {
                File.Create(LogFileFullPath);
            }
            LogFileStream = new FileStream(LogFileFullPath, FileMode.Append, FileAccess.Write);
            LogWriter = new StreamWriter(LogFileStream, Encoding.UTF8);
        }

        #region IDisposable 成员

        public void Dispose()
        {
            if (LogFileStream != null)
            {
                LogFileStream.Close();
            }
        }

        #endregion
    }
}