using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace RakeSaveFileExporter
{
    public partial class CloudLoad_R2 : Form
    {
        public string AuthToken { get; set; }
        public string SelectedKey { get; private set; }

        public CloudLoad_R2()
        {
            InitializeComponent();
            
            this.Shown += (s, e) =>
            {
                BeginInvoke((Action)(() => AdjustR2Columns()));
            };

            listR2.SizeChanged += (s, e) => AdjustR2Columns();

            listR2.LabelEdit = true;
            listR2.HideSelection = false;
            listR2.ContextMenuStrip = RMBMenu;
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

        private async void CloudLoad_R2_Load(object sender, EventArgs e)
        {
            try
            {
                btnLoad.Enabled = false;
                listR2.Items.Clear();

                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(15);
                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AuthToken);

                    string url = "https://github-gamesave.egeosmx.workers.dev/list-saves";
                    var resp = await client.GetAsync(url);
                    resp.EnsureSuccessStatusCode();

                    string json = await resp.Content.ReadAsStringAsync();
                    dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                    if (data?.items == null)
                        return;

                    foreach (var it in data.items)
                    {
                        string key = it.key;
                        string name = it.name ?? Path.GetFileName(key);
                        long size = it.size != null ? (long)it.size : 0;
                        string dateIso = it.lastModified != null ? (string)it.lastModified : "";
                        string date = FormatWindowsDateTimeWithSeconds(dateIso);

                        var item = new ListViewItem(name);
                        item.SubItems.Add(date);
                        item.SubItems.Add(FormatSize(size));

                        item.Tag = key;
                        listR2.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cloud Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            int count = listR2.Items.Count;
            columnName.Text = $"Name ({count})";

            AdjustR2Columns();
            BeginInvoke((Action)(() => AdjustR2Columns()));

            await UpdateStorageInfoAsync();
        }

        private void listR2_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnLoad.Enabled = listR2.SelectedItems.Count == 1;
            AdjustR2Columns();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (listR2.SelectedItems.Count != 1)
                return;

            SelectedKey = listR2.SelectedItems[0].Tag as string;

            if (string.IsNullOrEmpty(SelectedKey))
            {
                MessageBox.Show("Invalid file selection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
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
                {
                    columnSize.Width += remaining;
                }
            }
            catch { }
        }

        private static string FormatSize(long bytes)
        {
            if (bytes < 0) bytes = 0;

            const double KB = 1000.0;
            const double MB = 1000.0 * 1000.0;
            const double GB = 1000.0 * 1000.0 * 1000.0;

            if (bytes < KB) return $"{bytes} B";
            if (bytes < MB) return $"{bytes / KB:0.0} KB";
            if (bytes < GB) return $"{bytes / MB:0.00} MB";
            return $"{bytes / GB:0.00} GB";
        }

        private static string FormatStorageSmart(long bytes) => FormatSize(bytes);

        private async Task UpdateStorageInfoAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(10);
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", AuthToken);

                    var url = "https://github-gamesave.egeosmx.workers.dev/storage-info?ts=" +
                              DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                    var resp = await client.GetAsync(url);
                    var json = await resp.Content.ReadAsStringAsync();

                    if (!resp.IsSuccessStatusCode)
                    {
                        BeginInvoke((Action)(() =>
                            lblCloudStorage.Text = $"Cloud Storage: error ({(int)resp.StatusCode})"
                        ));
                        return;
                    }

                    dynamic data = JsonConvert.DeserializeObject(json);
                    if (data == null || (string)data.status != "ok") return;

                    long usedBytes = Convert.ToInt64(data.usedBytes);
                    long quotaBytes = Convert.ToInt64(data.quotaBytes);

                    BeginInvoke((Action)(() =>
                    {
                        lblCloudStorage.Text =
                            $"Cloud Storage: {FormatStorageSmart(usedBytes)} / {FormatStorageSmart(quotaBytes)}";

                        progressBarCloudStorage.Minimum = 0;
                        progressBarCloudStorage.Maximum = 1000;

                        int value = quotaBytes > 0
                            ? (int)Math.Min(1000, (usedBytes * 1000.0 / quotaBytes))
                            : 0;

                        progressBarCloudStorage.Value = Math.Max(0, value);
                    }));
                }
            }
            catch
            {
                
            }
        }

        private async Task LoadSavesAndStorageAsync()
        {
            try
            {
                btnLoad.Enabled = false;
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
                            long size = it.size != null ? Convert.ToInt64(it.size) : 0;
                            string dateIso = it.lastModified != null ? (string)it.lastModified : "";
                            string date = FormatWindowsDateTimeWithSeconds(dateIso);


                            var item = new ListViewItem(name);
                            item.SubItems.Add(date);
                            item.SubItems.Add(FormatSize(size));
                            item.Tag = key;

                            listR2.Items.Add(item);
                        }
                    }
                }

                columnName.Text = $"Name ({listR2.Items.Count})";

                AdjustR2Columns();
                BeginInvoke((Action)(() => AdjustR2Columns()));

                await UpdateStorageInfoAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cloud Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string FormatWindowsDateTimeWithSeconds(string iso)
        {
            if (string.IsNullOrWhiteSpace(iso)) return "";

            if (DateTime.TryParse(
                iso,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                out DateTime dt))
            {
                var culture = CultureInfo.CurrentCulture;
                var datePattern = culture.DateTimeFormat.ShortDatePattern;
                var timePattern = culture.DateTimeFormat.LongTimePattern;

                return dt.ToLocalTime().ToString($"{datePattern} {timePattern}", culture);
            }

            return iso;
        }

        private void lblCloudStorage_Click(object sender, EventArgs e) { }

        private void progressBarCloudStorage_Click(object sender, EventArgs e) { }
        private void RMBMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e) { }
    }
}