# Porting from the legacy code

Detail for the items in [TODO.md](TODO.md). `Form1` and `ReadMemory` hold features the active code doesn't have yet. See `CLAUDE.md` § Architecture for how the two generations relate.

**Dependencies:** WHN needs III or GVC, because San Andreas only has on and off. LEG comes last.

All addresses below were read on 2026-11-02 from [ReadMemory.cs](../VGRadioExternal/ReadMemory.cs), where `DetermineGameVersion*` tells versions apart by their signatures. The addresses are absolute, so each table also gives the offsets for a config, which subtract the `0x400000` image base. Every exe checked on 2026-11-03 has that image base and no ASLR in its PE header: a GTA III Steam exe, Vice City 1.0, 1.1 Steam and Japanese, and San Andreas 1.0 US and EU. Signatures go in a config's `signatures`, and each is read as `FourBytes`.

## SAV — Other San Andreas versions

The two shipped San Andreas configs are for 1.0, US or EU, and their signatures say so. The legacy code also handled these, with `FourBytes` values, 2 for on and 7 for off:

| Version | Legacy address | `addressOffset` | Signature offset | Legacy status |
|---|---|---|---|---|
| 1.1 US | `0x8CCFE8` | `0x4CCFE8` | `0x4252FC` | Unconfirmed |
| 1.1 EU | `0x8CCFE8` | `0x4CCFE8` | `0x42533C` | Unconfirmed |
| 3.0 Steam | `0x93AB68` | `0x53AB68` | `0x45EC4A` | Running |

Each signature's value is 38079. None of these signatures has been checked against a real exe.

**Evidence:** read 2026-11-02.

**Plan:** add a config for each version, and for each executable name as 1.0 has, with 1.1 taking both its signatures. Name every San Andreas file by version, the two 1.0 ones included, and add the new ones to `VGRadioExternal.csproj`.

