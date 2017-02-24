namespace GTASARadioExternal {
    partial class Form1 {
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.radioButtonIII = new System.Windows.Forms.RadioButton();
			this.radioButtonVC = new System.Windows.Forms.RadioButton();
			this.radioButtonSA = new System.Windows.Forms.RadioButton();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.radioButtonOther = new System.Windows.Forms.RadioButton();
			this.radioButtonWMP = new System.Windows.Forms.RadioButton();
			this.radioButtonVLC = new System.Windows.Forms.RadioButton();
			this.radioButtonWinamp = new System.Windows.Forms.RadioButton();
			this.radioButtonFoobar = new System.Windows.Forms.RadioButton();
			this.radioButtonSpotify = new System.Windows.Forms.RadioButton();
			this.labelVolume = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.radioButtonVolume = new System.Windows.Forms.RadioButton();
			this.radioButtonPause = new System.Windows.Forms.RadioButton();
			this.radioButtonMute = new System.Windows.Forms.RadioButton();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Loading...";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(111, 219);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(167, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Program made by twitch.tv/lotsofs";
			// 
			// radioButtonIII
			// 
			this.radioButtonIII.AutoSize = true;
			this.radioButtonIII.Enabled = false;
			this.radioButtonIII.Location = new System.Drawing.Point(6, 19);
			this.radioButtonIII.Name = "radioButtonIII";
			this.radioButtonIII.Size = new System.Drawing.Size(56, 17);
			this.radioButtonIII.TabIndex = 5;
			this.radioButtonIII.Text = "GTAIII";
			this.toolTip1.SetToolTip(this.radioButtonIII, "The game to be played");
			this.radioButtonIII.UseVisualStyleBackColor = true;
			// 
			// radioButtonVC
			// 
			this.radioButtonVC.AutoSize = true;
			this.radioButtonVC.Enabled = false;
			this.radioButtonVC.Location = new System.Drawing.Point(6, 42);
			this.radioButtonVC.Name = "radioButtonVC";
			this.radioButtonVC.Size = new System.Drawing.Size(61, 17);
			this.radioButtonVC.TabIndex = 5;
			this.radioButtonVC.Text = "GTAVC";
			this.toolTip1.SetToolTip(this.radioButtonVC, "The game to be played");
			this.radioButtonVC.UseVisualStyleBackColor = true;
			// 
			// radioButtonSA
			// 
			this.radioButtonSA.AutoSize = true;
			this.radioButtonSA.Location = new System.Drawing.Point(6, 65);
			this.radioButtonSA.Name = "radioButtonSA";
			this.radioButtonSA.Size = new System.Drawing.Size(61, 17);
			this.radioButtonSA.TabIndex = 5;
			this.radioButtonSA.Text = "GTASA";
			this.toolTip1.SetToolTip(this.radioButtonSA, "The game to be played");
			this.radioButtonSA.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.radioButtonIII);
			this.groupBox1.Controls.Add(this.radioButtonVC);
			this.groupBox1.Controls.Add(this.radioButtonSA);
			this.groupBox1.Location = new System.Drawing.Point(15, 33);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(87, 109);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Game";
			this.toolTip1.SetToolTip(this.groupBox1, "The game to be played");
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.label2.Location = new System.Drawing.Point(3, 93);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(51, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Vx.0x RE";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.radioButtonOther);
			this.groupBox2.Controls.Add(this.radioButtonWMP);
			this.groupBox2.Controls.Add(this.radioButtonVLC);
			this.groupBox2.Controls.Add(this.radioButtonWinamp);
			this.groupBox2.Controls.Add(this.radioButtonFoobar);
			this.groupBox2.Controls.Add(this.radioButtonSpotify);
			this.groupBox2.Location = new System.Drawing.Point(108, 33);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(164, 109);
			this.groupBox2.TabIndex = 7;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Music Player";
			this.toolTip1.SetToolTip(this.groupBox2, "Select which Music Player you are using");
			// 
			// radioButtonOther
			// 
			this.radioButtonOther.AutoSize = true;
			this.radioButtonOther.Location = new System.Drawing.Point(76, 65);
			this.radioButtonOther.Name = "radioButtonOther";
			this.radioButtonOther.Size = new System.Drawing.Size(51, 17);
			this.radioButtonOther.TabIndex = 8;
			this.radioButtonOther.Text = "Other";
			this.toolTip1.SetToolTip(this.radioButtonOther, "Select which Music Player you are using");
			this.radioButtonOther.UseVisualStyleBackColor = true;
			this.radioButtonOther.CheckedChanged += new System.EventHandler(this.radioButtonOther_CheckedChanged);
			// 
			// radioButtonWMP
			// 
			this.radioButtonWMP.AutoSize = true;
			this.radioButtonWMP.Enabled = false;
			this.radioButtonWMP.Location = new System.Drawing.Point(76, 19);
			this.radioButtonWMP.Name = "radioButtonWMP";
			this.radioButtonWMP.Size = new System.Drawing.Size(52, 17);
			this.radioButtonWMP.TabIndex = 6;
			this.radioButtonWMP.Text = "WMP";
			this.toolTip1.SetToolTip(this.radioButtonWMP, "Select which Music Player you are using");
			this.radioButtonWMP.UseVisualStyleBackColor = true;
			// 
			// radioButtonVLC
			// 
			this.radioButtonVLC.AutoSize = true;
			this.radioButtonVLC.Enabled = false;
			this.radioButtonVLC.Location = new System.Drawing.Point(76, 42);
			this.radioButtonVLC.Name = "radioButtonVLC";
			this.radioButtonVLC.Size = new System.Drawing.Size(45, 17);
			this.radioButtonVLC.TabIndex = 7;
			this.radioButtonVLC.Text = "VLC";
			this.toolTip1.SetToolTip(this.radioButtonVLC, "Select which Music Player you are using");
			this.radioButtonVLC.UseVisualStyleBackColor = true;
			// 
			// radioButtonWinamp
			// 
			this.radioButtonWinamp.AutoSize = true;
			this.radioButtonWinamp.Location = new System.Drawing.Point(6, 19);
			this.radioButtonWinamp.Name = "radioButtonWinamp";
			this.radioButtonWinamp.Size = new System.Drawing.Size(64, 17);
			this.radioButtonWinamp.TabIndex = 5;
			this.radioButtonWinamp.Text = "Winamp";
			this.toolTip1.SetToolTip(this.radioButtonWinamp, "Select which Music Player you are using");
			this.radioButtonWinamp.UseVisualStyleBackColor = true;
			this.radioButtonWinamp.CheckedChanged += new System.EventHandler(this.radioButtonWinamp_CheckedChanged);
			// 
			// radioButtonFoobar
			// 
			this.radioButtonFoobar.AutoSize = true;
			this.radioButtonFoobar.Enabled = false;
			this.radioButtonFoobar.Location = new System.Drawing.Point(6, 42);
			this.radioButtonFoobar.Name = "radioButtonFoobar";
			this.radioButtonFoobar.Size = new System.Drawing.Size(58, 17);
			this.radioButtonFoobar.TabIndex = 5;
			this.radioButtonFoobar.Text = "Foobar";
			this.toolTip1.SetToolTip(this.radioButtonFoobar, "Select which Music Player you are using");
			this.radioButtonFoobar.UseVisualStyleBackColor = true;
			// 
			// radioButtonSpotify
			// 
			this.radioButtonSpotify.AutoSize = true;
			this.radioButtonSpotify.Enabled = false;
			this.radioButtonSpotify.Location = new System.Drawing.Point(6, 65);
			this.radioButtonSpotify.Name = "radioButtonSpotify";
			this.radioButtonSpotify.Size = new System.Drawing.Size(57, 17);
			this.radioButtonSpotify.TabIndex = 5;
			this.radioButtonSpotify.Text = "Spotify";
			this.toolTip1.SetToolTip(this.radioButtonSpotify, "Select which Music Player you are using");
			this.radioButtonSpotify.UseVisualStyleBackColor = true;
			// 
			// labelVolume
			// 
			this.labelVolume.AutoSize = true;
			this.labelVolume.Location = new System.Drawing.Point(221, 9);
			this.labelVolume.Name = "labelVolume";
			this.labelVolume.Size = new System.Drawing.Size(51, 13);
			this.labelVolume.TabIndex = 8;
			this.labelVolume.Text = "Volume: -";
			this.labelVolume.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.checkBox1);
			this.groupBox3.Controls.Add(this.radioButtonVolume);
			this.groupBox3.Controls.Add(this.radioButtonPause);
			this.groupBox3.Controls.Add(this.radioButtonMute);
			this.groupBox3.Location = new System.Drawing.Point(15, 149);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(259, 67);
			this.groupBox3.TabIndex = 9;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Action";
			this.toolTip1.SetToolTip(this.groupBox3, resources.GetString("groupBox3.ToolTip"));
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Enabled = false;
			this.checkBox1.Location = new System.Drawing.Point(7, 42);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(92, 17);
			this.checkBox1.TabIndex = 8;
			this.checkBox1.Text = "Quick Volume";
			this.toolTip1.SetToolTip(this.checkBox1, resources.GetString("checkBox1.ToolTip"));
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// radioButtonVolume
			// 
			this.radioButtonVolume.AutoSize = true;
			this.radioButtonVolume.Enabled = false;
			this.radioButtonVolume.Location = new System.Drawing.Point(7, 19);
			this.radioButtonVolume.Name = "radioButtonVolume";
			this.radioButtonVolume.Size = new System.Drawing.Size(60, 17);
			this.radioButtonVolume.TabIndex = 7;
			this.radioButtonVolume.Text = "Volume";
			this.toolTip1.SetToolTip(this.radioButtonVolume, resources.GetString("radioButtonVolume.ToolTip"));
			this.radioButtonVolume.UseVisualStyleBackColor = true;
			this.radioButtonVolume.CheckedChanged += new System.EventHandler(this.radioButtonVolume_CheckedChanged);
			this.radioButtonVolume.EnabledChanged += new System.EventHandler(this.radioButtonVolume_EnabledChanged);
			// 
			// radioButtonPause
			// 
			this.radioButtonPause.AutoSize = true;
			this.radioButtonPause.Enabled = false;
			this.radioButtonPause.Location = new System.Drawing.Point(139, 19);
			this.radioButtonPause.Name = "radioButtonPause";
			this.radioButtonPause.Size = new System.Drawing.Size(55, 17);
			this.radioButtonPause.TabIndex = 6;
			this.radioButtonPause.Text = "Pause";
			this.toolTip1.SetToolTip(this.radioButtonPause, resources.GetString("radioButtonPause.ToolTip"));
			this.radioButtonPause.UseVisualStyleBackColor = true;
			this.radioButtonPause.CheckedChanged += new System.EventHandler(this.radioButtonPause_CheckedChanged);
			// 
			// radioButtonMute
			// 
			this.radioButtonMute.AutoSize = true;
			this.radioButtonMute.Enabled = false;
			this.radioButtonMute.Location = new System.Drawing.Point(73, 19);
			this.radioButtonMute.Name = "radioButtonMute";
			this.radioButtonMute.Size = new System.Drawing.Size(49, 17);
			this.radioButtonMute.TabIndex = 5;
			this.radioButtonMute.Text = "Mute";
			this.toolTip1.SetToolTip(this.radioButtonMute, resources.GetString("radioButtonMute.ToolTip"));
			this.radioButtonMute.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(286, 241);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.labelVolume);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Name = "Form1";
			this.Text = "GTASA x Winamp Radio";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
		private System.Windows.Forms.RadioButton radioButtonIII;
		private System.Windows.Forms.RadioButton radioButtonVC;
		private System.Windows.Forms.RadioButton radioButtonSA;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton radioButtonWinamp;
		private System.Windows.Forms.RadioButton radioButtonFoobar;
		private System.Windows.Forms.RadioButton radioButtonSpotify;
		private System.Windows.Forms.RadioButton radioButtonWMP;
		private System.Windows.Forms.RadioButton radioButtonVLC;
		private System.Windows.Forms.Label labelVolume;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.RadioButton radioButtonPause;
		private System.Windows.Forms.RadioButton radioButtonMute;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.RadioButton radioButtonVolume;
		private System.Windows.Forms.RadioButton radioButtonOther;
		private System.Windows.Forms.Label label2;
	}
}

