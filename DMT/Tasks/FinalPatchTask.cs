using System.Reflection;
using DMT.Attributes;
using Mono.Cecil;
using SDX.Compiler;

namespace DMT.Tasks
{
    [RunOrder(RunSection.FinalPatch, RunOrder.Start)]
    public class FinalPatchTask : BaseTask
    {
        public override bool Patch(PatchData data)
        {
            ModuleDefinition gameModule = data.ReadModuleDefinition(data.LinkedDllLocation);
            ModuleDefinition modsModule = data.ReadModuleDefinition(data.ModDllLocation);

            var builtInPatches = Assembly.GetExecutingAssembly().GetInterfaceImplementers<IPatcherMod>();
            foreach (var patch in builtInPatches)
            {
                var ipatch = patch as IPatch;
                ipatch?.FinalPatch(gameModule, modsModule);
            }

            var patches = data.Compiler.RunPatchScripts(data);
            Logging.Log("Final patching " + patches.Length + " files");
            foreach (var patch in patches)
            {
                var ipatch = patch as IPatch;
                ipatch?.FinalPatch(gameModule, modsModule);
            }

            gameModule.Write(data.GameDllLocation);
            return true;
        }
    }
}
