using System;
using System.IO;
using System.Windows;
using OsuOscVRC.I18n;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace OsuOscVRC.Config
{
    public static class ConfigManager
    {
        private const string ConfigPath = "config_osuosc.yaml";

        public static AppConfig Load()
        {
            if (!File.Exists(ConfigPath))
            {
                var defaultConfig = new AppConfig();
                Save(defaultConfig);
                return defaultConfig;
            }

            try
            {
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(UnderscoredNamingConvention.Instance)
                    .IgnoreUnmatchedProperties()
                    .Build();

                var yaml = File.ReadAllText(ConfigPath);
                var config = deserializer.Deserialize<AppConfig>(yaml) ?? new AppConfig();
                UpgradeTemplateDefaults(config);
                return config;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Translator.Get("ConfigLoadError"), ex.Message), Translator.Get("Error"), MessageBoxButton.OK, MessageBoxImage.Error);
                return new AppConfig();
            }
        }

        public static void Save(AppConfig config)
        {
            var serializer = new SerializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .Build();

            var yaml = serializer.Serialize(config);
            File.WriteAllText(ConfigPath, yaml);
        }

        public static void CheckFirstRun(AppConfig config)
        {
            if (!config.FirstRunDone)
            {
                MessageBox.Show(
                    Translator.Get("FirstRunMessage"),
                    Translator.Get("FirstRunTitle"),
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                config.FirstRunDone = true;
                Save(config);
            }
        }

        private static void UpgradeTemplateDefaults(AppConfig config)
        {
            config.Templates.WatchingReplay = UpgradeTemplate(
                config.Templates.WatchingReplay,
                new[]
                {
                    "Watching {title} [{version}] ★{stars} played by {player}",
                    "Watching {title} [{version}] 鈽厈stars} played by {player}"
                },
                "Watching osu!{mode} {title} [{version}] *{stars} played by {player}");

            config.Templates.ReplayResult = UpgradeTemplate(
                config.Templates.ReplayResult,
                new[]
                {
                    "{title} | {version} | ★{stars} | {rank} | {accuracy}% | {pp}PP",
                    "{title} | {version} | 鈽厈stars} | {rank} | {accuracy}% | {pp}PP"
                },
                "Replay result osu!{mode} {title} | {version} | *{stars} | {rank} | {mods} | {accuracy}% | {pp}PP");

            config.Templates.SongSelect = UpgradeTemplate(
                config.Templates.SongSelect,
                new[]
                {
                    "Idle: {title} [{version}] ★{stars}",
                    "Idle: {title} [{version}] 鈽厈stars}"
                },
                "Selecting osu!{mode} {title} [{version}] *{stars}");

            config.Templates.Editor = UpgradeTemplate(
                config.Templates.Editor,
                new[] { "Editing: {title} [{version}]" },
                "Editing osu!{mode} {title} [{version}]");

            config.Templates.ResultScreen = UpgradeTemplate(
                config.Templates.ResultScreen,
                new[]
                {
                    "[Cleared!] {title} | {version} | ★{stars} | {rank} | Finally {accuracy}% | Get {pp}PP",
                    "[Cleared!] {title} | {version} | 鈽厈stars} | {rank} | Finally {accuracy}% | Get {pp}PP",
                    "[Cleared!] osu!{mode} {title} | {version} | *{stars} | {mods} | {rank} | Finally {accuracy}% | Get {pp}PP"
                },
                "[Cleared!] osu!{mode} {title} | {version} | *{stars} | {mods} | {rank} | Finally {accuracy}% | {miss}miss | Get {pp}PP");

            config.Templates.PlayingLine2 = UpgradeTemplate(
                config.Templates.PlayingLine2,
                new[]
                {
                    "{time_current}/{time_total} {accuracy}% {mods} {pp}PP"
                },
                "{time_current}/{time_total} {accuracy}% {miss}miss {mods} {pp}PP");

            config.Templates.ReplayResult = UpgradeTemplate(
                config.Templates.ReplayResult,
                new[]
                {
                    "Replay result osu!{mode} {title} | {version} | *{stars} | {rank} | {mods} | {accuracy}% | {pp}PP"
                },
                "Replay result osu!{mode} {title} | {version} | *{stars} | {rank} | {mods} | {accuracy}% | {miss}miss | {pp}PP");

            config.Templates.IdleText = UpgradeTemplate(
                config.Templates.IdleText,
                new[] { "In Lobby" },
                "In osu! Lobby");
        }

        private static string UpgradeTemplate(string currentValue, string[] oldDefaults, string newDefault)
        {
            if (string.IsNullOrWhiteSpace(currentValue))
            {
                return newDefault;
            }

            foreach (var oldDefault in oldDefaults)
            {
                if (currentValue == oldDefault)
                {
                    return newDefault;
                }
            }

            return currentValue;
        }
    }
}
