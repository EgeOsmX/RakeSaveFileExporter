using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http.Headers;

namespace RakeSaveFileExporter
{
    public partial class CloudUpload : UserControl
    {
        private const int CLOUD_UPLOAD_FORM_HEIGHT = 114;

        public int PreferredPanelHeight
        {
            get { return CLOUD_UPLOAD_FORM_HEIGHT; }
        }

        public string SteamId { get; set; }
        public string AuthToken { get; set; }

        public CloudUpload()
        {
            InitializeComponent();
        }

        public async Task StartUploadAsync()
        {
            string registryPath = @"HKEY_CURRENT_USER\Software\Konsordo\Rake";
            string tempRegFile = Path.Combine(
                Path.GetTempPath(),
                $"Rake_{DateTime.Now:yyyyMMdd_HHmmss}.reg"
            );

            try
            {
                ResetUploadUI();
                await Task.Delay(1);

                // ================= STEP 1 =================
                await RunStepAsync(lblStepCloudUpload1, " Searching save file on Windows Registry", async () =>
                {
                    if (Registry.CurrentUser.OpenSubKey(@"Software\Konsordo\Rake") == null)
                        throw new Exception("Rake save not found");
                }, 33);

                // ================= STEP 2 =================
                await RunStepAsync(lblStepCloudUpload2, " Creating save file", async () =>
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = "reg.exe",
                        Arguments = $"export \"{registryPath}\" \"{tempRegFile}\" /y",
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
                        throw new Exception("Registry export failed");
                }, 66);

                // ================= STEP 3 =================
                await RunStepAsync(lblStepCloudUpload3, " Uploading save file to cloud", async () =>
                {
                    if (string.IsNullOrEmpty(AuthToken))
                        throw new Exception("AuthToken is missing. Please log in again.");

                    byte[] fileBytes = await Task.Run(() => File.ReadAllBytes(tempRegFile));

                    using (var client = new HttpClient())
                    {
                        client.Timeout = TimeSpan.FromSeconds(60);
                        client.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", AuthToken);

                        client.DefaultRequestHeaders.ExpectContinue = false;

                        string boundary = "----RakeBoundary" + Guid.NewGuid().ToString("N");

                        var header =
                            $"--{boundary}\r\n" +
                            "Content-Disposition: form-data; name=\"file\"; filename=\"upload.reg\"\r\n" +
                            "Content-Type: application/octet-stream\r\n" +
                            "\r\n";

                        var footer = "\r\n" + $"--{boundary}--\r\n";

                        byte[] headerBytes = Encoding.UTF8.GetBytes(header);
                        byte[] footerBytes = Encoding.UTF8.GetBytes(footer);

                        // ✅ body’yi tek buffer yap (Content-Length belli olsun)
                        byte[] bodyBytes = new byte[headerBytes.Length + fileBytes.Length + footerBytes.Length];

                        Buffer.BlockCopy(headerBytes, 0, bodyBytes, 0, headerBytes.Length);
                        Buffer.BlockCopy(fileBytes, 0, bodyBytes, headerBytes.Length, fileBytes.Length);
                        Buffer.BlockCopy(footerBytes, 0, bodyBytes, headerBytes.Length + fileBytes.Length, footerBytes.Length);

                        var content = new ByteArrayContent(bodyBytes);
                        content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data; boundary=" + boundary);

                        var resp = await client.PostAsync("https://github-gamesave.egeosmx.workers.dev/upload", content);
                        var respBody = await resp.Content.ReadAsStringAsync();

                        if (!resp.IsSuccessStatusCode)
                            throw new Exception($"HTTP {(int)resp.StatusCode}: {respBody}");
                    }
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
                await anim;
                Fail(lbl, text);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void ResetUploadUI()
        {
            lblStepCloudUpload1.ForeColor =
            lblStepCloudUpload2.ForeColor =
            lblStepCloudUpload3.ForeColor = System.Drawing.Color.Black;

            progressBarCloudUpload.Value = 0;

            lblStepCloudUpload1.Visible = true;
            lblStepCloudUpload2.Visible = true;
            lblStepCloudUpload3.Visible = true;
            progressBarCloudUpload.Visible = true;

            this.Refresh();
            Application.DoEvents();
        }

        private void Pass(Label lbl, string text, int progress)
        {
            lbl.Text = "✓" + text;
            lbl.ForeColor = System.Drawing.Color.Green;
            progressBarCloudUpload.Value = progress;
        }

        private void Fail(Label lbl, string text)
        {
            lbl.Text = "X" + text;
            lbl.ForeColor = System.Drawing.Color.Red;
        }

        private void CloudUpload_Load(object sender, EventArgs e) { }
        private void lblStepCloudUpload1_Click(object sender, EventArgs e) { }
        private void lblStepCloudUpload2_Click(object sender, EventArgs e) { }
        private void lblStepCloudUpload3_Click(object sender, EventArgs e) { }
        private void progressBarCloudUpload_Click(object sender, EventArgs e) { }
    }
}
