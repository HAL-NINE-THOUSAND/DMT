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
    [RunOrder(RunSection.FinalPatch, RunOrder.LateBuild)]
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
                    var contents = File.ReadAllText(scriptPaths[i]);
                    var needsUsingUpdate = (contents.ContainsIgnoreCase("using Harmony;") && !contents.ContainsIgnoreCase("using HarmonyLib;"));
                    var needsStaticCreatorUpdate = contents.ContainsIgnoreCase("HarmonyInstance.Create(");
         

                    if (BuildSettings.AutoUpdateHarmony)
                    {
                        if (needsUsingUpdate)   
                        {
                            contents = contents.Replace("using Harmony;", "using HarmonyLib;");
                        }
                        if (needsStaticCreatorUpdate)
                        {
                            contents = contents.Replace("HarmonyInstance.Create(", "new Harmony(");
                            contents = contents.Replace("HarmonyInstance", "Harmony");
                        }

                        if (needsStaticCreatorUpdate || needsUsingUpdate)
                        {
                            File.WriteAllText(scriptPaths[i], contents);
                        }
                        LogWarning("Attempting auto harmony update on " + scriptPaths[i]);
                    }
                    else
                    {
                        if (needsUsingUpdate || needsStaticCreatorUpdate)
                        {
                            LogWarning(scriptPaths[i]);
                            LogError($"Harmony 1.0 scripts detected in {mod.Name}. This needs updating to Harmony 2.0 to function correctly. You can try using the 'Attempt Harmony Auto Update' option in DMT");
                        }
                    }
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
                    var dllPath = dllPaths[i];
                    if (Path.GetFullPath(compilerSettings.OutputPath) == Path.GetFullPath(dllPath))
                    {
                        //Logging.Log("Ignoring harmony DLL: " + Path.GetFileName(dllPath));
                        continue; //can't reference self as we're building the DLL
                    }

                    compilerSettings.AddReference(dllPath);

                }

                var compilerResults = data.Compiler.Compile(data, compilerSettings);
               // Logging.LogInfo($"Built in {compilerResults.Duration}ms");

                if (!compilerResults.Success)
                {
                    foreach (var t in compilerResults.Warnings)
                    {
                        Logging.LogWarning(t);
                    }
                    for (int i = 0; i < compilerResults.Errors.Count; ++i)
                    {
                        LogError(compilerResults.Errors[i]);
                    }
                    LogError("Failed to compile Harmony dll " + filename);
                    return false;
                }

                if (!String.IsNullOrEmpty(compilerResults.AssemblyLocation))
                    File.Copy(compilerResults.AssemblyLocation, compilerSettings.OutputPath, true);

                // Log("Harmony dll compile successful");

            }

            return true;
        }
    }
}
