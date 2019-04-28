using SDX.Compiler;

namespace DMT.Compiler
{
    public interface ICompiler
    {

        CompilerResult Compile(PatchData data, CompilerSettings settings);
        IPatcherMod[] RunPatchScripts(PatchData data);

    }
}
