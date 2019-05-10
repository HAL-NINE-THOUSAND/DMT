using SDX.Compiler;

namespace DMT.Compiler
{
    public interface ICompiler
    {

        CompilerResult Compile(PatchData data, CompilerSettings settings);
        bool RunPatchScripts(PatchData data, out IPatcherMod[] patches);

    }
}
