# osu! → VRChat Chatbox

Sends real-time osu! gameplay data to the VRChat chatbox.

Automatically launches tosu, connects to its WebSocket, formats status text, and sends it to VRChat via OSC.

## Features

- Auto-launch and connect to [tosu](https://github.com/tosuapp/tosu)
- Supports multiple game states: Song Select, Playing, Paused, Failed, Result, Replay, Editor, Idle
- Customizable chatbox templates per state
- Template variables: `{mode}`, `{artist}`, `{stars}`, `{pp}`, and more
- Supports osu! / taiko / catch / mania modes with customizable mode names
- English / Chinese UI based on system language
- Works with both osu! Stable and osu! Lazer

## Usage

1. Download the latest release and extract it
2. Ensure `tosu/tosu.exe` exists (or set the path manually in the app)
3. Enable OSC in VRChat
4. Launch `OsuOscVRC.exe` and click Start

When started, the app will automatically start tosu, connect to its WebSocket, and send chatbox messages to `127.0.0.1:9000`.

## Running From Source

Requires .NET 8 SDK.

```powershell
dotnet build OsuOscVRC.csproj
dotnet run --project OsuOscVRC.csproj
```

## Configuration

A `config_osuosc.yaml` file is created on first launch. All settings can be edited from the UI.

### Template Variables

```
{title}      Beatmap title
{artist}     Artist name
{version}    Difficulty name
{stars}      Star rating
{mode}       Mode name (customizable)
{time_current}  Current time (m:ss)
{time_total}    Total time (m:ss)
{accuracy}   Accuracy
{pp}         Current PP
{pp_fc}      Full-combo PP
{rank}       Grade (SS/S/A/B/C/D/F)
{mods}       Active mods (shows NM if none)
{miss}       Miss count
{combo}      Current combo
{max_combo}  Max combo
{player}     Player name / replay author
```

`{mode}` uses the custom mode names from the Display tab. By default, osu! mode is empty and the rest are `taiko` / `catch` / `mania`.

`{player}` shows the replay author's name while watching replays, or your own name during normal play.

### Default Templates

| State | Template |
|-------|----------|
| Playing | `Playing osu!{mode} {title} [{version}] *{stars}` |
| Paused | `[Paused] ` + Playing template |
| Failed | `[Failed] ` + Playing template |
| Song Select | `Selecting osu!{mode} {title} [{version}] *{stars}` |
| Result | `[Cleared!] osu!{mode} {title} \| {version} \| *{stars} \| {mods} \| {rank} \| Finally {accuracy}% \| {miss}miss \| Get {pp}PP` |
| Replay | `Watching osu!{mode} {title} [{version}] *{stars} played by {player}` |
| Editor | `Editing osu!{mode} {title} [{version}]` |
| Idle | `In osu! Lobby` |
| tosu not running | (empty) |

## Notes

Please use this in private rooms. Frequently updating chatbox text can be distracting in public spaces.

## Disclaimer

This project is unofficial and not affiliated with VRChat, ppy, or the tosu project.
