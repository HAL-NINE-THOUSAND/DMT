using System.Reflection;
using DMT.Attributes;
using DMT.Patches;
using Mono.Cecil;
using SDX.Compiler;

namespace DMT.Tasks
{
    [RunOrder(RunSection.InitialPatch, RunOrder.Early)]
    public class InitialPatchTask : BaseTask
    {

        public override bool Patch(PatchData data)
        {
            if (BuildSettings.ScriptOnly) return true;

            ModuleDefinition gameModule = data.ReadModuleDefinition(data.BackupDllLocataion);
            ModuleDefinition dmt = data.ReadModuleDefinition(data.ManagedFolder + "DMT.dll");

            var builtInPatches = Assembly.GetExecutingAssembly().GetInterfaceImplementers<IPatcherMod>();
            foreach (var patch in builtInPatches)
            {
                patch.Patch(gameModule);
            }

            IPatcherMod[] patches = null;
            var didRun = data.Compiler.RunPatchScripts(data, out patches);

            if (didRun)
            {
                foreach (var patch in patches)
                {
                    patch.Patch(gameModule);
                }

                new BuiltInPatches().Patch(gameModule, dmt);

                gameModule.Write(data.BuildFolder + "InitialPatch.dll");
            }

            return didRun;

        }


    
    }
}
