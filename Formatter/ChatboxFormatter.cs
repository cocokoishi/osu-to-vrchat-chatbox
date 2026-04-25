using System;
using System.Text.RegularExpressions;
using OsuOscVRC.Config;
using OsuOscVRC.Data;

namespace OsuOscVRC.Formatter
{
    public static class ChatboxFormatter
    {
        public static string Format(OsuState? state, GameState gameState, AppConfig config)
        {
            if (state == null) return "";

            string template;
            switch (gameState)
            {
                case GameState.NotRunning:
                    return config.Templates.NotRunning ?? "";
                case GameState.Idle:
                case GameState.Menu:
                    template = config.Templates.IdleText ?? "";
                    if (string.IsNullOrEmpty(template)) return "";
                    break;
                case GameState.SongSelect:
                    template = config.Templates.SongSelect ?? "";
                    break;
                case GameState.WatchingReplay:
                    template = $"{config.Templates.WatchingReplay}\n{config.Templates.PlayingLine2}";
                    break;
                case GameState.Playing:
                    template = $"{config.Templates.PlayingLine1}\n{config.Templates.PlayingLine2}";
                    break;
                case GameState.Paused:
                    template = $"{config.Templates.PausedPrefix}{config.Templates.PlayingLine1}\n{config.Templates.PlayingLine2}";
                    break;
                case GameState.ReplayResultScreen:
                    template = config.Templates.ReplayResult ?? "";
                    break;
                case GameState.ResultScreen:
                    template = config.Templates.ResultScreen ?? "";
                    break;
                default:
                    return "";
            }

            return ApplyVariables(template, state, gameState, config);
        }

        private static string ApplyVariables(string template, OsuState state, GameState gameState, AppConfig config)
        {
            var title = config.UseUnicodeTitle && !string.IsNullOrEmpty(state.Beatmap?.TitleUnicode)
                ? state.Beatmap.TitleUnicode
                : (state.Beatmap?.Title ?? "");

            var artist = config.UseUnicodeTitle && !string.IsNullOrEmpty(state.Beatmap?.ArtistUnicode)
                ? state.Beatmap.ArtistUnicode
                : (state.Beatmap?.Artist ?? "");

            if (config.ShowArtist && !string.IsNullOrEmpty(artist))
                title = $"{artist} - {title}";

            // Truncate title and version to MaxTitleLength
            int maxLen = config.MaxTitleLength > 0 ? config.MaxTitleLength : 50;
            if (title.Length > maxLen)
                title = title.Substring(0, maxLen) + "…";

            string version = state.Beatmap?.Version ?? "";
            if (version.Length > maxLen)
                version = version.Substring(0, maxLen) + "…";

            // Mode
            var mode = (state.Play?.Mode?.Name ?? "").ToLower();
            bool isResult = gameState == GameState.ResultScreen || gameState == GameState.ReplayResultScreen;
            if (isResult) mode = (state.ResultsScreen?.Mode?.Name ?? mode).ToLower();

            var modeName = mode switch
            {
                "taiko" => config.ModeNames.Taiko,
                "catch" => config.ModeNames.Catch,
                "mania" => config.ModeNames.Mania,
                _ => config.ModeNames.Osu
            };

            double accuracy = isResult ? (state.ResultsScreen?.Accuracy ?? 0) : (state.Play?.Accuracy ?? 0);
            double pp = isResult ? (state.ResultsScreen?.Pp?.Current ?? 0) : (state.Play?.Pp?.Current ?? 0);
            string rank = isResult ? (state.ResultsScreen?.Rank ?? "") : (state.Play?.Rank?.Current ?? "");

            var timeCurrent = FormatTime(state.Beatmap?.Time?.Live ?? 0);
            var timeTotal = FormatTime(state.Beatmap?.Time?.LastObject ?? 0);
            string mods = state.Play?.Mods?.Name ?? "";

            double stars = state.Beatmap?.Stats?.Stars?.Total ?? 0;
            if (stars == 0) stars = state.Beatmap?.Stats?.Stars?.Live ?? 0;
            string starsStr = stars.ToString($"F{config.StarDecimals}");
            string accStr = accuracy.ToString($"F{config.AccuracyDecimals}");
            string ppStr = Math.Round(pp, config.PpDecimals).ToString($"F{config.PpDecimals}");
            string ppFcStr = Math.Round(state.Play?.Pp?.Fc ?? 0, config.PpDecimals).ToString($"F{config.PpDecimals}");

            var result = template
                .Replace("{title}", title)
                .Replace("{artist}", artist)
                .Replace("{version}", version)
                .Replace("{stars}", starsStr)
                .Replace("{mode}", modeName)
                .Replace("{time_current}", timeCurrent)
                .Replace("{time_total}", timeTotal)
                .Replace("{accuracy}", accStr)
                .Replace("{pp}", ppStr)
                .Replace("{pp_fc}", ppFcStr)
                .Replace("{rank}", rank)
                .Replace("{mods}", mods)
                .Replace("{combo}", (state.Play?.Combo?.Current ?? 0).ToString())
                .Replace("{max_combo}", (state.Play?.Combo?.Max ?? 0).ToString())
                .Replace("{player}", state.Play?.PlayerName ?? "");

            result = Regex.Replace(result, @"  +", " ");
            result = string.Join("\n", Array.ConvertAll(result.Split('\n'), l => l.Trim()));

            if (result.Length > config.MaxMessageLength)
                result = result.Substring(0, config.MaxMessageLength - 3) + "...";

            return result;
        }

        private static string FormatTime(int ms)
        {
            if (ms < 0) return "0:00";
            var ts = TimeSpan.FromMilliseconds(ms);
            return $"{(int)ts.TotalMinutes}:{ts.Seconds:D2}";
        }
    }
}