**Verify:** on each version, the base check in [LIV](live-checks.md#liv--configs-not-yet-seen-working-live) passes.

**Docs when done:** a new `LIV` check per config until seen working, and update the San Andreas entry under Removed in the changelog.

## III — GTA III

The process is `gta3`. The radio value is read as a `Byte`.

| Version | Legacy address | `addressOffset` | Signature offset |
|---|---|---|---|
| 1.0 | `0x8F3967` | `0x4F3967` | `0x1C1E70` |
| 1.1 | `0x8F3A1B` | `0x4F3A1B` | `0x1C2130` |
| 1.1 Steam | `0x903B5C` | `0x503B5C` | `0x1C6FD0` |

Each signature's value is 1407551829. A GTA III Steam exe on this machine matched none of them when read from the file on 2026-11-03. It has a `.bind` section, which suggests Steam's DRM decrypts its code at run time, so the Steam signature may only match in the running game.

`CheckRadioStatusIII` treats 0 to 9 as radio stations, 10 as emergency vehicles, 13 and 14 as the announcer, and 197 as the pause menu. Other values above 10 mean off.

**Evidence:** read 2026-11-02, and the signatures from the exe on 2026-11-03.

**Plan:** add a config per version with its signature, and with 0 to 9 in `onValues` until [WHN](#whn--when-options) makes the other groups selectable. Check the Steam signature in the running game first.

**Verify:** on each version, the base check in [LIV](live-checks.md#liv--configs-not-yet-seen-working-live) passes.

**Docs when done:** a new `LIV` check per config until seen working, and update the GTA III entry under Removed in the changelog.

## GVC — Vice City

The process is `gta-vc`. The radio value is read as `FourBytes`.

| Version | Legacy address | `addressOffset` | Signature offset | Signature value | Legacy status |
|---|---|---|---|---|---|
| 1.0 | `0x9839C0` | `0x5839C0` | `0x267BF0` | 1407551829 | Running |
| 1.1 | `0x9839C0` | `0x5839C0` | `0x267C40` | 1407551829 | Unconfirmed |
| 1.1 Steam | `0x9829C8` | `0x5829C8` | `0x6402ED` | 1448235347 | Running |
| 1.1 Japanese | `0x9809D0` | `0x5809D0` | `0x6CD0A2` | 1793887061 | Running |

The 1.0, 1.1 Steam and Japanese signatures each matched an exe on this machine when read from the file on 2026-11-03. The 1.1 one hasn't been checked.

`CheckRadioStatusVC` treats 0 to 9 as radio stations, 23 as emergency vehicles, 24 as Kaufman, 25 and 26 as the announcer, and 1225 as the pause menu. Other values mean off.

**Evidence:** read 2026-11-02, and the signatures from the exes on 2026-11-03.

**Plan:** add a config per version with its signature, and with 0 to 9 in `onValues` until [WHN](#whn--when-options) makes the other groups selectable.

**Verify:** on each version, the base check in [LIV](live-checks.md#liv--configs-not-yet-seen-working-live) passes.

**Docs when done:** a new `LIV` check per config until seen working, and update the Vice City entry under Removed in the changelog.

## CFG — Remember the selected game and player

`Form1` saved its options to the exe's config file with `ConfigurationManager`. `FormRE` saves nothing, so both lists start empty on every run. `Button_Save` exists in the designer with no handler.

**Evidence:** read 2026-11-02.

**Plan:** save the selected file names when they change, and restore them in `FormRE_Load` if the files still exist. Store them per user rather than beside the exe. Remove `Button_Save` unless [WHN](#whn--when-options) needs it.

**Verify:** pick a game and player, restart: both are selected and the tool is working. Delete one of the files, restart: no error, and that list is empty.

**Docs when done:** the saved settings entry under Removed in the changelog.

## WHN — When options

The legacy window let users choose whether music plays during Radio, Emergency, Interiors, Menu, Kaufman and Announcer. `FormRE` has a disabled `ComboBox_When`, `CheckBox_1` to `CheckBox_7` and `Button_Save`, none wired up. In the legacy code the Interiors box had an empty handler, so it never did anything.

**Evidence:** read 2026-11-02.

**Plan:** add named groups of radio values to `GData`, each with a default, and build the checkboxes from them. The ticked groups make up the on values, and everything else means off.

**Verify:** in Vice City, tick and untick Emergency: music follows the choice while in an emergency vehicle.

**Docs when done:** `CLAUDE.md` § JSON config files, `readme.txt`, and the When entry under Removed in the changelog.

## ACT — Pause and Volume actions

`RadioChangerPause` sends the media play/pause key with `keybd_event`. `RadioChangerVolume` steps the volume keys until a target is reached, with Quick Volume sending more steps per poll, Ignore Modifiers controlling whether held Shift, Alt or Ctrl stop it, and a failsafe that gives up after too many tries. Neither has a `HookMethods` value, and the Pause action was also what made the Other player option work with any player.

**Evidence:** read 2026-11-02.

**Plan:** use the `KeyPress` hook for pause, with `MediaPlayPause` as its `keys`, which should only need a config. Decide whether Volume Keypress is worth porting at all, given that Windows volume keys can affect the whole system, and move the item to the log with the decision if not.

**Verify:** with a player that takes media keys, music pauses and resumes with the radio.

**Docs when done:** the hook table in `CLAUDE.md`, `readme.txt`, and the actions entry under Removed in the changelog.

## SRT — Saints Row: The Third

`origin/saintsrow3` has detection for process `SaintsRowTheThird`, with a base-relative radio address `0x12F0734` and a second address `0x12EC24C` for states such as the game being paused. It's built on `Form1` and `ReadMemory`, and was never merged. A game config has only one address.

**Evidence:** read 2026-11-02 from the branch's diff. Not run.

**Plan:** read how the branch combines the two addresses, extend `GData` to allow more than one, and add a config. Delete the branch once ported.

**Verify:** in Saints Row: The Third, the base check in [LIV](live-checks.md#liv--configs-not-yet-seen-working-live) passes, including when paused.

**Docs when done:** `CLAUDE.md` § JSON config files, `readme.txt`, and an Added entry in the changelog.

## LEG — Delete the legacy code

`Form1`, `ReadMemory`, `Settings` and the comment blocks in `Program.cs` still compile, but nothing runs them. They stay until the items above no longer need them as a reference. The `appSettings` keys in `App.config` belong to them too.

**Evidence:** read 2026-11-02. Nothing instantiates `Form1`, and nothing outside `Form1` uses `ReadMemory` or `Settings`.

**Plan:** delete them and their `VGRadioExternal.csproj` entries once the rest of this doc is finished or decided against. The tags keep the old code available.

**Verify:** the solution builds and the tool runs as before.

**Docs when done:** `CLAUDE.md` § Architecture and the note on porting addresses.
