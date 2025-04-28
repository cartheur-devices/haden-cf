namespace Haden.NxtRemote.CF
{
    partial class RemoteForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.connectMenu = new System.Windows.Forms.MenuItem();
            this.mnuSearch = new System.Windows.Forms.MenuItem();
            this.connectBrick = new System.Windows.Forms.MenuItem();
            this.disconnectBrick = new System.Windows.Forms.MenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblPressed = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.valSound = new System.Windows.Forms.ProgressBar();
            this.label5 = new System.Windows.Forms.Label();
            this.valLight = new System.Windows.Forms.ProgressBar();
            this.label6 = new System.Windows.Forms.Label();
            this.lblDistance = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cboDevices = new System.Windows.Forms.ComboBox();
            this.nxtMotorControlCenter = new Haden.NxtSharp.NxtMotorControl();
            this.nxtMotorControlRight = new Haden.NxtSharp.NxtMotorControl();
            this.nxtMotorControlLeft = new Haden.NxtSharp.NxtMotorControl();
            this.nxtBrick = new Haden.NxtSharp.NxtBrick(this.components);
            this.nxtMotorA = new Haden.NxtSharp.NxtMotor(this.components);
            this.nxtMotorB = new Haden.NxtSharp.NxtMotor(this.components);
            this.nxtMotorC = new Haden.NxtSharp.NxtMotor(this.components);
            this.nxtSoundSensor = new Haden.NxtSharp.NxtSoundSensor(this.components);
            this.nxtLightSensor = new Haden.NxtSharp.NxtLightSensor(this.components);
            this.nxtSonar = new Haden.NxtSharp.NxtSonar(this.components);
            this.nxtPressureSensor = new Haden.NxtSharp.NxtPressureSensor(this.components);
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.connectMenu);
            this.mainMenu1.MenuItems.Add(this.disconnectBrick);
            // 
            // connectMenu
            // 
            this.connectMenu.MenuItems.Add(this.mnuSearch);
            this.connectMenu.MenuItems.Add(this.connectBrick);
            this.connectMenu.Text = "Connect";
            this.connectMenu.Click += new System.EventHandler(this.connectBrick_Click);
            // 
            // mnuSearch
            // 
            this.mnuSearch.Text = "Search Bluetooth";
            this.mnuSearch.Click += new System.EventHandler(this.mnuSearch_Click);
            // 
            // connectBrick
            // 
            this.connectBrick.Text = "Connect Brick";
            this.connectBrick.Click += new System.EventHandler(this.connectBrick_Click);
            // 
            // disconnectBrick
            // 
            this.disconnectBrick.Text = "Disconnect Brick";
            this.disconnectBrick.Click += new System.EventHandler(this.disconnectBrick_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 20);
            this.label1.Text = "Motors:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 134);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 20);
            this.label2.Text = "Sensors:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(4, 158);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 20);
            this.label3.Text = "Pressure:";
            // 
            // lblPressed
            // 
            this.lblPressed.Location = new System.Drawing.Point(87, 158);
            this.lblPressed.Name = "lblPressed";
            this.lblPressed.Size = new System.Drawing.Size(40, 20);
            this.lblPressed.Text = "-";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 182);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 20);
            this.label4.Text = "Sound:";
            // 
            // valSound
            // 
            this.valSound.Location = new System.Drawing.Point(87, 182);
            this.valSound.Name = "valSound";
            this.valSound.Size = new System.Drawing.Size(143, 20);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(4, 206);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 20);
            this.label5.Text = "Light:";
            // 
            // valLight
            // 
            this.valLight.Location = new System.Drawing.Point(87, 206);
            this.valLight.Name = "valLight";
            this.valLight.Size = new System.Drawing.Size(143, 20);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(4, 229);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 20);
            this.label6.Text = "Sonar:";
            // 
            // lblDistance
            // 
            this.lblDistance.Location = new System.Drawing.Point(87, 229);
            this.lblDistance.Name = "lblDistance";
            this.lblDistance.Size = new System.Drawing.Size(143, 20);
            this.lblDistance.Text = "-";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(4, 248);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 20);
            this.label7.Text = "Status:";
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(87, 248);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(463, 20);
            this.lblStatus.Text = "-";
            // 
            // cboDevices
            // 
            this.cboDevices.Location = new System.Drawing.Point(143, 158);
            this.cboDevices.Name = "cboDevices";
            this.cboDevices.Size = new System.Drawing.Size(100, 22);
            this.cboDevices.TabIndex = 12;
            // 
            // nxtMotorControlCenter
            // 
            this.nxtMotorControlCenter.Location = new System.Drawing.Point(87, 23);
            this.nxtMotorControlCenter.Name = "nxtMotorControlCenter";
            this.nxtMotorControlCenter.Size = new System.Drawing.Size(64, 108);
            this.nxtMotorControlCenter.TabIndex = 2;
            // 
            // nxtMotorControlRight
            // 
            this.nxtMotorControlRight.Location = new System.Drawing.Point(157, 23);
            this.nxtMotorControlRight.Name = "nxtMotorControlRight";
            this.nxtMotorControlRight.Size = new System.Drawing.Size(73, 108);
            this.nxtMotorControlRight.TabIndex = 1;
            // 
            // nxtMotorControlLeft
            // 
            this.nxtMotorControlLeft.Location = new System.Drawing.Point(14, 23);
            this.nxtMotorControlLeft.Name = "nxtMotorControlLeft";
            this.nxtMotorControlLeft.Size = new System.Drawing.Size(67, 108);
            this.nxtMotorControlLeft.TabIndex = 0;
            // 
            // nxtBrick
            // 
            this.nxtBrick.COMPortName = "COM7";
            // 
            // RemoteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.cboDevices);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblDistance);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.valLight);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.valSound);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblPressed);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nxtMotorControlCenter);
            this.Controls.Add(this.nxtMotorControlRight);
            this.Controls.Add(this.nxtMotorControlLeft);
            this.Menu = this.mainMenu1;
            this.Name = "RemoteForm";
            this.Text = "Haden Communication Form";
            this.Load += new System.EventHandler(this.RemoteForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Haden.NxtSharp.NxtMotorControl nxtMotorControlLeft;
        private Haden.NxtSharp.NxtMotorControl nxtMotorControlRight;
        private Haden.NxtSharp.NxtMotorControl nxtMotorControlCenter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblPressed;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ProgressBar valSound;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ProgressBar valLight;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblDistance;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.MenuItem connectMenu;
        private System.Windows.Forms.MenuItem disconnectBrick;
        private Haden.NxtSharp.NxtBrick nxtBrick;
        private Haden.NxtSharp.NxtMotor nxtMotorA;
        private Haden.NxtSharp.NxtMotor nxtMotorB;
        private Haden.NxtSharp.NxtMotor nxtMotorC;
        private Haden.NxtSharp.NxtSoundSensor nxtSoundSensor;
        private Haden.NxtSharp.NxtLightSensor nxtLightSensor;
        private Haden.NxtSharp.NxtSonar nxtSonar;
        private Haden.NxtSharp.NxtPressureSensor nxtPressureSensor;
        private System.Windows.Forms.MenuItem mnuSearch;
        private System.Windows.Forms.MenuItem connectBrick;
        private System.Windows.Forms.ComboBox cboDevices;
    }
}

