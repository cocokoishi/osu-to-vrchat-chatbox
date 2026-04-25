using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Win32;
using OsuOscVRC.Config;
using OsuOscVRC.Data;
using OsuOscVRC.Formatter;
using OsuOscVRC.I18n;
using OsuOscVRC.OSC;

namespace OsuOscVRC
{
    public partial class MainWindow : Window
    {
        private AppConfig _config;
        private TosuProcessManager? _tosuProcess;
        private TosuWebSocketClient? _tosuClient;
        private VRChatOscSender? _oscSender;
        private DispatcherTimer _updateTimer;

        private GameState _currentGameState = GameState.NotRunning;
        private int _lastTimeLive = -1;
        private DateTime _lastTimeLiveChanged = DateTime.MinValue;
        private DateTime _resultScreenStartTime = DateTime.MinValue;

        public MainWindow()
        {
            InitializeComponent();
            ApplyI18n();

            _config = ConfigManager.Load();
            ConfigManager.CheckFirstRun(_config);

            PopulateUiFromConfig();

            _updateTimer = new DispatcherTimer();
            _updateTimer.Tick += UpdateTimer_Tick;

            UpdateStatus();
        }

        // ─── i18n ────────────────────────────────────────────────

        private void ApplyI18n()
        {
            Title = Translator.Get("Title");
            GrpPreview.Header = Translator.Get("PreviewLabel");
            BtnStart.Content = Translator.Get("Start");
            BtnStop.Content = Translator.Get("Stop");
            BtnSave.Content = Translator.Get("SaveConfig");

            TabConnection.Header = Translator.Get("TabConnection");
            TabTemplates.Header = Translator.Get("TabTemplates");
            TabDisplay.Header = Translator.Get("TabDisplay");
            TabAdvanced.Header = Translator.Get("TabAdvanced");

            LblTosuExePath.Content = Translator.Get("TosuExePath");
            LblTosuPort.Content = Translator.Get("TosuPort");
            LblOscHost.Content = Translator.Get("OscHost");
            LblOscPort.Content = Translator.Get("OscPort");

            LblTplPlaying1.Content = Translator.Get("TplPlaying1");
            LblTplPlaying2.Content = Translator.Get("TplPlaying2");
            LblTplPaused.Content = Translator.Get("TplPaused");
            LblTplReplay.Content = Translator.Get("TplReplay");
            LblTplSongSelect.Content = Translator.Get("TplSongSelect");
            LblTplResult.Content = Translator.Get("TplResult");
            LblTplIdle.Content = Translator.Get("TplIdle");

            CbUseUnicode.Content = Translator.Get("DispUseUnicode");
            CbShowArtist.Content = Translator.Get("DispShowArtist");
            LblDispStar.Content = Translator.Get("DispStarDecimals");
            LblDispPp.Content = Translator.Get("DispPpDecimals");
            LblDispAcc.Content = Translator.Get("DispAccDecimals");
            LblDispModeOsu.Content = Translator.Get("DispModeOsu");
            LblDispModeTaiko.Content = Translator.Get("DispModeTaiko");
            LblDispModeCatch.Content = Translator.Get("DispModeCatch");
            LblDispModeMania.Content = Translator.Get("DispModeMania");

            LblAdvUpdate.Content = Translator.Get("AdvUpdateInterval");
            LblAdvResult.Content = Translator.Get("AdvResultDuration");
            LblAdvPause.Content = Translator.Get("AdvPauseThreshold");
            LblAdvReconnect.Content = Translator.Get("AdvReconnectDelay");
            LblAdvMaxLen.Content = Translator.Get("AdvMaxLength");
        }

        // ─── Config ↔ UI ─────────────────────────────────────────

