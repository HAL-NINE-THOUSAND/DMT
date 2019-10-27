using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DMT
{
    public static class dnSpyDebugging
    {

        static string DownloadUrl = "https://github.com/HAL-NINE-THOUSAND/DMT/releases/download/v1.0/dnSpy.zip";
        static string DnSpyLocation = Application.StartupPath + "/dnSpy/";
        static string UnityResourcesLocation = Application.StartupPath + "/UnityResources/";
        static string DnSpyZipLocation = Application.StartupPath + "/dnSpy.zip";

        public static bool DnSpyInstalled()
        {
            var ret = Directory.Exists(DnSpyLocation);
            return ret;
        }


        public static string StartDebugging()
        {

            try
            {

                Logging.Log("Starting debugging...");
                var data = PatchData.Create(BuildSettings.Instance);

                var isDedicatedServer = data.IsDedicatedServer;
                var exe = isDedicatedServer ? "7DaysToDieServer" : "7DaysToDie";
                Helper.KillProcessByName(exe);

                var settings = BuildSettings.Instance;

                if (settings.GameFolders.Count == 0) return "No game folder has been set up";

                var folder = settings.GameFolders[0];

                data.GameFolder = BuildSettings.Instance.GameFolders.FirstOrDefault();

                if (!File.Exists(data.ExePath)) return "No exe file found to run";

                var info = FileVersionInfo.GetVersionInfo(data.ExePath);
                var productVersion = info.ProductVersion.Substring(0, info.ProductVersion.LastIndexOf("."));

                var unityResourceLocation = UnityResourcesLocation + "Unity." + productVersion;

                if (!Directory.Exists(unityResourceLocation)) return "Can't find mono version to patch for Unity " + productVersion + "\nYou can try manually adding the files found at https://github.com/0xd4d/dnSpy/releases under 'Unity-debugging-XXX'";

                var monoLocationPatched = unityResourceLocation + "/mono.dll";
                var monoLocation = data.GameFolder + "/Mono/EmbedRuntime/mono.dll";
                if (File.Exists(monoLocation))
                {
                    File.Copy(monoLocationPatched, monoLocation, true);
                }
                var monov4LocationPatched = unityResourceLocation + "/mono-2.0-bdwgc.dll";
                var monov4Location = data.GameFolder + "/MonoBleedingEdge/EmbedRuntime/mono-2.0-bdwgc.dll";
                if (File.Exists(monov4Location))
                {
                    File.Copy(monov4LocationPatched, monov4Location, true);
                }


                var dnspyExe = DnSpyLocation + "dnSpy.exe";
                var args = $@"""{data.GameFolder + data.GameDllLocation}"" --start-debugger ""{data.ExePath}""";

                ProcessStartInfo pInfo = new ProcessStartInfo(Path.GetFileName(dnspyExe), args);
                pInfo.WorkingDirectory = Path.GetDirectoryName(dnspyExe);
                Process.Start(pInfo);

                return string.Empty;
            }
            catch (Exception ex)
            {
                return "I'm sorry Dave, I'm afraid I can't do that: " + ex.Message;
            }

        }



        public static void DownloadDnspy(Action<string> updateAction)
        {

            try
            {

                updateAction?.Invoke("Downloading dnSpy...");
                WebClient client = new WebClient();
                var zipBytes = client.DownloadData(DownloadUrl);
                if (Directory.Exists(DnSpyLocation))
                    Directory.Delete(DnSpyLocation, true);

                File.WriteAllBytes(DnSpyZipLocation, zipBytes);

                Directory.CreateDirectory(DnSpyLocation);
                updateAction?.Invoke("Extracting dnSpy...");
                Application.DoEvents();
                ZipFile.ExtractToDirectory(DnSpyZipLocation, DnSpyLocation);


            }
            catch (Exception e)
            {
                updateAction?.Invoke("Something went wrong: " + e.Message);
            }

        }

    }
}
