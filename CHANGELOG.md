# Changelog

What changed for users of GTA Radio External, per release, newest first. Downloads are on the [releases page](https://github.com/lotsofs/GTA-Radio-External/releases).

## Unreleased

### Action required

- **Install .NET Framework 4.8 if you don't have it.** Earlier releases needed 4.5.2. The program also only runs on 64-bit Windows now.

### Added

- **Pick your game and music player from lists.** The lists show the JSON files in the `input` folder, so you can add your own games and players. *Open Directory* opens that folder, and *Refresh* reloads the lists. If a file has a mistake, picking it shows what's wrong, and the game or player you had picked stays in use.
- **GTA IV and GTA San Andreas: The Definitive Edition** are now supported.
- **WACUP and Windows Media Player** are now supported. Start with Windows Media Player unmuted.
- **Spotify** is now supported. It's muted in the Windows Volume Mixer, so other programs keep playing. Play something in Spotify first: until you do, there's nothing to mute, and its status says so when the radio goes off.
- **Any other program in the Volume Mixer can be muted the same way.** Set `type` under `setVolume` to `AudioSession` in its JSON file, as `Spotify.json` does.
- **Any music player with a global mute hotkey can now be used.** Under `setVolume` in its JSON file, set `type` to `KeyPress` and `keys` to the hotkey, such as `Ctrl+Shift+P`. Start with the player unmuted, and pick a key your game doesn't use. If you're holding another Shift, Ctrl or Alt, the key waits until you let go, and the status says so.
- **A game file can name more than one program**, for a game whose exe goes by several names. Give `processName` a list, as the San Andreas file does.
- **Game files can say which version of the game they're for**, with a list of `signatures`. The San Andreas file is now for version 1.0 only. With another version, the game's status says it's not the version the file is for, and the music plays as if the game wasn't running.
- **A player that can only toggle mute no longer has to start unmuted.** Give its JSON file a `readVolume`, and the program checks the volume before toggling. Only do this if the volume drops to `minVolume` when muted. If it doesn't, the player mutes and never unmutes.

### Changed

- **The program is now `VGRadioExternal.exe`, and its window is called Radio External**, with a new layout. It shows `V0.3.0 WIP`.
- **Winamp now means v5.8 and Foobar2000 now means v1.4.8**, instead of v5.666 and v1.3.14.
- **The game and the music player each have their own status.** It says whether each one is selected and running, whether the game is the version its file is for, whether the music player is still starting, which file is missing, or what went wrong, and the game's status also says whether the radio is on. If the program can't read the radio or the music player's volume, or finds a radio value it doesn't know, it leaves the music as it is and the status says so. If a message is cut off, hover over it to read the rest.

### Fixed

- **The music player no longer stays muted after you close the game or this program.** It also comes back when you pick another music player. While no game is running, the music plays: picking a player that's muted unmutes it, if the program can tell it's muted, as with Spotify.
- **A music player you start while the radio is off now goes silent too**, a few seconds after it starts.

### Removed

- **GTA III and GTA Vice City** are no longer supported.
- **San Andreas 1.1 and Steam 3.0** are no longer supported. Version 1.0 still is.
- **The *Other* music player option.**
- **The *Pause Keypress* and *Volume Keypress* actions**, with *Quick Volume* and *Ignore Modifiers*.
- **The *When* options.** Music now simply follows the radio.
- **Saved settings.** Pick your game and music player each time you start the program.
- **Game version and volume in the status display.** The window no longer shows which game version it found, or the volume.
- **The tooltips**, including the credit to e216.

## 0.2.1 (2019-08-28)

### Action required

- **Pick your game, music player and action again.** The program is now called `GTA Radio External.exe`, and it doesn't read the settings saved by `GTASARadioExternal.exe`.

### Added

- **The program now has an icon.**
- **The window now shows the version number.**

### Fixed

- **Winamp now works on any PC.** Before, it only worked on the developer's.
- **The program no longer crashes when started with Winamp's volume at maximum.**

## 0.2 (2017-09-03)

### Added

- ***Mute Command* now works with Winamp**, so it no longer needs simulated keypresses.

### Changed

- **The download now starts with settings filled in:** GTASA, Winamp and *Mute Command*, with *Quick Volume* on, and music playing during Radio, Emergency, Interiors and Menu.
- **Volume Keypress now shows ERROR sooner** when it can't turn the volume up.

### Fixed

- **Closing the game no longer causes an error** when using *Volume Keypress*.
- **The volume no longer resets to 0** when the game closes or the program starts. It's remembered between game sessions, though it can still go quiet in between.

## 0.1 (2017-05-09)

First release. Needs .NET Framework 4.5.2.

- Follows the radio in GTA III, GTA Vice City and GTA San Andreas.
- Controls Winamp v5.666 Build 3516 and Foobar2000 v1.3.14, or any other player through *Pause Keypress*.
- Three actions: *Volume Keypress*, with *Quick Volume* and *Ignore Modifiers*; *Pause Keypress*; and *Mute Command*, for Foobar2000 only.
- *When* options choose whether music plays during Radio, Emergency, Interiors, Menu, Kaufman and Announcer.
- Remembers your settings between runs.
