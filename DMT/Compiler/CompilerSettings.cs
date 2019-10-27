using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Cecil;

namespace DMT.Compiler
{
    public class CompilerSettings
    {
        //public string CompilerVersion = "v4.0"; 
        public List<string> Files = new List<string>();
        public List<string> References = new List<string>();
        public string OutputPath;
        public bool GenerateInMemory;
        public bool IsDedicateServerBuild;

        public void AddReferences(IEnumerable<string> reference)
        {
            foreach (var s in reference)
                AddReference(s);
        }

        public void AddReferenceAtIndex(string reference, int index)
        {

            if (reference == null) return;
            reference = reference.Trim();
            foreach (var s in References)
            {
                if (s.Equals(reference, StringComparison.OrdinalIgnoreCase))
                    return;

                var filename = Path.GetFileName(s);
                var refFilename = Path.GetFileName(reference);

                if (filename == refFilename)
                {
                    References.Remove(filename);
                    break;
                }

            }

            References.Insert(index, reference);

        }

        public void AddReference(string reference)
        {

            if (reference == null) return;
            reference = reference.Trim();
            foreach (var s in References)
            {
                if (s.Equals(reference, StringComparison.OrdinalIgnoreCase))
                    return;

                var filename = Path.GetFileName(s);
                var refFilename = Path.GetFileName(reference);

                if (filename == refFilename)
                    return;

            }

            References.Add(reference);

        }

        public static CompilerSettings CreatePatchScripts(PatchData data)
        {
            CompilerSettings compilerSettings = new CompilerSettings();
            var patchScriptPaths = data.FindFiles("PatchScripts", "*.cs", true);
            if (patchScriptPaths.Count == 0)
            {
                //return compilerSettings;
            }

            foreach (string path in patchScriptPaths)
            {
                compilerSettings.Files.Add(path);
            }

            compilerSettings.GenerateInMemory = true;
            compilerSettings.AddReference("DMT.dll");
            compilerSettings.AddReference("Mono.Cecil.dll");
            compilerSettings.AddReferences(data.FindFiles("PatchScripts", "*.dll", true));

            var dllPaths = Directory.GetFiles(data.ManagedFolder, "*.dll").Where(d => d.EndsWith("/Mods.dll") == false && d.Contains("Assembly-CSharp") == false).ToArray();
            compilerSettings.AddReferences(dllPaths);

            if (data.RunSection == RunSection.InitialPatch)
                compilerSettings.AddReference(data.BackupDllLocataion);
            else if (data.RunSection == RunSection.LinkedPatch)
            {
                compilerSettings.AddReference(data.InitialDllLocation);
                //compilerSettings.AddReference(data.ManagedFolder + "Assembly-CSharp.dll");

            }
            else compilerSettings.AddReference(data.LinkedDllLocation);

            //for (int i = 0; i < dllPaths.Length; i++)
            //{
            //    var path = dllPaths[i];
            //    if (path.EndsWith(Path.DirectorySeparatorChar + "Assembly-CSharp.dll"))
            //    {
            //        var z = dllPaths[0];
            //        dllPaths[0] = path;
            //        dllPaths[i] = z;
            //        break;
            //    }
            //}

            List<string> namespaces = new List<string>();
            for (int i = 0; i < dllPaths.Length; i++)
            {
                string dllPath = dllPaths[i];
                if (!Path.GetFileName(dllPath).Contains("mscorlib") && !Path.GetFileName(dllPath).Contains("Mono.Cecil")) //
                {
                    var mod = data.ReadModuleDefinition(dllPath);
                    if (namespaces.Contains(mod.Name))
                    {
                        Logging.LogWarning($"Duplicate dll namespace found '{mod.Name}'. Skipping dll at path: '{dllPath}'");
                        continue;
                    }
                    namespaces.Add(mod.Name);
                    compilerSettings.AddReference(dllPath);
                }
            }

            compilerSettings.GenerateInMemory = true;
            return compilerSettings;
        }

    }
}
