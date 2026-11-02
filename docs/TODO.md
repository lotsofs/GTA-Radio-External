# TODO

Open work, one row per item. Within each group, rows are in suggested order of work. Each item's detail is in the doc that its group heading names. Finished items move to [DONE.md](DONE.md).

## Defects and tasks — detail in [issues.md](issues.md)

| Code | Area | Summary | Severity |
|---|---|---|---|
| [FOB](issues.md#fob--foobar2000json-fails-to-load) | Music players | `Foobar2000.json` fails to load because its `addressOffset` is a quoted string | High |
| [SYN](issues.md#syn--mute-state-drifts-from-the-player) | Radio | The player doesn't mute if the game starts with the radio off, and stays muted after the game or the tool closes or the selection changes | High |
| [VAL](issues.md#val--configs-arent-validated-on-load) | Config | Nothing validates a config on load: a bad file throws on the UI thread, and bad `wMsg`, `wParam` or `type` values only fail later in the poll | Medium |
| [RDF](issues.md#rdf--failed-reads-are-treated-as-real-values) | Memory | A failed read counts as radio off, and a failed volume read stores `int.MinValue`, which unmuting then sends to the player | Medium |
| [HWN](issues.md#hwn--window-handle-handling) | Music players | The player's window is looked up once and never again, so a player whose window appears after its process never mutes while its status says `Running`; window handles are also declared as `int` in a 64-bit build | Medium |
| [CWD](issues.md#cwd--input-is-found-through-the-current-directory) | Startup | `input/` is found through the current directory, so starting the exe from another folder breaks it | Medium |
| [DOC](issues.md#doc--user-docs-describe-old-behaviour) | Docs | `README.md` and `readme.txt` describe old behaviour, the game JSON format is undocumented, and `readme.txt` doesn't reach the output folder | Medium |
| [SPT](issues.md#spt--answer-pr-5) | Music players | Reply to PR #5: its Spotify muting is ported to the active code as the `AudioSession` hook, so the PR itself can't be merged | Low |
| [HKY](issues.md#hky--answer-issue-2) | Music players | Reply to issue #2 now that the `KeyPress` hook adds the mute hotkey it asks for | Low |
| [GMD](issues.md#gmd--radioaddressmodule-is-ignored) | Game config | `radioAddress.module` in game configs is ignored, because `Game` reads a top-level `moduleName` instead | Low |
| [MXV](issues.md#mxv--maxvolume-is-ignored) | Config | `maxVolume` in both WACUP configs is ignored, because `MPData` has no such field | Low |
| [MPR](issues.md#mpr--several-running-instances) | Processes | With several instances of a game or player running, `WinApi.GetProcess` takes the first without checking it | Low |
| [VER](issues.md#ver--stale-file-properties-and-project-settings) | Project | File properties are stale (`GTASARadioExternal`, 1.0.0.0, 2017), and ClickOnce publish settings and `App.config` leftovers remain | Low |
| [VST](issues.md#vst--publish-the-vice-city-set-file) | Docs | Issue #3: publish the Vice City `.set` file that fixes 0-volume radio detection, which isn't in this repo | Low |
| [SHR](issues.md#shr--game-and-musicplayer-duplicate-code) | Code | `Game` and `MusicPlayer` duplicate process lookup, `IsRunning` and address resolution | Low |
| [CLN](issues.md#cln--code-cleanup) | Code | Remove commented-out code, unused fields and the throwing `GetProcess(object)` stub, tidy `WinApi`, and settle the unexplained `FRAMEWORK` note on `Module` | Low |

## Porting from the legacy code — detail in [porting.md](porting.md)

| Code | Area | Summary | Severity |
|---|---|---|---|
| [VDT](porting.md#vdt--tell-game-versions-apart) | Games | Configs have one process name and one offset, and can't tell game versions apart the way `ReadMemory` does | Medium |
| [SAV](porting.md#sav--other-san-andreas-versions) | Games | Add San Andreas 1.1 and Steam 3.0; only 1.0 has a config | Medium |
| [III](porting.md#iii--gta-iii) | Games | Add GTA III configs for 1.0, 1.1 and 1.1 Steam | Medium |
| [GVC](porting.md#gvc--vice-city) | Games | Add Vice City configs for 1.0, 1.1, 1.1 Steam and the Japanese version | Medium |
| [CFG](porting.md#cfg--remember-the-selected-game-and-player) | Settings | Remember the selected game and player between runs; the Save button is an unwired placeholder | Medium |
| [WHN](porting.md#whn--when-options) | UI | Bring back the When options; the disabled list and checkboxes in `FormRE` are unwired placeholders | Medium |
| [ACT](porting.md#act--pause-and-volume-actions) | Actions | Bring back Pause Keypress and Volume Keypress, with Quick Volume and Ignore Modifiers | Medium |
| [SRT](porting.md#srt--saints-row-the-third) | Games | Port Saints Row: The Third detection from the unmerged `origin/saintsrow3` branch; it needs a second memory address | Low |
| [LEG](porting.md#leg--delete-the-legacy-code) | Code | Once everything above is ported, delete `Form1`, `ReadMemory`, `Settings` and the comment blocks in `Program.cs` | Low |

## Live checks — detail in [live-checks.md](live-checks.md)

| Code | Area | Summary | Severity |
|---|---|---|---|
| [LIV](live-checks.md#liv--configs-not-yet-seen-working-live) | Verification | Check each game and music player config against the real software; several say "Untested" in their own description | Medium |
