# Porting from the legacy code

Detail for the items in [TODO.md](TODO.md). `Form1` and `ReadMemory` hold features the active code doesn't have yet. See `CLAUDE.md` § Architecture for how the two generations relate.

**Dependencies:** VDT comes before SAV, III, GVC and SRT. WHN needs III or GVC, because San Andreas only has on and off. LEG comes last.

All addresses below were read on 2026-11-02 from [ReadMemory.cs](../VGRadioExternal/ReadMemory.cs). They're absolute, so each table also gives the `addressOffset` for a config, which subtracts the `0x400000` image base. That offset is confirmed for San Andreas 1.0, where it matches the shipped config, and assumed for the rest.

## VDT — Tell game versions apart

A game config has one `processName` and one offset. `ReadMemory.DetermineGameVersion*` instead reads a known signature at a fixed address for each version, and picks the radio address from that. Nothing in the active code can do this, so a config for the wrong version reads a wrong address and silently mutes or does nothing.

**Evidence:** read 2026-11-02.

**Plan:** use one file per version, as the `gta_sa` and `gta-sa` files already do per executable name, which needs no code. Add an optional signature to `GData` (offset and expected value), so `Game` can report a mismatch through `Status` instead of reading the wrong address. The signatures are in the `DetermineGameVersion*` methods.

**Verify:** a config with a signature for another version reports the mismatch and doesn't touch the player.

**Docs when done:** `CLAUDE.md` § JSON config files, and `readme.txt`.

## SAV — Other San Andreas versions

The two shipped San Andreas configs use the 1.0 offset. The legacy code also handled these, with `FourBytes` values, 2 for on and 7 for off:

| Version | Legacy address | `addressOffset` | Legacy status |
|---|---|---|---|
| 1.1 US and EU | `0x8CCFE8` | `0x4CCFE8` | Unconfirmed |
| 3.0 Steam | `0x93AB68` | `0x53AB68` | Running |

**Evidence:** read 2026-11-02.

**Plan:** add a config for each, with a signature from [VDT](#vdt--tell-game-versions-apart). Name them by version, and add them to `VGRadioExternal.csproj`.

**Verify:** on each version, the base check in [LIV](live-checks.md#liv--configs-not-yet-seen-working-live) passes.

**Docs when done:** a new `LIV` check per config until seen working, and update the San Andreas entry under Removed in the changelog.

## III — GTA III

The process is `gta3`. The radio value is read as a `Byte`.

| Version | Legacy address | `addressOffset` |
|---|---|---|
| 1.0 | `0x8F3967` | `0x4F3967` |
| 1.1 | `0x8F3A1B` | `0x4F3A1B` |
| 1.1 Steam | `0x903B5C` | `0x503B5C` |

`CheckRadioStatusIII` treats 0 to 9 as radio stations, 10 as emergency vehicles, 13 and 14 as the announcer, and 197 as the pause menu. Other values above 10 mean off.

**Evidence:** read 2026-11-02.

**Plan:** add a config per version, with 0 to 9 in `onValues` until [WHN](#whn--when-options) makes the other groups selectable.

**Verify:** on each version, the base check in [LIV](live-checks.md#liv--configs-not-yet-seen-working-live) passes.

**Docs when done:** a new `LIV` check per config until seen working, and update the GTA III entry under Removed in the changelog.

## GVC — Vice City

The process is `gta-vc`. The radio value is read as `FourBytes`.

| Version | Legacy address | `addressOffset` | Legacy status |
|---|---|---|---|
| 1.0 | `0x9839C0` | `0x5839C0` | Running |
| 1.1 | `0x9839C0` | `0x5839C0` | Unconfirmed |
| 1.1 Steam | `0x9829C8` | `0x5829C8` | Running |
| 1.1 Japanese | `0x9809D0` | `0x5809D0` | Running |

`CheckRadioStatusVC` treats 0 to 9 as radio stations, 23 as emergency vehicles, 24 as Kaufman, 25 and 26 as the announcer, and 1225 as the pause menu. Other values mean off.

**Evidence:** read 2026-11-02.

**Plan:** add a config per version, with 0 to 9 in `onValues` until [WHN](#whn--when-options) makes the other groups selectable.

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
