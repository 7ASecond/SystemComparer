namespace SystemCompare
{
    partial class FrmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.btnSnapshot = new System.Windows.Forms.Button();
            this.btnCompareSnapshots = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblFiles = new System.Windows.Forms.Label();
            this.lblKeys = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.Progress = new System.Windows.Forms.ProgressBar();
            this.OFileD = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // btnSnapshot
            // 
            this.btnSnapshot.BackColor = System.Drawing.Color.MintCream;
            this.btnSnapshot.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSnapshot.Location = new System.Drawing.Point(13, 13);
            this.btnSnapshot.Name = "btnSnapshot";
            this.btnSnapshot.Size = new System.Drawing.Size(345, 67);
            this.btnSnapshot.TabIndex = 0;
            this.btnSnapshot.Text = "Take Snapshot";
            this.btnSnapshot.UseVisualStyleBackColor = false;
            this.btnSnapshot.Click += new System.EventHandler(this.btnSnapshot_Click);
            // 
            // btnCompareSnapshots
            // 
            this.btnCompareSnapshots.BackColor = System.Drawing.Color.AntiqueWhite;
            this.btnCompareSnapshots.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCompareSnapshots.Location = new System.Drawing.Point(13, 190);
            this.btnCompareSnapshots.Name = "btnCompareSnapshots";
            this.btnCompareSnapshots.Size = new System.Drawing.Size(345, 67);
            this.btnCompareSnapshots.TabIndex = 1;
            this.btnCompareSnapshots.Text = "Compare Snapshots";
            this.btnCompareSnapshots.UseVisualStyleBackColor = false;
            this.btnCompareSnapshots.Click += new System.EventHandler(this.btnCompareSnapshots_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Files";
            // 
            // lblFiles
            // 
            this.lblFiles.AutoSize = true;
            this.lblFiles.Location = new System.Drawing.Point(44, 93);
            this.lblFiles.Name = "lblFiles";
            this.lblFiles.Size = new System.Drawing.Size(13, 13);
            this.lblFiles.TabIndex = 3;
            this.lblFiles.Text = "0";
            // 
            // lblKeys
            // 
            this.lblKeys.AutoSize = true;
            this.lblKeys.Location = new System.Drawing.Point(44, 117);
            this.lblKeys.Name = "lblKeys";
            this.lblKeys.Size = new System.Drawing.Size(13, 13);
            this.lblKeys.TabIndex = 5;
            this.lblKeys.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Keys";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 278);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Status:";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(56, 278);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(16, 13);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "...";
            // 
            // Progress
            // 
            this.Progress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Progress.Location = new System.Drawing.Point(0, 304);
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(370, 10);
            this.Progress.Step = 1;
            this.Progress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.Progress.TabIndex = 8;
            // 
            // OFileD
            // 
            this.OFileD.DefaultExt = "txt";
            this.OFileD.ReadOnlyChecked = true;
            this.OFileD.RestoreDirectory = true;
            this.OFileD.ShowReadOnly = true;
            this.OFileD.SupportMultiDottedExtensions = true;
            this.OFileD.Title = "Load Registry File";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 314);
            this.Controls.Add(this.Progress);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblKeys);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblFiles);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCompareSnapshots);
            this.Controls.Add(this.btnSnapshot);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMain";
            this.Text = "System Compare 2016";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSnapshot;
        private System.Windows.Forms.Button btnCompareSnapshots;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblFiles;
        private System.Windows.Forms.Label lblKeys;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar Progress;
        private System.Windows.Forms.OpenFileDialog OFileD;
    }
}

