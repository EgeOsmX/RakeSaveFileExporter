namespace RakeSaveFileExporter
{
    partial class CloudLoad_R2
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
            this.components = new System.ComponentModel.Container();
            this.listR2 = new System.Windows.Forms.ListView();
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblCloudStorage = new System.Windows.Forms.Label();
            this.progressBarCloudStorage = new System.Windows.Forms.ProgressBar();
            this.RMBMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SuspendLayout();
            // 
            // listR2
            // 
            this.listR2.AutoArrange = false;
            this.listR2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnDate,
            this.columnSize});
            this.listR2.FullRowSelect = true;
            this.listR2.GridLines = true;
            this.listR2.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listR2.HideSelection = false;
            this.listR2.Location = new System.Drawing.Point(-1, -1);
            this.listR2.MultiSelect = false;
            this.listR2.Name = "listR2";
            this.listR2.Size = new System.Drawing.Size(417, 231);
            this.listR2.TabIndex = 0;
            this.listR2.UseCompatibleStateImageBehavior = false;
            this.listR2.View = System.Windows.Forms.View.Details;
            this.listR2.SelectedIndexChanged += new System.EventHandler(this.listR2_SelectedIndexChanged);
            // 
            // columnName
            // 
            this.columnName.Text = "Name";
            this.columnName.Width = 170;
            // 
            // columnDate
            // 
            this.columnDate.Text = "Last Modified";
            this.columnDate.Width = 140;
            // 
            // columnSize
            // 
            this.columnSize.Text = "Size";
            this.columnSize.Width = 110;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(334, 236);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 1;
            this.btnLoad.Text = "LOAD";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(255, 236);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblCloudStorage
            // 
            this.lblCloudStorage.Location = new System.Drawing.Point(9, 237);
            this.lblCloudStorage.Name = "lblCloudStorage";
            this.lblCloudStorage.Size = new System.Drawing.Size(233, 14);
            this.lblCloudStorage.TabIndex = 3;
            this.lblCloudStorage.Text = "Cloud Storage: 0.0 MB / 0.0 MB";
            this.lblCloudStorage.Click += new System.EventHandler(this.lblCloudStorage_Click);
            // 
            // progressBarCloudStorage
            // 
            this.progressBarCloudStorage.Location = new System.Drawing.Point(12, 254);
            this.progressBarCloudStorage.Name = "progressBarCloudStorage";
            this.progressBarCloudStorage.Size = new System.Drawing.Size(230, 5);
            this.progressBarCloudStorage.TabIndex = 4;
            this.progressBarCloudStorage.Click += new System.EventHandler(this.progressBarCloudStorage_Click);
            // 
            // RMBMenu
            // 
            this.RMBMenu.Name = "RMBMenu";
            this.RMBMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // CloudLoad_R2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 266);
            this.Controls.Add(this.progressBarCloudStorage);
            this.Controls.Add(this.lblCloudStorage);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.listR2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "CloudLoad_R2";
            this.ShowIcon = false;
            this.Text = "Select Save File";
            this.Load += new System.EventHandler(this.CloudLoad_R2_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listR2;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnDate;
        private System.Windows.Forms.ColumnHeader columnSize;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblCloudStorage;
        private System.Windows.Forms.ProgressBar progressBarCloudStorage;
        private System.Windows.Forms.ContextMenuStrip RMBMenu;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
    }
}