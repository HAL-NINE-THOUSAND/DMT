using System;
using System.Collections.Generic;
using System.IO;

using DMT.Attributes;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace DMT.Tasks
{
    [RunOrder(RunSection.FinalPatch, RunOrder.LastBuild)]
    public class CopyModFolders : BaseTask
    {

        public override bool Patch(PatchData data)
        {

            try
            {

                if (!BuildSettings.ScriptOnly)
                {

                    Logging.Log("Copying mod folders");

                    var disabledMods = data.InactiveMods;
                    for (int i = 0; i < disabledMods.Count; i++)
                    {
                        ModInfo mod = disabledMods[i];

                        string nativeModsDir = $"{data.GameFolder}\\Mods";
                        Helper.MakeFolder(nativeModsDir);

                        string sdxDir = $"{nativeModsDir}/{mod.Name}";

                        if (Directory.Exists(sdxDir))
                        {
                            if (i > 0)
                            {
                                Logging.NewLine();
                            }

                            Logging.LogInfo($"Removing disabled mod " + sdxDir);
                            Directory.Delete(sdxDir, true);
                        }


                    }
                }

                IList<ModInfo> enabledMods = data.ActiveMods;
                for (int i = 0; i < enabledMods.Count; i++)
                {
                    ModInfo mod = enabledMods[i];
                    if (i > 0)
                    {
                        Logging.NewLine();
                    }

                    Logging.Log($"Copy from {mod.Location}");

                    string nativeModsDir = $"{data.GameFolder}/Mods";
                    Helper.MakeFolder(nativeModsDir);

                    string gameModDir = $"{nativeModsDir}/{mod.Name}";
                    gameModDir.MakeFolder();

                    string modInfoSource = $"{mod.Location}/ModInfo.xml";
                    string modInfoDestination = $"{gameModDir}/ModInfo.xml";
                    var p1 = Path.GetFullPath(modInfoSource);
                    var p2 = Path.GetFullPath(modInfoDestination);

                    if (p1.Equals(p2))
                    {
                        Logging.Log("Skipping copy as destination matches source: " + modInfoSource);
                        continue;
                    }

                    //Logging.Log("Copying ");
                    //Logging.Log("p1: " + p1);
                    //Logging.Log("p2: " + p2);

                    if (File.Exists(modInfoSource))
                    {
                        Logging.Log("Copying " + modInfoSource);
                        File.Copy(modInfoSource, modInfoDestination, true);
                    }
                    else
                    {
                        File.WriteAllText(modInfoDestination, $@"<?xml version=""1.0"" encoding=""UTF-8"" ?>
<xml>
	<ModInfo>
		<Name value=""{System.Security.SecurityElement.Escape(mod.Name)}"" />
		<Description value=""{System.Security.SecurityElement.Escape(mod.Description)}"" />
		<Author value=""{System.Security.SecurityElement.Escape(mod.Author)}"" />
		<Version value=""{System.Security.SecurityElement.Escape(mod.ModVersion)}"" />
		<Website value="""" />
	</ModInfo>
</xml>");
                    }

                    if (BuildSettings.ScriptOnly)
                        continue;

                    var foldersToCopy = new List<string>()
                    {
                        "ItemIcons",
                        "Config",
                        "Resources",
                        "Harmony",
                        "Texture",
                        "Textures",
                        "UIAtlases",
                        "Worlds",
                        "Prefabs",
                    };

                    foreach (var s in foldersToCopy)
                    {
                        CopyFolder(mod.Location, gameModDir, s, true, true);
                    }

                    CopyFolder($"{mod.Location}/Prefabs", $"{data.GameFolder}/Data/Prefabs", "", false, false);

                    var deployDir = $"{mod.Location}/Deploy";
                    if (Directory.Exists(deployDir))
                    {
                        CopyFolder(deployDir, gameModDir, "", false, true);
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                Logging.LogError(ex.Message);
            }

            return false;
        }


        private void CopyFolder(string modDir, string gameDir, string folderName, bool clearFilesInDestination, bool copyChildFolders)
        {

            string source = $"{modDir}/{folderName}";
            string destination = $"{gameDir}/{folderName}";

            if (clearFilesInDestination && Directory.Exists(destination))
                ClearFilesFromFolder(destination);

            if (!Directory.Exists(source)) return;

            destination.MakeFolder();

            foreach (var f in Directory.GetFiles(source))
            {
                var filePath = $"{destination}/{Path.GetFileName(f)}";
                Logging.LogInfo("Copying " + filePath);
                File.Copy(f, filePath, true);
            }


            if (copyChildFolders)
            {
                foreach (var dir in Directory.GetDirectories(source))
                {
                    var dirInfo = new DirectoryInfo(dir);
                    CopyFolder(dir, destination + "/" + dirInfo.Name, "", clearFilesInDestination, true);
                }
            }

        }

        private void ClearFilesFromFolder(string dir)
        {
            foreach (var f in Directory.GetFiles(dir))
            {
                Logging.LogInfo("Deleting asset " + f);
                File.Delete(f);
            }
        }

    }
}
