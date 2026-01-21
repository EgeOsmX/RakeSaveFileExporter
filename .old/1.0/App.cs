using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rake_Save_File_Exporter
{
    public partial class App : Form
    {
        private bool formExpanded = false;

        public App()
        {
            InitializeComponent();
            this.Load += Menu_Load;

        }

        private bool exportExpanded = false;
        private bool exportRunning = false;

        private bool loadExpanded = false;
        private bool loadRunning = false;

        private const int EXPORT_HEIGHT = 121;
        private const int LOAD_HEIGHT = 137;

        private void Menu_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.Manual;
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point(
                screen.X + (screen.Width - this.Width) / 2,
                screen.Y + (screen.Height - this.Height) / 2
            );

            LoadLogo();

            label1.Text = "Rake Save File Exporter";
            label2.Text = "Export your Rake save file and continue your progress\non any computer without starting over.";
            labelLoadInfo.Text = "After loading the save file, your current progress will be permanently removed\nand replaced with the progress from the selected save file.";

            lblStep1.Visible = lblStep2.Visible = lblStep3.Visible = false;
            lblStepLoad1.Visible = lblStepLoad2.Visible = lblStepLoad3.Visible = lblStepLoad4.Visible = false;
            progressBar.Visible = progressBarLoad.Visible = false;

            lblStep1.Text = " Searching save file on Windows Registry";
            lblStep2.Text = " Creating save file";
            lblStep3.Text = " Copying save file";

            lblStepLoad1.Text = " Selecting save file";
            lblStepLoad2.Text = " Validating save file";
            lblStepLoad3.Text = " Preparing registry";
            lblStepLoad4.Text = " Copying save file";
        }

        private void LoadLogo()
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = "RakeSaveFileExporter.Resources.RakeBMP.png";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    BMPIcon.Image = Image.FromStream(stream);
                    BMPIcon.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else
                {
                    MessageBox.Show("Logo not found! Please check the ResourceName.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async Task AdjustFormHeightSmooth(int deltaHeight, int durationMs = 200, int steps = 20)
        {
            if (steps <= 0) steps = 1;
            int start = this.Height;
            int target = start + deltaHeight;
            int totalDelta = target - start;

            if (totalDelta == 0) return;

            int delayPerStep = Math.Max(1, durationMs / steps);

            for (int i = 1; i <= steps; i++)
            {
                int newHeight = start + (int)Math.Round(totalDelta * (i / (double)steps));
                if (this.Height != newHeight)
                    this.Height = newHeight;
                await Task.Delay(delayPerStep);
            }

            this.Height = target;
        }

        private void ShowLoadUI()
        {
            lblStepLoad1.Visible = true;
            lblStepLoad2.Visible = true;
            lblStepLoad3.Visible = true;
            lblStepLoad4.Visible = true;
            progressBarLoad.Visible = true;
        }

        private void HideLoadUI()
        {
            lblStepLoad1.Visible = false;
            lblStepLoad2.Visible = false;
            lblStepLoad3.Visible = false;
            lblStepLoad4.Visible = false;
            progressBarLoad.Visible = false;
        }

        private void ResetLoadUI()
        {
            lblStepLoad1.Text = " Selecting save file";
            lblStepLoad2.Text = " Validating save file";
            lblStepLoad3.Text = " Preparing registry";
            lblStepLoad4.Text = " Copying save file";

            lblStepLoad1.ForeColor =
            lblStepLoad2.ForeColor =
            lblStepLoad3.ForeColor =
            lblStepLoad4.ForeColor = Color.Black;

            progressBarLoad.Value = 0;
        }

        private void PassLoad(Label lbl, string text, int progress)
        {
            lbl.Text = "✓" + text;
            lbl.ForeColor = Color.Green;
            progressBarLoad.Value = progress;
            Application.DoEvents();
        }

        private void FailLoad(Label lbl, string text)
        {
            lbl.Text = "X" + text;
            lbl.ForeColor = Color.Red;
            Application.DoEvents();
        }


        private void HideExportUI()
        {
            lblStep1.Visible = false;
            lblStep2.Visible = false;
            lblStep3.Visible = false;
            progressBar.Visible = false;
        }

        private void ShowExportUI()
        {
            lblStep1.Visible = true;
            lblStep2.Visible = true;
            lblStep3.Visible = true;
            progressBar.Visible = true;
        }

        private void ResetExportUI()
        {
            lblStep1.Text = " Searching save file on Windows Registry";
            lblStep2.Text = " Creating save file";
            lblStep3.Text = " Copying save file";

            lblStep1.ForeColor =
            lblStep2.ForeColor =
            lblStep3.ForeColor = Color.Black;

            progressBar.Value = 0;
        }

        private void PassExport(Label lbl, string text, int progress)
        {
            lbl.Text = "✓" + text;
            lbl.ForeColor = Color.Green;
            progressBar.Value = progress;
            Application.DoEvents();
        }

        private void FailExport(Label lbl, string text)
        {
            lbl.Text = "X" + text;
            lbl.ForeColor = Color.Red;
            Application.DoEvents();
        }


        // ------------------------ EXPORT ------------------------
        private async void exportBtn_Click(object sender, EventArgs e)
        {
            if (exportRunning) return;
            exportRunning = true;

            exportBtn.Enabled = false;
            loadBtn.Enabled = false;

            try
            {
                if (loadExpanded)
                {
                    await AdjustFormHeightSmooth(-LOAD_HEIGHT);
                    HideLoadUI();
                    loadExpanded = false;
                }

                if (!exportExpanded)
                {
                    ShowExportUI();
                    await AdjustFormHeightSmooth(EXPORT_HEIGHT);
                    exportExpanded = true;
                }

                ResetExportUI();

                string registryPath = @"HKEY_CURRENT_USER\Software\Konsordo\Rake";
                string tempRegFile = Path.Combine(
                    Path.GetTempPath(),
                    $"Rake_{DateTime.Now:yyyyMMdd_HHmmss}.reg"
                );

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
                await Task.Delay(500);

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

                    using (var proc = Process.Start(psi))
                    {
                        proc.WaitForExit();
                        if (proc.ExitCode != 0)
                            throw new Exception("Registry export failed.");
                    }
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
                await Task.Delay(500);

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
                    FileName = $"Rake-{DateTime.Now:ddMMyyyy-HHmmss}.reg"
                })
                {
                    if (sfd.ShowDialog(this) == DialogResult.OK)
                    {
                        try
                        {
                            File.Copy(tempRegFile, sfd.FileName, true);
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
                exportBtn.Enabled = true;
                loadBtn.Enabled = true;
                exportRunning = false;
            }
        }





        // ------------------------ LOAD ------------------------
        private async void loadBtn_Click(object sender, EventArgs e)
        {
            if (loadRunning) return;
            loadRunning = true;

            exportBtn.Enabled = false;
            loadBtn.Enabled = false;

            try
            {
                if (exportExpanded)
                {
                    await AdjustFormHeightSmooth(-EXPORT_HEIGHT);
                    HideExportUI();
                    exportExpanded = false;
                }

                if (!loadExpanded)
                {
                    ShowLoadUI();
                    await AdjustFormHeightSmooth(LOAD_HEIGHT);
                    loadExpanded = true;
                }

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
                    Title = "Select Rake Save File"
                })
                {
                    if (ofd.ShowDialog(this) == DialogResult.OK)
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
                await Task.Delay(500);

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
                await Task.Delay(500);

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
                    Registry.CurrentUser.DeleteSubKeyTree(@"Software\Konsordo\Rake", false);
                    Registry.CurrentUser.CreateSubKey(@"Software\Konsordo\Rake");
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
                await Task.Delay(500);

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

                    using (var proc = Process.Start(psi))
                    {
                        proc.WaitForExit();
                        if (proc.ExitCode != 0)
                            throw new Exception("Registry import failed.");
                    }
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
                await Task.Delay(500);
            }
            finally
            {
                exportBtn.Enabled = true;
                loadBtn.Enabled = true;
                loadRunning = false;
            }
        }

        private void BMPIcon_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void lblStep1_Click(object sender, EventArgs e) { }
        private void lblStep2_Click(object sender, EventArgs e) { }
        private void lblStep3_Click(object sender, EventArgs e) { }
        private void labelLoadInfo_Click(object sender, EventArgs e) { }
        private void lblStepLoad1_Click(object sender, EventArgs e) { }
        private void lblStepLoad2_Click(object sender, EventArgs e) { }
        private void lblStepLoad3_Click(object sender, EventArgs e) { }
        private void lblStepLoad4_Click(object sender, EventArgs e) { }
        private void progressBar_Click(object sender, EventArgs e) { }
        private void progressBarLoad_Click(object sender, EventArgs e) { }
    }
}
