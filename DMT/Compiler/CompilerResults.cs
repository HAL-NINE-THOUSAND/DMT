using System.Collections.Generic;
using System.Reflection;

namespace DMT.Compiler
{
    public class CompilerResult
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public Assembly Assembly { get; set; }
        public long Duration { get; set; }
    }
}
