using System;
using System.IO;
using System.Windows;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using OsuOscVRC.I18n;

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
                var config = deserializer.Deserialize<AppConfig>(yaml);
                
                // Set default for replay if it was missing in an old config
                if (string.IsNullOrEmpty(config.Templates.WatchingReplay))
                {
                    config.Templates.WatchingReplay = "Watching {title} [{version}] ★{stars} played by {player}";
                }
                
                return config ?? new AppConfig();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load config: {ex.Message}\nUsing default config.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
    }
}
