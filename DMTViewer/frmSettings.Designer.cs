namespace DMT
{
    partial class frmSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtModFolder = new System.Windows.Forms.TextBox();
            this.btnModFolder = new System.Windows.Forms.Button();
            this.dirDiag = new System.Windows.Forms.FolderBrowserDialog();
            this.panBuilds = new System.Windows.Forms.Panel();
            this.btnAddBuildLocation = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lstRecentLocations = new System.Windows.Forms.ListBox();
            this.lblRecentLocations = new System.Windows.Forms.Label();
            this.chkUpdates = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(228, 454);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Mod Folder:";
            // 
            // txtModFolder
            // 
            this.txtModFolder.Location = new System.Drawing.Point(98, 39);
            this.txtModFolder.Name = "txtModFolder";
            this.txtModFolder.Size = new System.Drawing.Size(371, 20);
            this.txtModFolder.TabIndex = 2;
            this.txtModFolder.Resize += new System.EventHandler(this.TxtModFolder_Resize);
            // 
            // btnModFolder
            // 
            this.btnModFolder.Location = new System.Drawing.Point(475, 37);
            this.btnModFolder.Name = "btnModFolder";
            this.btnModFolder.Size = new System.Drawing.Size(25, 23);
            this.btnModFolder.TabIndex = 3;
            this.btnModFolder.Text = "...";
            this.btnModFolder.UseVisualStyleBackColor = true;
            this.btnModFolder.Click += new System.EventHandler(this.btnModFolder_Click);
            // 
            // panBuilds
            // 
            this.panBuilds.AutoScroll = true;
            this.panBuilds.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panBuilds.Location = new System.Drawing.Point(32, 108);
            this.panBuilds.Name = "panBuilds";
            this.panBuilds.Size = new System.Drawing.Size(468, 181);
            this.panBuilds.TabIndex = 4;
            // 
            // btnAddBuildLocation
            // 
            this.btnAddBuildLocation.Location = new System.Drawing.Point(387, 295);
            this.btnAddBuildLocation.Name = "btnAddBuildLocation";
            this.btnAddBuildLocation.Size = new System.Drawing.Size(113, 23);
            this.btnAddBuildLocation.TabIndex = 5;
            this.btnAddBuildLocation.Text = "Add Game Location";
            this.btnAddBuildLocation.UseVisualStyleBackColor = true;
            this.btnAddBuildLocation.Click += new System.EventHandler(this.btnAddBuildLocation_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Game Locations";
            // 
            // lstRecentLocations
            // 
            this.lstRecentLocations.FormattingEnabled = true;
            this.lstRecentLocations.Location = new System.Drawing.Point(32, 346);
            this.lstRecentLocations.Name = "lstRecentLocations";
            this.lstRecentLocations.Size = new System.Drawing.Size(468, 95);
            this.lstRecentLocations.TabIndex = 7;
            this.lstRecentLocations.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstRecentLocations_MouseDoubleClick);
            // 
            // lblRecentLocations
            // 
            this.lblRecentLocations.AutoSize = true;
            this.lblRecentLocations.Location = new System.Drawing.Point(29, 330);
            this.lblRecentLocations.Name = "lblRecentLocations";
            this.lblRecentLocations.Size = new System.Drawing.Size(91, 13);
            this.lblRecentLocations.TabIndex = 8;
            this.lblRecentLocations.Text = "Recent Locations";
            // 
            // chkUpdates
            // 
            this.chkUpdates.AutoSize = true;
            this.chkUpdates.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkUpdates.Location = new System.Drawing.Point(305, 76);
            this.chkUpdates.Name = "chkUpdates";
            this.chkUpdates.Size = new System.Drawing.Size(195, 17);
            this.chkUpdates.TabIndex = 9;
            this.chkUpdates.Text = "Automatically check for updates      ";
            this.chkUpdates.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkUpdates.UseVisualStyleBackColor = true;
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 498);
            this.Controls.Add(this.chkUpdates);
            this.Controls.Add(this.lblRecentLocations);
            this.Controls.Add(this.lstRecentLocations);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnAddBuildLocation);
            this.Controls.Add(this.panBuilds);
            this.Controls.Add(this.btnModFolder);
            this.Controls.Add(this.txtModFolder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSave);
            this.Name = "frmSettings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.ResizeEnd += new System.EventHandler(this.FrmSettings_ResizeEnd);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtModFolder;
        private System.Windows.Forms.Button btnModFolder;
        private System.Windows.Forms.FolderBrowserDialog dirDiag;
        private System.Windows.Forms.Panel panBuilds;
        private System.Windows.Forms.Button btnAddBuildLocation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lstRecentLocations;
        private System.Windows.Forms.Label lblRecentLocations;
        private System.Windows.Forms.CheckBox chkUpdates;
    }
}