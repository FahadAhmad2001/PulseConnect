using System;
using System.Collections.Generic;
using System.Text;

namespace PulseConnectServer.Utilities
{
    public static class Log
    {
        private static string LogFilePath = "aaaa";
        public static void StartLogging()
        {
            LogFilePath = "logfile_" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Date.ToString() + "--" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString() + "-" + DateTime.Now.Millisecond.ToString() + ".log";
        }
        public static void AddLog(string message, LogLevel level)
        {
            Console.WriteLine(DateTime.Now.ToString() + "    " + level.ToString().ToUpper() + ": " + message);
            File.AppendAllText(LogFilePath, DateTime.Now.ToString() + "    " + level.ToString().ToUpper() + ": " + message + "\n");
        }
    }

    public enum LogLevel
    {
        Debug, Info, Warning, Error
    }
}
