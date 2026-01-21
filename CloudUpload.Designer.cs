namespace RakeSaveFileExporter
{
    partial class CloudUpload
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

        #region Bileşen Tasarımcısı üretimi kod

        /// <summary> 
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBarCloudUpload = new System.Windows.Forms.ProgressBar();
            this.lblStepCloudUpload3 = new System.Windows.Forms.Label();
            this.lblStepCloudUpload2 = new System.Windows.Forms.Label();
            this.lblStepCloudUpload1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBarCloudUpload
            // 
            this.progressBarCloudUpload.Location = new System.Drawing.Point(24, 83);
            this.progressBarCloudUpload.Name = "progressBarCloudUpload";
            this.progressBarCloudUpload.Size = new System.Drawing.Size(324, 13);
            this.progressBarCloudUpload.TabIndex = 19;
            this.progressBarCloudUpload.Click += new System.EventHandler(this.progressBarCloudUpload_Click);
            // 
            // lblStepCloudUpload3
            // 
            this.lblStepCloudUpload3.AutoSize = true;
            this.lblStepCloudUpload3.Location = new System.Drawing.Point(21, 53);
            this.lblStepCloudUpload3.Name = "lblStepCloudUpload3";
            this.lblStepCloudUpload3.Size = new System.Drawing.Size(141, 13);
            this.lblStepCloudUpload3.TabIndex = 18;
            this.lblStepCloudUpload3.Text = " Uploading save file to cloud";
            this.lblStepCloudUpload3.Click += new System.EventHandler(this.lblStepCloudUpload3_Click);
            // 
            // lblStepCloudUpload2
            // 
            this.lblStepCloudUpload2.AutoSize = true;
            this.lblStepCloudUpload2.Location = new System.Drawing.Point(21, 32);
            this.lblStepCloudUpload2.Name = "lblStepCloudUpload2";
            this.lblStepCloudUpload2.Size = new System.Drawing.Size(91, 13);
            this.lblStepCloudUpload2.TabIndex = 17;
            this.lblStepCloudUpload2.Text = " Creating save file";
            this.lblStepCloudUpload2.Click += new System.EventHandler(this.lblStepCloudUpload2_Click);
            // 
            // lblStepCloudUpload1
            // 
            this.lblStepCloudUpload1.AutoSize = true;
            this.lblStepCloudUpload1.Location = new System.Drawing.Point(21, 11);
            this.lblStepCloudUpload1.Name = "lblStepCloudUpload1";
            this.lblStepCloudUpload1.Size = new System.Drawing.Size(203, 13);
            this.lblStepCloudUpload1.TabIndex = 16;
            this.lblStepCloudUpload1.Text = " Searching save file on Windows Registry";
            this.lblStepCloudUpload1.Click += new System.EventHandler(this.lblStepCloudUpload1_Click);
            // 
            // CloudUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.progressBarCloudUpload);
            this.Controls.Add(this.lblStepCloudUpload3);
            this.Controls.Add(this.lblStepCloudUpload2);
            this.Controls.Add(this.lblStepCloudUpload1);
            this.Name = "CloudUpload";
            this.Size = new System.Drawing.Size(364, 131);
            this.Load += new System.EventHandler(this.CloudUpload_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBarCloudUpload;
        private System.Windows.Forms.Label lblStepCloudUpload3;
        private System.Windows.Forms.Label lblStepCloudUpload2;
        private System.Windows.Forms.Label lblStepCloudUpload1;
    }
}
