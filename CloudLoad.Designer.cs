namespace RakeSaveFileExporter
{
    partial class CloudLoad
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
            this.progressBarCloudLoad = new System.Windows.Forms.ProgressBar();
            this.lblStepCloudLoad3 = new System.Windows.Forms.Label();
            this.lblStepCloudLoad2 = new System.Windows.Forms.Label();
            this.lblStepCloudLoad1 = new System.Windows.Forms.Label();
            this.lblStepCloudLoad4 = new System.Windows.Forms.Label();
            this.lblStepCloudLoad5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBarCloudLoad
            // 
            this.progressBarCloudLoad.Location = new System.Drawing.Point(24, 121);
            this.progressBarCloudLoad.Name = "progressBarCloudLoad";
            this.progressBarCloudLoad.Size = new System.Drawing.Size(324, 13);
            this.progressBarCloudLoad.TabIndex = 23;
            this.progressBarCloudLoad.Click += new System.EventHandler(this.progressBarCloudLoad_Click);
            // 
            // lblStepCloudLoad3
            // 
            this.lblStepCloudLoad3.AutoSize = true;
            this.lblStepCloudLoad3.Location = new System.Drawing.Point(21, 53);
            this.lblStepCloudLoad3.Name = "lblStepCloudLoad3";
            this.lblStepCloudLoad3.Size = new System.Drawing.Size(98, 13);
            this.lblStepCloudLoad3.TabIndex = 22;
            this.lblStepCloudLoad3.Text = " Validating save file";
            this.lblStepCloudLoad3.Click += new System.EventHandler(this.lblStepCloudLoad3_Click);
            // 
            // lblStepCloudLoad2
            // 
            this.lblStepCloudLoad2.AutoSize = true;
            this.lblStepCloudLoad2.Location = new System.Drawing.Point(21, 32);
            this.lblStepCloudLoad2.Name = "lblStepCloudLoad2";
            this.lblStepCloudLoad2.Size = new System.Drawing.Size(114, 13);
            this.lblStepCloudLoad2.TabIndex = 21;
            this.lblStepCloudLoad2.Text = " Downloading save file";
            this.lblStepCloudLoad2.Click += new System.EventHandler(this.lblStepCloudLoad2_Click);
            // 
            // lblStepCloudLoad1
            // 
            this.lblStepCloudLoad1.AutoSize = true;
            this.lblStepCloudLoad1.Location = new System.Drawing.Point(21, 11);
            this.lblStepCloudLoad1.Name = "lblStepCloudLoad1";
            this.lblStepCloudLoad1.Size = new System.Drawing.Size(96, 13);
            this.lblStepCloudLoad1.TabIndex = 20;
            this.lblStepCloudLoad1.Text = " Selecting save file";
            this.lblStepCloudLoad1.Click += new System.EventHandler(this.lblStepCloudLoad1_Click);
            // 
            // lblStepCloudLoad4
            // 
            this.lblStepCloudLoad4.AutoSize = true;
            this.lblStepCloudLoad4.Location = new System.Drawing.Point(21, 74);
            this.lblStepCloudLoad4.Name = "lblStepCloudLoad4";
            this.lblStepCloudLoad4.Size = new System.Drawing.Size(139, 13);
            this.lblStepCloudLoad4.TabIndex = 24;
            this.lblStepCloudLoad4.Text = " Preparing Registry directory";
            this.lblStepCloudLoad4.Click += new System.EventHandler(this.lblStepCloudLoad4_Click);
            // 
            // lblStepCloudLoad5
            // 
            this.lblStepCloudLoad5.AutoSize = true;
            this.lblStepCloudLoad5.Location = new System.Drawing.Point(21, 95);
            this.lblStepCloudLoad5.Name = "lblStepCloudLoad5";
            this.lblStepCloudLoad5.Size = new System.Drawing.Size(90, 13);
            this.lblStepCloudLoad5.TabIndex = 25;
            this.lblStepCloudLoad5.Text = " Copying save file";
            this.lblStepCloudLoad5.Click += new System.EventHandler(this.lblStepCloudLoad5_Click);
            // 
            // CloudLoad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblStepCloudLoad5);
            this.Controls.Add(this.lblStepCloudLoad4);
            this.Controls.Add(this.progressBarCloudLoad);
            this.Controls.Add(this.lblStepCloudLoad3);
            this.Controls.Add(this.lblStepCloudLoad2);
            this.Controls.Add(this.lblStepCloudLoad1);
            this.Name = "CloudLoad";
            this.Size = new System.Drawing.Size(364, 150);
            this.Load += new System.EventHandler(this.CloudLoad_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBarCloudLoad;
        private System.Windows.Forms.Label lblStepCloudLoad3;
        private System.Windows.Forms.Label lblStepCloudLoad2;
        private System.Windows.Forms.Label lblStepCloudLoad1;
        private System.Windows.Forms.Label lblStepCloudLoad4;
        private System.Windows.Forms.Label lblStepCloudLoad5;
    }
}