        private void PopulateUiFromConfig()
        {
            TbTosuExePath.Text = _config.TosuExePath;
            TbTosuPort.Text = _config.TosuPort.ToString();
            TbOscHost.Text = _config.VRChatOscHost;
            TbOscPort.Text = _config.VRChatOscPort.ToString();

            TbTplPlaying1.Text = _config.Templates.PlayingLine1;
            TbTplPlaying2.Text = _config.Templates.PlayingLine2;
            TbTplPaused.Text = _config.Templates.PausedPrefix;
            TbTplReplay.Text = _config.Templates.WatchingReplay;
            TbTplSongSelect.Text = _config.Templates.SongSelect;
            TbTplResult.Text = _config.Templates.ResultScreen;
            TbTplIdle.Text = _config.Templates.IdleText;

            CbUseUnicode.IsChecked = _config.UseUnicodeTitle;
            CbShowArtist.IsChecked = _config.ShowArtist;
            TbDispStar.Text = _config.StarDecimals.ToString();
            TbDispPp.Text = _config.PpDecimals.ToString();
            TbDispAcc.Text = _config.AccuracyDecimals.ToString();
            TbDispModeOsu.Text = _config.ModeNames.Osu;
            TbDispModeTaiko.Text = _config.ModeNames.Taiko;
            TbDispModeCatch.Text = _config.ModeNames.Catch;
            TbDispModeMania.Text = _config.ModeNames.Mania;

            TbAdvUpdate.Text = _config.UpdateIntervalMs.ToString();
            TbAdvResult.Text = _config.ResultScreenDurationS.ToString();
            TbAdvPause.Text = _config.PauseDetectionThresholdMs.ToString();
            TbAdvReconnect.Text = _config.ReconnectDelayMs.ToString();
            TbAdvMaxLen.Text = _config.MaxMessageLength.ToString();
        }

        private void SaveUiToConfig()
        {
            _config.TosuExePath = TbTosuExePath.Text;
            if (int.TryParse(TbTosuPort.Text, out int tp)) _config.TosuPort = tp;
            _config.VRChatOscHost = TbOscHost.Text;
            if (int.TryParse(TbOscPort.Text, out int op)) _config.VRChatOscPort = op;

            _config.Templates.PlayingLine1 = TbTplPlaying1.Text;
            _config.Templates.PlayingLine2 = TbTplPlaying2.Text;
            _config.Templates.PausedPrefix = TbTplPaused.Text;
            _config.Templates.WatchingReplay = TbTplReplay.Text;
            _config.Templates.SongSelect = TbTplSongSelect.Text;
            _config.Templates.ResultScreen = TbTplResult.Text;
            _config.Templates.IdleText = TbTplIdle.Text;

            _config.UseUnicodeTitle = CbUseUnicode.IsChecked ?? false;
            _config.ShowArtist = CbShowArtist.IsChecked ?? false;
            if (int.TryParse(TbDispStar.Text, out int sd)) _config.StarDecimals = sd;
            if (int.TryParse(TbDispPp.Text, out int pd)) _config.PpDecimals = pd;
            if (int.TryParse(TbDispAcc.Text, out int ad)) _config.AccuracyDecimals = ad;

            _config.ModeNames.Osu = TbDispModeOsu.Text;
            _config.ModeNames.Taiko = TbDispModeTaiko.Text;
            _config.ModeNames.Catch = TbDispModeCatch.Text;
            _config.ModeNames.Mania = TbDispModeMania.Text;

            if (int.TryParse(TbAdvUpdate.Text, out int au)) _config.UpdateIntervalMs = au;
            if (int.TryParse(TbAdvResult.Text, out int ar)) _config.ResultScreenDurationS = ar;
            if (int.TryParse(TbAdvPause.Text, out int ap)) _config.PauseDetectionThresholdMs = ap;
            if (int.TryParse(TbAdvReconnect.Text, out int arec)) _config.ReconnectDelayMs = arec;
            if (int.TryParse(TbAdvMaxLen.Text, out int am)) _config.MaxMessageLength = am;

            ConfigManager.Save(_config);
        }

        // ─── Button handlers ──────────────────────────────────────

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveUiToConfig();
            MessageBox.Show(Translator.Get("ConfigSaved"), "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnBrowseTosu_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "tosu|tosu.exe|All|*.*",
                Title = "Select tosu.exe"
            };
            if (dlg.ShowDialog() == true)
            {
                TbTosuExePath.Text = dlg.FileName;
            }
        }

        private async void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            SaveUiToConfig();

            BtnStart.IsEnabled = false;
            BtnStop.IsEnabled = true;

            // 1) Start tosu process
            TxtStatus.Text = Translator.Get("StatusStartingTosu");
            _tosuProcess = new TosuProcessManager(_config.TosuExePath, _config.TosuHost, _config.TosuPort);

            string? err = _tosuProcess.Start();
            if (err != null)
            {
                MessageBox.Show(err, Translator.Get("TosuNotFound"), MessageBoxButton.OK, MessageBoxImage.Warning);
                DoStop();
                return;
            }

