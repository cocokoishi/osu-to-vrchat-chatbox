# osu! to VRChat Chatbox

A small Windows desktop app that reads live gameplay data from [tosu](https://github.com/tosuapp/tosu) and sends formatted status text to the VRChat chatbox over OSC.

This project is aimed at players who want their current osu! activity to appear in VRChat automatically, including gameplay, pause, replay, results, and failed runs.

## Features

- Launches and connects to `tosu` automatically
- Sends chatbox text to VRChat over OSC
- Live preview window before messages are sent
- Editable templates for different game states
- Supports:
  - Playing
  - Paused
  - Failed
  - Song Select
  - Editor
  - Replay
  - Replay Result
  - Result Screen
  - Idle / Lobby
- Adjustable update rate, result hold time, pause threshold, and message length
- Unicode title support
- Optional artist display
- English / Chinese UI based on system UI language

## Requirements

- Windows
- .NET 8 SDK if you want to build from source
- VRChat with OSC enabled
- `tosu`
- osu! stable or lazer supported by your `tosu` setup

## Quick Start

### Option 1: Use a Release Build

1. Download the latest release zip from this repository.
2. Extract it anywhere on your PC.
3. Make sure the bundled `tosu` folder is present.
4. Start `OsuOscVRC.exe`.
5. In VRChat, make sure OSC is enabled.
6. Click `Start` in the app.

The app will:

- start `tosu` if needed
- connect to its websocket
- send messages to VRChat on `127.0.0.1:9000`

## Running From Source

```powershell
dotnet build .\OsuOscVRC.csproj
dotnet run --project .\OsuOscVRC.csproj
```

If you run from source, make sure `tosu.exe` exists at:

```text
tosu/tosu.exe
```

or update the path in the app UI.

## Configuration

The app creates a config file on first launch:

```text
config_osuosc.yaml
```

Default connection values:

- `tosu host`: `127.0.0.1`
- `tosu port`: `24050`
- `VRChat OSC host`: `127.0.0.1`
- `VRChat OSC port`: `9000`

### Main Config Areas

- `Connection`
  - `tosu.exe` path
  - `tosu` websocket port
  - VRChat OSC host / port
- `Templates`
  - message format for each state
- `Display`
  - unicode titles
  - show artist
  - decimal precision
  - custom mode names
- `Advanced`
  - update interval
  - result screen duration
  - pause detection threshold
  - reconnect delay
  - max message length
  - max title length

## Template Variables

Available placeholders:

```text
{title} {artist} {version} {stars} {mode} {time_current} {time_total}
{accuracy} {pp} {pp_fc} {rank} {mods} {combo} {max_combo} {player}
```

Example defaults:

```text
Playing line 1: Playing osu!{mode} {title} [{version}] ★{stars}
Playing line 2: {time_current}/{time_total} {accuracy}% {mods} {pp}PP
Paused prefix: [Paused]
Result screen: [Cleared!] {title} | {version} | ★{stars} | {rank} | Finally {accuracy}% | Get {pp}PP
```

## State Notes

- Failed gameplay is latched until the run exits, so it does not flicker back into `Paused`.
- Replay detection is based on the current player name versus the local profile name.
- Result states are held briefly to keep the chatbox readable.

## Building a Release Package

The repository includes a GitHub Actions workflow that:

- publishes a self-contained Windows build
- downloads the latest `tosu` Windows release
- bundles everything into a release zip when a `v*` tag is pushed

## Project Structure

```text
Config/      App config loading and defaults
Data/        State models, tosu process manager, websocket client
Formatter/   Chatbox text formatting
I18n/        UI translation strings
OSC/         VRChat OSC sender
```

## Usage Notice

Please use this responsibly. Sending constantly changing status text into public spaces can be distracting. Private rooms are recommended.

## Disclaimer

This project is unofficial and is not affiliated with VRChat, ppy, or the tosu project.
