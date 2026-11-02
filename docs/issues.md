# Defects and tasks

Detail for the items in [TODO.md](TODO.md).

**Dependencies:** SHR is easier once VAL and RDF have settled what the two classes do.

## SYN — Mute state drifts from the player

`Radio` keeps one `_on` flag, false at the start, and acts only when `Game.IsRadioOn()` differs from it. Several situations leave the player in the wrong state:

- **Game starts with the radio off.** `_on` is already false, so nothing is sent, and the player keeps playing until the radio has come on and gone off once.
- **Game closes while muted.** `Radio.Check` only touches the player while the game is running, so the player stays muted.
- **Tool closes while muted.** `FormRE` has no closing handler and `Radio` no shutdown, so the player stays at `minVolume`.
- **Selection changes.** A new `Game` or `MusicPlayer` keeps the old `_on` and isn't told the current state. A new `MusicPlayer` also stores the volume it finds at startup, which is `minVolume` if the player is muted, and unmuting then restores that.
- **Toggle configs.** `ToggleMute` assumes the player starts unmuted and that nothing else toggles it, as the description in `Windows Media Player.json` warns. Starting muted, or pressing a `KeyPress` hotkey by hand, inverts the player until it's fixed by hand. A new `MusicPlayer` also forgets that it had muted the player.
- **Volume Mixer mutes.** The `AudioSession` hook has the gaps above, and they're harder to spot: the mute stays in the Mixer when the tool closes, and Spotify's own window doesn't show it. Whether it also survives restarting Spotify isn't known.

**Evidence:** read 2026-11-02. Seeing these needs a real game and player.

**Plan:** make `_on` nullable so the first poll always applies the state, and reset it when the game or player changes. When the game's process is gone, and when `FormRE` closes, restore the player. Keep the stored volume across a `MusicPlayer` swap only if it's the same player.

**Verify:** start the game with the radio off: the player mutes. Mute it, then close the game: the volume returns. Mute it, then close the tool: the volume returns. Pick another config while muted: the state stays correct.

**Docs when done:** `CLAUDE.md` § Architecture if `Radio` gains a shutdown step.

## VAL — Configs aren't validated on load

`Radio.MusicPlayerChanged` and `Radio.GameChanged` read and deserialize on the UI thread with no `try`, so a malformed file throws from the combo box handler. A missing `setVolume` only shows as an `Error:` status with a `NullReferenceException` message, once the player's process is found.

Values that are used later aren't checked at all. `ReadVolume.Type` and `RadioAddress.Type` go through `Enum.Parse` in catch-all blocks, and `WMsg`, `WParam` and `LParam` go through `Enum.Parse` and `int.Parse` in `Mute`, on the timer thread. A misspelt `type` fails quietly every poll, and a misspelt `wMsg`, `wParam` or `lParam` only shows as a raw `Error:` status once the radio changes.

A `setVolume` with no `type` becomes `None`, which does nothing and shows no error. `{volume}` in a config with no `readVolume` sends `minVolume` for both muting and unmuting.

**Evidence:** the load failure reproduced 2026-11-03 by deserializing a bad `addressOffset` into `MPData` from the built assembly: `"banana"` throws `FormatException`. How that reaches the user is only read: `FormRE.ComboBox_MusicPlayer_SelectedIndexChanged` has no `try`, so it should land in the WinForms unhandled exception dialog. The rest read 2026-11-02.

**Plan:** add one load step that deserializes, checks required fields, and parses the enum and integer values once into typed fields. On failure, show a message naming the file and field, and keep the previous selection.

**Verify:** try malformed JSON, a misspelt `wMsg`, and a missing `setVolume`: each gives a clear message and the tool keeps running.

**Docs when done:** `CLAUDE.md` § JSON config files, and `readme.txt`.

## RDF — Failed reads are treated as real values

`Game.IsRadioOn` catches everything and leaves `radio` at -1, which is in neither `onValues` nor `offValues`, so a failed read means "radio off" and mutes the player. An unknown value does the same. `MusicPlayer.ReadVolume` returns `int.MinValue` on an exception, `Mute(true)` stores it in `_volume`, and `Mute(false)` sends it as the volume. `WinApi.ReadValue` also ignores the result of `ReadProcessMemory`, so a failed read looks like the value 0.

Read toggles decide from the same read, so a failure can skip a toggle: `int.MinValue` counts as already muted, and 0 counts as already unmuted when `minVolume` is below 0.

**Evidence:** read 2026-11-02.

