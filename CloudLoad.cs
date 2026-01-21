using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RakeSaveFileExporter
{
    public partial class CloudLoad : UserControl
    {
        private const int CLOUD_LOAD_FORM_HEIGHT = 150;
        public int PreferredPanelHeight => CLOUD_LOAD_FORM_HEIGHT;

        public string SteamId { get; set; }
        public string AuthToken { get; set; }

        public CloudLoad()
        {
            InitializeComponent();
        }

        public async Task StartCloudLoadAsync()
        {
            ResetCloudLoadUI();
            await Task.Delay(1);

            string selectedKey = null;
            string tempRegFile = null;

            try
            {
                // ================= STEP 1 =================
                await RunStepAsync(lblStepCloudLoad1, " Selecting save file", async () =>
                {
                    using (var picker = new CloudLoad_R2())
                    {
                        picker.AuthToken = AuthToken;

                        picker.StartPosition = FormStartPosition.CenterScreen;

                        var res = picker.ShowDialog(this.FindForm());

                        if (res != DialogResult.OK || string.IsNullOrEmpty(picker.SelectedKey))
                            throw new Exception("No file selected.");

                        selectedKey = picker.SelectedKey;
                    }

                }, 20);

                // ================= STEP 2 =================
                await RunStepAsync(lblStepCloudLoad2, " Downloading save file", async () =>
                {
                    if (string.IsNullOrEmpty(AuthToken))
                        throw new Exception("AuthToken is missing. Please log in again.");

                    tempRegFile = Path.Combine(Path.GetTempPath(), $"RakeCloud_{DateTime.Now:yyyyMMdd_HHmmss}.reg");

                    using (var client = new HttpClient())
                    {
                        client.Timeout = TimeSpan.FromSeconds(60);
                        client.DefaultRequestHeaders.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AuthToken);

                        string url = "https://github-gamesave.egeosmx.workers.dev/download-save?key=" +
                                     Uri.EscapeDataString(selectedKey);

                        using (var resp = await client.GetAsync(url))
                        {
                            string body = await resp.Content.ReadAsStringAsync();
                            if (!resp.IsSuccessStatusCode)
                                throw new Exception($"HTTP {(int)resp.StatusCode}: {body}");

                            var bytes = await resp.Content.ReadAsByteArrayAsync();
                            await Task.Run(() => File.WriteAllBytes(tempRegFile, bytes));

                        }
                    }
                }, 40);

                // ================= STEP 3 =================
                await RunStepAsync(lblStepCloudLoad3, " Validating save file", async () =>
                {
                    if (string.IsNullOrEmpty(tempRegFile) || !File.Exists(tempRegFile))
                        throw new Exception("Downloaded file not found.");

                    if (Path.GetExtension(tempRegFile).ToLowerInvariant() != ".reg")
                        throw new Exception("Downloaded file is not a .reg file.");

                    string firstLine = File.ReadLines(tempRegFile).FirstOrDefault() ?? "";
                    if (firstLine.IndexOf("Registry Editor", StringComparison.OrdinalIgnoreCase) < 0)
                        throw new Exception("Downloaded file does not look like a valid registry export.");

                }, 60);

                // ================= STEP 4 =================
                await RunStepAsync(lblStepCloudLoad4, " Preparing Registry directory", async () =>
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            Registry.CurrentUser.DeleteSubKeyTree(@"Software\Konsordo\Rake", false);
                        }
                        catch { }

                        Registry.CurrentUser.CreateSubKey(@"Software\Konsordo\Rake");
                    });
                }, 80);

                // ================= STEP 5 =================
                await RunStepAsync(lblStepCloudLoad5, " Copying save file", async () =>
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = "reg.exe",
                        Arguments = $"import \"{tempRegFile}\"",
                        CreateNoWindow = true,
                        UseShellExecute = false
                    };

                    int exitCode = await Task.Run(() =>
                    {
                        using (var p = Process.Start(psi))
                        {
                            p.WaitForExit();
                            return p.ExitCode;
                        }
                    });

                    if (exitCode != 0)
                        throw new Exception("Registry import failed.");
                }, 100);
            }
            finally
            {
                try
                {
                    if (!string.IsNullOrEmpty(tempRegFile) && File.Exists(tempRegFile))
                        File.Delete(tempRegFile);
                }
                catch { }
            }
        }

        // ---------------- HELPERS ----------------

        private async Task RunStepAsync(Label lbl, string text, Func<Task> action, int progress)
        {
            var cts = new System.Threading.CancellationTokenSource();

            var anim = Task.Run(async () =>
            {
                int i = 0;
                while (!cts.Token.IsCancellationRequested)
                {
                    string dots = new string('.', i % 4);
                    try { BeginInvoke((Action)(() => lbl.Text = text + dots)); } catch { }
                    i++;
                    await Task.Delay(500);
                }
            });

            try
            {
                await action();
                cts.Cancel();
                await anim;
                Pass(lbl, text, progress);
                await Task.Delay(300);
            }
            catch (Exception ex)
            {
                cts.Cancel();
                try { await anim; } catch { }
                Fail(lbl, text);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void ResetCloudLoadUI()
        {
            lblStepCloudLoad1.ForeColor =
            lblStepCloudLoad2.ForeColor =
            lblStepCloudLoad3.ForeColor =
            lblStepCloudLoad4.ForeColor =
            lblStepCloudLoad5.ForeColor = System.Drawing.Color.Black;

            progressBarCloudLoad.Value = 0;

            lblStepCloudLoad1.Visible =
            lblStepCloudLoad2.Visible =
            lblStepCloudLoad3.Visible =
            lblStepCloudLoad4.Visible =
            lblStepCloudLoad5.Visible = true;

            progressBarCloudLoad.Visible = true;

            this.Refresh();
            Application.DoEvents();
        }

        private void Pass(Label lbl, string text, int progress)
        {
            lbl.Text = "✓" + text;
            lbl.ForeColor = System.Drawing.Color.Green;
            progressBarCloudLoad.Value = progress;
        }

        private void Fail(Label lbl, string text)
        {
            lbl.Text = "X" + text;
            lbl.ForeColor = System.Drawing.Color.Red;
        }

        private void CloudLoad_Load(object sender, EventArgs e) { }
        private void lblStepCloudLoad1_Click(object sender, EventArgs e) { }
        private void lblStepCloudLoad2_Click(object sender, EventArgs e) { }
        private void lblStepCloudLoad3_Click(object sender, EventArgs e) { }
        private void lblStepCloudLoad4_Click(object sender, EventArgs e) { }
        private void lblStepCloudLoad5_Click(object sender, EventArgs e) { }
        private void progressBarCloudLoad_Click(object sender, EventArgs e) { }
    }
}
