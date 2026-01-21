namespace Rake_Save_File_Exporter
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
            this.BMPIcon = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblStep1 = new System.Windows.Forms.Label();
            this.lblStep2 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblStep3 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.loadBtn = new System.Windows.Forms.Button();
            this.labelLoadInfo = new System.Windows.Forms.Label();
            this.lblStepLoad1 = new System.Windows.Forms.Label();
            this.lblStepLoad2 = new System.Windows.Forms.Label();
            this.lblStepLoad3 = new System.Windows.Forms.Label();
            this.lblStepLoad4 = new System.Windows.Forms.Label();
            this.progressBarLoad = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.BMPIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // exportBtn
            // 
            this.exportBtn.Location = new System.Drawing.Point(63, 202);
            this.exportBtn.Name = "exportBtn";
            this.exportBtn.Size = new System.Drawing.Size(123, 30);
            this.exportBtn.TabIndex = 0;
            this.exportBtn.Text = "EXPORT SAVE FILE";
            this.exportBtn.UseVisualStyleBackColor = true;
            this.exportBtn.Click += new System.EventHandler(this.exportBtn_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(-2, 138);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(378, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "LABEL1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // BMPIcon
            // 
            this.BMPIcon.ImageLocation = "";
            this.BMPIcon.Location = new System.Drawing.Point(-2, 0);
            this.BMPIcon.Name = "BMPIcon";
            this.BMPIcon.Size = new System.Drawing.Size(378, 126);
            this.BMPIcon.TabIndex = 2;
            this.BMPIcon.TabStop = false;
            this.BMPIcon.Tag = "";
            this.BMPIcon.Click += new System.EventHandler(this.BMPIcon_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(-2, 165);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(378, 29);
            this.label2.TabIndex = 3;
            this.label2.Text = "LABEL2";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // lblStep1
            // 
            this.lblStep1.AutoSize = true;
            this.lblStep1.Location = new System.Drawing.Point(19, 292);
            this.lblStep1.Name = "lblStep1";
            this.lblStep1.Size = new System.Drawing.Size(60, 13);
            this.lblStep1.TabIndex = 4;
            this.lblStep1.Text = "LBLSTEP1";
            this.lblStep1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStep1.Visible = false;
            this.lblStep1.Click += new System.EventHandler(this.lblStep1_Click);
            // 
            // lblStep2
            // 
            this.lblStep2.AutoSize = true;
            this.lblStep2.Location = new System.Drawing.Point(19, 314);
            this.lblStep2.Name = "lblStep2";
            this.lblStep2.Size = new System.Drawing.Size(60, 13);
            this.lblStep2.TabIndex = 5;
            this.lblStep2.Text = "LBLSTEP2";
            this.lblStep2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStep2.Visible = false;
            this.lblStep2.Click += new System.EventHandler(this.lblStep2_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(23, 365);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(324, 13);
            this.progressBar.TabIndex = 6;
            this.progressBar.Visible = false;
            this.progressBar.Click += new System.EventHandler(this.progressBar_Click);
            // 
            // lblStep3
            // 
            this.lblStep3.AutoSize = true;
            this.lblStep3.Location = new System.Drawing.Point(19, 336);
            this.lblStep3.Name = "lblStep3";
            this.lblStep3.Size = new System.Drawing.Size(60, 13);
            this.lblStep3.TabIndex = 7;
            this.lblStep3.Text = "LBLSTEP3";
            this.lblStep3.Visible = false;
            this.lblStep3.Click += new System.EventHandler(this.lblStep3_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "v1.0";
            // 
            // loadBtn
            // 
            this.loadBtn.Location = new System.Drawing.Point(189, 202);
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
            this.labelLoadInfo.Location = new System.Drawing.Point(-2, 242);
            this.labelLoadInfo.Name = "labelLoadInfo";
            this.labelLoadInfo.Size = new System.Drawing.Size(378, 25);
            this.labelLoadInfo.TabIndex = 10;
            this.labelLoadInfo.Text = "labelLoadInfo";
            this.labelLoadInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelLoadInfo.Click += new System.EventHandler(this.labelLoadInfo_Click);
            // 
            // lblStepLoad1
            // 
            this.lblStepLoad1.AutoSize = true;
            this.lblStepLoad1.Location = new System.Drawing.Point(19, 286);
            this.lblStepLoad1.Name = "lblStepLoad1";
            this.lblStepLoad1.Size = new System.Drawing.Size(89, 13);
            this.lblStepLoad1.TabIndex = 11;
            this.lblStepLoad1.Tag = "Selecting save file";
            this.lblStepLoad1.Text = "LBLSTEPLOAD1";
            this.lblStepLoad1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStepLoad1.Visible = false;
            this.lblStepLoad1.Click += new System.EventHandler(this.lblStepLoad1_Click);
            // 
            // lblStepLoad2
            // 
            this.lblStepLoad2.AutoSize = true;
            this.lblStepLoad2.Location = new System.Drawing.Point(19, 307);
            this.lblStepLoad2.Name = "lblStepLoad2";
            this.lblStepLoad2.Size = new System.Drawing.Size(89, 13);
            this.lblStepLoad2.TabIndex = 12;
            this.lblStepLoad2.Tag = "Validating save file";
            this.lblStepLoad2.Text = "LBLSTEPLOAD2";
            this.lblStepLoad2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStepLoad2.Visible = false;
            this.lblStepLoad2.Click += new System.EventHandler(this.lblStepLoad2_Click);
            // 
            // lblStepLoad3
            // 
            this.lblStepLoad3.AutoSize = true;
            this.lblStepLoad3.Location = new System.Drawing.Point(19, 329);
            this.lblStepLoad3.Name = "lblStepLoad3";
            this.lblStepLoad3.Size = new System.Drawing.Size(89, 13);
            this.lblStepLoad3.TabIndex = 13;
            this.lblStepLoad3.Tag = "Preparing registry";
            this.lblStepLoad3.Text = "LBLSTEPLOAD3";
            this.lblStepLoad3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStepLoad3.Visible = false;
            this.lblStepLoad3.Click += new System.EventHandler(this.lblStepLoad3_Click);
            // 
            // lblStepLoad4
            // 
            this.lblStepLoad4.AutoSize = true;
            this.lblStepLoad4.Location = new System.Drawing.Point(19, 351);
            this.lblStepLoad4.Name = "lblStepLoad4";
            this.lblStepLoad4.Size = new System.Drawing.Size(89, 13);
            this.lblStepLoad4.TabIndex = 14;
            this.lblStepLoad4.Tag = "Copying save file";
            this.lblStepLoad4.Text = "LBLSTEPLOAD4";
            this.lblStepLoad4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStepLoad4.Visible = false;
            this.lblStepLoad4.Click += new System.EventHandler(this.lblStepLoad4_Click);
            // 
            // progressBarLoad
            // 
            this.progressBarLoad.Location = new System.Drawing.Point(23, 377);
            this.progressBarLoad.Name = "progressBarLoad";
            this.progressBarLoad.Size = new System.Drawing.Size(324, 13);
            this.progressBarLoad.TabIndex = 15;
            this.progressBarLoad.Visible = false;
            this.progressBarLoad.Click += new System.EventHandler(this.progressBarLoad_Click);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 277);
            this.Controls.Add(this.progressBarLoad);
            this.Controls.Add(this.lblStepLoad4);
            this.Controls.Add(this.lblStepLoad3);
            this.Controls.Add(this.lblStepLoad2);
            this.Controls.Add(this.lblStepLoad1);
            this.Controls.Add(this.labelLoadInfo);
            this.Controls.Add(this.loadBtn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblStep3);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblStep2);
            this.Controls.Add(this.lblStep1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BMPIcon);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.exportBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "Menu";
            this.ShowIcon = false;
            this.Tag = "321";
            this.Text = "Rake Save File Exporter - by EgeOsmX";
            this.Load += new System.EventHandler(this.Menu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BMPIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button exportBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox BMPIcon;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblStep1;
        private System.Windows.Forms.Label lblStep2;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblStep3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button loadBtn;
        private System.Windows.Forms.Label labelLoadInfo;
        private System.Windows.Forms.Label lblStepLoad1;
        private System.Windows.Forms.Label lblStepLoad2;
        private System.Windows.Forms.Label lblStepLoad3;
        private System.Windows.Forms.Label lblStepLoad4;
        private System.Windows.Forms.ProgressBar progressBarLoad;
    }
}

