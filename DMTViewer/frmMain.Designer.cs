namespace DMTViewer
{
    partial class frmMain
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
            this.lstMods = new System.Windows.Forms.ListView();
            this.ColModName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColAuthor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColDesc = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuVersion = new System.Windows.Forms.ToolStripMenuItem();
            this.madeByMachineElvesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.manualUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.experimentalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableDebuggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rtbOutput = new System.Windows.Forms.RichTextBox();
            this.btnModsFolder = new System.Windows.Forms.Button();
            this.panButtons = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkHarmonyUpdate = new System.Windows.Forms.CheckBox();
            this.btnDebug = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.Play = new System.Windows.Forms.Button();
            this.chkPlay = new System.Windows.Forms.CheckBox();
            this.chkAutoClose = new System.Windows.Forms.CheckBox();
            this.btnBuild = new System.Windows.Forms.Button();
            this.diag = new System.Windows.Forms.OpenFileDialog();
            this.chkAssetCopy = new System.Windows.Forms.CheckBox();
            this.makeReleaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstMods
            // 
            this.lstMods.AllowDrop = true;
            this.lstMods.CheckBoxes = true;
            this.lstMods.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColModName,
            this.ColAuthor,
            this.ColDesc});
            this.lstMods.Dock = System.Windows.Forms.DockStyle.Left;
            this.lstMods.HideSelection = false;
            this.lstMods.Location = new System.Drawing.Point(0, 104);
            this.lstMods.Name = "lstMods";
            this.lstMods.Size = new System.Drawing.Size(668, 402);
            this.lstMods.TabIndex = 0;
            this.lstMods.UseCompatibleStateImageBehavior = false;
            this.lstMods.View = System.Windows.Forms.View.Details;
            this.lstMods.SelectedIndexChanged += new System.EventHandler(this.LstMods_SelectedIndexChanged);
            this.lstMods.DragDrop += new System.Windows.Forms.DragEventHandler(this.lstMods_DragDrop);
            this.lstMods.DragOver += new System.Windows.Forms.DragEventHandler(this.lstMods_DragOver);
            this.lstMods.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstMods_MouseDoubleClick);
            this.lstMods.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstMods_MouseDown);
            // 
            // ColModName
            // 
            this.ColModName.Text = "        Mod Name";
            this.ColModName.Width = 227;
            // 
            // ColAuthor
            // 
            this.ColAuthor.Text = "Author";
            this.ColAuthor.Width = 127;
            // 
            // ColDesc
            // 
            this.ColDesc.Text = "Description";
            this.ColDesc.Width = 353;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1460, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Settings";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuVersion,
            this.madeByMachineElvesToolStripMenuItem1,
            this.toolStripMenuItem1,
            this.experimentalToolStripMenuItem,
            this.makeReleaseToolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // mnuVersion
            // 
            this.mnuVersion.Name = "mnuVersion";
            this.mnuVersion.Size = new System.Drawing.Size(198, 22);
            this.mnuVersion.Text = "version";
            this.mnuVersion.Click += new System.EventHandler(this.MnuVersion_Click);
            // 
            // madeByMachineElvesToolStripMenuItem1
            // 
            this.madeByMachineElvesToolStripMenuItem1.Name = "madeByMachineElvesToolStripMenuItem1";
            this.madeByMachineElvesToolStripMenuItem1.Size = new System.Drawing.Size(198, 22);
            this.madeByMachineElvesToolStripMenuItem1.Text = "Made by Machine Elves";
            this.madeByMachineElvesToolStripMenuItem1.Click += new System.EventHandler(this.MadeByMachineElvesToolStripMenuItem1_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manualUpdateToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(198, 22);
            this.toolStripMenuItem1.Text = "Check for updates";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.ToolStripMenuItem1_Click);
            // 
            // manualUpdateToolStripMenuItem
            // 
            this.manualUpdateToolStripMenuItem.Name = "manualUpdateToolStripMenuItem";
            this.manualUpdateToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.manualUpdateToolStripMenuItem.Text = "ManualUpdate";
            this.manualUpdateToolStripMenuItem.Click += new System.EventHandler(this.manualUpdateToolStripMenuItem_Click);
            // 
            // experimentalToolStripMenuItem
            // 
            this.experimentalToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableDebuggerToolStripMenuItem});
            this.experimentalToolStripMenuItem.Name = "experimentalToolStripMenuItem";
            this.experimentalToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.experimentalToolStripMenuItem.Text = "Experimental";
            // 
            // enableDebuggerToolStripMenuItem
            // 
            this.enableDebuggerToolStripMenuItem.Name = "enableDebuggerToolStripMenuItem";
            this.enableDebuggerToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.enableDebuggerToolStripMenuItem.Text = "Enable Debugger";
            this.enableDebuggerToolStripMenuItem.Click += new System.EventHandler(this.EnableDebuggerToolStripMenuItem_Click);
            // 
            // rtbOutput
            // 
            this.rtbOutput.Dock = System.Windows.Forms.DockStyle.Right;
            this.rtbOutput.Location = new System.Drawing.Point(819, 104);
            this.rtbOutput.Name = "rtbOutput";
            this.rtbOutput.Size = new System.Drawing.Size(641, 402);
            this.rtbOutput.TabIndex = 3;
            this.rtbOutput.Text = "";
            this.rtbOutput.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rtbOutput_LinkClicked);
            // 
            // btnModsFolder
            // 
            this.btnModsFolder.Location = new System.Drawing.Point(11, 10);
            this.btnModsFolder.Name = "btnModsFolder";
            this.btnModsFolder.Size = new System.Drawing.Size(118, 32);
            this.btnModsFolder.TabIndex = 6;
            this.btnModsFolder.Text = "Mods Folder";
            this.btnModsFolder.UseVisualStyleBackColor = true;
            this.btnModsFolder.Click += new System.EventHandler(this.btnModsFolder_Click);
            // 
            // panButtons
            // 
            this.panButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panButtons.Location = new System.Drawing.Point(0, 506);
            this.panButtons.Name = "panButtons";
            this.panButtons.Size = new System.Drawing.Size(1460, 62);
            this.panButtons.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.chkAssetCopy);
            this.panel1.Controls.Add(this.chkHarmonyUpdate);
            this.panel1.Controls.Add(this.btnDebug);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.Play);
            this.panel1.Controls.Add(this.chkPlay);
            this.panel1.Controls.Add(this.chkAutoClose);
            this.panel1.Controls.Add(this.btnBuild);
            this.panel1.Controls.Add(this.btnModsFolder);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1460, 80);
            this.panel1.TabIndex = 9;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel1_Paint);
            // 
            // chkHarmonyUpdate
            // 
            this.chkHarmonyUpdate.AutoSize = true;
            this.chkHarmonyUpdate.Location = new System.Drawing.Point(674, 53);
            this.chkHarmonyUpdate.Name = "chkHarmonyUpdate";
            this.chkHarmonyUpdate.Size = new System.Drawing.Size(170, 17);
            this.chkHarmonyUpdate.TabIndex = 13;
            this.chkHarmonyUpdate.Text = "Attempt Harmony Auto Update";
            this.chkHarmonyUpdate.UseVisualStyleBackColor = true;
            this.chkHarmonyUpdate.CheckedChanged += new System.EventHandler(this.chkHarmonyUpdate_CheckedChanged);
            // 
            // btnDebug
            // 
            this.btnDebug.Location = new System.Drawing.Point(1124, 12);
            this.btnDebug.Name = "btnDebug";
            this.btnDebug.Size = new System.Drawing.Size(75, 32);
            this.btnDebug.TabIndex = 12;
            this.btnDebug.Text = "Debug Exe";
            this.btnDebug.UseVisualStyleBackColor = true;
            this.btnDebug.Visible = false;
            this.btnDebug.Click += new System.EventHandler(this.BtnDebug_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(454, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(118, 32);
            this.btnRefresh.TabIndex = 11;
            this.btnRefresh.Text = "Refresh Mods";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // Play
            // 
            this.Play.Location = new System.Drawing.Point(965, 12);
            this.Play.Name = "Play";
            this.Play.Size = new System.Drawing.Size(75, 32);
            this.Play.TabIndex = 10;
            this.Play.Text = "Play";
            this.Play.UseVisualStyleBackColor = true;
            this.Play.Click += new System.EventHandler(this.Play_Click);
            // 
            // chkPlay
            // 
            this.chkPlay.AutoSize = true;
            this.chkPlay.Location = new System.Drawing.Point(674, 30);
            this.chkPlay.Name = "chkPlay";
            this.chkPlay.Size = new System.Drawing.Size(95, 17);
            this.chkPlay.TabIndex = 9;
            this.chkPlay.Text = "Play after build";
            this.chkPlay.UseVisualStyleBackColor = true;
            this.chkPlay.CheckedChanged += new System.EventHandler(this.chkPlay_CheckedChanged);
            // 
            // chkAutoClose
            // 
            this.chkAutoClose.AutoSize = true;
            this.chkAutoClose.Location = new System.Drawing.Point(674, 12);
            this.chkAutoClose.Name = "chkAutoClose";
            this.chkAutoClose.Size = new System.Drawing.Size(77, 17);
            this.chkAutoClose.TabIndex = 8;
            this.chkAutoClose.Text = "Auto Close";
            this.chkAutoClose.UseVisualStyleBackColor = true;
            this.chkAutoClose.CheckedChanged += new System.EventHandler(this.chkAutoClose_CheckedChanged);
            // 
            // btnBuild
            // 
            this.btnBuild.Location = new System.Drawing.Point(806, 11);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(75, 32);
            this.btnBuild.TabIndex = 7;
            this.btnBuild.Text = "Build";
            this.btnBuild.UseVisualStyleBackColor = true;
            this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
            // 
            // diag
            // 
            this.diag.DefaultExt = "zip";
            this.diag.FileName = "diag";
            // 
            // chkAssetCopy
            // 
            this.chkAssetCopy.AutoSize = true;
            this.chkAssetCopy.Location = new System.Drawing.Point(454, 53);
            this.chkAssetCopy.Name = "chkAssetCopy";
            this.chkAssetCopy.Size = new System.Drawing.Size(103, 17);
            this.chkAssetCopy.TabIndex = 14;
            this.chkAssetCopy.Text = "Skip Asset Copy";
            this.chkAssetCopy.UseVisualStyleBackColor = true;
            this.chkAssetCopy.CheckedChanged += new System.EventHandler(this.chkAssetCopy_CheckedChanged);
            // 
            // makeReleaseToolStripMenuItem
            // 
            this.makeReleaseToolStripMenuItem.Name = "makeReleaseToolStripMenuItem";
            this.makeReleaseToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.makeReleaseToolStripMenuItem.Text = "MakeRelease";
            this.makeReleaseToolStripMenuItem.Click += new System.EventHandler(this.makeReleaseToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1460, 568);
            this.Controls.Add(this.lstMods);
            this.Controls.Add(this.rtbOutput);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panButtons);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "DMT";
            this.Activated += new System.EventHandler(this.frmMain_Activated);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lstMods;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader ColModName;
        private System.Windows.Forms.ColumnHeader ColAuthor;
        private System.Windows.Forms.ToolStripMenuItem mnuVersion;
        private System.Windows.Forms.RichTextBox rtbOutput;
        private System.Windows.Forms.Button btnModsFolder;
        private System.Windows.Forms.Panel panButtons;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.CheckBox chkPlay;
        public System.Windows.Forms.CheckBox chkAutoClose;
        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.Button Play;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ColumnHeader ColDesc;
        private System.Windows.Forms.ToolStripMenuItem madeByMachineElvesToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.Button btnDebug;
        private System.Windows.Forms.ToolStripMenuItem experimentalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enableDebuggerToolStripMenuItem;
        public System.Windows.Forms.CheckBox chkHarmonyUpdate;
        private System.Windows.Forms.ToolStripMenuItem manualUpdateToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog diag;
        public System.Windows.Forms.CheckBox chkAssetCopy;
        private System.Windows.Forms.ToolStripMenuItem makeReleaseToolStripMenuItem;
    }
}

