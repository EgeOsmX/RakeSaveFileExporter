namespace RakeSaveFileExporter
{
    partial class App
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.exportBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.loadBtn = new System.Windows.Forms.Button();
            this.labelLoadInfo = new System.Windows.Forms.Label();
            this.BMPIcon = new System.Windows.Forms.PictureBox();
            this.uploadCloudBtn = new System.Windows.Forms.Button();
            this.loadCloudBtn = new System.Windows.Forms.Button();
            this.viewCloudBtn = new System.Windows.Forms.Button();
            this.uploadCloudIcon = new System.Windows.Forms.PictureBox();
            this.loadCloudIcon = new System.Windows.Forms.PictureBox();
            this.viewCloudIcon = new System.Windows.Forms.PictureBox();
            this.panelActive = new System.Windows.Forms.Panel();
            this.stripMenu = new System.Windows.Forms.MenuStrip();
            this.stripMenuTab1 = new System.Windows.Forms.ToolStripMenuItem();
            this.stripMenuTab2 = new System.Windows.Forms.ToolStripMenuItem();
            this.stripMenuTab2Ver = new System.Windows.Forms.ToolStripMenuItem();
            this.stripMenuTab2Made = new System.Windows.Forms.ToolStripMenuItem();
            this.stripMenuTab2VisitGitHub = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.BMPIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uploadCloudIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.loadCloudIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewCloudIcon)).BeginInit();
            this.stripMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // exportBtn
            // 
            this.exportBtn.Location = new System.Drawing.Point(64, 237);
            this.exportBtn.Name = "exportBtn";
            this.exportBtn.Size = new System.Drawing.Size(123, 30);
            this.exportBtn.TabIndex = 0;
            this.exportBtn.Text = "EXPORT SAVE FILE";
            this.exportBtn.UseVisualStyleBackColor = true;
            this.exportBtn.Click += new System.EventHandler(this.exportBtn_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(-2, 173);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(378, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Rake Save File Exporter";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(-2, 200);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(378, 29);
            this.label2.TabIndex = 3;
            this.label2.Text = "Export your Rake save file and continue your progress\nnon any computer without st" +
    "arting over.";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 163);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "v1.1";
            // 
            // loadBtn
            // 
            this.loadBtn.Location = new System.Drawing.Point(188, 237);
            this.loadBtn.Name = "loadBtn";
            this.loadBtn.Size = new System.Drawing.Size(123, 30);
            this.loadBtn.TabIndex = 9;
            this.loadBtn.Text = "LOAD SAVE FILE";
            this.loadBtn.UseVisualStyleBackColor = true;
            this.loadBtn.Click += new System.EventHandler(this.loadBtn_Click);
            // 
            // labelLoadInfo
            // 
            this.labelLoadInfo.BackColor = System.Drawing.SystemColors.Control;
            this.labelLoadInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelLoadInfo.Location = new System.Drawing.Point(-2, 312);
            this.labelLoadInfo.Name = "labelLoadInfo";
            this.labelLoadInfo.Size = new System.Drawing.Size(378, 25);
            this.labelLoadInfo.TabIndex = 10;
            this.labelLoadInfo.Text = "After loading the save file, your current progress will be permanently removed\nan" +
    "d replaced with the progress from the selected save file.";
            this.labelLoadInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelLoadInfo.Click += new System.EventHandler(this.labelLoadInfo_Click);
            // 
            // BMPIcon
            // 
            this.BMPIcon.Location = new System.Drawing.Point(0, 23);
            this.BMPIcon.Name = "BMPIcon";
            this.BMPIcon.Size = new System.Drawing.Size(376, 137);
            this.BMPIcon.TabIndex = 16;
            this.BMPIcon.TabStop = false;
            this.BMPIcon.Click += new System.EventHandler(this.BMPIcon_Click);
            // 
            // uploadCloudBtn
            // 
            this.uploadCloudBtn.Location = new System.Drawing.Point(16, 271);
            this.uploadCloudBtn.Name = "uploadCloudBtn";
            this.uploadCloudBtn.Size = new System.Drawing.Size(119, 34);
            this.uploadCloudBtn.TabIndex = 17;
            this.uploadCloudBtn.Text = "UPLOAD SAVE\nFILE TO CLOUD";
            this.uploadCloudBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.uploadCloudBtn.UseVisualStyleBackColor = true;
            this.uploadCloudBtn.Click += new System.EventHandler(this.uploadCloudBtn_Click);
            // 
            // loadCloudBtn
            // 
            this.loadCloudBtn.Location = new System.Drawing.Point(136, 271);
            this.loadCloudBtn.Name = "loadCloudBtn";
            this.loadCloudBtn.Size = new System.Drawing.Size(125, 34);
            this.loadCloudBtn.TabIndex = 18;
            this.loadCloudBtn.Text = "LOAD SAVE FILE\nFROM CLOUD";
            this.loadCloudBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.loadCloudBtn.UseVisualStyleBackColor = true;
            this.loadCloudBtn.Click += new System.EventHandler(this.loadCloudBtn_Click);
            // 
            // viewCloudBtn
            // 
            this.viewCloudBtn.Location = new System.Drawing.Point(262, 271);
            this.viewCloudBtn.Name = "viewCloudBtn";
            this.viewCloudBtn.Size = new System.Drawing.Size(98, 34);
            this.viewCloudBtn.TabIndex = 19;
            this.viewCloudBtn.Text = "VIEW FILES\nON CLOUD";
            this.viewCloudBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.viewCloudBtn.UseVisualStyleBackColor = true;
            this.viewCloudBtn.Click += new System.EventHandler(this.viewCloudBtn_Click);
            // 
            // uploadCloudIcon
            // 
            this.uploadCloudIcon.BackColor = System.Drawing.Color.Transparent;
            this.uploadCloudIcon.Location = new System.Drawing.Point(23, 278);
            this.uploadCloudIcon.Name = "uploadCloudIcon";
            this.uploadCloudIcon.Size = new System.Drawing.Size(20, 20);
            this.uploadCloudIcon.TabIndex = 20;
            this.uploadCloudIcon.TabStop = false;
            this.uploadCloudIcon.Click += new System.EventHandler(this.uploadCloudIcon_Click);
            // 
            // loadCloudIcon
            // 
            this.loadCloudIcon.BackColor = System.Drawing.Color.Transparent;
            this.loadCloudIcon.Location = new System.Drawing.Point(143, 278);
            this.loadCloudIcon.Name = "loadCloudIcon";
            this.loadCloudIcon.Size = new System.Drawing.Size(20, 20);
            this.loadCloudIcon.TabIndex = 21;
            this.loadCloudIcon.TabStop = false;
            this.loadCloudIcon.Click += new System.EventHandler(this.loadCloudIcon_Click);
            // 
            // viewCloudIcon
            // 
            this.viewCloudIcon.BackColor = System.Drawing.Color.Transparent;
            this.viewCloudIcon.Location = new System.Drawing.Point(268, 278);
            this.viewCloudIcon.Name = "viewCloudIcon";
            this.viewCloudIcon.Size = new System.Drawing.Size(20, 20);
            this.viewCloudIcon.TabIndex = 22;
            this.viewCloudIcon.TabStop = false;
            this.viewCloudIcon.Click += new System.EventHandler(this.viewCloudIcon_Click);
            // 
            // panelActive
            // 
            this.panelActive.Location = new System.Drawing.Point(0, 351);
            this.panelActive.Name = "panelActive";
            this.panelActive.Size = new System.Drawing.Size(376, 175);
            this.panelActive.TabIndex = 23;
            this.panelActive.Paint += new System.Windows.Forms.PaintEventHandler(this.panelActive_Paint);
            // 
            // stripMenu
            // 
            this.stripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stripMenuTab1,
            this.stripMenuTab2});
            this.stripMenu.Location = new System.Drawing.Point(0, 0);
            this.stripMenu.Name = "stripMenu";
            this.stripMenu.Size = new System.Drawing.Size(374, 24);
            this.stripMenu.TabIndex = 24;
            this.stripMenu.Text = "menuStrip";
            // 
            // stripMenuTab1
            // 
            this.stripMenuTab1.Name = "stripMenuTab1";
            this.stripMenuTab1.Size = new System.Drawing.Size(64, 20);
            this.stripMenuTab1.Text = "Account";
            this.stripMenuTab1.Click += new System.EventHandler(this.stripMenu_Click);
            // 
            // stripMenuTab2
            // 
            this.stripMenuTab2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stripMenuTab2VisitGitHub,
            this.stripMenuTab2Made,
            this.stripMenuTab2Ver});
            this.stripMenuTab2.Name = "stripMenuTab2";
            this.stripMenuTab2.Size = new System.Drawing.Size(52, 20);
            this.stripMenuTab2.Text = "About";
            // 
            // stripMenuTab2Ver
            // 
            this.stripMenuTab2Ver.Enabled = false;
            this.stripMenuTab2Ver.Name = "stripMenuTab2Ver";
            this.stripMenuTab2Ver.Size = new System.Drawing.Size(221, 22);
            this.stripMenuTab2Ver.Text = "v1.1";
            // 
            // stripMenuTab2Made
            // 
            this.stripMenuTab2Made.Enabled = false;
            this.stripMenuTab2Made.Name = "stripMenuTab2Made";
            this.stripMenuTab2Made.Size = new System.Drawing.Size(221, 22);
            this.stripMenuTab2Made.Text = "Made with ❤︎ in Frankfurt 🇩🇪";
            // 
            // stripMenuTab2VisitGitHub
            // 
            this.stripMenuTab2VisitGitHub.Name = "stripMenuTab2VisitGitHub";
            this.stripMenuTab2VisitGitHub.Size = new System.Drawing.Size(221, 22);
            this.stripMenuTab2VisitGitHub.Text = "Visit Project GitHub Page";
            this.stripMenuTab2VisitGitHub.Click += new System.EventHandler(this.stripMenuTab2VisitGitHub_Click);
            // 
            // App
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 353);
            this.Controls.Add(this.panelActive);
            this.Controls.Add(this.viewCloudIcon);
            this.Controls.Add(this.loadCloudIcon);
            this.Controls.Add(this.uploadCloudIcon);
            this.Controls.Add(this.viewCloudBtn);
            this.Controls.Add(this.loadCloudBtn);
            this.Controls.Add(this.uploadCloudBtn);
            this.Controls.Add(this.BMPIcon);
            this.Controls.Add(this.labelLoadInfo);
            this.Controls.Add(this.loadBtn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.exportBtn);
            this.Controls.Add(this.stripMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.stripMenu;
            this.MaximizeBox = false;
            this.Name = "App";
            this.ShowIcon = false;
            this.Tag = "393";
            this.Text = "Rake Save File Exporter - by EgeOsmX";
            this.Load += new System.EventHandler(this.Menu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BMPIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uploadCloudIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.loadCloudIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewCloudIcon)).EndInit();
            this.stripMenu.ResumeLayout(false);
            this.stripMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button exportBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button loadBtn;
        private System.Windows.Forms.Label labelLoadInfo;
        private System.Windows.Forms.PictureBox BMPIcon;
        private System.Windows.Forms.Button uploadCloudBtn;
        private System.Windows.Forms.Button loadCloudBtn;
        private System.Windows.Forms.Button viewCloudBtn;
        private System.Windows.Forms.PictureBox uploadCloudIcon;
        private System.Windows.Forms.PictureBox loadCloudIcon;
        private System.Windows.Forms.PictureBox viewCloudIcon;
        private System.Windows.Forms.Panel panelActive;
        private System.Windows.Forms.MenuStrip stripMenu;
        private System.Windows.Forms.ToolStripMenuItem stripMenuTab1;
        private System.Windows.Forms.ToolStripMenuItem stripMenuTab2;
        private System.Windows.Forms.ToolStripMenuItem stripMenuTab2Ver;
        private System.Windows.Forms.ToolStripMenuItem stripMenuTab2Made;
        private System.Windows.Forms.ToolStripMenuItem stripMenuTab2VisitGitHub;
    }
}

