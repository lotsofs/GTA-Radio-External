namespace VGRadioExternal {
    partial class FormRE {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRE));
			this.ComboBox_MusicPlayer = new System.Windows.Forms.ComboBox();
			this.GroupBox_Processes = new System.Windows.Forms.GroupBox();
			this.Button_Refresh = new System.Windows.Forms.Button();
			this.Button_OpenDirectory = new System.Windows.Forms.Button();
			this.Label_StatusGame = new System.Windows.Forms.Label();
			this.Label_StatusMusicPlayer = new System.Windows.Forms.Label();
			this.Label_Game = new System.Windows.Forms.Label();
			this.Label_MusicPlayer = new System.Windows.Forms.Label();
			this.ComboBox_Game = new System.Windows.Forms.ComboBox();
			this.ComboBox_When = new System.Windows.Forms.GroupBox();
			this.Button_Save = new System.Windows.Forms.Button();
			this.CheckBox_7 = new System.Windows.Forms.CheckBox();
			this.CheckBox_6 = new System.Windows.Forms.CheckBox();
			this.CheckBox_5 = new System.Windows.Forms.CheckBox();
			this.CheckBox_4 = new System.Windows.Forms.CheckBox();
			this.CheckBox_3 = new System.Windows.Forms.CheckBox();
			this.CheckBox_2 = new System.Windows.Forms.CheckBox();
			this.CheckBox_1 = new System.Windows.Forms.CheckBox();
			this.Label_Info = new System.Windows.Forms.Label();
			this.GroupBox_Processes.SuspendLayout();
			this.ComboBox_When.SuspendLayout();
			this.SuspendLayout();
			// 
			// ComboBox_MusicPlayer
			// 
			this.ComboBox_MusicPlayer.FormattingEnabled = true;
			this.ComboBox_MusicPlayer.Location = new System.Drawing.Point(3, 32);
			this.ComboBox_MusicPlayer.Name = "ComboBox_MusicPlayer";
			this.ComboBox_MusicPlayer.Size = new System.Drawing.Size(228, 21);
			this.ComboBox_MusicPlayer.TabIndex = 0;
			this.ComboBox_MusicPlayer.SelectedIndexChanged += new System.EventHandler(this.ComboBox_MusicPlayer_SelectedIndexChanged);
			// 
			// GroupBox_Processes
			// 
			this.GroupBox_Processes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.GroupBox_Processes.Controls.Add(this.Button_Refresh);
			this.GroupBox_Processes.Controls.Add(this.Button_OpenDirectory);
			this.GroupBox_Processes.Controls.Add(this.Label_StatusGame);
			this.GroupBox_Processes.Controls.Add(this.Label_StatusMusicPlayer);
			this.GroupBox_Processes.Controls.Add(this.Label_Game);
			this.GroupBox_Processes.Controls.Add(this.Label_MusicPlayer);
			this.GroupBox_Processes.Controls.Add(this.ComboBox_MusicPlayer);
			this.GroupBox_Processes.Controls.Add(this.ComboBox_Game);
			this.GroupBox_Processes.Location = new System.Drawing.Point(7, 3);
			this.GroupBox_Processes.Name = "GroupBox_Processes";
			this.GroupBox_Processes.Size = new System.Drawing.Size(237, 129);
			this.GroupBox_Processes.TabIndex = 1;
			this.GroupBox_Processes.TabStop = false;
			this.GroupBox_Processes.Text = "Processes";
			// 
			// Button_Refresh
			// 
			this.Button_Refresh.Location = new System.Drawing.Point(116, 100);
			this.Button_Refresh.Name = "Button_Refresh";
			this.Button_Refresh.Size = new System.Drawing.Size(71, 23);
			this.Button_Refresh.TabIndex = 8;
			this.Button_Refresh.Text = "Refresh";
			this.Button_Refresh.UseVisualStyleBackColor = true;
			this.Button_Refresh.Click += new System.EventHandler(this.Button_Refresh_Click);
			// 
			// Button_OpenDirectory
			// 
			this.Button_OpenDirectory.Location = new System.Drawing.Point(6, 100);
			this.Button_OpenDirectory.Name = "Button_OpenDirectory";
			this.Button_OpenDirectory.Size = new System.Drawing.Size(104, 23);
			this.Button_OpenDirectory.TabIndex = 7;
			this.Button_OpenDirectory.Text = "Open Directory";
			this.Button_OpenDirectory.UseVisualStyleBackColor = true;
			this.Button_OpenDirectory.Click += new System.EventHandler(this.Button_OpenDirectory_Click);
			// 
			// Label_StatusGame
			// 
			this.Label_StatusGame.AutoSize = true;
			this.Label_StatusGame.Location = new System.Drawing.Point(100, 56);
			this.Label_StatusGame.Name = "Label_StatusGame";
			this.Label_StatusGame.Size = new System.Drawing.Size(10, 13);
			this.Label_StatusGame.TabIndex = 6;
			this.Label_StatusGame.Text = "-";
			// 
			// Label_StatusMusicPlayer
			// 
			this.Label_StatusMusicPlayer.AutoSize = true;
			this.Label_StatusMusicPlayer.Location = new System.Drawing.Point(100, 16);
			this.Label_StatusMusicPlayer.Name = "Label_StatusMusicPlayer";
			this.Label_StatusMusicPlayer.Size = new System.Drawing.Size(10, 13);
			this.Label_StatusMusicPlayer.TabIndex = 5;
			this.Label_StatusMusicPlayer.Text = "-";
			// 
			// Label_Game
			// 
			this.Label_Game.AutoSize = true;
			this.Label_Game.Location = new System.Drawing.Point(3, 56);
			this.Label_Game.Name = "Label_Game";
			this.Label_Game.Size = new System.Drawing.Size(35, 13);
			this.Label_Game.TabIndex = 4;
			this.Label_Game.Text = "Game";
			// 
			// Label_MusicPlayer
			// 
			this.Label_MusicPlayer.AutoSize = true;
			this.Label_MusicPlayer.Location = new System.Drawing.Point(3, 16);
			this.Label_MusicPlayer.Name = "Label_MusicPlayer";
			this.Label_MusicPlayer.Size = new System.Drawing.Size(67, 13);
			this.Label_MusicPlayer.TabIndex = 2;
			this.Label_MusicPlayer.Text = "Music Player";
			// 
			// ComboBox_Game
			// 
			this.ComboBox_Game.FormattingEnabled = true;
			this.ComboBox_Game.Location = new System.Drawing.Point(3, 72);
			this.ComboBox_Game.Name = "ComboBox_Game";
			this.ComboBox_Game.Size = new System.Drawing.Size(228, 21);
			this.ComboBox_Game.TabIndex = 3;
			this.ComboBox_Game.SelectedIndexChanged += new System.EventHandler(this.ComboBox_Game_SelectedIndexChanged);
			// 
			// ComboBox_When
			// 
			this.ComboBox_When.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ComboBox_When.Controls.Add(this.Button_Save);
			this.ComboBox_When.Controls.Add(this.CheckBox_7);
			this.ComboBox_When.Controls.Add(this.CheckBox_6);
			this.ComboBox_When.Controls.Add(this.CheckBox_5);
			this.ComboBox_When.Controls.Add(this.CheckBox_4);
			this.ComboBox_When.Controls.Add(this.CheckBox_3);
			this.ComboBox_When.Controls.Add(this.CheckBox_2);
			this.ComboBox_When.Controls.Add(this.CheckBox_1);
			this.ComboBox_When.Enabled = false;
			this.ComboBox_When.Location = new System.Drawing.Point(7, 138);
			this.ComboBox_When.Name = "ComboBox_When";
			this.ComboBox_When.Size = new System.Drawing.Size(237, 112);
			this.ComboBox_When.TabIndex = 2;
			this.ComboBox_When.TabStop = false;
			this.ComboBox_When.Text = "When";
			// 
			// Button_Save
			// 
			this.Button_Save.Location = new System.Drawing.Point(161, 84);
			this.Button_Save.Name = "Button_Save";
			this.Button_Save.Size = new System.Drawing.Size(71, 23);
			this.Button_Save.TabIndex = 9;
			this.Button_Save.Text = "Save";
			this.Button_Save.UseVisualStyleBackColor = true;
			// 
			// CheckBox_7
			// 
			this.CheckBox_7.AutoSize = true;
			this.CheckBox_7.Location = new System.Drawing.Point(107, 65);
			this.CheckBox_7.Name = "CheckBox_7";
			this.CheckBox_7.Size = new System.Drawing.Size(29, 17);
			this.CheckBox_7.TabIndex = 6;
			this.CheckBox_7.Text = "-";
			this.CheckBox_7.UseVisualStyleBackColor = true;
			// 
			// CheckBox_6
			// 
			this.CheckBox_6.AutoSize = true;
			this.CheckBox_6.Location = new System.Drawing.Point(107, 42);
			this.CheckBox_6.Name = "CheckBox_6";
			this.CheckBox_6.Size = new System.Drawing.Size(29, 17);
			this.CheckBox_6.TabIndex = 5;
			this.CheckBox_6.Text = "-";
			this.CheckBox_6.UseVisualStyleBackColor = true;
			// 
			// CheckBox_5
			// 
			this.CheckBox_5.AutoSize = true;
			this.CheckBox_5.Location = new System.Drawing.Point(107, 19);
			this.CheckBox_5.Name = "CheckBox_5";
			this.CheckBox_5.Size = new System.Drawing.Size(29, 17);
			this.CheckBox_5.TabIndex = 4;
			this.CheckBox_5.Text = "-";
			this.CheckBox_5.UseVisualStyleBackColor = true;
			// 
			// CheckBox_4
			// 
			this.CheckBox_4.AutoSize = true;
			this.CheckBox_4.Location = new System.Drawing.Point(6, 88);
			this.CheckBox_4.Name = "CheckBox_4";
			this.CheckBox_4.Size = new System.Drawing.Size(29, 17);
			this.CheckBox_4.TabIndex = 3;
			this.CheckBox_4.Text = "-";
			this.CheckBox_4.UseVisualStyleBackColor = true;
			// 
			// CheckBox_3
			// 
			this.CheckBox_3.AutoSize = true;
			this.CheckBox_3.Location = new System.Drawing.Point(6, 65);
			this.CheckBox_3.Name = "CheckBox_3";
			this.CheckBox_3.Size = new System.Drawing.Size(29, 17);
			this.CheckBox_3.TabIndex = 2;
			this.CheckBox_3.Text = "-";
			this.CheckBox_3.UseVisualStyleBackColor = true;
			// 
			// CheckBox_2
			// 
			this.CheckBox_2.AutoSize = true;
			this.CheckBox_2.Location = new System.Drawing.Point(6, 42);
			this.CheckBox_2.Name = "CheckBox_2";
			this.CheckBox_2.Size = new System.Drawing.Size(29, 17);
			this.CheckBox_2.TabIndex = 1;
			this.CheckBox_2.Text = "-";
			this.CheckBox_2.UseVisualStyleBackColor = true;
			// 
			// CheckBox_1
			// 
			this.CheckBox_1.AutoSize = true;
			this.CheckBox_1.Location = new System.Drawing.Point(6, 19);
			this.CheckBox_1.Name = "CheckBox_1";
			this.CheckBox_1.Size = new System.Drawing.Size(29, 17);
			this.CheckBox_1.TabIndex = 0;
			this.CheckBox_1.Text = "-";
			this.CheckBox_1.UseVisualStyleBackColor = true;
			// 
			// Label_Info
			// 
			this.Label_Info.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.Label_Info.AutoSize = true;
			this.Label_Info.Location = new System.Drawing.Point(77, 256);
			this.Label_Info.Name = "Label_Info";
			this.Label_Info.Size = new System.Drawing.Size(167, 26);
			this.Label_Info.TabIndex = 5;
			this.Label_Info.Text = "Program made by twitch.tv/lotsofs\r\nV0.3.0 WIP";
			this.Label_Info.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// FormRE
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(251, 285);
			this.Controls.Add(this.ComboBox_When);
			this.Controls.Add(this.GroupBox_Processes);
			this.Controls.Add(this.Label_Info);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormRE";
			this.Text = "Radio External";
			this.Load += new System.EventHandler(this.FormRE_Load);
			this.GroupBox_Processes.ResumeLayout(false);
			this.GroupBox_Processes.PerformLayout();
			this.ComboBox_When.ResumeLayout(false);
			this.ComboBox_When.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ComboBox_MusicPlayer;
        private System.Windows.Forms.GroupBox GroupBox_Processes;
        private System.Windows.Forms.Label Label_MusicPlayer;
        private System.Windows.Forms.ComboBox ComboBox_Game;
        private System.Windows.Forms.Label Label_Game;
        private System.Windows.Forms.Button Button_Refresh;
        private System.Windows.Forms.Button Button_OpenDirectory;
        private System.Windows.Forms.Label Label_StatusGame;
        private System.Windows.Forms.Label Label_StatusMusicPlayer;
        private System.Windows.Forms.GroupBox ComboBox_When;
        private System.Windows.Forms.CheckBox CheckBox_7;
        private System.Windows.Forms.CheckBox CheckBox_6;
        private System.Windows.Forms.CheckBox CheckBox_5;
        private System.Windows.Forms.CheckBox CheckBox_4;
        private System.Windows.Forms.CheckBox CheckBox_3;
        private System.Windows.Forms.CheckBox CheckBox_2;
        private System.Windows.Forms.CheckBox CheckBox_1;
        private System.Windows.Forms.Label Label_Info;
        private System.Windows.Forms.Button Button_Save;
    }
}