using System.Collections.Generic;
using System.Reflection;

namespace DMT.Compiler
{
    public class CompilerResult
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
        public Assembly Assembly { get; set; }
        public string AssemblyLocation { get; set; }
        public long Duration { get; set; }
    }
}
