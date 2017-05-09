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
			this.radioButtonWinamp = new System.Windows.Forms.RadioButton();
			this.radioButtonFoobar = new System.Windows.Forms.RadioButton();
			this.labelVolume = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.checkBox7 = new System.Windows.Forms.CheckBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.radioButtonVolume = new System.Windows.Forms.RadioButton();
			this.radioButtonPause = new System.Windows.Forms.RadioButton();
			this.radioButtonMute = new System.Windows.Forms.RadioButton();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.checkBoxF = new System.Windows.Forms.CheckBox();
			this.checkBoxE = new System.Windows.Forms.CheckBox();
			this.checkBoxD = new System.Windows.Forms.CheckBox();
			this.checkBoxC = new System.Windows.Forms.CheckBox();
			this.checkBoxB = new System.Windows.Forms.CheckBox();
			this.checkBoxA = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
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
			this.label3.Location = new System.Drawing.Point(111, 313);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(167, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Program made by twitch.tv/lotsofs";
			this.toolTip1.SetToolTip(this.label3, "Some parts made by e216");
			// 
			// radioButtonIII
			// 
			this.radioButtonIII.AutoSize = true;
			this.radioButtonIII.Location = new System.Drawing.Point(6, 19);
			this.radioButtonIII.Name = "radioButtonIII";
			this.radioButtonIII.Size = new System.Drawing.Size(56, 17);
			this.radioButtonIII.TabIndex = 5;
			this.radioButtonIII.Text = "GTAIII";
			this.toolTip1.SetToolTip(this.radioButtonIII, "The game to be played");
			this.radioButtonIII.UseVisualStyleBackColor = true;
			this.radioButtonIII.CheckedChanged += new System.EventHandler(this.radioButtonIII_CheckedChanged);
			// 
			// radioButtonVC
			// 
			this.radioButtonVC.AutoSize = true;
			this.radioButtonVC.Location = new System.Drawing.Point(6, 42);
			this.radioButtonVC.Name = "radioButtonVC";
			this.radioButtonVC.Size = new System.Drawing.Size(61, 17);
			this.radioButtonVC.TabIndex = 5;
			this.radioButtonVC.Text = "GTAVC";
			this.toolTip1.SetToolTip(this.radioButtonVC, "The game to be played");
			this.radioButtonVC.UseVisualStyleBackColor = true;
			this.radioButtonVC.CheckedChanged += new System.EventHandler(this.radioButtonVC_CheckedChanged);
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
			this.radioButtonSA.CheckedChanged += new System.EventHandler(this.radioButtonSA_CheckedChanged);
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
			this.groupBox2.Controls.Add(this.radioButtonWinamp);
			this.groupBox2.Controls.Add(this.radioButtonFoobar);
			this.groupBox2.Location = new System.Drawing.Point(108, 33);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(168, 109);
			this.groupBox2.TabIndex = 7;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Music Player";
			this.toolTip1.SetToolTip(this.groupBox2, "Select which Music Player you are using\r\nNote: Foobar does not natively support v" +
        "olume up/down keys. Please bind these manually if desiring to use the Volume opt" +
        "ion with Foobar\r\n");
			// 
			// radioButtonOther
			// 
			this.radioButtonOther.AutoSize = true;
			this.radioButtonOther.Location = new System.Drawing.Point(6, 65);
			this.radioButtonOther.Name = "radioButtonOther";
			this.radioButtonOther.Size = new System.Drawing.Size(51, 17);
			this.radioButtonOther.TabIndex = 8;
			this.radioButtonOther.Text = "Other";
			this.toolTip1.SetToolTip(this.radioButtonOther, "Select which Music Player you are using\r\nNote: Foobar does not natively support v" +
        "olume up/down keys. Please bind these manually if desiring to use the Volume opt" +
        "ion with Foobar\r\n");
			this.radioButtonOther.UseVisualStyleBackColor = true;
			this.radioButtonOther.CheckedChanged += new System.EventHandler(this.radioButtonOther_CheckedChanged);
			// 
			// radioButtonWinamp
			// 
			this.radioButtonWinamp.AutoSize = true;
			this.radioButtonWinamp.Location = new System.Drawing.Point(6, 19);
			this.radioButtonWinamp.Name = "radioButtonWinamp";
			this.radioButtonWinamp.Size = new System.Drawing.Size(153, 17);
			this.radioButtonWinamp.TabIndex = 5;
			this.radioButtonWinamp.Text = "Winamp v5.666 Build 3516";
			this.toolTip1.SetToolTip(this.radioButtonWinamp, "Select which Music Player you are using\r\nNote: Foobar does not natively support v" +
        "olume up/down keys. Please bind these manually if desiring to use the Volume opt" +
        "ion with Foobar\r\n");
			this.radioButtonWinamp.UseVisualStyleBackColor = true;
			this.radioButtonWinamp.CheckedChanged += new System.EventHandler(this.radioButtonWinamp_CheckedChanged);
			// 
			// radioButtonFoobar
			// 
			this.radioButtonFoobar.AutoSize = true;
			this.radioButtonFoobar.Location = new System.Drawing.Point(6, 42);
			this.radioButtonFoobar.Name = "radioButtonFoobar";
			this.radioButtonFoobar.Size = new System.Drawing.Size(121, 17);
			this.radioButtonFoobar.TabIndex = 5;
			this.radioButtonFoobar.Text = "Foobar2000 v1.3.14";
			this.toolTip1.SetToolTip(this.radioButtonFoobar, "Select which Music Player you are using\r\nNote: Foobar does not natively support v" +
        "olume up/down keys. Please bind these manually if desiring to use the Volume opt" +
        "ion with Foobar\r\n");
			this.radioButtonFoobar.UseVisualStyleBackColor = true;
			this.radioButtonFoobar.CheckedChanged += new System.EventHandler(this.radioButtonFoobar_CheckedChanged);
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
			this.groupBox3.Controls.Add(this.checkBox7);
			this.groupBox3.Controls.Add(this.checkBox1);
			this.groupBox3.Controls.Add(this.radioButtonVolume);
			this.groupBox3.Controls.Add(this.radioButtonPause);
			this.groupBox3.Controls.Add(this.radioButtonMute);
			this.groupBox3.Location = new System.Drawing.Point(15, 148);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(263, 91);
			this.groupBox3.TabIndex = 9;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Action";
			this.toolTip1.SetToolTip(this.groupBox3, resources.GetString("groupBox3.ToolTip"));
			// 
			// checkBox7
			// 
			this.checkBox7.AutoSize = true;
			this.checkBox7.Enabled = false;
			this.checkBox7.Location = new System.Drawing.Point(128, 66);
			this.checkBox7.Name = "checkBox7";
			this.checkBox7.Size = new System.Drawing.Size(126, 17);
			this.checkBox7.TabIndex = 8;
			this.checkBox7.Text = "Adv: Ignore Modifiers";
			this.toolTip1.SetToolTip(this.checkBox7, resources.GetString("checkBox7.ToolTip"));
			this.checkBox7.UseVisualStyleBackColor = true;
			this.checkBox7.CheckedChanged += new System.EventHandler(this.checkBox7_CheckedChanged);
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Enabled = false;
			this.checkBox1.Location = new System.Drawing.Point(128, 42);
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
			this.radioButtonVolume.Location = new System.Drawing.Point(6, 42);
			this.radioButtonVolume.Name = "radioButtonVolume";
			this.radioButtonVolume.Size = new System.Drawing.Size(106, 17);
			this.radioButtonVolume.TabIndex = 7;
			this.radioButtonVolume.Text = "Volume Keypress";
			this.toolTip1.SetToolTip(this.radioButtonVolume, resources.GetString("radioButtonVolume.ToolTip"));
			this.radioButtonVolume.UseVisualStyleBackColor = true;
			this.radioButtonVolume.CheckedChanged += new System.EventHandler(this.radioButtonVolume_CheckedChanged);
			this.radioButtonVolume.EnabledChanged += new System.EventHandler(this.radioButtonVolume_EnabledChanged);
			// 
			// radioButtonPause
			// 
			this.radioButtonPause.AutoSize = true;
			this.radioButtonPause.Enabled = false;
			this.radioButtonPause.Location = new System.Drawing.Point(6, 65);
			this.radioButtonPause.Name = "radioButtonPause";
			this.radioButtonPause.Size = new System.Drawing.Size(101, 17);
			this.radioButtonPause.TabIndex = 6;
			this.radioButtonPause.Text = "Pause Keypress";
			this.toolTip1.SetToolTip(this.radioButtonPause, resources.GetString("radioButtonPause.ToolTip"));
			this.radioButtonPause.UseVisualStyleBackColor = true;
			this.radioButtonPause.CheckedChanged += new System.EventHandler(this.radioButtonPause_CheckedChanged);
			// 
			// radioButtonMute
			// 
			this.radioButtonMute.AutoSize = true;
			this.radioButtonMute.Enabled = false;
			this.radioButtonMute.Location = new System.Drawing.Point(6, 19);
			this.radioButtonMute.Name = "radioButtonMute";
			this.radioButtonMute.Size = new System.Drawing.Size(99, 17);
			this.radioButtonMute.TabIndex = 5;
			this.radioButtonMute.Text = "Mute Command";
			this.toolTip1.SetToolTip(this.radioButtonMute, resources.GetString("radioButtonMute.ToolTip"));
			this.radioButtonMute.UseVisualStyleBackColor = true;
			this.radioButtonMute.CheckedChanged += new System.EventHandler(this.radioButtonMute_CheckedChanged);
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.checkBoxF);
			this.groupBox4.Controls.Add(this.checkBoxE);
			this.groupBox4.Controls.Add(this.checkBoxD);
			this.groupBox4.Controls.Add(this.checkBoxC);
			this.groupBox4.Controls.Add(this.checkBoxB);
			this.groupBox4.Controls.Add(this.checkBoxA);
			this.groupBox4.Location = new System.Drawing.Point(15, 245);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(261, 65);
			this.groupBox4.TabIndex = 10;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "When";
			this.toolTip1.SetToolTip(this.groupBox4, resources.GetString("groupBox4.ToolTip"));
			// 
			// checkBoxF
			// 
			this.checkBoxF.AutoSize = true;
			this.checkBoxF.Enabled = false;
			this.checkBoxF.Location = new System.Drawing.Point(99, 42);
			this.checkBoxF.Name = "checkBoxF";
			this.checkBoxF.Size = new System.Drawing.Size(78, 17);
			this.checkBoxF.TabIndex = 13;
			this.checkBoxF.Text = "Announcer";
			this.toolTip1.SetToolTip(this.checkBoxF, resources.GetString("checkBoxF.ToolTip"));
			this.checkBoxF.UseVisualStyleBackColor = true;
			this.checkBoxF.CheckedChanged += new System.EventHandler(this.checkBoxF_CheckedChanged);
			// 
			// checkBoxE
			// 
			this.checkBoxE.AutoSize = true;
			this.checkBoxE.Enabled = false;
			this.checkBoxE.Location = new System.Drawing.Point(192, 19);
			this.checkBoxE.Name = "checkBoxE";
			this.checkBoxE.Size = new System.Drawing.Size(68, 17);
			this.checkBoxE.TabIndex = 12;
			this.checkBoxE.Text = "Kaufman";
			this.toolTip1.SetToolTip(this.checkBoxE, resources.GetString("checkBoxE.ToolTip"));
			this.checkBoxE.UseVisualStyleBackColor = true;
			this.checkBoxE.CheckedChanged += new System.EventHandler(this.checkBoxE_CheckedChanged);
			// 
			// checkBoxD
			// 
			this.checkBoxD.AutoSize = true;
			this.checkBoxD.Enabled = false;
			this.checkBoxD.Location = new System.Drawing.Point(192, 42);
			this.checkBoxD.Name = "checkBoxD";
			this.checkBoxD.Size = new System.Drawing.Size(53, 17);
			this.checkBoxD.TabIndex = 11;
			this.checkBoxD.Text = "Menu";
			this.toolTip1.SetToolTip(this.checkBoxD, resources.GetString("checkBoxD.ToolTip"));
			this.checkBoxD.UseVisualStyleBackColor = true;
			this.checkBoxD.CheckedChanged += new System.EventHandler(this.checkBoxD_CheckedChanged);
			// 
			// checkBoxC
			// 
			this.checkBoxC.AutoSize = true;
			this.checkBoxC.Enabled = false;
			this.checkBoxC.Location = new System.Drawing.Point(99, 19);
			this.checkBoxC.Name = "checkBoxC";
			this.checkBoxC.Size = new System.Drawing.Size(63, 17);
			this.checkBoxC.TabIndex = 10;
			this.checkBoxC.Text = "Interiors";
			this.toolTip1.SetToolTip(this.checkBoxC, resources.GetString("checkBoxC.ToolTip"));
			this.checkBoxC.UseVisualStyleBackColor = true;
			this.checkBoxC.CheckedChanged += new System.EventHandler(this.checkBoxC_CheckedChanged);
			// 
			// checkBoxB
			// 
			this.checkBoxB.AutoSize = true;
			this.checkBoxB.Enabled = false;
			this.checkBoxB.Location = new System.Drawing.Point(6, 42);
			this.checkBoxB.Name = "checkBoxB";
			this.checkBoxB.Size = new System.Drawing.Size(54, 17);
			this.checkBoxB.TabIndex = 9;
			this.checkBoxB.Text = "Radio";
			this.toolTip1.SetToolTip(this.checkBoxB, resources.GetString("checkBoxB.ToolTip"));
			this.checkBoxB.UseVisualStyleBackColor = true;
			this.checkBoxB.CheckedChanged += new System.EventHandler(this.checkBoxB_CheckedChanged);
			// 
			// checkBoxA
			// 
			this.checkBoxA.AutoSize = true;
			this.checkBoxA.Enabled = false;
			this.checkBoxA.Location = new System.Drawing.Point(6, 19);
			this.checkBoxA.Name = "checkBoxA";
			this.checkBoxA.Size = new System.Drawing.Size(79, 17);
			this.checkBoxA.TabIndex = 8;
			this.checkBoxA.Text = "Emergency";
			this.toolTip1.SetToolTip(this.checkBoxA, resources.GetString("checkBoxA.ToolTip"));
			this.checkBoxA.UseVisualStyleBackColor = true;
			this.checkBoxA.CheckedChanged += new System.EventHandler(this.checkBoxA_CheckedChanged);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(288, 332);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.labelVolume);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Name = "Form1";
			this.Text = "GTA Radio External";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
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
		private System.Windows.Forms.Label labelVolume;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.RadioButton radioButtonPause;
		private System.Windows.Forms.RadioButton radioButtonMute;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.RadioButton radioButtonVolume;
		private System.Windows.Forms.RadioButton radioButtonOther;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.CheckBox checkBoxC;
		private System.Windows.Forms.CheckBox checkBoxB;
		private System.Windows.Forms.CheckBox checkBoxA;
		private System.Windows.Forms.CheckBox checkBoxD;
		private System.Windows.Forms.CheckBox checkBoxE;
		private System.Windows.Forms.CheckBox checkBoxF;
		private System.Windows.Forms.CheckBox checkBox7;
	}
}

