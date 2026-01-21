namespace RakeSaveFileExporter
{
    partial class LocalExport
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
            this.lblStep3 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblStep2 = new System.Windows.Forms.Label();
            this.lblStep1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblStep3
            // 
            this.lblStep3.AutoSize = true;
            this.lblStep3.Location = new System.Drawing.Point(20, 53);
            this.lblStep3.Name = "lblStep3";
            this.lblStep3.Size = new System.Drawing.Size(90, 13);
            this.lblStep3.TabIndex = 11;
            this.lblStep3.Text = " Copying save file";
            this.lblStep3.Visible = false;
            this.lblStep3.Click += new System.EventHandler(this.lblStep3_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(24, 83);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(324, 13);
            this.progressBar.TabIndex = 10;
            this.progressBar.Visible = false;
            this.progressBar.Click += new System.EventHandler(this.progressBar_Click);
            // 
            // lblStep2
            // 
            this.lblStep2.AutoSize = true;
            this.lblStep2.Location = new System.Drawing.Point(20, 32);
            this.lblStep2.Name = "lblStep2";
            this.lblStep2.Size = new System.Drawing.Size(91, 13);
            this.lblStep2.TabIndex = 9;
            this.lblStep2.Text = " Creating save file";
            this.lblStep2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStep2.Visible = false;
            this.lblStep2.Click += new System.EventHandler(this.lblStep2_Click);
            // 
            // lblStep1
            // 
            this.lblStep1.AutoSize = true;
            this.lblStep1.Location = new System.Drawing.Point(20, 11);
            this.lblStep1.Name = "lblStep1";
            this.lblStep1.Size = new System.Drawing.Size(203, 13);
            this.lblStep1.TabIndex = 8;
            this.lblStep1.Text = " Searching save file on Windows Registry";
            this.lblStep1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStep1.Visible = false;
            this.lblStep1.Click += new System.EventHandler(this.lblStep1_Click);
            // 
            // LocalExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblStep3);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblStep2);
            this.Controls.Add(this.lblStep1);
            this.Name = "LocalExport";
            this.Size = new System.Drawing.Size(364, 126);
            this.Load += new System.EventHandler(this.LocalExport_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStep3;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblStep2;
        private System.Windows.Forms.Label lblStep1;
    }
}
