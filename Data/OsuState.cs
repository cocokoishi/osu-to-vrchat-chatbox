using System.Text.Json.Serialization;

namespace OsuOscVRC.Data
{
    public class OsuState
    {
        [JsonPropertyName("state")] public StateData State { get; set; } = new();
        [JsonPropertyName("settings")] public SettingsData Settings { get; set; } = new();
        [JsonPropertyName("beatmap")] public BeatmapData Beatmap { get; set; } = new();
        [JsonPropertyName("play")] public PlayData Play { get; set; } = new();
        [JsonPropertyName("resultsScreen")] public ResultsScreenData ResultsScreen { get; set; } = new();
    }

    public class StateData
    {
        [JsonPropertyName("number")] public int Number { get; set; }
    }

    public class SettingsData
    {
        [JsonPropertyName("mode")] public ModeData Mode { get; set; } = new();
    }

    public class ModeData
    {
        [JsonPropertyName("name")] public string Name { get; set; } = "";
    }

    public class BeatmapData
    {
        [JsonPropertyName("title")] public string Title { get; set; } = "";
        [JsonPropertyName("titleUnicode")] public string TitleUnicode { get; set; } = "";
        [JsonPropertyName("artist")] public string Artist { get; set; } = "";
        [JsonPropertyName("artistUnicode")] public string ArtistUnicode { get; set; } = "";
        [JsonPropertyName("version")] public string Version { get; set; } = "";
        [JsonPropertyName("time")] public TimeData Time { get; set; } = new();
        [JsonPropertyName("stats")] public StatsData Stats { get; set; } = new();
    }

    public class TimeData
    {
        [JsonPropertyName("live")] public int Live { get; set; }
        [JsonPropertyName("lastObject")] public int LastObject { get; set; }
    }

    public class StatsData
    {
        [JsonPropertyName("stars")] public StarsData Stars { get; set; } = new();
    }

    public class StarsData
    {
        [JsonPropertyName("live")] public double Live { get; set; }
        [JsonPropertyName("total")] public double Total { get; set; }
    }

    public class PlayData
    {
        [JsonPropertyName("playerName")] public string PlayerName { get; set; } = "";
        [JsonPropertyName("isReplay")] public bool IsReplay { get; set; }
        [JsonPropertyName("mode")] public ModeData Mode { get; set; } = new();
        [JsonPropertyName("accuracy")] public double Accuracy { get; set; }
        [JsonPropertyName("pp")] public PpData Pp { get; set; } = new();
        [JsonPropertyName("rank")] public RankData Rank { get; set; } = new();
        [JsonPropertyName("mods")] public ModsData Mods { get; set; } = new();
        [JsonPropertyName("combo")] public ComboData Combo { get; set; } = new();
    }

    public class ComboData
    {
        [JsonPropertyName("current")] public int Current { get; set; }
        [JsonPropertyName("max")] public int Max { get; set; }
    }

    public class PpData
    {
        [JsonPropertyName("current")] public double Current { get; set; }
        [JsonPropertyName("fc")] public double Fc { get; set; }
    }

    public class RankData
    {
        [JsonPropertyName("current")] public string Current { get; set; } = "";
    }

    public class ModsData
    {
        [JsonPropertyName("name")] public string Name { get; set; } = "";
    }

    public class ResultsScreenData
    {
        [JsonPropertyName("accuracy")] public double Accuracy { get; set; }
        [JsonPropertyName("mode")] public ModeData Mode { get; set; } = new();
        [JsonPropertyName("rank")] public string Rank { get; set; } = "";
        [JsonPropertyName("pp")] public PpData Pp { get; set; } = new();
    }
}
