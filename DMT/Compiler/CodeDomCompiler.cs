using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.CSharp;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SDX.Compiler;

namespace DMT.Compiler
{
    public class CodeDomCompiler : ICompiler
    {
        public static IPatch[] Empty = new IPatch[0];

        public CompilerResult Compile(PatchData data, CompilerSettings settings)
        {
            CompilerParameters compilerParameters = new CompilerParameters();
            compilerParameters.GenerateInMemory = settings.GenerateInMemory;
            compilerParameters.GenerateExecutable = false;
            compilerParameters.IncludeDebugInformation = true;
            compilerParameters.ReferencedAssemblies.AddRange(settings.References.ToArray());
            compilerParameters.WarningLevel = -1;

            if (!settings.GenerateInMemory)
                compilerParameters.OutputAssembly = settings.OutputPath; // data.BuildFolder + "Mods.dll"; // settings.OutputPath;

            var flag = settings.IsDedicateServerBuild ? "IsDedi" : "IsClient";
            compilerParameters.CompilerOptions += " /define:" + flag + " /nostdlib";

            var codeProvider = new CSharpCodeProvider(new Dictionary<string, string>
                {
                    //{"CompilerVersion", settings.CompilerVersion},
                }
            );


            CompilerResult scriptCompilerResults = new CompilerResult();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            CompilerResults compilerResults = codeProvider.CompileAssemblyFromFile(compilerParameters, settings.Files.ToArray());

            scriptCompilerResults.Duration = stopwatch.ElapsedMilliseconds;
            stopwatch.Stop();

            foreach (string line in compilerResults.Output)
            {
                if (line.Contains("error CS"))
                {
                    scriptCompilerResults.Errors.Add(line);
                }
            }

            if (compilerResults.Errors.Count == 0)
            {
                scriptCompilerResults.Assembly = compilerResults.CompiledAssembly;
                scriptCompilerResults.Success = true;
            }

            return scriptCompilerResults;
        }

        public bool RunPatchScripts(PatchData data, out IPatcherMod[] ret)
        {
            var settings = CompilerSettings.CreatePatchScripts(data);
            if (settings.Files.Count == 0)
            {
                Logging.Log("No patch scripts found...");
                ret =  Empty;
                return true;
            }

            Logging.Log("Compiling PatchScripts assembly for " + data.RunSection + "...");
            var compilerResults = Compile(data, settings); // ScriptCompiler.Compile(compilerSettings, Plugin.DataDirectory, gamePath, false);
            Logging.LogInfo($"Built patch file in {compilerResults.Duration}ms");
            
            if (compilerResults.Success == false)
            {
                Logging.LogError("Compile errors");
                foreach (var t in compilerResults.Warnings)
                {
                    Logging.LogWarning(t);
                }
                foreach (var t in compilerResults.Errors)
                {
                    Logging.LogError(t);
                }

                Logging.LogError("Failed to compile PatchMods");
                ret = Empty;
                return false;
            }

            Logging.Log("PatchMods compile successful");
            var patches = compilerResults.Assembly.GetInterfaceImplementers<IPatcherMod>();
            Logging.Log("Found patcher mods: " + patches.Length);
            ret = patches;
            return true;

        }
    }
}
