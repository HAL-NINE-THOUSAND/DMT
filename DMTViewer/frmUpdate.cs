using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
namespace DMT
{
    public partial class frmUpdate : Form
    {
        private UpdateCheckResult UpdateInfo;
        public frmUpdate(UpdateCheckResult info)
        {
            InitializeComponent();
            UpdateInfo = info;
            lblMessage.Text = info.Message;
            btnDownload.Enabled = info.DownloadUrl != String.Empty;

        }

        private void BtnDownload_Click(object sender, EventArgs e)
        {

            try
            {


                lblMessage.Text = "Downloading...";
                Application.DoEvents();
                WebClient client = new WebClient();
                var zipBytes = client.DownloadData(UpdateInfo.DownloadUrl);

                //zipBytes = File.ReadAllBytes(@"C:\!Projects\DMT\DMTViewer\bin\Debug\Debug.zip");

                var zipLocation = Application.StartupPath + "/Update/";
                if (Directory.Exists(zipLocation))
                    Directory.Delete(zipLocation, true);

                Directory.CreateDirectory(zipLocation);

                lblMessage.Text = "Extracting...";
                Application.DoEvents();
                using (MemoryStream stream = new MemoryStream(zipBytes))
                {
                    using (ZipArchive arch = new ZipArchive(stream))
                    {
                        foreach (var entry in arch.Entries)
                        {

                            if (entry.Name != String.Empty)
                            {

                                var fullPath = Path.Combine(zipLocation, entry.FullName);
                                var dir = Path.GetDirectoryName(fullPath);

                                Directory.CreateDirectory(dir);
                                var name = entry.FullName;
                                entry.ExtractToFile(fullPath);
                            }
                        }
                    }
                }

                ProcessStartInfo info = new ProcessStartInfo();
                info.Arguments = $"/updatesource \"{zipLocation}\" /updatedestination \"{Application.StartupPath}\"";
                //info.CreateNoWindow = true;
                info.FileName = zipLocation + "DMTViewer.exe";

                Process.Start(info);
                Application.Exit();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Something went wrong while updating: " + exception.Message + "\nYou can download the release here: " + UpdateInfo.DownloadUrl);
            }

        }

        private void FrmUpdate_Load(object sender, EventArgs e)
        {

            chkUpdate.Checked = BuildSettings.Instance.AutoCheckForUpdates;
        }

        private void ChkUpdate_CheckedChanged(object sender, EventArgs e)
        {
            BuildSettings.Instance.AutoCheckForUpdates = chkUpdate.Checked;
            BuildSettings.Save();
        }
    }
}
