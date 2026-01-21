using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RakeSaveFileExporter
{
    public partial class LocalExport : UserControl
    {
        public LocalExport()
        {
            InitializeComponent();
        }

        public async Task StartExportAsync()
        {
            ResetExportUI();

            await Task.Yield();
            await Task.Delay(1);

            string registryPath = @"HKEY_CURRENT_USER\Software\Konsordo\Rake";
            string tempRegFile = Path.Combine(
                Path.GetTempPath(),
                $"Rake_{DateTime.Now:yyyyMMdd_HHmmss}.reg"
            );

            try
            {
                // ================= STEP 1 =================
                string baseText = " Searching save file on Windows Registry";
                var cts = new CancellationTokenSource();

                var animationTask = Task.Run(async () =>
                {
                    int i = 0;
                    while (!cts.Token.IsCancellationRequested)
                    {
                        int dots = i % 4;
                        string dotsText = dots == 0 ? "" : new string('.', dots);
                        string text = baseText + dotsText;
                        try { this.BeginInvoke((Action)(() => lblStep1.Text = text)); } catch { }
                        i++;
                        await Task.Delay(500);
                    }
                });

                try
                {
                    if (Registry.CurrentUser.OpenSubKey(@"Software\Konsordo\Rake") == null)
                    {
                        cts.Cancel();
                        try { await animationTask; } catch { }
                        FailExport(lblStep1, " Searching save file on Windows Registry");
                        MessageBox.Show(
                            "Rake save data was not found in the Windows Registry.",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                        return;
                    }

                    cts.Cancel();
                    try { await animationTask; } catch { }
                    PassExport(lblStep1, " Searching save file on Windows Registry", 33);
                    await Task.Delay(300);
                }
                catch (Exception ex)
                {
                    cts.Cancel();
                    try { await animationTask; } catch { }
                    FailExport(lblStep1, " Searching save file on Windows Registry");
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ================= STEP 2 =================
                baseText = " Creating save file";
                cts = new CancellationTokenSource();
                animationTask = Task.Run(async () =>
                {
                    int i = 0;
                    while (!cts.Token.IsCancellationRequested)
                    {
                        int dots = i % 4;
                        string dotsText = dots == 0 ? "" : new string('.', dots);
                        string text = baseText + dotsText;
                        try { this.BeginInvoke((Action)(() => lblStep2.Text = text)); } catch { }
                        i++;
                        await Task.Delay(500);
                    }
                });

                try
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
                        using (var proc = Process.Start(psi))
                        {
                            proc.WaitForExit();
                            return proc.ExitCode;
                        }
                    });

                    if (exitCode != 0)
                        throw new Exception("Registry export failed.");
                }
                catch (Exception ex)
                {
                    cts.Cancel();
                    try { await animationTask; } catch { }
                    FailExport(lblStep2, " Creating save file");
                    MessageBox.Show(
                        ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                cts.Cancel();
                try { await animationTask; } catch { }
                PassExport(lblStep2, " Creating save file", 66);
                await Task.Delay(300);

                // ================= STEP 3 =================
                baseText = " Copying save file";
                cts = new CancellationTokenSource();
                animationTask = Task.Run(async () =>
                {
                    int i = 0;
                    while (!cts.Token.IsCancellationRequested)
                    {
                        int dots = i % 4;
                        string dotsText = dots == 0 ? "" : new string('.', dots);
                        string text = baseText + dotsText;
                        try { this.BeginInvoke((Action)(() => lblStep3.Text = text)); } catch { }
                        i++;
                        await Task.Delay(500);
                    }
                });

                bool copySuccess = false;
                using (SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Registry File (*.reg)|*.reg",
                    FileName = $"Rake-{DateTime.Now:ddMMyyyy-HHmmss}.reg",
                    InitialDirectory = GetDefaultGameSaveFolder(),
                    RestoreDirectory = true
                })
                {
                    if (sfd.ShowDialog(this.FindForm()) == DialogResult.OK)
                    {
                        try
                        {
                            await Task.Run(() => File.Copy(tempRegFile, sfd.FileName, true));
                            copySuccess = true;
                        }
                        catch (Exception ex)
                        {
                            copySuccess = false;
                            MessageBox.Show(
                                ex.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }
                    }
                }

                cts.Cancel();
                try { await animationTask; } catch { }

                if (copySuccess)
                    PassExport(lblStep3, " Copying save file", 100);
                else
                    FailExport(lblStep3, " Copying save file");
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

        private static string GetDefaultGameSaveFolder()
        {
            string dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "AppData",
                "LocalLow",
                "EgeOsmX",
                "Game Save"
            );

            try
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
            catch
            {
                
            }

            return dir;
        }

        private void ResetExportUI()
        {

            lblStep1.ForeColor =
            lblStep2.ForeColor =
            lblStep3.ForeColor = System.Drawing.Color.Black;

            progressBar.Value = 0;

            lblStep1.Visible = true;
            lblStep2.Visible = true;
            lblStep3.Visible = true;
            progressBar.Visible = true;

            this.Refresh();
            Application.DoEvents();
        }

        private void PassExport(Label lbl, string text, int progress)
        {
            try
            {
                lbl.Text = "✓" + text;
                lbl.ForeColor = System.Drawing.Color.Green;
                progressBar.Value = progress;
                Application.DoEvents();
            }
            catch { }
        }

        private void FailExport(Label lbl, string text)
        {
            try
            {
                lbl.Text = "X" + text;
                lbl.ForeColor = System.Drawing.Color.Red;
                Application.DoEvents();
            }
            catch { }
        }

        private void lblStep1_Click(object sender, EventArgs e) { }
        private void lblStep2_Click(object sender, EventArgs e) { }
        private void lblStep3_Click(object sender, EventArgs e) { }
        private void progressBar_Click(object sender, EventArgs e) { }
        private void LocalExport_Load(object sender, EventArgs e) { }
    }
}
