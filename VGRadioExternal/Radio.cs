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
        MusicPlayer _musicPlayer;
        Game _game;

        int _timerInterval = 40;
        Timer _timer;

        bool _on;
        bool _gameRunning;
        bool _musicPlayerRunning;
        
        public Radio() {
            //string path = Path.GetFullPath(".\\input\\music players\\Wacup - Not So Direct.json");   // todo: Nice hardcode
            //string json = File.ReadAllText(path);

            //JsonObject json = Json.OpenFile(path);      // todo: move this to some dedicated json handler for the tool, or at least a load script

            //_musicPlayer = new MusicPlayer(json);
            //_game = new Game();
            StartTimer();
        }

        public void MusicPlayerChanged(string path) {
            string json = File.ReadAllText(path);
            _musicPlayer = new MusicPlayer(json);
		}

        public void GameChanged(string path) {
            string json = File.ReadAllText(path);
            _game = new Game(json);
		}

        void StartTimer() {
            _timer = new Timer();
            _timer.Elapsed += Check;
            _timer.Interval = _timerInterval;
            _timer.Start();
        }

        void Check(Object source, EventArgs e) {
            if (!CheckProcesses()) {
                return;
            }
            CheckRadio();
        }

        /// <summary>
        /// Checks if the radio should be playing, and changes it if it's doing the opposite
        /// </summary>
        void CheckRadio() {
            bool on = _game.IsRadioOn();
            if (on != _on) {
                _musicPlayer.Mute(!on);
                _on = on;
            }
        }

        /// <summary>
        /// Checks if the processes are still running
        /// </summary>
        /// <returns></returns>
        bool CheckProcesses() {
            _gameRunning = _game != null && _game.IsRunning();
            _musicPlayerRunning = _musicPlayer != null && _musicPlayer.IsRunning();   // todo: call an event when these change, inside a set { thing?
            return _gameRunning && _musicPlayerRunning;
        }

    }
}
