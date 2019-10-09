using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DMT
{
    public partial class frmDnspy : Form
    {
        public frmDnspy()
        {
            InitializeComponent();
        }

        private void FrmDnspy_Load(object sender, EventArgs e)
        {

          new Thread(()=>{ 
                          
              System.Threading.Thread.Sleep(500);
                var ret = MessageBox.Show("Would you like to download dnSpy to debug?", "Install required", MessageBoxButtons.YesNo);

                if (ret == DialogResult.No)
                {
                    this.DialogResult = ret;
                    this.Close();
                    return;
                }


                Action<string> updateAction = new Action<string>((s)=> { SetMessage(s); });
                dnSpyDebugging.DownloadDnspy(updateAction);
                this.DialogResult = DialogResult.OK;
            
              }).Start();

        }

        public void SetMessage(string msg)
        {

            if (lblMessage.InvokeRequired)
            {
                lblMessage.Invoke(new Action(()=> { SetMessage(msg); }));
                return;
            }

            lblMessage.Text = msg;

        }

    }
}
