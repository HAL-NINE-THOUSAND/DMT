using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace DMT
{
    
    public class BuildSettings
    {
 
        public static int MajorVersion { get; set; }
        public static int MinorVersion { get; set; }
        public static int BuildNumber { get; set; }
        public static string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        public static bool IsLocalBuild { get; set; }
        public bool AutoCheckForUpdates { get; set; } = true;

        public static bool AutoBuild { get; set; }
        public static bool IsSilent { get; set; }
        public static bool ScriptOnly { get; set; }
        public static bool DisableLocalisation { get; set; }


        public static BuildSettings Instance { get; set; } = new BuildSettings();

        public string BackupFolder { get; set; } = String.Empty;

        public List<string> GameFolders { get; set; }
        
        public string ModFolder { get; set; } = String.Empty;

        public List<string> EnabledMods { get; set; }

        public List<ModInfo> Mods { get; set; } = new List<ModInfo>();

        public List<string> PreviousLocations { get; set; } = new List<string>();

        public bool AutoClose { get; set; }
        public bool AutoPlay { get; set; }

        public static void Load()
        {

            var path = Application.StartupPath + "/Settings.json";
            if (File.Exists(path))
            {
                var txt = File.ReadAllText(path);
                try
                {
                    Instance = JsonConvert.DeserializeObject<BuildSettings>(txt);
                }
                catch (Exception e)
                {
                    MessageBox.Show(@"Failed loading settings file: " + path);
                }
            }

            Instance.Init();
        }

        public static void Save()
        {

            Instance.EnabledMods = Instance.Mods.Where(d => d.Enabled).Select(d => d.Name).ToList();
            var path = Application.StartupPath + "/Settings.json";
            File.WriteAllText(path, JsonConvert.SerializeObject(Instance));
            
        }

        public void Init()
        {
            
            var backup = Application.StartupPath + "/Backups/";
            Instance.BackupFolder = backup;

            Mods.Clear();
            if (Directory.Exists(Instance.ModFolder))
            {
                var dirs = Directory.GetDirectories(Instance.ModFolder);
                foreach (var d in dirs)
                {
                    var mod = ModInfo.Create(d);

                    if (mod == null) continue;
                    
                    mod.Enabled = EnabledMods == null || EnabledMods.Contains(mod.Name);
                    Mods.Add(mod);
                }

            }
            
        }

    }
}
