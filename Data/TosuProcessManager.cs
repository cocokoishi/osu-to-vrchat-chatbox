using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OsuOscVRC.Data
{
    public class TosuProcessManager : IDisposable
    {
        private Process? _process;
        private readonly string _exePath;
        private readonly string _host;
        private readonly int _port;

        public bool IsRunning => _process != null && !_process.HasExited;

        public TosuProcessManager(string exePath, string host, int port)
        {
            _exePath = exePath;
            _host = host;
            _port = port;
        }

        public bool IsPortInUse()
        {
            try
            {
                using var tcp = new TcpClient();
                var result = tcp.BeginConnect(_host, _port, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(500));
                if (success) { tcp.EndConnect(result); return true; }
                return false;
            }
            catch { return false; }
        }

        public string? Start()
        {
            if (IsPortInUse()) return null; // tosu already running

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = Path.IsPathRooted(_exePath) 
                ? _exePath 
                : Path.GetFullPath(Path.Combine(basePath, _exePath));
                
            if (!File.Exists(fullPath))
            {
                return $"tosu.exe not found at:\n{fullPath}\n\n" +
                       "Download tosu from:\nhttps://github.com/tosuapp/tosu/releases\n" +
                       "and place it in the tosu/ subfolder.";
            }

            try
            {
                // Write config to disable browser opening
                EnsureNoBrowser(Path.GetDirectoryName(fullPath)!);

                var psi = new ProcessStartInfo
                {
                    FileName = fullPath,
                    WorkingDirectory = Path.GetDirectoryName(fullPath) ?? ".",
                    UseShellExecute = true,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                };

                _process = Process.Start(psi);
                return _process == null ? "Failed to start tosu process." : null;
            }
            catch (Exception ex)
            {
                return $"Failed to start tosu: {ex.Message}";
            }
        }

        /// <summary>
        /// Writes/updates toso.toml to set openDashboardOnStartup = false
        /// </summary>
        private void EnsureNoBrowser(string tosuDir)
        {
            var configPath = Path.Combine(tosuDir, "toso.toml");
            try
            {
                if (File.Exists(configPath))
                {
                    string content = File.ReadAllText(configPath);
                    if (content.Contains("openDashboardOnStartup"))
                    {
                        content = Regex.Replace(content,
                            @"openDashboardOnStartup\s*=\s*\w+",
                            "openDashboardOnStartup = false");
                    }
                    else
                    {
                        content += "\nopenDashboardOnStartup = false\n";
                    }
                    File.WriteAllText(configPath, content);
                }
                else
                {
                    File.WriteAllText(configPath,
                        "[dangerous]\nopenDashboardOnStartup = false\n");
                }
            }
            catch { /* ignore config write errors */ }
        }

        public async Task<bool> WaitForReady(int timeoutMs = 15000)
        {
            var sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < timeoutMs)
            {
                if (_process != null && _process.HasExited) return false;
                if (IsPortInUse()) return true;
                await Task.Delay(500);
            }
            return false;
        }

        public void Stop()
        {
            if (_process == null) return;
            try
            {
                if (!_process.HasExited)
                {
                    _process.Kill(entireProcessTree: true);
                    _process.WaitForExit(3000);
                }
            }
            catch { }
            finally { _process.Dispose(); _process = null; }
        }

        public void Dispose() => Stop();
    }
}
