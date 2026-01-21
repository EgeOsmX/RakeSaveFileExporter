namespace RakeSaveFileExporter
{
    partial class LocalLoad
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
            this.progressBarLoad = new System.Windows.Forms.ProgressBar();
            this.lblStepLoad4 = new System.Windows.Forms.Label();
            this.lblStepLoad3 = new System.Windows.Forms.Label();
            this.lblStepLoad2 = new System.Windows.Forms.Label();
            this.lblStepLoad1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBarLoad
            // 
            this.progressBarLoad.Location = new System.Drawing.Point(25, 103);
            this.progressBarLoad.Name = "progressBarLoad";
            this.progressBarLoad.Size = new System.Drawing.Size(324, 13);
            this.progressBarLoad.TabIndex = 20;
            this.progressBarLoad.Visible = false;
            this.progressBarLoad.Click += new System.EventHandler(this.progressBarLoad_Click);
            // 
            // lblStepLoad4
            // 
            this.lblStepLoad4.AutoSize = true;
            this.lblStepLoad4.Location = new System.Drawing.Point(21, 76);
            this.lblStepLoad4.Name = "lblStepLoad4";
            this.lblStepLoad4.Size = new System.Drawing.Size(90, 13);
            this.lblStepLoad4.TabIndex = 19;
            this.lblStepLoad4.Tag = "Copying save file";
            this.lblStepLoad4.Text = " Copying save file";
            this.lblStepLoad4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStepLoad4.Visible = false;
            this.lblStepLoad4.Click += new System.EventHandler(this.lblStepLoad4_Click);
            // 
            // lblStepLoad3
            // 
            this.lblStepLoad3.AutoSize = true;
            this.lblStepLoad3.Location = new System.Drawing.Point(21, 54);
            this.lblStepLoad3.Name = "lblStepLoad3";
            this.lblStepLoad3.Size = new System.Drawing.Size(91, 13);
            this.lblStepLoad3.TabIndex = 18;
            this.lblStepLoad3.Tag = "Preparing registry";
            this.lblStepLoad3.Text = " Preparing registry";
            this.lblStepLoad3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStepLoad3.Visible = false;
            this.lblStepLoad3.Click += new System.EventHandler(this.lblStepLoad3_Click);
            // 
            // lblStepLoad2
            // 
            this.lblStepLoad2.AutoSize = true;
            this.lblStepLoad2.Location = new System.Drawing.Point(21, 33);
            this.lblStepLoad2.Name = "lblStepLoad2";
            this.lblStepLoad2.Size = new System.Drawing.Size(98, 13);
            this.lblStepLoad2.TabIndex = 17;
            this.lblStepLoad2.Tag = "Validating save file";
            this.lblStepLoad2.Text = " Validating save file";
            this.lblStepLoad2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStepLoad2.Visible = false;
            this.lblStepLoad2.Click += new System.EventHandler(this.lblStepLoad2_Click);
            // 
            // lblStepLoad1
            // 
            this.lblStepLoad1.AutoSize = true;
            this.lblStepLoad1.Location = new System.Drawing.Point(21, 12);
            this.lblStepLoad1.Name = "lblStepLoad1";
            this.lblStepLoad1.Size = new System.Drawing.Size(96, 13);
            this.lblStepLoad1.TabIndex = 16;
            this.lblStepLoad1.Tag = "Selecting save file";
            this.lblStepLoad1.Text = " Selecting save file";
            this.lblStepLoad1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStepLoad1.Visible = false;
            this.lblStepLoad1.Click += new System.EventHandler(this.lblStepLoad1_Click);
            // 
            // LocalLoad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.progressBarLoad);
            this.Controls.Add(this.lblStepLoad4);
            this.Controls.Add(this.lblStepLoad3);
            this.Controls.Add(this.lblStepLoad2);
            this.Controls.Add(this.lblStepLoad1);
            this.Name = "LocalLoad";
            this.Size = new System.Drawing.Size(364, 139);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBarLoad;
        private System.Windows.Forms.Label lblStepLoad4;
        private System.Windows.Forms.Label lblStepLoad3;
        private System.Windows.Forms.Label lblStepLoad2;
        private System.Windows.Forms.Label lblStepLoad1;
    }
}
