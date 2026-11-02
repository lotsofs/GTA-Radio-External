# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

A Windows desktop tool (C#, WinForms, .NET Framework 4.8) that reads a game's radio state from process memory and mutes or unmutes an external music player to match, so the player stands in for the in-game radio. It only reads game memory and never writes to it.

## Tracked work

Start at [docs/TODO.md](docs/TODO.md). It indexes open defects, tasks, porting work and live checks by three-letter code. Each item's evidence, plan and checks are in the doc that its group heading names. Finished items move to [docs/DONE.md](docs/DONE.md). Track new work there, not in `// TODO` comments. Changes that users will notice go under Unreleased in [CHANGELOG.md](CHANGELOG.md), written for users and without item codes.

## Build and run

`dotnet build` and `dotnet msbuild` can't build this project: they fail on the binary `.resx` resources (MSB3822/MSB3823). Use Visual Studio's MSBuild, which usually isn't on PATH:

```powershell
$msbuild = & "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe'
& $msbuild VGRadioExternal.sln /p:Configuration=Debug /v:minimal
```

- Output goes to `VGRadioExternal/bin/Debug/`. `packages/` isn't tracked, so a fresh clone needs a NuGet restore first.
- The CS0649 warnings on `GData` and `MPData` fields are expected: Newtonsoft.Json fills those fields by reflection.
- Run the exe from its output directory. `FormRE` finds `input/` through `Environment.CurrentDirectory`.
- There are no tests and no linter. Seeing a change work end to end needs a real game and music player running.

## Architecture

Two generations of code sit side by side, and only one of them runs.

**Active, config-driven:** `Program` starts `FormRE`, which owns a `Radio`.

- `FormRE` lists the JSON files in `input/games` and `input/music players` in two combo boxes and passes the selected path to `Radio`.
- `Radio` builds a `Game` and a `MusicPlayer` from those files and polls every 40 ms. When `Game.IsRadioOn()` changes, it calls `MusicPlayer.Mute()`.
- `Radio` raises `StatusChanged` when either status text changes, and `FormRE` shows both in its labels through `BeginInvoke`. `FormRE` starts the timer from `Load`, because `BeginInvoke` needs the window handle and a status is only reported when it changes.
- The poll uses `System.Timers.Timer`, so it runs on a thread-pool thread, one poll at a time: `AutoReset` is off and `Check` restarts the timer. On .NET Framework that timer swallows exceptions from its handler, so `GetProcess` and `Radio.CheckRadio` catch theirs and show them in the labels. Any other failure inside the poll is silent.
- `Game` and `MusicPlayer` deserialize their file into `GData` and `MPData`, look their process up again when it exits, and keep a `Status` text saying what they last found.
- `WinApi` holds the P/Invoke declarations, process and module lookup, typed memory reads, and simulated key presses.
- `AudioSessions` holds the Core Audio COM declarations and mutes an app at the per-app level the Windows Volume Mixer shows. It matches every process with the app's name, because any of them can own the session, and it skips output devices that refuse to list their sessions.

**Legacy, hardcoded, never run:** `Form1`, `ReadMemory`, `Settings` and the commented-out blocks in `Program.cs` still compile, but nothing instantiates `Form1`. `ReadMemory` is the reference for what hasn't been ported yet: GTA III and Vice City version detection, their radio status codes, and the pause and volume-key actions. `README.md` still describes this generation. What the active code supports is what's in `input/`.

Addresses in `ReadMemory` are absolute, while `addressOffset` in JSON is relative to the module base. Subtract the `0x400000` image base when porting: San Andreas 1.0 `0x8CB760` becomes `0x4CB760`.

The build targets x64 so it can read 64-bit games. `WinApi.GetModuleAddress` falls back to `EnumProcessModulesEx` for modules that `Process.Modules` doesn't list.

## JSON config files

`VGRadioExternal/readme.txt` documents the music player format for users. Where it differs from the code, the code is right.

- Keys match `GData` and `MPData` fields case-insensitively. Hex literals such as `0xB0A0` parse even though they aren't standard JSON, but a quoted `"0xB0A0"` fails.
- An address is the module base plus one static `addressOffset`. Pointer chains aren't supported.
- `type` must name a `WinApi.Types` value, `setVolume.type` a `HookMethods` value, and `wMsg` a `WinApi.wMsg` value. `WinApi.wMsg` is incomplete, so add any message that's missing.
- `wParam` and `lParam` are integers or the literal `"{volume}"`.
- `setVolume.keys` takes what `KeysConverter` reads, such as `Ctrl+Shift+P`, `F13` or `VolumeMute`. The keys go to the window in front unless a program has claimed them, so the player must have them set as a global hotkey.
- Each file has one `processName`, so a variant executable name needs its own file (`gta_sa` and `gta-sa`).
- `radioAddress.module` is ignored: `Game` reads a top-level `moduleName` instead. `maxVolume` is ignored too.
- A radio value in neither `onValues` nor `offValues` counts as off.
- A new file under `input/` needs a `<None Include>` entry with `CopyToOutputDirectory` in `VGRadioExternal.csproj`, or it won't reach `bin/`.

`MusicPlayer.Mute` handles each hook as below.

| `setVolume.type` | With `readVolume` | Without `readVolume` |
| --- | --- | --- |
| `SendMessage` with `{volume}` in `wParam` or `lParam` | Sets volume to `minVolume`, then restores the stored volume | Sends `minVolume` both ways, as a blind toggle |
| `SendMessage` with a fixed message | Sends the message, as a read toggle | Sends the message, as a blind toggle |
| `ProcessStart` | Runs the player's exe with `arguments`, as a read toggle | Runs the player's exe with `arguments`, as a blind toggle |
| `AudioSession` | Same as without: `readVolume` isn't used | Sets the mute state of the app's audio sessions on every output device |
| `KeyPress` | Presses `keys`, as a read toggle | Presses `keys`, as a blind toggle |

An app has no audio session until it has played something. With none, muting throws, so `Radio` shows the message and tries again every poll, and unmuting does nothing.

A read toggle reads the volume and only acts if the player isn't already in the wanted state, counting `minVolume` or below as muted. It can't end up inverted, but it only suits a player whose value at that address drops to `minVolume` when muted. If the value doesn't move, the player mutes once and never unmutes.

A blind toggle can't see the player's state. `MusicPlayer.ToggleMute` remembers what it last set and only acts when that changes, assuming the player starts unmuted and nothing else toggles it. `KeyPress` throws, and so retries, while a Shift, Ctrl or Alt outside `keys` is held, because the hotkey wouldn't match.
