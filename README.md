# osu! → VRChat Chatbox

Sends real-time osu! gameplay data to the VRChat chatbox via OSC.

Automatically launches tosu, connects to its WebSocket, formats status text with customizable templates, and sends it to your VRChat chatbox.

## Features

- Auto-launch and connect to [tosu](https://github.com/tosuapp/tosu)
- Supports multiple game states: Song Select, Playing, Paused, Failed, Result, Replay, Editor, Idle
- Fully customizable chatbox templates per state with template variables
- Supports osu! / taiko / catch / mania with customizable mode names
- English / Chinese UI (follows system language)

## Usage

1. Download the latest release and extract it
2. Make sure `tosu/tosu.exe` exists (or set the path manually in the app)
3. Enable OSC in VRChat (Actions → OSC → Enable)
4. Launch `OsuOscVRC.exe` and click **Start**

The app will automatically start tosu, connect to its WebSocket, and send chatbox messages to `127.0.0.1:9000`.

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
{title}        {artist}       {version}      {stars}        {mode}
{time_current} {time_total}   {accuracy}     {rank}
{pp}           {pp_fc}        {combo}        {max_combo}
{mods}         {player}
```

- `{mode}` — uses the custom mode names from the Display tab. Defaults: osu! = `""`, taiko = `"taiko"`, catch = `"catch"`, mania = `"mania"`.
- `{player}` — shows the replay author while watching replays, or your own name during normal play.

### Default Templates

| State | Line 1 | Line 2 |
|-------|--------|--------|
| Playing | `Playing osu!{mode} {title} [{version}] *{stars}` | `{time_current}/{time_total} {accuracy}% {combo}x {mods} {pp}PP` |
| Paused | `[Paused] Playing osu!{mode} {title} [{version}] *{stars}` | `{time_current}/{time_total} {accuracy}% {combo}x {mods} {pp}PP` |
| Failed | `[Failed] Playing osu!{mode} {title} [{version}] *{stars}` | `{time_current}/{time_total} {accuracy}% {combo}x {mods} {pp}PP` |
| Song Select | `Selecting osu!{mode} {title} [{version}] *{stars}` | *(same as Playing)* |
| Result | `[Cleared!] osu!{mode} {title} \| {version} \| *{stars} \| {rank} \| Finally {accuracy}% \| Get {pp}PP` | *(single line)* |
| Watching Replay | `Watching osu!{mode} {title} [{version}] *{stars} played by {player}` | `{time_current}/{time_total} {accuracy}% {combo}x {mods} {pp}PP` |
| Editor | `Editing osu!{mode} {title} [{version}]` | *(single line)* |
| Idle | `In osu! Lobby` | *(single line)* |

> The second line for Playing templates is shared across Playing, Paused, Failed, and Watching Replay states via the **Playing Line 2** setting.

## Building a Release

Pushing a `v*` tag triggers GitHub Actions to build and publish automatically.

To build manually:

```powershell
dotnet publish OsuOscVRC.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish
```

## Notes

Please use this in private rooms. Frequently updating chatbox text can be distracting in public spaces.

## Disclaimer

This project is unofficial and not affiliated with VRChat, ppy, or the tosu project.

---

*Built with DeepSeek V4 Pro + Claude Code — cost: 6 RMB.*
