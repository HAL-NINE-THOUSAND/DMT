﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using SDX.Compiler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DMT.Compiler
{
    class RoslynCompiler : ICompiler
    {
        public static IPatch[] Empty = new IPatch[0];

        public CompilerResult Compile(PatchData data, CompilerSettings settings)
        {

            Logging.Log("Compiling Roslyn Scripts assembly for " + data.RunSection + "...");

            var trees = new List<SyntaxTree>();
            foreach (var f in settings.Files)
            {
                var sourceCode = System.IO.File.ReadAllText(f);
                trees.Add(SyntaxFactory.ParseSyntaxTree(SourceText.From(sourceCode)));
            }
            //var syntaxTree = SyntaxFactory.ParseSyntaxTree(SourceText.From(sourceCode));

            var assemblyPath = Path.ChangeExtension(Path.GetTempFileName(), "dll");
            
            var compilation = CSharpCompilation.Create(Path.GetFileName(assemblyPath))
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                 .AddReferences(
                    settings.References.Select(d => MetadataReference.CreateFromFile(d))
                )
                .AddSyntaxTrees(trees);


            CompilerResult scriptCompilerResults = new CompilerResult();

            scriptCompilerResults.AssemblyLocation = assemblyPath;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var result = compilation.Emit(assemblyPath);
            stopwatch.Stop();


            if (result.Success)
            {
                scriptCompilerResults.Assembly = Assembly.LoadFile(assemblyPath);
                scriptCompilerResults.Success = true;
                //File.Copy(assemblyPath, data.ModsDllTempLocation, true); 
            }

            foreach (var d in result.Diagnostics)
            {
                if (d.Severity == DiagnosticSeverity.Error)
                    scriptCompilerResults.Errors.Add(d.ToString());
                else
                {
                    if (d.ToString().Contains("Assuming assembly reference 'mscorlib") && (scriptCompilerResults.Warnings.Any(e => e.Contains("Assuming assembly reference 'mscorlib"))))
                        continue;
                    //result.Success || 
                    scriptCompilerResults.Warnings.Add(d.ToString());
                }
            }


            return scriptCompilerResults;
        }

        public bool RunPatchScripts(PatchData data, out IPatcherMod[] ret)
        {
            var settings = CompilerSettings.CreatePatchScripts(data);
            if (settings.Files.Count == 0)
            {
                Logging.Log("No patch scripts found...");
                ret = Empty;
                return true;
            }

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