**Plan:** have `WinApi.ReadValue` report failure. On a failed read or unknown value, `IsRadioOn` keeps the last known state, `ReadVolume` keeps the stored volume, and both report through `Game.Status` and `MusicPlayer.Status`. A read toggle must not decide from a failed read.

**Verify:** point a config at an unreadable address: the player's state doesn't change, the volume isn't touched, and the status says the read failed.

**Docs when done:** the "counts as off" bullet in `CLAUDE.md` § JSON config files.

## CWD — input is found through the current directory

The `FormRE` constructor builds `inputPath` from `Environment.CurrentDirectory`. Starting the exe from a shortcut with another start folder, or from a terminal elsewhere, makes `RefreshLists` throw when it reads the missing folder.

**Evidence:** read 2026-11-02.

**Plan:** use `AppDomain.CurrentDomain.BaseDirectory` instead.

**Verify:** run the exe from another folder: both lists fill.

**Docs when done:** the run bullet in `CLAUDE.md` § Build and run.

## DOC — User docs describe old behaviour

- [README.md](../README.md) lists GTA III, Vice City, Winamp 5.666, Foobar2000 1.3.14, and mute or pause, which is the legacy feature set. Its "To add" list only says "Cool stuff", and open work lives in [TODO.md](TODO.md) now.
- [readme.txt](../VGRadioExternal/readme.txt) says the volume goes in `wParam` automatically, but the code needs the literal `"{volume}"`. It talks about a `volume` object where the files have `readVolume` and `setVolume`, and says "jason" for JSON.
- Nothing documents the game config format.
- `readme.txt` is a `Content` item without `CopyToOutputDirectory`, so it never reaches the output folder, where people would add their own files.

**Evidence:** read 2026-11-02.

**Plan:** do this close to the next release, so the feature list only changes once. Rewrite the README's features from `input/`, rewrite `readme.txt` from `GData` and `MPData`, cover game configs, and copy it to the output folder.

**Verify:** write a new config from `readme.txt` alone, and it loads.

**Docs when done:** this item is the docs. Mention it in the changelog if `readme.txt` starts shipping.

## SPT — Answer PR #5

