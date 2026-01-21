using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.InteropServices;
using System.Globalization;

namespace RakeSaveFileExporter
{
    public partial class ViewFiles : Form
    {
        public ViewFiles()
        {
            InitializeComponent();

            listR2.LabelEdit = true;
            listR2.HideSelection = false;
            listR2.ContextMenuStrip = RMBMenu;

            listR2.MouseDown += listR2_MouseDown;

            listR2.SizeChanged += (s, e) => AdjustR2Columns();

            RMBMenu.Opening -= RMBMenu_Opening;
            RMBMenu.Opening += RMBMenu_Opening;

            RMBMenuRename.Click -= RMBMenuRename_Click;
            RMBMenuRename.Click += RMBMenuRename_Click;

            RMBMenuDelete.Click -= RMBMenuDelete_Click;
            RMBMenuDelete.Click += RMBMenuDelete_Click;

            this.Shown += async (s, e) =>
            {
                await LoadSavesAndStorageAsync();

                AdjustR2Columns();
                BeginInvoke((Action)(() => AdjustR2Columns()));
            };

            listR2.AfterLabelEdit -= listR2_AfterLabelEdit;
            listR2.AfterLabelEdit += listR2_AfterLabelEdit;
        }

        public string AuthToken { get; set; }

        private void ViewFiles_Load(object sender, EventArgs e)
        {
            
        }

        private const int GWL_STYLE = -16;
        private const int WS_VSCROLL = 0x00200000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        private bool HasVerticalScrollBar()
        {
            try
            {
                int style = GetWindowLong(listR2.Handle, GWL_STYLE);
                return (style & WS_VSCROLL) != 0;
            }
            catch { return false; }
        }

        private void AdjustR2Columns()
        {
            try
            {
                if (!listR2.IsHandleCreated) return;

                columnSize.Width = -2;

                int totalWidth = listR2.ClientSize.Width;
                int scrollbarWidth = HasVerticalScrollBar() ? SystemInformation.VerticalScrollBarWidth : 0;

                int usedWidth = columnName.Width + columnDate.Width + columnSize.Width;
                int remaining = totalWidth - usedWidth - scrollbarWidth - 4;

                if (remaining > 0)
                    columnSize.Width += remaining;
            }
            catch { }
        }

        private void listR2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            var hit = listR2.HitTest(e.Location);
            if (hit.Item != null)
            {
                listR2.SelectedItems.Clear();
                hit.Item.Selected = true;
                hit.Item.Focused = true;
            }
        }

        private void RMBMenu_Opening(object sender, CancelEventArgs e)
        {
            bool oneSelected = listR2.SelectedItems.Count == 1;
            RMBMenuRename.Enabled = oneSelected;
            RMBMenuDelete.Enabled = oneSelected;
        }

        private void RMBMenuRename_Click(object sender, EventArgs e)
        {
            if (listR2.SelectedItems.Count != 1)
                return;

            listR2.SelectedItems[0].BeginEdit();
        }

        private bool _deleteInProgress = false;

        private async void RMBMenuDelete_Click(object sender, EventArgs e)
        {
            if (_deleteInProgress) return;
            if (listR2.SelectedItems.Count != 1) return;

            var item = listR2.SelectedItems[0];
            string key = item.Tag as string;

            var res = MessageBox.Show(
                "Are you sure you want to delete this file?\n\nThis action cannot be undone.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2
            );

            if (res != DialogResult.Yes) return;

            _deleteInProgress = true;
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", AuthToken);

                    var payload = new { key = key };
                    var content = new StringContent(
                        JsonConvert.SerializeObject(payload),
                        Encoding.UTF8,
                        "application/json"
                    );

                    var resp = await client.PostAsync(
                        "https://github-gamesave.egeosmx.workers.dev/delete-save",
                        content
                    );

                    if (!resp.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Failed to delete the file.", "Delete",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    listR2.Items.Remove(item);
                    if (columnName != null)
                        columnName.Text = $"Name ({listR2.Items.Count})";

                    await UpdateStorageInfoAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _deleteInProgress = false;
            }
        }

        private static string FormatSize(long bytes)
        {
            if (bytes < 1000) return $"{bytes} B";
            if (bytes < 1000 * 1000) return $"{bytes / 1000.0:F1} KB";
            return $"{bytes / (1000.0 * 1000.0):F1} MB";
        }

        private static string FormatStorageMB(long bytes)
        {
            double mb = bytes / (1024.0 * 1024.0);
            return $"{mb:0.00} MB";
        }

