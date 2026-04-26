using System.Globalization;

namespace OsuOscVRC.I18n
{
    public static class Translator
    {
        public static bool IsChinese { get; }

        static Translator()
        {
            var culture = CultureInfo.CurrentUICulture.Name.ToLower();
            IsChinese = culture.StartsWith("zh");
        }

        public static string Get(string key) => IsChinese ? ZH(key) : EN(key);

        private static string ZH(string k) => k switch
        {
            "Title" => "osu! → VRChat OSC",
            "StatusConnected" => "● 已连接 | tosu 运行中 | OSC 活动中",
            "StatusDisconnected" => "○ 未连接 | 等待 tosu...",
            "StatusTosuDisconnected" => "○ 正在连接 tosu...",
            "StatusStartingTosu" => "○ 正在启动 tosu...",
            "PreviewLabel" => "当前输出预览:",
            "Start" => "▶ 开始",
            "Stop" => "⏹ 停止",
            "SaveConfig" => "保存配置",
            "ConfigSaved" => "配置已保存！",
            "FirstRunTitle" => "⚠ 注意事项 / NOTICE",
            "FirstRunMessage" => "请尽量在私人房间使用本软件，以免影响他人游玩体验。\nPlease use this software in PRIVATE rooms to avoid disturbing others' gameplay experience.",
            "TabConnection" => "连接",
            "TabTemplates" => "模板",
            "TabDisplay" => "显示",
            "TabAdvanced" => "高级",
            "TosuExePath" => "tosu.exe 路径:",
            "TosuPort" => "tosu 端口:",
            "OscHost" => "VRC OSC 地址:",
            "OscPort" => "VRC OSC 端口:",
            "TplPlaying1" => "游玩中 (第一行):",
            "TplPlaying2" => "游玩中 (第二行):",
            "TplPaused" => "暂停前缀:",
            "TplReplay" => "观看回放:",
            "TplReplayResult" => "回放结算:",
            "TplSongSelect" => "选歌界面:",
            "TplEditor" => "编辑模式:",
            "TplResult" => "结算界面:",
            "TplIdle" => "大厅/空闲:",
            "TplVarsHint" => "提示：模板中使用 {变量名} 作为占位符，运行时会自动替换为实际数据。以下为全部可用变量（可从下方复制）：",
            "TplVarsTitle" => "全部可用变量（可复制）：",
            "TplVarsAll" => "{title} - 谱面标题\n{artist} - 艺术家\n{version} - 难度名\n{stars} - 星数\n{mode} - 模式名（可自定义）\n{time_current} - 当前时间 (m:ss)\n{time_total} - 总时长 (m:ss)\n{accuracy} - 准确度\n{pp} - 当前 PP\n{pp_fc} - FC PP\n{rank} - 评级 (SS/S/A/B/C/D/F)\n{mods} - 模组 (无模组显示 NM)\n{miss} - miss 数量\n{combo} - 当前连击\n{max_combo} - 最大连击\n{player} - 玩家名 / 回放作者",
            "DispUseUnicode" => "使用原版标题 (Unicode)",
            "DispShowArtist" => "显示艺术家名称",
            "DispStarDecimals" => "星数小数位:",
            "DispPpDecimals" => "PP 小数位:",
            "DispAccDecimals" => "准确度小数位:",
            "DispModeOsu" => "osu! 模式:",
            "DispModeTaiko" => "taiko 模式:",
            "DispModeCatch" => "catch 模式:",
            "DispModeMania" => "mania 模式:",
            "AdvOscRate" => "OSC 发送间隔 (ms):",
            "AdvResultDuration" => "结算停留 (s):",
            "AdvPauseThreshold" => "暂停阈值 (ms):",
            "AdvReconnectDelay" => "重连延迟 (ms):",
            "AdvMaxLength" => "最大消息长度:",
            "AdvMaxTitleLen" => "标题最大长度:",
            "Browse" => "浏览...",
            "TosuNotFound" => "找不到 tosu.exe",
            "TosuWaitTimeout" => "tosu 启动超时，请确认路径正确后重试。",
            _ => k
        };

        private static string EN(string k) => k switch
        {
            "Title" => "osu! → VRChat OSC",
            "StatusConnected" => "● Connected | tosu Running | OSC Active",
            "StatusDisconnected" => "○ Disconnected | Waiting for tosu...",
            "StatusTosuDisconnected" => "○ Connecting to tosu...",
            "StatusStartingTosu" => "○ Starting tosu...",
            "PreviewLabel" => "Current Output Preview:",
            "Start" => "▶ Start",
            "Stop" => "⏹ Stop",
            "SaveConfig" => "Save Config",
            "ConfigSaved" => "Configuration saved!",
            "FirstRunTitle" => "NOTICE / ⚠ 注意事项",
            "FirstRunMessage" => "Please use this software in PRIVATE rooms to avoid disturbing others' gameplay experience.\n请尽量在私人房间使用本软件，以免影响他人游玩体验。",
            "TabConnection" => "Connection",
            "TabTemplates" => "Templates",
            "TabDisplay" => "Display",
            "TabAdvanced" => "Advanced",
            "TosuExePath" => "tosu.exe Path:",
            "TosuPort" => "tosu Port:",
            "OscHost" => "VRC OSC Host:",
            "OscPort" => "VRC OSC Port:",
            "TplPlaying1" => "Playing (Line 1):",
            "TplPlaying2" => "Playing (Line 2):",
            "TplPaused" => "Paused Prefix:",
            "TplReplay" => "Watching Replay:",
            "TplReplayResult" => "Replay Result:",
            "TplSongSelect" => "Song Select:",
            "TplEditor" => "Editor Mode:",
            "TplResult" => "Result Screen:",
            "TplIdle" => "Idle/Lobby:",
            "TplVarsHint" => "Tip: Use {variable} as placeholders in templates. They will be replaced with real data at runtime. All available variables (copy from below):",
            "TplVarsTitle" => "All Available Variables (copyable):",
            "TplVarsAll" => "{title} - Beatmap title\n{artist} - Artist name\n{version} - Difficulty name\n{stars} - Star rating\n{mode} - Mode name (customizable)\n{time_current} - Current time (m:ss)\n{time_total} - Total time (m:ss)\n{accuracy} - Accuracy\n{pp} - Current PP\n{pp_fc} - FC PP\n{rank} - Grade (SS/S/A/B/C/D/F)\n{mods} - Mods (shows NM if none)\n{miss} - Miss count\n{combo} - Current combo\n{max_combo} - Max combo\n{player} - Player name / replay author",
            "DispUseUnicode" => "Use Unicode Titles",
            "DispShowArtist" => "Show Artist",
            "DispStarDecimals" => "Star Decimals:",
            "DispPpDecimals" => "PP Decimals:",
            "DispAccDecimals" => "Accuracy Decimals:",
            "DispModeOsu" => "osu! Mode:",
            "DispModeTaiko" => "taiko Mode:",
            "DispModeCatch" => "catch Mode:",
            "DispModeMania" => "mania Mode:",
            "AdvOscRate" => "OSC Send Rate (ms):",
            "AdvResultDuration" => "Result Duration (s):",
            "AdvPauseThreshold" => "Pause Threshold (ms):",
            "AdvReconnectDelay" => "Reconnect Delay (ms):",
            "AdvMaxLength" => "Max Message Length:",
            "AdvMaxTitleLen" => "Max Title Length:",
            "Browse" => "Browse...",
            "TosuNotFound" => "tosu.exe not found",
            "TosuWaitTimeout" => "tosu startup timed out. Verify the path and try again.",
            _ => k
        };
    }
}
