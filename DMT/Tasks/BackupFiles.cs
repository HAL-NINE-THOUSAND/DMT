using System.IO;
using System.Linq;
using DMT.Attributes;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace DMT.Tasks
{
    [MinRun]
    public class BackupFiles : BaseTask
    {

   

        private ModuleDefinition Module { get; set; }

        private void GetVersionInfo()
        {

            var consts = Module.Types.First(d => d.Name == "Constants");
            var ctor = consts.Methods.Single(d => d.Name == ".cctor");
            var pro = ctor.Body.GetILProcessor();
            var ins = pro.Body.Instructions;
            var instruction = ins.First(d => d.OpCode == OpCodes.Stsfld && ((FieldDefinition)d.Operand).Name.Contains("cCompatibilityVersion"));


            instruction = instruction.GetNextIntInstruction().GetNextIntInstruction();
            BuildSettings.MajorVersion = instruction.GetValueAsInt();
            instruction = instruction.GetNextIntInstruction();
            BuildSettings.MinorVersion = instruction.GetValueAsInt();
            instruction = instruction.GetNextIntInstruction();
            BuildSettings.BuildNumber = instruction.GetValueAsInt();

        }

        private void RemoveSdxReference(PatchData data)
        {

            var filesToRemove = new[] { "SDX.Core.dll", "SDX.Payload.dll", };


            foreach (var s in filesToRemove)
            {
                var path = data.ManagedFolder + s;
                if (File.Exists(path))
                {
                    LogWarning("Removing SDX reference as it's no longer needed: " + s);
                    File.Delete(path);
                }
            }

        }

        public override bool Patch(PatchData data)
        {

            RemoveSdxReference(data);

            var ass = AssemblyDefinition.ReadAssembly(data.GameDllLocation);
            Module = ass.MainModule;
            GetVersionInfo();

            data.BackupFolder = data.BackupFolder + (data.IsDedicatedServer ? "Dedi/" : "SP/") + BuildSettings.MajorVersion + "." + BuildSettings.MinorVersion + "b" + BuildSettings.BuildNumber + "/";
            data.BackupFolder.MakeFolder();
            data.BackupDllLocataion = data.BackupFolder + PatchData.AssemblyFilename;


            if (BuildSettings.IsLocalBuild)
                Helper.CopyFolder(data.ManagedFolder, data.BackupFolder, true, "Assembly-CSharp.dll");

            if (File.Exists(data.BackupDllLocataion))
            {
                LogInfo("Backup dll found: " + data.BackupDllLocataion);
                return true;
            }

            var modManager = Module.Types.FirstOrDefault(d => d.Name == "ModManager");
            var patchedObject = modManager?.Fields.FirstOrDefault(d => d.Name == "SdxPatchedCheck");
            var isPatched = patchedObject != null;
            if (isPatched)
            {
                LogError("This Assembly-CSharp.dll file has already been patched by SDX so can not be used as a backup. Reset your game files and try again.");
                return false;
            }


            File.Copy(data.GameDllLocation, data.BackupDllLocataion, true);

            if (BuildSettings.IsLocalBuild)
                Helper.CopyFolder(data.ManagedFolder, data.BackupFolder, true);

            Helper.CopyFolder(data.ConfigFolder, data.BackupFolder + "Config", true);
            LogInfo("Copied backup dll to: " + data.BackupDllLocataion);
            return true;

        }

    }
}
