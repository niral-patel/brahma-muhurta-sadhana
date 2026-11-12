// Brahma Muhurta Sadhana for Windows.
// One .exe that is both the app and its installer: it hosts the web app (embedded in this file) in a WebView2 window.
// Built with the C# compiler that ships with Windows (.NET Framework 4.x), so the code stays within C# 5.
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace Sadhana
{
    static class Program
    {
        public const string AppName = "Brahma Muhurta Sadhana";
        public const string Id = "BrahmaMuhurtaSadhana";
        public const string Version = "1.0.0";
        public static string DataDir, InstallDir, InstalledExe;

        [STAThread]
        static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            DataDir = Path.Combine(local, Id);
            InstallDir = Path.Combine(Path.Combine(local, "Programs"), Id);
            InstalledExe = Path.Combine(InstallDir, Id + ".exe");
            string self = Application.ExecutablePath;
            bool installed = string.Equals(Path.GetFullPath(self), Path.GetFullPath(InstalledExe), StringComparison.OrdinalIgnoreCase);
            string arg = args.Length > 0 ? args[0].ToLowerInvariant() : "";
            try
            {
                if (arg == "--uninstall") { Installer.Uninstall(); return 0; }
                if (arg == "--install") { Installer.Install(self, true); return 0; }
                if (!installed && arg != "--portable")
                {
                    DialogResult r = MessageBox.Show(
                        "Install " + AppName + " on this PC?\n\n" +
                        "Yes: install it and add Start menu and Desktop shortcuts.\n" +
                        "No: just open it this time without installing.",
                        AppName + " " + Version, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (r == DialogResult.Cancel) return 0;
                    if (r == DialogResult.Yes) { Installer.Install(self, false); return 0; }
                }
                bool first;
                using (Mutex one = new Mutex(true, "Local\\" + Id, out first))
                {
                    if (!first)
                    {
                        MessageBox.Show(AppName + " is already open.", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return 0;
                    }
                    // The WebView2 libraries are embedded in this exe; hand them to .NET when it asks for them.
                    AppDomain.CurrentDomain.AssemblyResolve += ResolveEmbedded;
                    Run();
                }
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static void Run() { Application.Run(new MainForm()); }

        public static byte[] Resource(string name)
        {
            using (Stream st = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
            {
                if (st == null) return null;
                byte[] b = new byte[st.Length];
                int off = 0;
                while (off < b.Length) { int n = st.Read(b, off, b.Length - off); if (n <= 0) break; off += n; }
                return b;
            }
        }

        static Assembly ResolveEmbedded(object sender, ResolveEventArgs e)
        {
            byte[] b = Resource(new AssemblyName(e.Name).Name + ".dll");
            return b == null ? null : Assembly.Load(b);
        }

        public static void Log(string line)
        {
            try
            {
                Directory.CreateDirectory(DataDir);
                File.AppendAllText(Path.Combine(DataDir, "app.log"), DateTime.Now.ToString("s") + " " + line + Environment.NewLine);
            }
            catch { }
        }
    }

    class MainForm : Form
    {
        const string Host = "sadhana.local";
        readonly WebView2 web = new WebView2();

        public MainForm()
        {
            Text = Program.AppName;
            try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch { }
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1180, 860);
            MinimumSize = new Size(420, 600);
            BackColor = Color.FromArgb(255, 245, 230);
            web.Dock = DockStyle.Fill;
            web.DefaultBackgroundColor = BackColor;
            Controls.Add(web);
            Load += OnLoad;
        }

        // Copies the embedded files (the web app and the WebView2 loader) next to the user's data on every start,
        // so a new version of the exe always shows its own copy of the app.
        static void Unpack(string prefix, string dir)
        {
            Directory.CreateDirectory(dir);
            foreach (string name in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                if (!name.StartsWith(prefix, StringComparison.Ordinal)) continue;
                string path = Path.Combine(dir, name.Substring(prefix.Length));
                byte[] data = Program.Resource(name);
                try { File.WriteAllBytes(path, data); }
                catch (IOException) { if (!File.Exists(path)) throw; }       // in use by a running copy: keep it
            }
        }

        async void OnLoad(object sender, EventArgs e)
        {
            try
            {
                string webDir = Path.Combine(Program.DataDir, "web");
                string binDir = Path.Combine(Program.DataDir, "bin");
                Unpack("web/", webDir);
                Unpack("native/", binDir);
                CoreWebView2Environment.SetLoaderDllFolderPath(binDir);
                CoreWebView2Environment env = await CoreWebView2Environment.CreateAsync(null, Path.Combine(Program.DataDir, "WebView2"), null);
                await web.EnsureCoreWebView2Async(env);
                CoreWebView2 core = web.CoreWebView2;
                core.SetVirtualHostNameToFolderMapping(Host, webDir, CoreWebView2HostResourceAccessKind.Allow);
                core.Settings.AreDevToolsEnabled = false;
                core.Settings.AreDefaultContextMenusEnabled = false;
                core.Settings.IsStatusBarEnabled = false;
                core.PermissionRequested += delegate(object s, CoreWebView2PermissionRequestedEventArgs a)
                {
                    // the Panchang asks for the device location; this window only ever shows the app's own pages
                    if (a.PermissionKind == CoreWebView2PermissionKind.Geolocation) a.State = CoreWebView2PermissionState.Allow;
                };
                core.NewWindowRequested += delegate(object s, CoreWebView2NewWindowRequestedEventArgs a)
                {
                    a.Handled = true;
                    try { if (a.Uri.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) Process.Start(a.Uri); } catch { }
                };
                core.NavigationCompleted += delegate(object s, CoreWebView2NavigationCompletedEventArgs a)
                {
                    Program.Log("navigation " + (a.IsSuccess ? "ok" : "failed: " + a.WebErrorStatus));
                };
                core.Navigate("https://" + Host + "/index.html");
            }
            catch (WebView2RuntimeNotFoundException)
            {
                Program.Log("WebView2 runtime missing");
                MessageBox.Show(this,
                    "This app needs the Microsoft Edge WebView2 Runtime, which is missing on this PC.\n\n" +
                    "Install it from https://developer.microsoft.com/microsoft-edge/webview2/ (Evergreen Bootstrapper) and open the app again.",
                    Program.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
            catch (Exception ex)
            {
                Program.Log("start failed: " + ex);
                MessageBox.Show(this, "The app could not start.\n\n" + ex.Message, Program.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
    }

    static class Installer
    {
        const string UninstallKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\" + Program.Id;

        static string StartMenuLink
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Programs), Program.AppName + ".lnk"); }
        }

        static string DesktopLink
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), Program.AppName + ".lnk"); }
        }

        public static void Install(string self, bool silent)
        {
            Directory.CreateDirectory(Program.InstallDir);
            try { File.Copy(self, Program.InstalledExe, true); }
            catch (IOException)
            {
                throw new Exception("Close " + Program.AppName + " first, then run this installer again.");
            }
            Shortcut(StartMenuLink, Program.InstalledExe);
            Shortcut(DesktopLink, Program.InstalledExe);
            using (RegistryKey k = Registry.CurrentUser.CreateSubKey(UninstallKey))
            {
                k.SetValue("DisplayName", Program.AppName);
                k.SetValue("DisplayVersion", Program.Version);
                k.SetValue("Publisher", "Niral Patel");
                k.SetValue("DisplayIcon", Program.InstalledExe);
                k.SetValue("InstallLocation", Program.InstallDir);
                k.SetValue("UninstallString", "\"" + Program.InstalledExe + "\" --uninstall");
                k.SetValue("EstimatedSize", (int)(new FileInfo(Program.InstalledExe).Length / 1024), RegistryValueKind.DWord);
                k.SetValue("NoModify", 1, RegistryValueKind.DWord);
                k.SetValue("NoRepair", 1, RegistryValueKind.DWord);
            }
            Program.Log("installed " + Program.Version);
            if (silent) return;
            MessageBox.Show(Program.AppName + " is installed.\n\nOpen it from the Start menu or the Desktop shortcut. It will open now.",
                Program.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            Process.Start(Program.InstalledExe);
        }

        public static void Uninstall()
        {
            if (MessageBox.Show("Remove " + Program.AppName + " from this PC?", Program.AppName,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            bool wipe = MessageBox.Show(
                "Also delete your saved routine, history and books on this PC?\n\nChoose No to keep them for a later install.",
                Program.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
            try { File.Delete(StartMenuLink); } catch { }
            try { File.Delete(DesktopLink); } catch { }
            try { Registry.CurrentUser.DeleteSubKeyTree(UninstallKey, false); } catch { }
            // this exe cannot delete itself while it runs: a short hidden command finishes the job after it exits
            string cmd = "/c ping 127.0.0.1 -n 3 >nul & rmdir /s /q \"" + Program.InstallDir + "\"";
            if (wipe) cmd += " & rmdir /s /q \"" + Program.DataDir + "\"";
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", cmd);
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
            MessageBox.Show(Program.AppName + " was removed.", Program.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        static void Shortcut(string link, string target)
        {
            Type t = Type.GetTypeFromProgID("WScript.Shell");
            object shell = Activator.CreateInstance(t);
            object sc = t.InvokeMember("CreateShortcut", BindingFlags.InvokeMethod, null, shell, new object[] { link });
            Type st = sc.GetType();
            st.InvokeMember("TargetPath", BindingFlags.SetProperty, null, sc, new object[] { target });
            st.InvokeMember("WorkingDirectory", BindingFlags.SetProperty, null, sc, new object[] { Path.GetDirectoryName(target) });
            st.InvokeMember("Description", BindingFlags.SetProperty, null, sc, new object[] { "Guided morning sadhana with Panchang" });
            st.InvokeMember("IconLocation", BindingFlags.SetProperty, null, sc, new object[] { target + ",0" });
            st.InvokeMember("Save", BindingFlags.InvokeMethod, null, sc, null);
        }
    }
}