[PR #5](https://github.com/lotsofs/GTA-Radio-External/pull/5) by Goldiedtv, open since 2023-05-17, adds Spotify muting to the legacy code. It targets `master`, written against `Form1` and `ReadMemory`, so it can't be merged. Its idea is in the active code now as the `AudioSession` hook, and the PR still needs a reply.

**Evidence:** read 2026-11-02 from the PR's description and diff.

**Plan:** thank them, say how the idea was ported and that no audio device setting is needed any more, and close the PR once the active code reaches `master`.

**Verify:** the PR has a reply.

**Docs when done:** none.

## HKY — Answer issue #2

[Issue #2](https://github.com/lotsofs/GTA-Radio-External/issues/2) by MhmdFVC, 2021-03-24, asks for a user-set mute hotkey, so that muting works across most players and version differences stop mattering. The issue also reports an unhandled exception with Foobar2000 1.6.4 in the legacy code. The `KeyPress` hook adds the hotkey, and the issue still needs a reply.

**Evidence:** the issue read 2026-11-02.

**Plan:** reply to the issue, and close it once the active code reaches `master`.

**Verify:** the issue has a reply.

**Docs when done:** none.

## GMD — radioAddress.module is ignored

`Game.GetProcess` passes `_gameData.ModuleName`, which comes from a top-level `moduleName`. The shipped files set `radioAddress.module` instead, which fills `RadioAddress.Module`, and nothing reads that. All the shipped game configs leave it blank, so nothing goes wrong today, but a game whose radio lives in a DLL can't be configured the documented way.

**Evidence:** read 2026-11-02. Deserializing the shipped files confirmed both fields are empty.

**Plan:** use `RadioAddress.Module` and delete `GData.ModuleName`.

**Verify:** a config with `radioAddress.module` set resolves against that module.

**Docs when done:** the bullet in `CLAUDE.md` § JSON config files.

## MXV — maxVolume is ignored

`Wacup.json` and `Wacup - Not So Direct.json` set `maxVolume`, but `MPData` has no such field, so it's dropped on load.

**Evidence:** read 2026-11-02.

**Plan:** remove the key from both files. The volume to restore comes from reading the player, so nothing needs a maximum.

**Verify:** both files load and behave the same.

**Docs when done:** the bullet in `CLAUDE.md` § JSON config files.

## HWN — Window handle handling

`MusicPlayer.GetProcess` calls `FindWindow` once. If the player's window doesn't exist yet, `_window` stays 0 and is never looked up again, because `_process` is already set. Muting then does nothing, with no error, and the status says `Running`. `WinApi.SendMessage` and `FindWindow` declare handles and parameters as `int` in an x64 build, and `CollectModules` passes the wrong size to `GetModuleInformation`.

**Evidence:** the missed window reproduced 2026-11-02 with a stand-in player that creates its window 1.5 seconds after starting: it received no messages, while one with its window already up was muted and restored. Whether a real player's window lags enough to hit this isn't known. The rest read 2026-11-02.

**Plan:** declare handles as `IntPtr`, look the window up again before sending if it's missing or no longer valid, and pass the size of `ModuleInformation`.

**Verify:** start the tool while the player is still loading: muting works once it's up.

**Docs when done:** none.

## MPR — Several running instances

`WinApi.GetProcess(string)` returns `processes[0]` when several processes match, whichever that is.

**Evidence:** read 2026-11-02.

**Plan:** prefer a process that hasn't exited and has a main window.

**Verify:** run two instances of a player and close the first: the tool follows the second.

**Docs when done:** none.

## VER — Stale file properties and project settings

- [AssemblyInfo.cs](../VGRadioExternal/Properties/AssemblyInfo.cs) still says `GTASARadioExternal`, version 1.0.0.0 and copyright 2017, while the window says `V0.3.0 WIP`.
- [VGRadioExternal.csproj](../VGRadioExternal/VGRadioExternal.csproj) keeps ClickOnce publish settings: an install URL, manifests, a `.pfx` that isn't in the repo, and a 4.5.2 bootstrapper package for a 4.8 build.
- [App.config](../VGRadioExternal/App.config) has `system.web` client providers nothing uses. Its `appSettings` keys belong to the legacy window and go with [LEG](porting.md#leg--delete-the-legacy-code).

**Evidence:** read 2026-11-02.

**Plan:** update the assembly attributes and keep the version in one place, so the window and file properties agree. Remove the ClickOnce settings and the unused `App.config` sections.

**Verify:** the exe's properties show the right name and version, and a build no longer writes `.application` or `.manifest` files.

**Docs when done:** a Changed entry in the changelog, because file properties are visible to users.

## VST — Publish the Vice City set file

[Issue #3](https://github.com/lotsofs/GTA-Radio-External/issues/3) by Patrick-1994-, 2021-05-03, asks for `gtavc-use-this-if-doesnt-work.set` to be added to the README or a release, because it fixes the tool not detecting the radio at 0 volume. The file isn't in this repo.

**Evidence:** read 2026-11-02 from the issue. Not run.

**Plan:** find the file, add it to the repo or a release, and link it from the README. It's only useful once [GVC](porting.md#gvc--vice-city) has brought Vice City back, unless it's attached to the old releases.

**Verify:** with the file in place, Vice City at 0 radio volume is detected.

**Docs when done:** `README.md`. Close the issue.

## SHR — Game and MusicPlayer duplicate code

`GetProcess` and `IsRunning` are nearly the same in [Game.cs](../VGRadioExternal/Game.cs) and [MusicPlayer.cs](../VGRadioExternal/MusicPlayer.cs), including the module lookup and turning an offset into an address.

**Evidence:** read 2026-11-02.

**Plan:** move them into a small base class that owns the process and resolves addresses, after [VAL](#val--configs-arent-validated-on-load) and [RDF](#rdf--failed-reads-are-treated-as-real-values).

**Verify:** the solution builds, and [LIV 1](live-checks.md#with-san-andreas-and-a-working-player) still passes.

**Docs when done:** `CLAUDE.md` § Architecture.

## CLN — Code cleanup

- Commented-out code in `FormRE`, `Game`, `Radio` and `MusicPlayer`. The blocks in `Program.cs` go with [LEG](porting.md#leg--delete-the-legacy-code).
- Unused members: `Game._window`, `Game._volume`, `Game._addressRunning` and `FormRE.GetClassName`.
- `WinApi.GetProcess(object)` is a stub that throws. It's only avoided because callers pass a `string`.
- `WinApi.Types` describes config values and doesn't belong in `WinApi`.
- `WinApi.ReadValue` mixes reading bytes and converting them, and the fallback in `GetModuleAddress` could be tidier.
- `WinApi.wMsg` lists only some messages, so a config using any other fails.
- `Module` carried an unexplained `FRAMEWORK` note. Settle what it meant, or drop it.

**Evidence:** read 2026-11-02.

**Plan:** delete what's dead, move `Types` next to the config classes, and separate reading from conversion. Do it after the items above that touch the same code, to keep their diffs small.

**Verify:** the solution builds, and [LIV 1](live-checks.md#with-san-andreas-and-a-working-player) still passes.

**Docs when done:** none.
