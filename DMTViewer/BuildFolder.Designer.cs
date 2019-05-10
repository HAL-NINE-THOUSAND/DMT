namespace DMT
{
    partial class BuildFolder
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnModFolder = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtLocation
            // 
            this.txtLocation.Location = new System.Drawing.Point(37, 3);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(330, 20);
            this.txtLocation.TabIndex = 0;
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(4, 3);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(27, 20);
            this.btnDel.TabIndex = 1;
            this.btnDel.Text = "X";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnModFolder
            // 
            this.btnModFolder.Location = new System.Drawing.Point(373, 3);
            this.btnModFolder.Name = "btnModFolder";
            this.btnModFolder.Size = new System.Drawing.Size(25, 23);
            this.btnModFolder.TabIndex = 4;
            this.btnModFolder.Text = "...";
            this.btnModFolder.UseVisualStyleBackColor = true;
            this.btnModFolder.Click += new System.EventHandler(this.btnModFolder_Click);
            // 
            // BuildFolder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnModFolder);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.txtLocation);
            this.Name = "BuildFolder";
            this.Size = new System.Drawing.Size(401, 29);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnModFolder;
    }
}
