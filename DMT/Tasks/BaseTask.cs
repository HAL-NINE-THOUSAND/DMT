namespace DMT.Tasks
{
    public abstract class BaseTask : IPatchTask
    {
        public virtual bool PrePatch(PatchData data)
        {
            return true;
        }

        public virtual bool Patch(PatchData data)
        {
            return true;
        }

        public virtual bool PostPatch(PatchData data)
        {
            return true;
        }


        public static void Log(string msg)
        {
            Logging.Log(msg);
        }
        public static void LogInternal(string msg)
        {
            Log(msg);
        }
        public static void LogWarning(string msg)
        {
            Logging.LogWarning(msg);
        }
        public static void LogError(string msg)
        {
            Logging.LogError(msg);
        }
        public static void LogInfo(string msg)
        {
            Logging.LogInfo(msg);
        }

    }
}
