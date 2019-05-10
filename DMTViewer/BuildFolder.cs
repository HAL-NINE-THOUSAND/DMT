using System;
using System.Windows.Forms;

namespace DMT
{
    public partial class BuildFolder : UserControl
    {

        public EventHandler OnUpdate { get; set; }

        public BuildFolder()
        {
            InitializeComponent();
        }
        public BuildFolder(string location)
        {
            InitializeComponent();
            txtLocation.Text = location;
        }

        internal string GetValue()
        {
            return txtLocation.Text;
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
            OnUpdate(this, null);
        }

        private void btnModFolder_Click(object sender, EventArgs e)
        {
            frmSettings.Instance.ApplyFolderLookup(txtLocation);
        }
    }
}
