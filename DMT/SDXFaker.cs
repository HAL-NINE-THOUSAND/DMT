using System;
using System.Collections.Generic;

using System.Text;

namespace SDX.Core
{
    public class Logging
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

        public delegate void OnLogEvent(string text, LogType type);
        public static OnLogEvent Logged;

        public static void NewLine()
        {
            LogInternal("", LogType.Log);
        }

        public static void LogEvent(string text)
        {
            LogInternal(text, LogType.Event);
        }

        public static void LogError(string text)
        {
            LogInternal(text, LogType.Error);
        }

        public static void LogWarning(string text)
        {
            LogInternal(text, LogType.Warning);
        }

        public static void LogInfo(string text)
        {
            LogInternal(text, LogType.Info);
        }

        private static string GetPrefix(LogType t)
        {
            switch (t)
            {
                case LogType.Info:
                    return "INFO:";
                case LogType.Event:
                    return "EVENT:";
                case LogType.Error:
                    return "ERROR:";
                case LogType.Warning:
                    return "WARN:";
                case LogType.Popup:
                    return "POP:";
                default:
                    return "";
            }
        }

        public static void Log(string text)
        {
            LogInternal(text, LogType.Log);
        }

        public static void LogInternal(string text, LogType type = LogType.Log)
        {
            DMT.Logging.LogInternal(text, (DMT.LogType)(int)type);
        }

    }
}


namespace SDX.Payload
{
    class SDXPayloadFaker
    {
    }
}
