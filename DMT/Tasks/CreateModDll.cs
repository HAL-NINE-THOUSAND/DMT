using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DMT.Attributes;
using DMT.Compiler;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace DMT.Tasks
{
    [RunOrder(RunSection.InitialPatch, RunOrder.LateBuild)]
    public class CreateModDll : BaseTask
    {
        public override bool Patch(PatchData data)
        {

            LogInternal("Compiling Mods.dll...");

            CompilerSettings compilerSettings = new CompilerSettings();

            compilerSettings.OutputPath = data.ModsDllTempLocation;

            var scriptPaths = data.FindFiles("Scripts", "*.cs", true);
            for (int i = 0; i < scriptPaths.Count; ++i)
            {
                compilerSettings.Files.Add(scriptPaths[i]);
            }
            var sdxfakeFile = @"namespace SDX.Compiler { public class SDXCompilerWrapper {} } namespace SDX { public class SDXWrapper {} } namespace SDX.Core { public class SDXCoreWrapper {} } namespace SDX.Payload { public class SDXPayloadWrapper {} }";
            var sdxtempPath = (data.BuildFolder + "BlankClass.cs").Replace("/", "\\");
            File.WriteAllText(sdxtempPath, sdxfakeFile);
            compilerSettings.Files.Add(sdxtempPath);


            compilerSettings.AddReference("DMT.dll");
            var dllRefs = Directory.GetFiles(data.ManagedFolder, "*.dll").Where(d => d.EndsWith("/Mods.dll") == false && d.Contains("Assembly-CSharp") == false && d.Contains("mscorlib") == false).ToArray();
            compilerSettings.AddReferences(dllRefs);
    
            var patchedDll = $"{data.BuildFolder}InitialPatch.dll"; 
            compilerSettings.AddReferenceAtIndex(patchedDll, 0);

            var dllPaths = data.FindFiles("Scripts", "*.dll", true);
            for (int i = 0; i < dllPaths.Count; ++i)
            {
                compilerSettings.AddReference(dllPaths[i]);
                Logging.LogInfo($"Copy file {dllPaths[i]} -> {data.ManagedFolder}");
                Helper.CopyFileToDir(dllPaths[i], data.ManagedFolder);
            }

            var compilerResults = data.Compiler.Compile(data, compilerSettings);
            Logging.LogInfo($"Built in {compilerResults.Duration}ms");

            if (compilerResults.Success == false)
            {
                for (int i = 0; i < compilerResults.Errors.Count; ++i)
                {
                    LogWarning(compilerResults.Errors[i]);
                }
                LogError("Failed to compile Mods.dll");
                return false;
            }

            Log("Mods.dll compile successful");
            return true;

        }
    }
}
