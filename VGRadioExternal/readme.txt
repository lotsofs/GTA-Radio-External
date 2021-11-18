JSON explanation:

For information about the JSON format, see www.json.org
Please ensure each jason has a description, and the name of the process (without the .exe extension), as well as information about the method to send information to the music player (hook).

Hook: There are two kinds of hooks, ProcessStart and SendMessage.
ProcessStart will start the music player process but with certain arguments. If the music player supports it, this will instead send the argument to the existing process, as opposed to starting a duplicate process.
SendMessage will have Windows send a message to the process, telling it to mute or unmute. A message requires a windowName, wMsg, wParam, and lParam.

Volume:
The volume object is used to determine what volume a program is set to. This allows for the unmuting of programs that do not support native muting by instead setting volume directly.
If a volume object is provided combined with the SendMessage hook, the program will automatically assume the wParam contains the volume. If no volume object is provided, please provide a wParam yourself.
The volume object requires a memory address to watch, which consists of a module (dll) and an addressoffset. Leave module blank if the offset is to the main module (exe).
minVolume is the minimum volume of the music player. Typically this is 0, but some players might set volume below this for muted. The number can be raised to instead of muting, simply lower music volume.
Type means the kind of value type is stored in the address. Currently supported are Byte, FourBytes & Float. These work the same as they do in Cheat Engine, which you might find handy to use if you wish to add your own music player.

It is recommended to look at the standard provided json files for examples.