using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DMT.Attributes;
using DMT.Compiler;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace DMT.Tasks
{
    [RunOrder(RunSection.FinalPatch, RunOrder.LastBuild)]
    public class CreateHarmonyDlls : BaseTask
    {
        public override bool Patch(PatchData data)
        {
            
            
            var mods = data.GetHarmonyMods();

            foreach (var mod in mods)
            {

                CompilerSettings compilerSettings = new CompilerSettings();
                LogInternal($"Compiling Harmony mod for {mod.Name}...");

                var scriptPaths = mod.FindFiles("Harmony", "*.cs", true);

                for (int i = 0; i < scriptPaths.Count; ++i)
                {
                    compilerSettings.Files.Add(scriptPaths[i]);
                }

                var startPath = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
                compilerSettings.AddReference("DMT.dll");
                compilerSettings.AddReference(startPath + "/0Harmony.dll");

                var filename = $"Harmony-{mod.Name.MakeSafeFilename()}.dll";
                compilerSettings.OutputPath = mod.Location + "/Harmony/" + filename;

                var dllRefs = Directory.GetFiles(data.ManagedFolder, "*.dll").Where(d => !d.EndsWith("/Mods.dll")).ToArray();
                compilerSettings.AddReferences(dllRefs);

                var modsDll = $"{data.BuildFolder}Mods.dll";
                compilerSettings.AddReferenceAtIndex(modsDll, 0);

                var dllPaths = mod.FindFiles("Harmony", "*.dll", true);
                for (int i = 0; i < dllPaths.Count; ++i)
                {
                    compilerSettings.AddReference(dllPaths[i]);
                }

                var compilerResults = data.Compiler.Compile(data, compilerSettings);
               // Logging.LogInfo($"Built in {compilerResults.Duration}ms");

                if (!compilerResults.Success)
                {
                    for (int i = 0; i < compilerResults.Errors.Count; ++i)
                    {
                        LogWarning(compilerResults.Errors[i]);
                    }
                    LogError("Failed to compile Harmony dll " + filename);
                    return false;
                }

                // Log("Harmony dll compile successful");
                
            }

            return true;
        }
    }
}
