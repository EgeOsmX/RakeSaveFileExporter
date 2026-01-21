using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RakeSaveFileExporter
{
    public partial class LocalLoad : UserControl
    {
        public LocalLoad()
        {
            InitializeComponent();
        }

        public async Task StartLoadAsync()
        {
            ResetLoadUI();

            string selectedFile = null;

            string baseText = " Selecting save file";
            var cts = new CancellationTokenSource();

            var animationTask = Task.Run(async () =>
            {
                int i = 0;
                while (!cts.Token.IsCancellationRequested)
                {
                    int dots = i % 4;
                    string dotsText = dots == 0 ? "" : new string('.', dots);
                    string text = baseText + dotsText;
                    try { this.BeginInvoke((Action)(() => lblStepLoad1.Text = text)); } catch { }
                    i++;
                    await Task.Delay(500);
                }
            });

            using (OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Registry File (*.reg)|*.reg",
                Title = "Select Rake Save File",
                InitialDirectory = GetDefaultGameSaveFolder(),
                RestoreDirectory = true
            })
            {
                if (ofd.ShowDialog(this.FindForm()) == DialogResult.OK)
                    selectedFile = ofd.FileName;
            }

            cts.Cancel();
            try { await animationTask; } catch { }

            if (selectedFile == null)
            {
                FailLoad(lblStepLoad1, " Selecting save file");
                return;
            }

            PassLoad(lblStepLoad1, " Selecting save file", 25);
            await Task.Delay(300);

            // ================= STEP 2 =================
            baseText = " Validating save file";
            cts = new CancellationTokenSource();
            animationTask = Task.Run(async () =>
            {
                int i = 0;
                while (!cts.Token.IsCancellationRequested)
                {
                    int dots = i % 4;
                    string dotsText = dots == 0 ? "" : new string('.', dots);
                    string text = baseText + dotsText;
                    try { this.BeginInvoke((Action)(() => lblStepLoad2.Text = text)); } catch { }
                    i++;
                    await Task.Delay(500);
                }
            });

            if (!File.Exists(selectedFile) ||
                Path.GetExtension(selectedFile).ToLower() != ".reg")
            {
                cts.Cancel();
                try { await animationTask; } catch { }

                FailLoad(lblStepLoad2, " Validating save file");
                MessageBox.Show(
                    "The selected file is not a valid .reg file.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            cts.Cancel();
            try { await animationTask; } catch { }
            PassLoad(lblStepLoad2, " Validating save file", 50);
            await Task.Delay(300);

            // ================= STEP 3 =================
            baseText = " Preparing registry";
            cts = new CancellationTokenSource();
            animationTask = Task.Run(async () =>
            {
                int i = 0;
                while (!cts.Token.IsCancellationRequested)
                {
                    int dots = i % 4;
                    string dotsText = dots == 0 ? "" : new string('.', dots);
                    string text = baseText + dotsText;
                    try { this.BeginInvoke((Action)(() => lblStepLoad3.Text = text)); } catch { }
                    i++;
                    await Task.Delay(500);
                }
            });

            try
            {
                await Task.Run(() =>
                {
                    try
                    {
                        Registry.CurrentUser.DeleteSubKeyTree(@"Software\Konsordo\Rake", false);
                    }
                    catch
                    {
                        
                    }
                    Registry.CurrentUser.CreateSubKey(@"Software\Konsordo\Rake");
                });
            }
            catch (Exception ex)
            {
                cts.Cancel();
                try { await animationTask; } catch { }

                FailLoad(lblStepLoad3, " Preparing registry");
                MessageBox.Show(
                    $"Failed to prepare registry:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            cts.Cancel();
            try { await animationTask; } catch { }
            PassLoad(lblStepLoad3, " Preparing registry", 75);
            await Task.Delay(300);

            // ================= STEP 4 =================
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
                    try { this.BeginInvoke((Action)(() => lblStepLoad4.Text = text)); } catch { }
                    i++;
                    await Task.Delay(500);
                }
            });

            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "reg.exe",
                    Arguments = $"import \"{selectedFile}\"",
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
                    throw new Exception("Registry import failed.");
            }
            catch (Exception ex)
            {
                cts.Cancel();
                try { await animationTask; } catch { }

                FailLoad(lblStepLoad4, " Copying save file");
                MessageBox.Show(
                    $"Failed to import save file:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            cts.Cancel();
            try { await animationTask; } catch { }
            PassLoad(lblStepLoad4, " Copying save file", 100);
            await Task.Delay(300);
        }

        private static string GetDefaultGameSaveFolder()
        {
            string dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "AppData", "LocalLow", "EgeOsmX", "Game Save"
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

        private void ResetLoadUI()
        {
            lblStepLoad1.ForeColor =
            lblStepLoad2.ForeColor =
            lblStepLoad3.ForeColor =
            lblStepLoad4.ForeColor = System.Drawing.Color.Black;

            progressBarLoad.Value = 0;

            lblStepLoad1.Visible =
            lblStepLoad2.Visible =
            lblStepLoad3.Visible =
            lblStepLoad4.Visible = true;

            progressBarLoad.Visible = true;

            this.Refresh();
            Application.DoEvents();
        }

        private void PassLoad(Label lbl, string text, int progress)
        {
            try
            {
                lbl.Text = "✓" + text;
                lbl.ForeColor = System.Drawing.Color.Green;
                progressBarLoad.Value = progress;
                Application.DoEvents();
            }
            catch { }
        }

        private void FailLoad(Label lbl, string text)
        {
            try
            {
                lbl.Text = "X" + text;
                lbl.ForeColor = System.Drawing.Color.Red;
                Application.DoEvents();
            }
            catch { }
        }

        private void lblStepLoad1_Click(object sender, EventArgs e) { }
        private void lblStepLoad2_Click(object sender, EventArgs e) { }
        private void lblStepLoad3_Click(object sender, EventArgs e) { }
        private void lblStepLoad4_Click(object sender, EventArgs e) { }
        private void progressBarLoad_Click(object sender, EventArgs e) { }
    }
}
