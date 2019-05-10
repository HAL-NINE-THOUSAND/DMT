using System;

namespace DMT.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class RunOrder : MinRun
    {
        public const int Start = 0;
        public const int Early = 100;
        public const int MidBuild = 200;
        public const int LateBuild = 300;
        public const int LastBuild = int.MaxValue;

        public RunOrder(RunSection section, int value)
        {

            if (value < 0)
            {
                throw new NotImplementedException("Run order must be greater than zero");
            }

            this.Section = section;
            this.Value = value;
        }
    }
}