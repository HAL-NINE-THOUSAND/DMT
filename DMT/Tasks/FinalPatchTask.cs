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

            if (BuildSettings.ScriptOnly) return true;

            ModuleDefinition gameModule = data.ReadModuleDefinition(data.LinkedDllLocation);
            ModuleDefinition modsModule = data.ReadModuleDefinition(data.ModDllLocation);

            var builtInPatches = Assembly.GetExecutingAssembly().GetInterfaceImplementers<IPatcherMod>();
            foreach (var patch in builtInPatches)
            {
                var ipatch = patch as IPatch;
                ipatch?.FinalPatch(gameModule, modsModule);
            }

            IPatcherMod[] patches = null;
            var didRun = data.Compiler.RunPatchScripts(data, out patches);

            if (didRun)
            {
                Logging.Log("Final patching " + patches.Length + " files");
                foreach (var patch in patches)
                {
                    var ipatch = patch as IPatch;
                    ipatch?.FinalPatch(gameModule, modsModule);
                }

                gameModule.Write(data.GameDllLocation);
            }

            return didRun;
        }
    }
}
