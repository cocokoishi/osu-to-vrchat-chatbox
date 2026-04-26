# osu! → VRChat Chatbox

把 osu! 的实时游戏数据发送到 VRChat 聊天框。

自动启动 tosu、连接 WebSocket、格式化文字、通过 OSC 发送到 VRChat。

## 功能

- 自动启动和连接 [tosu](https://github.com/tosuapp/tosu)
- 支持多种游戏状态：选歌、游玩、暂停、失败、结算、回放、编辑、空闲
- 每个状态可自定义聊天框模板
- 支持 `{mode}` / `{artist}` / `{stars}` / `{pp}` 等变量
- 支持 osu! / taiko / catch / mania 四种模式，模式名可自定义
- 中文 / English 界面（跟随系统语言）
- 同时支持 osu! Stable 和 osu! Lazer

## 使用

1. 下载最新 Release 并解压
2. 确保 `tosu/tosu.exe` 存在（或手动指定路径）
3. 打开 VRChat 的 OSC 功能
4. 启动 `OsuOscVRC.exe`，点击「开始」

启动后应用会自动启动 tosu、连接其 WebSocket，并发送聊天框消息到 `127.0.0.1:9000`。

## 从源码运行

需要 .NET 8 SDK。

```powershell
dotnet build OsuOscVRC.csproj
dotnet run --project OsuOscVRC.csproj
```

## 配置

首次运行生成 `config_osuosc.yaml`，所有设置可在 UI 中修改。

### 模板变量

```
{title}      谱面标题
{artist}     艺术家
{version}    难度名
{stars}      星数
{mode}       模式名（可自定义）
{time_current}  当前时间 (m:ss)
{time_total}    总时长 (m:ss)
{accuracy}   准确度
{pp}         当前 PP
{pp_fc}      FC PP
{rank}       评级 (SS/S/A/B/C/D/F)
{mods}       模组（无模组显示 NM）
{miss}       miss 数量
{combo}      当前连击
{max_combo}  最大连击
{player}     玩家名 / 回放作者
```

`{mode}` 使用「显示」选项卡里的自定义模式名，默认 osu! 为空、其余为 `taiko` / `catch` / `mania`。

`{player}` 在观看回放时显示回放对象的名字，其他状态显示自己的名字。

### 默认模板

| 状态 | 模板 |
|------|------|
| 游玩 | `Playing osu!{mode} {title} [{version}] *{stars}`（第一行） |
| 游玩（第二行） | `{time_current}/{time_total} {accuracy}% {miss}miss {mods} {pp}PP` |
| 暂停 | `[Paused] ` + 游玩第一行 + 第二行 |
| 失败 | `[Failed] ` + 游玩第一行 + 第二行 |
| 选歌 | `Selecting osu!{mode} {title} [{version}] *{stars}` |
| 结算 | `[Cleared!] osu!{mode} {title} \| {version} \| *{stars} \| {mods} \| {rank} \| Finally {accuracy}% \| {miss}miss \| Get {pp}PP` |
| 回放 | `Watching osu!{mode} {title} [{version}] *{stars} played by {player}`（第一行 + 游玩第二行） |
| 回放结算 | `Replay result osu!{mode} {title} \| {version} \| *{stars} \| {rank} \| {mods} \| {accuracy}% \| {miss}miss \| {pp}PP` |
| 编辑 | `Editing osu!{mode} {title} [{version}]` |
| 空闲 | `In osu! Lobby` |
| tosu 未启动 | (空) |

## 注意事项

- 请尽量在**私人房间**使用，频繁变动的聊天框文本在公共场所可能打扰他人。
- 应用会自动缓存最后一次的模组（mods）信息，避免 tosu 数据更新延迟时错误显示 "NM"。
- 所有配置保存在 `config_osuosc.yaml` 文件中，可以通过 UI 或直接编辑文件修改。

## 免责声明

本项目与 VRChat、ppy、tosu 项目无关，为非官方第三方工具。
