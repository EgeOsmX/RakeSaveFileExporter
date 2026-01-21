using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace RakeSaveFileExporter
{
    public partial class App : Form
    {
        public App()
        {
            InitializeComponent();
            this.Load += Menu_Load;
        }

        private Image uploadCloudIconOriginal;
        private Image loadCloudIconOriginal;
        private Image viewCloudIconOriginal;

        private bool exportExpanded = false;
        private bool exportRunning = false;

        private bool loadExpanded = false;
        private bool loadRunning = false;

        private const int EXPORT_HEIGHT = 121;
        private const int LOAD_HEIGHT = 137;

        private LocalExport preloadExport;
        private LocalLoad preloadLoad;

        private bool steamLoggedIn = false;
        private string steamID = null;
        private string steamUsername;

        private string accessToken = null;
        private string refreshToken = null;

        private readonly string RefreshTokenFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "AppData", "LocalLow", "EgeOsmX", "Game Save", "Data","refresh_token.dat"
        );

        private CancellationTokenSource loginPollCts;

        private async void Menu_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.Manual;
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point(
                screen.X + (screen.Width - this.Width) / 2,
                screen.Y + (screen.Height - this.Height) / 2
            );

            LoadLogo();
            LoadCloudIcons();

            steamLoggedIn = false;
            UpdateCloudUI();

            panelActive.Height = 0;
            panelActive.Controls.Clear();

            exportExpanded = false;
            loadExpanded = false;

            baseFormHeight = this.Height;

            PreloadPanels();

            InitializeAccountMenu();

            await TryPersistentLoginAsync();

            if (steamLoggedIn)
            {
                await SyncSessionFromWorkerAsync();
                UpdateAutoLogoutText();
            }
        }

        // ------------------------ PRELOAD IFRAMES ------------------------

        private void PreloadPanels()
        {
            exportControl = new LocalExport
            {
                Dock = DockStyle.Fill,
                Visible = false
            };

            loadControl = new LocalLoad
            {
                Dock = DockStyle.Fill,
                Visible = false
            };

            panelActive.Controls.Add(exportControl);
            panelActive.Controls.Add(loadControl);

            exportControl.CreateControl();
            loadControl.CreateControl();

            exportControl.PerformLayout();
            loadControl.PerformLayout();
        }

        // ------------------------ LOAD IMAGES ------------------------
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

        private void LoadCloudIcons()
        {
            var assembly = Assembly.GetExecutingAssembly();

            string uploadPath = "RakeSaveFileExporter.Resources.Cloud-Upload.png";
            using (var stream = assembly.GetManifestResourceStream(uploadPath))
            {
                if (stream != null)
                {
                    uploadCloudIconOriginal = Image.FromStream(stream);
                    uploadCloudIcon.Image = uploadCloudIconOriginal;
                    uploadCloudIcon.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else
                {
                    
                }
            }

            string loadPath = "RakeSaveFileExporter.Resources.Cloud-Download.png";
            using (var stream = assembly.GetManifestResourceStream(loadPath))
            {
                if (stream != null)
                {
                    loadCloudIconOriginal = Image.FromStream(stream);
                    loadCloudIcon.Image = loadCloudIconOriginal;
                    loadCloudIcon.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }

            string viewPath = "RakeSaveFileExporter.Resources.Cloud-ViewFiles.png";
            using (var stream = assembly.GetManifestResourceStream(viewPath))
            {
                if (stream != null)
                {
                    viewCloudIconOriginal = Image.FromStream(stream);
                    viewCloudIcon.Image = viewCloudIconOriginal;
                    viewCloudIcon.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
        }

        // ------------------------ FORM HEIGHT SETTINGS ------------------------

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

        // ------------------------ NAVBAR ------------------------

        private ToolStripMenuItem loginMenuItem;
        private ToolStripMenuItem logoutMenuItem;

        private ToolStripMenuItem autoLogoutInfoItem;
        private System.Windows.Forms.Timer logoutCountdownTimer;
        private DateTime? refreshExpiresAtUtc = null;

        private void InitializeAccountMenu()
        {
            stripMenuTab1.DropDownItems.Clear();

            loginMenuItem = new ToolStripMenuItem("Log in with Steam");
            loginMenuItem.Click += async (s, e) => await DoSteamLoginAsync();

            logoutMenuItem = new ToolStripMenuItem("Log out");
            logoutMenuItem.Click += async (s, e) => await DoSteamLogoutAsync();

            UpdateAccountMenu();
        }

        private void UpdateAccountMenu()
        {
            stripMenuTab1.DropDownItems.Clear();

            if (!steamLoggedIn)
            {
                stripMenuTab1.DropDownItems.Add(loginMenuItem);
                return;
            }

            string username = steamUsername ?? steamID;

            var infoItem = new ToolStripMenuItem($"Logged in with Steam ({username})")
            {
                Enabled = false
            };

            autoLogoutInfoItem = new ToolStripMenuItem("Auto log out in 00:00:00:00")
            {
                Enabled = false
            };

            stripMenuTab1.DropDownItems.Add(infoItem);
            stripMenuTab1.DropDownItems.Add(autoLogoutInfoItem);
            stripMenuTab1.DropDownItems.Add(new ToolStripSeparator());
            stripMenuTab1.DropDownItems.Add(logoutMenuItem);
        }

        private async Task SyncSessionFromWorkerAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(refreshToken)) return;

                using (var client = new HttpClient())
                {
                    var payload = new { refreshToken = refreshToken };
                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                    var resp = await client.PostAsync("https://github-gamesave.egeosmx.workers.dev/session-info", content);
                    if (!resp.IsSuccessStatusCode) return;

                    var json = await resp.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(json);

                    if ((string)data.status != "ok") return;

                    long remainingMs = data.remainingMs != null ? (long)data.remainingMs : -1;
                    long expiresAtMs = data.expiresAt != null ? (long)data.expiresAt : -1;

                    if (expiresAtMs > 0)
                    {
                        refreshExpiresAtUtc = DateTimeOffset.FromUnixTimeMilliseconds(expiresAtMs).UtcDateTime;
                        StartOrUpdateCountdownTimer();
                    }
                    else if (remainingMs >= 0)
                    {
                        refreshExpiresAtUtc = DateTime.UtcNow.AddMilliseconds(remainingMs);
                        StartOrUpdateCountdownTimer();
                    }
                    this.BeginInvoke((Action)(() =>
                    {
                        UpdateAccountMenu();
                        UpdateAutoLogoutText();
                    }));
                }
            }
            catch
            {
                
            }
        }

        private void StartOrUpdateCountdownTimer()
        {
            if (logoutCountdownTimer == null)
            {
                logoutCountdownTimer = new System.Windows.Forms.Timer();
                logoutCountdownTimer.Interval = 1000;
                logoutCountdownTimer.Tick += (s, e) => UpdateAutoLogoutText();
            }

            if (!logoutCountdownTimer.Enabled)
                logoutCountdownTimer.Start();

            UpdateAutoLogoutText();
        }

        private void UpdateAutoLogoutText()
        {
            if (autoLogoutInfoItem == null) return;

            if (refreshExpiresAtUtc == null)
            {
                autoLogoutInfoItem.Text = "Auto log out in 00:00:00:00";
                return;
            }

            var remaining = refreshExpiresAtUtc.Value - DateTime.UtcNow;
            if (remaining <= TimeSpan.Zero)
            {
                autoLogoutInfoItem.Text = "Auto log out in 00:00:00:00";
                return;
            }

            autoLogoutInfoItem.Text =
                $"Auto log out in {remaining.Days:D2}:{remaining.Hours:D2}:{remaining.Minutes:D2}:{remaining.Seconds:D2}";
        }

        private void stripMenuTab2VisitGitHub_Click(object sender, EventArgs e)
        {
            const string githubUrl = "https://github.com/EgeOsmX/RakeSaveFileExporter";

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = githubUrl,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not open the GitHub page.\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // ------------------------ CLOUD HELPERS ------------------------

        private void UpdateCloudUI()
        {
            bool active = steamLoggedIn;

            uploadCloudBtn.Enabled = active;
            loadCloudBtn.Enabled = active;
            viewCloudBtn.Enabled = active;

            SetPictureBoxOpacity(uploadCloudIcon, active ? 1f : 0.4f);
            SetPictureBoxOpacity(loadCloudIcon, active ? 1f : 0.4f);
            SetPictureBoxOpacity(viewCloudIcon, active ? 1f : 0.4f);
        }

        private void SetPictureBoxOpacity(PictureBox pb, float opacity)
        {
            Image original = null;

            if (pb == uploadCloudIcon) original = uploadCloudIconOriginal;
            else if (pb == loadCloudIcon) original = loadCloudIconOriginal;
            else if (pb == viewCloudIcon) original = viewCloudIconOriginal;

            if (original == null) return;

            Bitmap bmp = new Bitmap(original.Width, original.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                g.Clear(Color.Transparent);

                System.Drawing.Imaging.ColorMatrix cm = new System.Drawing.Imaging.ColorMatrix
                {
                    Matrix33 = opacity
                };
                System.Drawing.Imaging.ImageAttributes ia = new System.Drawing.Imaging.ImageAttributes();
                ia.SetColorMatrix(cm, System.Drawing.Imaging.ColorMatrixFlag.Default, System.Drawing.Imaging.ColorAdjustType.Bitmap);

                g.DrawImage(original,
                            new Rectangle(0, 0, bmp.Width, bmp.Height),
                            0, 0, original.Width, original.Height,
                            GraphicsUnit.Pixel, ia);
            }

            pb.Image = bmp;
            pb.BackColor = Color.Transparent;
            pb.Parent = pb.Parent;
            pb.Invalidate();
        }

        // ------------------------ EXPORT, LOAD ------------------------

        private const int EXPANDED_HEIGHT = 160;

        private LocalExport exportControl;
        private LocalLoad loadControl;
        private CloudUpload cloudUploadControl;

        private bool isPanelBusy = false;
        private bool panelOpen = false;
        private bool formExpanded = false;

        private int baseFormHeight;

        private ActivePanel currentPanel = ActivePanel.None;

        private enum ActivePanel
        {
            None,
            Export,
            Load,
            CloudUpload,
            CloudLoad
        }

        private async void exportBtn_Click(object sender, EventArgs e)
        {
            exportBtn.Enabled = false;
            loadBtn.Enabled = false;
            uploadCloudBtn.Enabled = false;
            viewCloudBtn.Enabled = false;
            loadCloudBtn.Enabled = false;

            try
            {
                if (cloudUploadControl != null)
                    cloudUploadControl.Visible = false;

                if (currentPanel == ActivePanel.Export)
                {
                    exportControl.Visible = true;
                    exportControl.BringToFront();

                    await Task.Delay(16);
                    await exportControl.StartExportAsync();
                    return;
                }

                if (currentPanel != ActivePanel.None)
                    await AdjustFormHeightSmooth(baseFormHeight - this.Height);

                exportControl.Visible = true;
                loadControl.Visible = false;

                exportControl.BringToFront();

                await AdjustFormHeightSmooth(110);
                panelActive.Height = 110;

                await Task.Delay(16);

                currentPanel = ActivePanel.Export;

                await exportControl.StartExportAsync();
            }
            finally
            {
                exportBtn.Enabled = true;
                loadBtn.Enabled = true;
                
                UpdateCloudUI();
            }
        }

        private async void loadBtn_Click(object sender, EventArgs e)
        {
            exportBtn.Enabled = false;
            loadBtn.Enabled = false;
            uploadCloudBtn.Enabled = false;
            viewCloudBtn.Enabled = false;
            loadCloudBtn.Enabled = false;

            try
            {
                if (cloudUploadControl != null)
                    cloudUploadControl.Visible = false;

                if (currentPanel == ActivePanel.Load)
                {
                    loadControl.Visible = true;
                    loadControl.BringToFront();

                    await Task.Delay(16);
                    await loadControl.StartLoadAsync();
                    return;
                }

                if (currentPanel != ActivePanel.None)
                    await AdjustFormHeightSmooth(baseFormHeight - this.Height);

                loadControl.Visible = true;
                exportControl.Visible = false;

                loadControl.BringToFront();

                await AdjustFormHeightSmooth(132);
                panelActive.Height = 132;

                await Task.Delay(16);

                currentPanel = ActivePanel.Load;

                await loadControl.StartLoadAsync();
            }
            finally
            {
                exportBtn.Enabled = true;
                loadBtn.Enabled = true;

                UpdateCloudUI();
            }
        }

        // ------------------------ CLOUD UPLOAD/LOAD ------------------------

        private CloudLoad cloudLoadControl;

        private async void uploadCloudBtn_Click(object sender, EventArgs e)
        {
            exportBtn.Enabled = false;
            loadBtn.Enabled = false;
            uploadCloudBtn.Enabled = false;
            viewCloudBtn.Enabled = false;
            loadCloudBtn.Enabled = false;

            try
            {
                if (!steamLoggedIn)
                {
                    MessageBox.Show("Please log in with Steam first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cloudUploadControl == null)
                    cloudUploadControl = new CloudUpload();

                cloudUploadControl.SteamId = steamID;
                cloudUploadControl.AuthToken = accessToken ?? await GetJwtTokenAsync(steamID);
                cloudUploadControl.Dock = DockStyle.Fill;

                if (currentPanel == ActivePanel.CloudUpload)
                {
                    cloudUploadControl.Visible = true;
                    cloudUploadControl.BringToFront();

                    await Task.Delay(16);
                    await cloudUploadControl.StartUploadAsync();
                    return;
                }

                if (currentPanel != ActivePanel.None)
                {
                    await AdjustFormHeightSmooth(baseFormHeight - this.Height);
                    panelActive.Height = 0;
                    await Task.Delay(16);
                }

                exportControl.Visible = false;
                loadControl.Visible = false;

                if (!panelActive.Controls.Contains(cloudUploadControl))
                    panelActive.Controls.Add(cloudUploadControl);

                cloudUploadControl.Visible = true;
                cloudUploadControl.BringToFront();

                int targetH = cloudUploadControl.PreferredPanelHeight;

                await AdjustFormHeightSmooth(targetH);
                panelActive.Height = targetH;

                await Task.Delay(16);

                currentPanel = ActivePanel.CloudUpload;

                await cloudUploadControl.StartUploadAsync();
            }
            finally
            {
                exportBtn.Enabled = true;
                loadBtn.Enabled = true;

                UpdateCloudUI();
            }
        }

        private string AuthToken = null;
        private string RefreshToken = null;

        private async void loadCloudBtn_Click(object sender, EventArgs e)
        {
            exportBtn.Enabled = false;
            loadBtn.Enabled = false;
            uploadCloudBtn.Enabled = false;
            loadCloudBtn.Enabled = false;
            viewCloudBtn.Enabled = false;

            try
            {
                if (!steamLoggedIn)
                {
                    MessageBox.Show("Please log in with Steam first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cloudLoadControl == null)
                    cloudLoadControl = new CloudLoad();

                cloudLoadControl.SteamId = steamID;
                cloudLoadControl.AuthToken = accessToken ?? await GetJwtTokenAsync(steamID);
                cloudLoadControl.Dock = DockStyle.Fill;

                if (currentPanel == ActivePanel.CloudLoad)
                {
                    cloudLoadControl.Visible = true;
                    cloudLoadControl.BringToFront();

                    await Task.Delay(16);
                    await cloudLoadControl.StartCloudLoadAsync();
                    return;
                }

                if (currentPanel != ActivePanel.None)
                {
                    await AdjustFormHeightSmooth(baseFormHeight - this.Height);
                    panelActive.Height = 0;
                    await Task.Delay(16);
                }

                exportControl.Visible = false;
                loadControl.Visible = false;
                if (cloudUploadControl != null) cloudUploadControl.Visible = false;

                if (!panelActive.Controls.Contains(cloudLoadControl))
                    panelActive.Controls.Add(cloudLoadControl);

                cloudLoadControl.Visible = true;
                cloudLoadControl.BringToFront();

                int targetH = cloudLoadControl.PreferredPanelHeight; // CloudUpload gibi
                await AdjustFormHeightSmooth(targetH);
                panelActive.Height = targetH;

                await Task.Delay(16);

                currentPanel = ActivePanel.CloudLoad;

                await cloudLoadControl.StartCloudLoadAsync();
            }
            finally
            {
                exportBtn.Enabled = true;
                loadBtn.Enabled = true;

                UpdateCloudUI();
            }
        }

        // ------------------------ STEAM LOGIN/LOGOUT ------------------------

        private async Task DoSteamLoginAsync()
        {
            loginPollCts?.Cancel();
            loginPollCts = new CancellationTokenSource();

            try
            {
                using (var client = new HttpClient())
                {
                    var respBody = await client.GetStringAsync("https://github-gamesave.egeosmx.workers.dev/start-login");
                    dynamic data = JsonConvert.DeserializeObject(respBody);
                    string loginId = data.loginId;
                    string loginUrl = data.loginUrl;

                    try
                    {
                        OpenLoginUrlSmart(loginUrl, width: 1100, height: 820);
                    }
                    catch
                    {
                        MessageBox.Show("Could not open browser automatically. Please open this URL manually:\n" + loginUrl,
                            "Browser launch failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    bool completed = false;
                    int maxAttempts = 120;
                    for (int i = 0; i < maxAttempts; i++)
                    {
                        if (loginPollCts.Token.IsCancellationRequested) break;
                        await Task.Delay(2000, loginPollCts.Token);

                        string statusUrl = $"https://github-gamesave.egeosmx.workers.dev/login-status?loginId={Uri.EscapeDataString(loginId)}";
                        HttpResponseMessage statusResp = null;

                        try
                        {
                            statusResp = await client.GetAsync(statusUrl, loginPollCts.Token);
                        }
                        catch
                        {
                            continue;
                        }

                        if (!statusResp.IsSuccessStatusCode)
                        {
                            if ((int)statusResp.StatusCode == 400 || (int)statusResp.StatusCode == 404)
                                break;
                            continue;
                        }

                        var json = await statusResp.Content.ReadAsStringAsync();
                        dynamic st = JsonConvert.DeserializeObject(json);

                        string status = st.status;
                        if (status == "pending")
                        {
                            continue;
                        }
                        else if (status == "expired")
                        {
                            break;
                        }
                        else if (status == "ok")
                        {
                            accessToken = st.token;
                            refreshToken = st.refreshToken;
                            steamID = st.steamId;

                            try
                            {
                                SaveRefreshToken(refreshToken);
                            }
                            catch
                            {
                                
                            }

                            steamUsername = await GetSteamUsernameAsync(steamID);

                            steamLoggedIn = true;

                            UpdateCloudUI();
                            UpdateAccountMenu();

                            await SyncSessionFromWorkerAsync();

                            UpdateAutoLogoutText();

                            await SyncSessionFromWorkerAsync();
                            this.BeginInvoke((Action)(() => UpdateAutoLogoutText()));

                            completed = true;
                            break;
                        }
                    }

                    if (!completed)
                    {
                        MessageBox.Show("Steam login timed out or was cancelled.", "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                
            }
        }
        private async Task DoSteamLogoutAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    using (var client = new HttpClient())
                    {
                        var payload = new { refreshToken = refreshToken };
                        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                        try
                        {
                            await client.PostAsync("https://github-gamesave.egeosmx.workers.dev/revoke-refresh", content);
                        }
                        catch
                        {
                            
                        }
                    }
                }
            }
            catch
            {
                
            }

            DeleteRefreshToken();
            accessToken = null;
            refreshToken = null;
            steamID = null;
            steamUsername = null;
            steamLoggedIn = false;

            logoutCountdownTimer?.Stop();
            logoutCountdownTimer = null;
            refreshExpiresAtUtc = null;
            autoLogoutInfoItem = null;

            loginPollCts?.Cancel();

            this.BeginInvoke((Action)(() =>
            {
                UpdateCloudUI();
                UpdateAccountMenu();
            }));
        }

        private void DoSteamLogout()
        {
            steamLoggedIn = false;
            steamID = null;
            steamUsername = null;

            AuthToken = null;
            RefreshToken = null;

            UpdateCloudUI();
            UpdateAccountMenu();
        }

        // ------------------------ HELPERS ------------------------

        private static string FindChromeExe()
        {
            try
            {
                using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe"))
                {
                    var path = key?.GetValue("") as string;
                    if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
                        return path;
                }
            }
            catch { }

            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe"))
                {
                    var path = key?.GetValue("") as string;
                    if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
                        return path;
                }
            }
            catch { }

            string[] candidates =
            {
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Google\\Chrome\\Application\\chrome.exe"),
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Google\\Chrome\\Application\\chrome.exe"),
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Google\\Chrome\\Application\\chrome.exe"),
    };

            foreach (var p in candidates)
                if (File.Exists(p)) return p;

            return null;
        }

        private void OpenLoginUrlSmart(string loginUrl, int width = 520, int height = 760)
        {
            var chromeExe = FindChromeExe();

            if (!string.IsNullOrWhiteSpace(chromeExe))
            {
                string profileDir = Path.Combine(Path.GetTempPath(), "RakeSaveFileExporter_SteamLoginProfile");
                try { Directory.CreateDirectory(profileDir); } catch { }

                Rectangle wa = Screen.PrimaryScreen.WorkingArea;

                int w = Math.Min(width, wa.Width);
                int h = Math.Min(height, wa.Height);

                int x = wa.X + (wa.Width - w) / 2;
                int y = wa.Y + (wa.Height - h) / 2;

                var psi = new ProcessStartInfo
                {
                    FileName = chromeExe,
                    Arguments =
                        $"--app=\"{loginUrl}\" " +
                        $"--user-data-dir=\"{profileDir}\" " +
                        $"--window-size={w},{h} " +
                        $"--window-position={x},{y}",
                    UseShellExecute = false
                };

                Process.Start(psi);
                return;
            }

            Process.Start(new ProcessStartInfo
            {
                FileName = loginUrl,
                UseShellExecute = true
            });
        }

        private async Task TryPersistentLoginAsync()
        {
            try
            {
                var rt = LoadRefreshToken();
                if (string.IsNullOrEmpty(rt)) return;

                using (var client = new HttpClient())
                {
                    var payload = new { refreshToken = rt };
                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                    HttpResponseMessage resp = null;
                    try
                    {
                        resp = await client.PostAsync("https://github-gamesave.egeosmx.workers.dev/refresh-token", content);
                    }
                    catch
                    {
                        return;
                    }

                    if (!resp.IsSuccessStatusCode)
                    {
                        DeleteRefreshToken();
                        return;
                    }

                    var json = await resp.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(json);

                    accessToken = data.token;
                    refreshToken = data.refreshToken;
                    steamID = data.steamId;

                    SaveRefreshToken(refreshToken);

                    steamUsername = await GetSteamUsernameAsync(steamID);

                    steamLoggedIn = true;

                    this.BeginInvoke((Action)(() =>
                    {
                        UpdateCloudUI();
                        UpdateAccountMenu();
                    }));
                }
            }
            catch
            {
                
            }
        }

        private void SaveRefreshToken(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token)) return;

                var dir = Path.GetDirectoryName(RefreshTokenFilePath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                var bytes = Encoding.UTF8.GetBytes(token);
                var encrypted = ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser);
                File.WriteAllBytes(RefreshTokenFilePath, encrypted);
            }
            catch
            {
                
            }
        }

        private string LoadRefreshToken()
        {
            try
            {
                if (!File.Exists(RefreshTokenFilePath)) return null;
                var encrypted = File.ReadAllBytes(RefreshTokenFilePath);
                var bytes = ProtectedData.Unprotect(encrypted, null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return null;
            }
        }

        private void DeleteRefreshToken()
        {
            try
            {
                if (File.Exists(RefreshTokenFilePath))
                    File.Delete(RefreshTokenFilePath);
            }
            catch
            {
                
            }
        }

        private async Task<string> GetJwtTokenAsync(string steamId)
        {
            try
            {
                if (string.IsNullOrEmpty(refreshToken))
                    return null;

                using (HttpClient client = new HttpClient())
                {
                    var payload = new { refreshToken = refreshToken };
                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                    using (HttpResponseMessage resp = await client.PostAsync("https://github-gamesave.egeosmx.workers.dev/get-jwt", content))
                    {
                        if (!resp.IsSuccessStatusCode)
                            return null;

                        string json = await resp.Content.ReadAsStringAsync();
                        dynamic data = JsonConvert.DeserializeObject(json);

                        string newAccess = data?.token;
                        string newRefresh = data?.refreshToken;

                        if (!string.IsNullOrEmpty(newRefresh))
                        {
                            refreshToken = newRefresh;
                            try { SaveRefreshToken(refreshToken); } catch { }
                        }

                        string sid = data?.steamId;
                        if (!string.IsNullOrEmpty(sid))
                            steamID = sid;

                        return newAccess;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        private async Task<string> GetSteamUsernameAsync(string steamId)
        {
            try
            {
                string workerUrl = $"https://github-gamesave.egeosmx.workers.dev/username?steamid={steamId}";

                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(5);
                    using (HttpResponseMessage resp = await client.GetAsync(workerUrl))
                    {
                        resp.EnsureSuccessStatusCode();
                        string json = await resp.Content.ReadAsStringAsync();
                        dynamic data = JsonConvert.DeserializeObject(json);
                        return data?.username;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        // ------------------------ ICON CLICK SHORTCUTS ------------------------

        private void uploadCloudIcon_Click(object sender, EventArgs e)
        {
            if (!steamLoggedIn) return;
            uploadCloudBtn.PerformClick();
        }

        private void loadCloudIcon_Click(object sender, EventArgs e)
        {
            if (!steamLoggedIn) return;
            loadCloudBtn.PerformClick();
        }

        private void viewCloudIcon_Click(object sender, EventArgs e)
        {
            if (!steamLoggedIn) return;
            viewCloudBtn.PerformClick();
        }

        private async void viewCloudBtn_Click(object sender, EventArgs e)
        {
            exportBtn.Enabled = false;
            loadBtn.Enabled = false;
            uploadCloudBtn.Enabled = false;
            loadCloudBtn.Enabled = false;
            viewCloudBtn.Enabled = false;

            try
            {
                if (!steamLoggedIn)
                {
                    MessageBox.Show("Please log in with Steam first.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var token = accessToken ?? await GetJwtTokenAsync(steamID);
                if (string.IsNullOrEmpty(token))
                {
                    MessageBox.Show("Auth token could not be obtained.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var vf = new ViewFiles())
                {
                    vf.AuthToken = token;
                    vf.StartPosition = FormStartPosition.CenterParent;

                    vf.ShowDialog(this);
                }
            }
            finally
            {
                exportBtn.Enabled = true;
                loadBtn.Enabled = true;

                UpdateCloudUI();
            }
        }

        private void panelActive_Paint(object sender, PaintEventArgs e) { }
        private void labelLoadInfo_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void BMPIcon_Click(object sender, EventArgs e) { }
        private void stripMenu_Click(object sender, EventArgs e) { }
    }
}