        private async Task LoadSavesAndStorageAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(AuthToken))
                {
                    MessageBox.Show("AuthToken is missing.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                listR2.Items.Clear();

                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(15);
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", AuthToken);

                    var resp = await client.GetAsync("https://github-gamesave.egeosmx.workers.dev/list-saves");
                    resp.EnsureSuccessStatusCode();

                    string json = await resp.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(json);

                    if (data?.items != null)
                    {
                        foreach (var it in data.items)
                        {
                            string key = it.key;
                            string name = it.name ?? Path.GetFileName(key);
                            long size = it.size != null ? (long)it.size : 0;
                            string date = "";

                            if (it.lastModified != null)
                            {
                                if (DateTime.TryParse(
                                    (string)it.lastModified,
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                                    out DateTime dt))
                                {
                                    date = GetWindowsDateTimeWithSeconds(dt.ToLocalTime());
                                }
                            }

                            var item = new ListViewItem(name);
                            item.SubItems.Add(date);
                            item.SubItems.Add(FormatSize(size));
                            item.Tag = key;

                            listR2.Items.Add(item);
                        }
                    }
                }

                if (columnName != null)
                    columnName.Text = $"Name ({listR2.Items.Count})";

                await UpdateStorageInfoAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ViewFiles Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string GetWindowsDateTimeWithSeconds(DateTime dt)
        {
            var culture = CultureInfo.CurrentCulture;
            var datePattern = culture.DateTimeFormat.ShortDatePattern;
            var timePattern = culture.DateTimeFormat.LongTimePattern;

            return dt.ToString($"{datePattern} {timePattern}", culture);
        }

        private async Task UpdateStorageInfoAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(10);
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", AuthToken);

                    var resp = await client.GetAsync("https://github-gamesave.egeosmx.workers.dev/storage-info");
                    var json = await resp.Content.ReadAsStringAsync();

                    if (!resp.IsSuccessStatusCode)
                    {
                        lblCloudStorage.Text = $"Cloud Storage: error ({(int)resp.StatusCode})";
                        return;
                    }

                    dynamic data = JsonConvert.DeserializeObject(json);
                    if (data == null || (string)data.status != "ok") return;

                    long usedBytes = (long)data.usedBytes;
                    long quotaBytes = (long)data.quotaBytes;

                    lblCloudStorage.Text =
                        $"Cloud Storage: {FormatBytesSmart1(usedBytes)} / {FormatBytesSmart1(quotaBytes)}";

                    progressBarCloudStorage.Minimum = 0;
                    progressBarCloudStorage.Maximum = 1000;
                    progressBarCloudStorage.Style = ProgressBarStyle.Blocks;
                    progressBarCloudStorage.MarqueeAnimationSpeed = 0;

                    int value = quotaBytes > 0
                        ? (int)Math.Min(1000, (usedBytes * 1000.0 / quotaBytes))
                        : 0;

                    progressBarCloudStorage.Value = Math.Max(0, value);
                }
            }
            catch
            {
                lblCloudStorage.Text = "Cloud Storage: error";
            }
        }

        private static string FormatBytesSmart(long bytes)
        {
            if (bytes < 0) bytes = 0;

            const double KB = 1000.0;
            const double MB = 1000.0 * 1000.0;
            const double GB = 1000.0 * 1000.0 * 1000.0;

            if (bytes < KB)
                return $"{bytes} B";

            if (bytes < MB)
                return $"{bytes / KB:0.00} KB";

            if (bytes < GB)
                return $"{bytes / MB:0.00} MB";

            return $"{bytes / GB:0.00} GB";
        }

        private static string FormatBytesSmart1(long bytes)
        {
            if (bytes < 0) bytes = 0;

            const double KB = 1000.0;
            const double MB = 1000.0 * 1000.0;
            const double GB = 1000.0 * 1000.0 * 1000.0;

            if (bytes < KB)
                return $"{bytes} B";

            if (bytes < MB)
                return $"{bytes / KB:0.0} KB";

            if (bytes < GB)
                return $"{bytes / MB:0.00} MB";

            return $"{bytes / GB:0.00} GB";
        }

        private async void listR2_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label == null) return;

            string newName = e.Label.Trim();
            if (string.IsNullOrWhiteSpace(newName))
            {
                e.CancelEdit = true;
                return;
            }

            var item = listR2.Items[e.Item];
            string oldKey = item.Tag as string;
            if (string.IsNullOrEmpty(oldKey))
            {
                e.CancelEdit = true;
                return;
            }

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", AuthToken);

                    var payload = new { key = oldKey, newName = newName };
                    var content = new StringContent(
                        JsonConvert.SerializeObject(payload),
                        Encoding.UTF8,
                        "application/json"
                    );

                    var resp = await client.PostAsync(
                        "https://github-gamesave.egeosmx.workers.dev/rename-save",
                        content
                    );

                    if (!resp.IsSuccessStatusCode)
                    {
                        e.CancelEdit = true;

                        if ((int)resp.StatusCode == 409)
                            MessageBox.Show("A file with that name already exists.", "Rename",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        else
                            MessageBox.Show("Rename failed.", "Rename",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }

                    var json = await resp.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(json);

                    string newKey = (string)data.newKey;
                    item.Tag = newKey;
                }
            }
            catch
            {
                e.CancelEdit = true;
                MessageBox.Show("Rename failed.", "Rename",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblCloudStorage_Click(object sender, EventArgs e) { }
        private void progressBarCloudStorage_Click(object sender, EventArgs e) { }
        private void listR2_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}
