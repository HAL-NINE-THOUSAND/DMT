using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32.SafeHandles;

namespace DMT
{

    public enum LogType
    {
        Log,
        Info,
        Event,
        Error,
        Warning,
        Popup,
    }

    public static class Logging
    {
        public const string LogFile = "BuildLog.txt";

        //public static string LogPath;
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);

        private const int ATTACH_PARENT_PROCESS = -1;

        static StreamWriter _stdOutWriter;

        // this must be called early in the program
        static Logging()
        {
            //LogPath = Application.StartupPath + "/" + LogFile;

            // this needs to happen before attachconsole.
            // If the output is not redirected we still get a valid stream but it doesn't appear to write anywhere
            // I guess it probably does write somewhere, but nowhere I can find out about
            var stdout = Console.OpenStandardOutput();
            _stdOutWriter = new StreamWriter(stdout);
            _stdOutWriter.AutoFlush = true;

            AttachConsole(ATTACH_PARENT_PROCESS);
        }

        public static Action<string> LogAction;

        public static void NewLine()
        {
            LogInternal(String.Empty, LogType.Info);
        }
        public static void Log(string msg)
        {
            LogInternal(msg, LogType.Log);
        }
        public static void LogWarning(string msg)
        {
            LogInternal(msg, LogType.Warning);
        }
        public static void LogError(string msg)
        {
            LogInternal(msg, LogType.Error);
        }
        public static void LogInfo(string msg)
        {
            LogInternal(msg, LogType.Info);
        }

        public static void StartFile()
        {
            //File.WriteAllText(LogPath, "");
        }

        public static void ResetLog()
        {
            File.WriteAllText(LogFile, string.Empty);
        }

        public static void CommandLine(string s)
        {


            //Console.WriteLine("Console: '{0}'", s);
            Console.WriteLine(s);

            File.AppendAllText(LogFile, s + "\n");

            //var attempts = 0;

            //while(true)
            //{

            //    if (attempts++ > 10)
            //        throw new NotImplementedException("Could not write to log file");
            //    //try
            //    //{
            //        File.AppendAllText(LogPath, s + "\n");
            //        break;
            //    //}
            //    //catch (Exception e)
            //    //{
            //    //    System.Threading.Thread.Sleep(100);
            //    //}

            //}
        }

        public static void LogInternal(string text, LogType type)
        {
            text = (int)type + "|" + text;
            //Console.WriteLine("msg '{0}'", text);
            Console.WriteLine(text);
            LogAction?.Invoke(text);

        }

    }
}
