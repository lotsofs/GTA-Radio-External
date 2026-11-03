//using S.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using Newtonsoft.Json;

namespace VGRadioExternal {
    class Radio {
        const string NotSelected = "Not selected";
        // Alt+F4 leaves Alt held while closing, and a KeyPress hook waits for it to be let go
        const int StopTimeout = 1000;

        /// <summary>
        /// Raised on the timer thread with the game's and the music player's status, when either changes
        /// </summary>
        public event Action<string, string> StatusChanged;

        // the poll does all the work on the player, so it swaps the selected one in itself
        MusicPlayer _musicPlayer;
        volatile MusicPlayer _selectedMusicPlayer;
        Game _game;

        int _timerInterval = 40;
        Timer _timer;
        volatile bool _stopping;
        System.Threading.ManualResetEventSlim _stopped = new System.Threading.ManualResetEventSlim();

        // what the player was last told. Null when it hasn't been told anything, so the next poll tells it
        bool? _on;
        bool _gameRunning;
        bool _musicPlayerRunning;
        string _gameStatus;
        string _musicPlayerStatus;
        string _muteError;

        public Radio() {
            //string path = Path.GetFullPath(".\\input\\music players\\Wacup - Not So Direct.json");
            //string json = File.ReadAllText(path);

            //JsonObject json = Json.OpenFile(path);

            //_musicPlayer = new MusicPlayer(json);
            //_game = new Game();
        }

        public void MusicPlayerChanged(string path) {
            string json = File.ReadAllText(path);
            _selectedMusicPlayer = new MusicPlayer(json);
		}

        public void GameChanged(string path) {
            string json = File.ReadAllText(path);
            _game = new Game(json);
		}

        /// <summary>
        /// Starts polling. Call this once the form can show the status, because a status is only reported when it changes.
        /// </summary>
        public void StartTimer() {
            _timer = new Timer();
            _timer.AutoReset = false;
            _timer.Elapsed += Check;
            _timer.Interval = _timerInterval;
            _timer.Start();
        }

        /// <summary>
        /// Stops polling once the poll has left the music player playing, or after a moment if it can't
        /// </summary>
        public void Stop() {
            _stopping = true;
            _stopped.Wait(StopTimeout);
        }

        void Check(Object source, EventArgs e) {
            try {
                _muteError = null;
                CheckProcesses();
                CheckRadio();
                ReplaceMusicPlayer();
                ReportStatus();
            }
            finally {
                if (_stopping && _on != false) {
                    _stopped.Set();
                }
                else {
                    // AutoReset is off, so a slow poll can't overlap the next one
                    _timer.Start();
                }
            }
        }

        /// <summary>
        /// Checks if the radio should be playing, and changes it if it's doing the opposite
        /// </summary>
        void CheckRadio() {
            // the music plays when there's no radio to follow, and a player that's being let go of is left playing
            bool releasing = _stopping || _selectedMusicPlayer != _musicPlayer;
            bool on = releasing || !_gameRunning || _game.IsRadioOn();
            if (!_musicPlayerRunning) {
                // a player that comes back starts in its own state, so it has to be told again
                _on = null;
                return;
            }
            if (on != _on) {
                try {
                    _musicPlayer.Mute(!on);
                    _on = on;
                }
                catch (Exception ex) {
                    // the timer would swallow this, so show it instead
                    _muteError = "Error: " + ex.Message;
                }
            }
        }

        /// <summary>
        /// Swaps in the selected music player once the old one is playing, because nothing will tell the old one again
        /// </summary>
        void ReplaceMusicPlayer() {
            if (_selectedMusicPlayer != _musicPlayer && _on != false) {
                _musicPlayer = _selectedMusicPlayer;
                _on = null;
            }
        }

        /// <summary>
        /// Checks if the processes are still running
        /// </summary>
        void CheckProcesses() {
            _gameRunning = _game != null && _game.IsRunning();
            _musicPlayerRunning = _musicPlayer != null && _musicPlayer.IsRunning();
        }

        /// <summary>
        /// Raises StatusChanged if the game's or the music player's status is different from the last one reported
        /// </summary>
        void ReportStatus() {
            string gameStatus = _game?.Status ?? NotSelected;
            string musicPlayerStatus = _muteError ?? _musicPlayer?.Status ?? NotSelected;
            if (gameStatus != _gameStatus || musicPlayerStatus != _musicPlayerStatus) {
                _gameStatus = gameStatus;
                _musicPlayerStatus = musicPlayerStatus;
                StatusChanged?.Invoke(gameStatus, musicPlayerStatus);
            }
        }

    }
}