            // Wait for tosu to be ready
            bool ready = await _tosuProcess.WaitForReady(8000);
            if (!ready)
            {
                MessageBox.Show(Translator.Get("TosuWaitTimeout"), "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                DoStop();
                return;
            }

            // 2) Init OSC sender
            try
            {
                _oscSender = new VRChatOscSender(_config.VRChatOscHost, _config.VRChatOscPort);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"OSC Init Error: {ex.Message}", "Error");
                DoStop();
                return;
            }

            // 3) Connect WebSocket to tosu
            _tosuClient = new TosuWebSocketClient(_config.TosuHost, _config.TosuPort, _config.ReconnectDelayMs);
            _tosuClient.OnConnectionChanged += (connected) =>
            {
                Dispatcher.Invoke(UpdateStatus);
            };
            await _tosuClient.StartAsync();

            // 4) Start update timer
            _updateTimer.Interval = TimeSpan.FromMilliseconds(_config.UpdateIntervalMs);
            _updateTimer.Start();

            UpdateStatus();
        }

        private void BtnStop_Click(object? sender, RoutedEventArgs? e) => DoStop();

        private void DoStop()
        {
            _updateTimer.Stop();

            _tosuClient?.Dispose();
            _tosuClient = null;

            try { _oscSender?.ClearChatbox(); } catch { }
            _oscSender?.Dispose();
            _oscSender = null;

            _tosuProcess?.Dispose();
            _tosuProcess = null;

            BtnStart.IsEnabled = true;
            BtnStop.IsEnabled = false;
            _currentGameState = GameState.NotRunning;

            UpdateStatus();
            TxtPreview.Text = "...";
        }

        // ─── Periodic update ──────────────────────────────────────

        private void UpdateTimer_Tick(object? sender, EventArgs e)
        {
            if (_tosuClient == null || !_tosuClient.IsConnected || _tosuClient.LatestState == null)
            {
                _currentGameState = GameState.NotRunning;
                UpdateStatus();
                return;
            }

            UpdateStatus();

            var state = _tosuClient.LatestState;
            DetermineGameState(state);

            string output = ChatboxFormatter.Format(state, _currentGameState, _config);
            TxtPreview.Text = string.IsNullOrEmpty(output) ? "..." : output;

            if (_oscSender != null && !string.IsNullOrEmpty(output))
            {
                try { _oscSender.SendChatbox(output); }
                catch { }
            }
        }

        // ─── State machine ───────────────────────────────────────

        private void DetermineGameState(OsuState state)
        {
            int rawState = state.State?.Number ?? 0;

            // Forced result screen hold
            if (_currentGameState == GameState.ResultScreen)
            {
                bool holdExpired = (DateTime.Now - _resultScreenStartTime).TotalSeconds >= _config.ResultScreenDurationS;
                if (!holdExpired && rawState != 2)
                    return;
            }

            if (rawState == 7)
            {
                if (_currentGameState != GameState.ResultScreen)
                    _resultScreenStartTime = DateTime.Now;
                _currentGameState = GameState.ResultScreen;
                return;
            }

            if (rawState == 5)
            {
                _currentGameState = GameState.SongSelect;
                return;
            }

            if (rawState == 2)
            {
                bool isReplay = !string.IsNullOrEmpty(state.Play?.PlayerName);

                int timeLive = state.Beatmap?.Time?.Live ?? 0;
                if (timeLive != _lastTimeLive)
                {
                    _lastTimeLive = timeLive;
                    _lastTimeLiveChanged = DateTime.Now;
                    _currentGameState = isReplay ? GameState.WatchingReplay : GameState.Playing;
                }
                else
                {
                    if ((DateTime.Now - _lastTimeLiveChanged).TotalMilliseconds > _config.PauseDetectionThresholdMs)
                        _currentGameState = GameState.Paused;
                }
                return;
            }

            _currentGameState = GameState.Idle;
        }

        // ─── Status indicator ─────────────────────────────────────

        private void UpdateStatus()
        {
            if (_tosuClient == null)
            {
                TxtStatus.Text = Translator.Get("StatusDisconnected");
            }
            else if (_tosuClient.IsConnected)
            {
                TxtStatus.Text = Translator.Get("StatusConnected");
            }
            else
            {
                TxtStatus.Text = Translator.Get("StatusTosuDisconnected");
            }
        }

        // ─── Cleanup ──────────────────────────────────────────────

        protected override void OnClosing(CancelEventArgs e)
        {
            SaveUiToConfig();
            DoStop();
            base.OnClosing(e);
        }
    }
}