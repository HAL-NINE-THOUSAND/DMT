using System;

namespace DMT.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MinRun : Attribute
    {
        public int Value { get; set; }
        public RunSection Section { get; set; }

        public MinRun()
        {
            this.Section = RunSection.InitialPatch;
            this.Value = -1;
        }
    }
}