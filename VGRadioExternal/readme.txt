JSON explanation:

For information about the JSON format, see www.json.org
Please ensure each jason has a description, and the name of the process (without the .exe extension), as well as information about the method to send information to the music player (hook).

Hook: There are four kinds of hooks, ProcessStart, SendMessage, AudioSession and KeyPress.
ProcessStart will start the music player process but with certain arguments. If the music player supports it, this will instead send the argument to the existing process, as opposed to starting a duplicate process. The argument should toggle mute. Without a volume object, start with the music player unmuted. With one, it only runs when needed.
SendMessage will have Windows send a message to the process, telling it to mute or unmute. A message requires a windowName, wMsg, wParam, and lParam. If neither wParam nor lParam is "{volume}", the message should toggle mute. Without a volume object, start with the music player unmuted. With one, it's only sent when needed.
AudioSession will mute the process in the Windows Volume Mixer, so it works for anything listed there. It needs no other settings and doesn't use a volume object. The process has to have played something first, or there is nothing to mute yet.
KeyPress will press a hotkey to toggle the music player's mute. Set it as a global hotkey in the music player, and put the same key in keys, for example Ctrl+Shift+P. Without a volume object, start with the music player unmuted. With one, the key is only pressed when needed. Pick a key the game doesn't use: the game still sees Shift, Ctrl and Alt, so a key without them, like F13, is best. If you hold another Shift, Ctrl or Alt, the key waits until you let go.

Volume:
The volume object is used to determine what volume a program is set to. This allows for the unmuting of programs that do not support native muting by instead setting volume directly.
If a volume object is provided combined with the SendMessage hook, the program will automatically assume the wParam contains the volume. If no volume object is provided, please provide a wParam yourself.
The volume object requires a memory address to watch, which consists of a module (dll) and an addressoffset. Leave module blank if the offset is to the main module (exe). Write the addressoffset as a number, or as hex in quotes, for example "0x1BE9E8".
minVolume is the minimum volume of the music player. Typically this is 0, but some players might set volume below this for muted. The number can be raised to instead of muting, simply lower music volume.
With a hook that toggles mute, the volume object shows whether the music player is already muted: minVolume or below counts as muted. Only use one if the value really drops that low when muted. If it doesn't, the music player will mute and never unmute.
Type means the kind of value type is stored in the address. Currently supported are Byte, FourBytes & Float. These work the same as they do in Cheat Engine, which you might find handy to use if you wish to add your own music player.

It is recommended to look at the standard provided json files for examples.