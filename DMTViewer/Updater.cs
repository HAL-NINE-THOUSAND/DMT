using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Octokit;

namespace DMT
{

    public class UpdateCheckResult
    {
        internal bool UpdateAvailable { get; set; }
        internal string Message { get; set; }
        internal string DownloadUrl { get; set; }

    }

    public class Updater
    {
        private const string ReleaseUrl = @"https://api.github.com/repos/HAL-NINE-THOUSAND/DMT/releases/latest";


        public static void Update(string sourceFolder, string destinationFolder)
        {

            try
            {

                destinationFolder = destinationFolder.FolderFormat();

                Directory.CreateDirectory(destinationFolder);
                
                foreach (var f in Directory.GetFiles(sourceFolder))
                {
                    var copyTo = destinationFolder + Path.GetFileName(f);
                    File.Copy(f, copyTo, true);
                }
                foreach (var f in Directory.GetDirectories(sourceFolder))
                {

                    Update(sourceFolder.FolderFormat() + new DirectoryInfo(f).Name, f);
                }

                Process.Start(destinationFolder + "DMTViewer.exe");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }

        }




        public static UpdateCheckResult CheckForUpdate()
        {

            bool updateAvailable = false;
            string message = String.Empty;
            string url = String.Empty;

            try
            {

                var github = new GitHubClient(new ProductHeaderValue("DMT-Update-Checker"));
                var releases = github.Repository.Release.GetAll("HAL-NINE-THOUSAND", "DMT").Result;
                var latest = releases[0];
                var version = latest.Assets[0].Name.ToLower().Replace("dmtv", "").Replace(".zip", "");

                var thisVersion = BuildSettings.GetVersion();
                updateAvailable = version != thisVersion;
                var s = "";
                url = updateAvailable ? latest.Assets[0].BrowserDownloadUrl : "";
                message = updateAvailable ? "An update is available. " + version : "There is no update available"; 
            }
            catch (Exception boo)
            {

                message = "Something went wrong checking for updates: " + boo.Message;
            }

            return new UpdateCheckResult()
            {
                DownloadUrl = url,
                Message = message,
                UpdateAvailable = updateAvailable,
            };

        }

  
        public static string GetInfo()
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(ReleaseUrl);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded"; // or whatever - application/json, etc, etc
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

    }
}
