using System.Reflection;
using DMT.Attributes;
using Mono.Cecil;
using SDX.Compiler;

namespace DMT.Tasks
{
    [RunOrder(RunSection.LinkedPatch, RunOrder.MidBuild)]
    public class LinkedPatchTask : BaseTask
    {


        public override bool Patch(PatchData data)
        {


            ModuleDefinition gameModule = data.ReadModuleDefinition(data.InitialDllLocation);
            ModuleDefinition modsModule = data.ReadModuleDefinition(data.ModsDllTempLocation);

            var builtInPatches = Assembly.GetExecutingAssembly().GetInterfaceImplementers<IPatcherMod>();
            foreach (var patch in builtInPatches)
            {
                patch.Link(gameModule, modsModule);
            }

            IPatcherMod[] patches = null;
            var didRun = data.Compiler.RunPatchScripts(data, out patches);

            if (didRun)
            {

                Logging.Log("Link patching " + patches.Length + " files");
                foreach (var patch in patches)
                {
                    patch.Link(gameModule, modsModule);
                }

                gameModule.Write(data.LinkedDllLocation);
                modsModule.Write(data.ModDllLocation);
            }

            return didRun;

        }


    
    }
}
