# Live checks

Detail for the item in [TODO.md](TODO.md).

## LIV — Configs not yet seen working live

These need the real software, and nothing records them passing on the current build. Several configs say "Untested" in their own description, and the ones that say "Tested" don't say on which build.

**Base check:** pick the game and player, then play. In a car with the radio on, the music plays at its normal volume. Out of the car, the player goes silent. Back in the car, the volume returns to where it was. The game's status reads `Radio on` and `Radio off` to match, and the player's reads `Running`.

A check keeps its number for good. When one passes, delete it and log it by number in [DONE.md](DONE.md). When none are left, delete this doc and the index row, and log `LIV` as finished.

### With San Andreas and a working player

Use any player from the next group.

- **LIV 1.** `Grand Theft Auto San Andreas.json` on version 1.0 as `gta_sa`: base check.
- **LIV 2.** `Grand Theft Auto Dash San Andreas.json` on version 1.0 as `gta-sa`: base check.

### With other games

- **LIV 3.** `Grand Theft Auto San Andreas The Definitive Edition.json`: base check.
- **LIV 4.** `Grand Theft Auto IV.json`: base check, including on foot and in the menu, because its off values differ from San Andreas.

### With each music player

Use any game that passed above.

- **LIV 5.** `Winamp.json` on Winamp 5.8 with the `out_ds.dll` output: base check.
- **LIV 6.** `Wacup.json` on WACUP 1.0.20.7170: base check.
- **LIV 7.** `Wacup - Not So Direct.json` on WACUP 1.0.20.7170 with the Not So Direct output: base check.

### After FOB and HOK

These can't pass until [FOB](issues.md#fob--foobar2000json-fails-to-load) and [HOK](issues.md#hok--two-hook-combinations-throw) are done.

- **LIV 8.** `Foobar2000.json` on Foobar2000 1.4.8: base check, and the mute stays in step with the radio over several changes.
- **LIV 9.** `Windows Media Player.json` on Windows Media Player 12: base check, starting with the player unmuted.
