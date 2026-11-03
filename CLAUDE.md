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

- `FormRE` lists the JSON files in `input/games` and `input/music players` in two combo boxes and passes the selected path to `Radio`, which loads it through `GData.Load` or `MPData.Load`. A file that can't be used throws `InvalidDataException`. `FormRE` shows its message and puts the list back on the loaded file, which stays in use.
- `Radio` builds a `Game` and a `MusicPlayer` from those files and polls every 40 ms. `_on` holds what the player was last told, and `Radio` calls `MusicPlayer.Mute()` whenever the radio differs from it. With no game running, the radio counts as on, so closing the game restores the player. When `Game.IsRadioOn` can't tell, it returns null and the player isn't told anything. `_on` is null while the player hasn't been told anything, which includes after it restarts, so the next poll tells it. `Mute` has to be safe to repeat, because the player can already be in the state it's told.
- Only the poll sends anything to the player. A newly selected `MusicPlayer` waits in `_selectedMusicPlayer` until the poll has restored the old one. When `FormRE` closes, it calls `Radio.Stop`, which waits up to a second for the poll to restore the player. The wait gives a held Alt from Alt+F4 time to be let go, because a `KeyPress` hook waits for that.
- `Radio` raises `StatusChanged` when either status text changes, and `FormRE` shows both in its labels through `BeginInvoke`. `FormRE` starts the timer from `Load`, because `BeginInvoke` needs the window handle and a status is only reported when it changes.
- The poll uses `System.Timers.Timer`, so it runs on a thread-pool thread, one poll at a time: `AutoReset` is off and `Check` restarts the timer. On .NET Framework that timer swallows exceptions from its handler, so `GetProcess` and `Radio.CheckRadio` catch theirs and show them in the labels. Any other failure inside the poll is silent.
- `Game` and `MusicPlayer` take the loaded `GData` and `MPData`, look their process up again when it exits, and keep a `Status` text saying what they last found. `Game` treats a process that doesn't match its file's `signatures` as not running. `MusicPlayer` treats a process as not running for its first 3 seconds, with the status `Starting`. A player that has only just started can lose what it's sent, and a lost toggle would leave it inverted.
- `WinApi` holds the P/Invoke declarations, process and module lookup, typed memory reads, and simulated key presses. `ReadValue` throws `Win32Exception` when a read fails, so a failure can't pass for a value.
- `AudioSessions` holds the Core Audio COM declarations and mutes an app at the per-app level the Windows Volume Mixer shows. It matches every process with the app's name, because any of them can own the session, and it skips output devices that refuse to list their sessions.

**Legacy, hardcoded, never run:** `Form1`, `ReadMemory`, `Settings` and the commented-out blocks in `Program.cs` still compile, but nothing instantiates `Form1`. `ReadMemory` is the reference for what hasn't been ported yet: GTA III, Vice City and the other San Andreas versions, their radio status codes, and the pause and volume-key actions. `README.md` still describes this generation. What the active code supports is what's in `input/`.

Addresses in `ReadMemory` are absolute, while `addressOffset` in JSON is relative to the module base. Subtract the `0x400000` image base when porting: San Andreas 1.0 `0x8CB760` becomes `0x4CB760`.

The build targets x64 so it can read 64-bit games. `WinApi.GetModuleAddress` falls back to `EnumProcessModulesEx` for modules that `Process.Modules` doesn't list.

## JSON config files

`VGRadioExternal/readme.txt` documents the music player format for users. Where it differs from the code, the code is right.

- A file is checked when it's picked, so a mistake shows then and not in the poll. `ConfigFile.Read` deserializes it, and `Required.Always` on a field's `[JsonProperty]` makes every file need that field. `GData.Load` and `MPData.Load` check the rest, such as the fields each hook needs. A new field needs its check in one of those places.
- Keys match `GData` and `MPData` fields case-insensitively. Hex literals such as `0xB0A0` parse even though they aren't standard JSON. `addressOffset`, a signature's `value`, `wParam` and `lParam` also take them quoted, as `"0xB0A0"`, but other integers don't.
- An address is the module base plus one static `addressOffset`. Pointer chains aren't supported.
- `signatures` in a game file is optional: a list of `addressOffset`, `type` and `value`, read from the same module as the radio. `Game` only uses a process where one of them reads its value, and a read that fails doesn't match. A process that doesn't match counts as not running, so the music plays, and the status says it's not the version the file is for. It's checked again every poll until it matches, because a game can decrypt its code after it starts.
- `type` must name a `WinApi.Types` value, `setVolume.type` a `HookMethods` value, and `wMsg` a `WinApi.wMsg` value, in any case. `EnumNameConverter` rejects numbers, even ones the enum has. `WinApi.wMsg` is incomplete, so add any message that's missing.
- `windowName` is the window's class, not its title: it goes to `FindWindow` as the class name.
- `wParam` and `lParam` are integers or the literal `"{volume}"`, which needs `readVolume` so the volume can be restored. `MessageParameter` holds either.
- `setVolume.keys` takes what `KeysConverter` reads, such as `Ctrl+Shift+P`, `F13` or `VolumeMute`, and needs a key besides Shift, Ctrl and Alt. The keys go to the window in front unless a program has claimed them, so the player must have them set as a global hotkey.
- Each file has one `processName`, without `.exe`, so a variant executable name needs its own file (`gta_sa` and `gta-sa`).
- `radioAddress.module` is ignored: `Game` reads a top-level `moduleName` instead. `maxVolume` is ignored too, and so is any other unknown key.
- `onValues` and `offValues` each need a value. A radio value in neither, or a failed read, makes `Game.IsRadioOn` return null: the player is left as it is, and the game's status shows the value or the error.
- A new file under `input/` needs a `<None Include>` entry with `CopyToOutputDirectory` in `VGRadioExternal.csproj`, or it won't reach `bin/`.

`MusicPlayer.Mute` handles each hook as below.

| `setVolume.type` | With `readVolume` | Without `readVolume` |
| --- | --- | --- |
| `SendMessage` with `{volume}` in `wParam` or `lParam` | Sets volume to `minVolume`, then restores the stored volume | Rejected when the file is picked |
| `SendMessage` with a fixed message | Sends the message, as a read toggle | Sends the message, as a blind toggle |
| `ProcessStart` | Runs the player's exe with `arguments`, as a read toggle | Runs the player's exe with `arguments`, as a blind toggle |
| `AudioSession` | Same as without: `readVolume` isn't used | Sets the mute state of the app's audio sessions on every output device |
| `KeyPress` | Presses `keys`, as a read toggle | Presses `keys`, as a blind toggle |

`SendMessage` looks the window up when it sends, if it has none yet or the one it had is gone, so a window that appears late or is recreated is still found. With none, `Mute` throws, so `Radio` shows it and tries again every poll.

An app has no audio session until it has played something. With none, muting throws, so `Radio` shows the message and tries again every poll, and unmuting does nothing.

A read toggle reads the volume and only acts if the player isn't already in the wanted state, counting `minVolume` or below as muted. It can't end up inverted, but it only suits a player whose value at that address drops to `minVolume` when muted. If the value doesn't move, the player mutes once and never unmutes.

A volume read that fails throws from `Mute`, for a read toggle and for a `{volume}` mute alike, so nothing is stored or toggled. `Radio` shows the error, and the next poll tries again.

A blind toggle can't see the player's state. `MusicPlayer.ToggleMute` remembers what it last set and only acts when that changes, assuming the player starts unmuted and nothing else toggles it. `KeyPress` throws, and so retries, while a Shift, Ctrl or Alt outside `keys` is held, because the hotkey wouldn't match.
