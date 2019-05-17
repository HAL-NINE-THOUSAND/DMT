using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DMT.Properties;

namespace DMT
{
    public partial class frmSettings : Form
    {

        internal static frmSettings Instance;

        private string OldModFolder;

        public frmSettings()
        {
            InitializeComponent();
        }

        private void btnModFolder_Click(object sender, EventArgs e)
        {
            ApplyFolderLookup(txtModFolder);
        }

        internal void ApplyFolderLookup(TextBox txt)
        {

            if (dirDiag.ShowDialog() == DialogResult.OK)
            {
                txt.Text = dirDiag.SelectedPath;
            }
        }

        public bool ModsFolderChanged()
        {
            return OldModFolder != BuildSettings.Instance.ModFolder;
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {

            this.Icon = Resources.HAL9000;
            Instance = this;
            txtModFolder.Text = BuildSettings.Instance.ModFolder;
            OldModFolder = txtModFolder.Text;
            chkUpdates.Checked = BuildSettings.Instance.AutoCheckForUpdates;
            var builds = BuildSettings.Instance.GameFolders ?? new List<string>();
            
            foreach (var s in builds)
            {
                AddBuildFolder(s);
            }

            ReorderBuilds(null, null);

            foreach (var s in BuildSettings.Instance.PreviousLocations)
            {
                lstRecentLocations.Items.Add(s);
            }

        }

        private void AddBuildFolder(string location)
        {

            foreach (Control c in panBuilds.Controls)
            {
                if (((BuildFolder) c).GetValue() == location)
                    return;
            }

            var comp = new BuildFolder(location);
            comp.OnUpdate += ReorderBuilds;
            panBuilds.Controls.Add(comp);
        }

        private void ReorderBuilds(object sender, EventArgs e)
        {

            int top = 0;
            if (panBuilds.Controls.Count == 0)
            {
                AddBuildFolder(String.Empty);
            }

            foreach(Control con in panBuilds.Controls)
            {
                con.Location = new Point(0, top);
                top += con.Size.Height + 1;
            }

        }


        private void btnSave_Click(object sender, EventArgs e)
        {

            var settings = BuildSettings.Instance;

            settings.ModFolder = txtModFolder.Text;

            BuildSettings.Instance.AutoCheckForUpdates = chkUpdates.Checked;

            settings.GameFolders = new List<string>();

            foreach (BuildFolder c in panBuilds.Controls)
            {
                var val = c.GetValue().FolderFormat();
                settings.GameFolders.Add(val);
                if (Directory.Exists(val) && !settings.PreviousLocations.Contains(val))
                {
                    settings.PreviousLocations.Add(val);
                    if (settings.PreviousLocations.Count > 10)
                        settings.PreviousLocations.RemoveAt(0);
                }
            }

            BuildSettings.Save();
            this.Close();
        }

        private void btnAddBuildLocation_Click(object sender, EventArgs e)
        {
            AddBuildFolder("");
            ReorderBuilds(null, null);
        }

        private void lstRecentLocations_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            if (lstRecentLocations.SelectedIndices.Count == 0) return;

            AddBuildFolder(lstRecentLocations.SelectedItem.ToString());

            ReorderBuilds(null, null);
        }
    }
}
