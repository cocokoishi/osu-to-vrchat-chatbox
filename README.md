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
{title}   {artist}   {version}   {stars}   {mode}
{time_current}   {time_total}   {accuracy}   {rank}
{pp}   {pp_fc}   {combo}   {max_combo}   {mods}   {player}
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
| Result | `[Cleared!] osu!{mode} {title} \| {version} \| *{stars} \| {rank} \| Finally {accuracy}% \| Get {pp}PP` |
| Replay | `Watching osu!{mode} {title} [{version}] *{stars} played by {player}` |
| Editor | `Editing osu!{mode} {title} [{version}]` |
| Idle | `In osu! Lobby` |
| tosu not running | (empty) |

## Building a Release

GitHub Actions builds and packages a release automatically when a `v*` tag is pushed.

To build manually:

```powershell
dotnet publish OsuOscVRC.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish
```

## Notes

Please use this in private rooms. Frequently updating chatbox text can be distracting in public spaces.

## Disclaimer

This project is unofficial and not affiliated with VRChat, ppy, or the tosu project.

This project was built entirely with DeepSeek V4 Pro + Claude Code. Cost: 6 RMB.
