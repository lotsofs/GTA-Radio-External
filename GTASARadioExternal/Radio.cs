using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace GTASARadioExternal {
    class Radio {
        MusicPlayer _musicPlayer;
        Game _game;

        int _timerInterval = 40;
        Timer _timer;

        bool _on;
        bool _gameRunning;
        bool _musicPlayerRunning;
        
        public Radio() {
            _musicPlayer = new MusicPlayer();
            _game = new Game();
            StartTimer();
        }

        public void StartTimer() {
            _timer = new Timer();
            _timer.Elapsed += Check;
            _timer.Interval = _timerInterval;
            _timer.Start();
        }

        public void Check(Object source, EventArgs e) {
            if (!CheckProcesses()) {
                return;
            }
            CheckRadio();
        }

        /// <summary>
        /// Checks if the radio should be playing, and changes it if it's doing the opposite
        /// </summary>
        void CheckRadio() {
            bool on = _game.RadioOn();
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
            _gameRunning = _game.Running();
            _musicPlayerRunning = _musicPlayer.Running();   // call an event when these change, inside a set { thing?
            return _gameRunning && _musicPlayerRunning;
        }

    }
}
