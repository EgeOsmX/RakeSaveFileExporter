namespace RakeSaveFileExporter
{
    partial class ViewFiles
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
            this.progressBarCloudStorage = new System.Windows.Forms.ProgressBar();
            this.lblCloudStorage = new System.Windows.Forms.Label();
            this.listR2 = new System.Windows.Forms.ListView();
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RMBMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.RMBMenuRename = new System.Windows.Forms.ToolStripMenuItem();
            this.RMBMenuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.RMBMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBarCloudStorage
            // 
            this.progressBarCloudStorage.Location = new System.Drawing.Point(92, 253);
            this.progressBarCloudStorage.Name = "progressBarCloudStorage";
            this.progressBarCloudStorage.Size = new System.Drawing.Size(230, 5);
            this.progressBarCloudStorage.TabIndex = 7;
            this.progressBarCloudStorage.Click += new System.EventHandler(this.progressBarCloudStorage_Click);
            // 
            // lblCloudStorage
            // 
            this.lblCloudStorage.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblCloudStorage.Location = new System.Drawing.Point(92, 236);
            this.lblCloudStorage.Name = "lblCloudStorage";
            this.lblCloudStorage.Size = new System.Drawing.Size(230, 14);
            this.lblCloudStorage.TabIndex = 6;
            this.lblCloudStorage.Text = "Cloud Storage: 0.0 MB / 0.0 MB";
            this.lblCloudStorage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblCloudStorage.Click += new System.EventHandler(this.lblCloudStorage_Click);
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
            this.listR2.TabIndex = 5;
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
            // RMBMenu
            // 
            this.RMBMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RMBMenuRename,
            this.RMBMenuDelete});
            this.RMBMenu.Name = "RMBMenu";
            this.RMBMenu.Size = new System.Drawing.Size(118, 48);
            this.RMBMenu.Opening += new System.ComponentModel.CancelEventHandler(this.RMBMenu_Opening);
            // 
            // RMBMenuRename
            // 
            this.RMBMenuRename.Name = "RMBMenuRename";
            this.RMBMenuRename.Size = new System.Drawing.Size(117, 22);
            this.RMBMenuRename.Text = "Rename";
            this.RMBMenuRename.Click += new System.EventHandler(this.RMBMenuRename_Click);
            // 
            // RMBMenuDelete
            // 
            this.RMBMenuDelete.Name = "RMBMenuDelete";
            this.RMBMenuDelete.Size = new System.Drawing.Size(117, 22);
            this.RMBMenuDelete.Text = "Delete";
            this.RMBMenuDelete.Click += new System.EventHandler(this.RMBMenuDelete_Click);
            // 
            // ViewFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 266);
            this.Controls.Add(this.progressBarCloudStorage);
            this.Controls.Add(this.lblCloudStorage);
            this.Controls.Add(this.listR2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "ViewFiles";
            this.ShowIcon = false;
            this.Text = "View Cloud Files";
            this.Load += new System.EventHandler(this.ViewFiles_Load);
            this.RMBMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBarCloudStorage;
        private System.Windows.Forms.Label lblCloudStorage;
        private System.Windows.Forms.ListView listR2;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnDate;
        private System.Windows.Forms.ColumnHeader columnSize;
        private System.Windows.Forms.ContextMenuStrip RMBMenu;
        private System.Windows.Forms.ToolStripMenuItem RMBMenuRename;
        private System.Windows.Forms.ToolStripMenuItem RMBMenuDelete;
    }
}