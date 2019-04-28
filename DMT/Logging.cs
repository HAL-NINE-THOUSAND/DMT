using System;

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

        public static void LogInternal(string text, LogType type)
        {
            text = (int)type + "|" + text;
            Console.WriteLine(text);
            LogAction?.Invoke(text);

        }

    }
}
