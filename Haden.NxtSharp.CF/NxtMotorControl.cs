using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Haden.NxtSharp {
	public partial class NxtMotorControl : UserControl {
		public NxtMotorControl() {
			InitializeComponent();
			updateView();
		}

		#region Settings

		/// <summary>
		/// Should the motor put in brake mode when stopped?
		/// </summary>
		[Category("MotorControl"), Description("Should the motor put in brake mode when stopped?")]
		public bool Brake {
			get {
				return _brake;
			}
			set {
				_brake = value;
			}
		}
		bool _brake = false;

		/// <summary>
		/// power of the motor [0-100]
		/// </summary>
		[Category("MotorControl"), Description("power of the motor [0-100]")]
		public int Power {
			get {
				return _power;
			}
			set {
				if(value < 0 || value > 100) {
					throw new ArgumentException("power should be between 0 and 100");
				} else {
					_power = value;
				}
			}
		}
		int _power = 75;

		/// <summary>
		/// The motor this control is assigned to
		/// </summary>
		[Category("MotorControl"), Description("The motor this control is assigned to")]
		public NxtMotor Motor {
			get {
				return _motor;
			}
			set {
				_motor = value;
			}
		}
		NxtMotor _motor = null;

		/// <summary>
		/// Orientation of the buttons.
		/// 
		/// Vertical: Up/Down
		/// Horizontal: Left/Right
		/// </summary>
		[Category("MotorControl"), Description("Orientation of the buttons.\r\nVertical: Up/Down\r\nHorizontal: Left/Right")]
		public NxtMotorControlOrientation Orientation {
			get {
				return _orientation;
			}
			set {
				_orientation = value;
				updateView();
			}
		}
		NxtMotorControlOrientation _orientation = NxtMotorControlOrientation.Vertical;

		/// <summary>
		/// Distance between the buttons
		/// </summary>
		[Category("MotorControl"), Description("Distance between the buttons.")]
		public int ButtonDistance {
			get {
				return _buttonDistance;
			}
			set {
				_buttonDistance = value;
				updateView();
			}
		}
		int _buttonDistance = 4;

		/// <summary>
		/// The up/left key
		/// </summary>
		[Category("MotorControl"), Description("The up/left key")]
		public Keys Key1 {
			get {
				return _key1;
			}
			set {
				_key1 = value;
			}
		}
		Keys _key1 = Keys.None;


		/// <summary>
		/// The down/right key
		/// </summary>
		[Category("MotorControl"), Description("The down/right key")]
		public Keys Key2 {
			get {
				return _key2;
			}
			set {
				_key2 = value;
			}
		}
		Keys _key2 = Keys.None;


		#endregion

		#region Layout

		protected override void OnMarginChanged(EventArgs e) {
			base.OnMarginChanged(e);
			updateView();
		}

		protected override void OnResize(EventArgs e) {
			base.OnResize(e);
			updateView();
		}

		private void updateView() {
			int hMargin = Margin.Left + Margin.Right;
			int vMargin = Margin.Top + Margin.Bottom;

			if(Orientation == NxtMotorControlOrientation.Vertical) {
				// Set minimum size
				MinimumSize = new Size(36 + hMargin, 48 + vMargin + ButtonDistance);

				// Orient buttons
				int width = Width - hMargin;
				int height = (Height - (vMargin + ButtonDistance)) / 2;

				btnTurnClockwise.Width = width;
				btnTurnClockwise.Height = height;
				btnTurnClockwise.Left = Margin.Left;
				btnTurnClockwise.Top = Margin.Top;
				btnTurnClockwise.Text = "5"; // Up arrow in marlet font

				btnTurnCounterClockwise.Width = width;
				btnTurnCounterClockwise.Height = height;
				btnTurnCounterClockwise.Left = Margin.Left;
				btnTurnCounterClockwise.Top = Height - (Margin.Bottom + height);
				btnTurnCounterClockwise.Text = "6"; // Down arrow in marlet font				
			} else {
				// Set minimum size
				MinimumSize = new Size(48 + hMargin + ButtonDistance, 36 + vMargin);

				int width = (Width - (hMargin + ButtonDistance)) / 2;
				int height = Height - vMargin;

				// Orient buttons
				btnTurnClockwise.Width = width;
				btnTurnClockwise.Height = height;
				btnTurnClockwise.Left = Margin.Left;
				btnTurnClockwise.Top = Margin.Top;
				btnTurnClockwise.Text = "3"; // Left arrow in marlet font

				btnTurnCounterClockwise.Width = width;
				btnTurnCounterClockwise.Height = height;
				btnTurnCounterClockwise.Left = Width - (Margin.Right + width);
				btnTurnCounterClockwise.Top = Margin.Top;
				btnTurnCounterClockwise.Text = "4"; // Right arrow in marlet font				
			}
		}

		#endregion

		#region Event handlers

		protected override void OnLayout(LayoutEventArgs e) {
			base.OnLayout(e);
			Form f = FindForm();
			if(f != null) {
				f.KeyDown += new KeyEventHandler(f_KeyDown);
				f.KeyUp += new KeyEventHandler(f_KeyUp);
			}
		}

		void f_KeyDown(object sender, KeyEventArgs e) {
			handleKeyDown(e.KeyCode);
		}

		void f_KeyUp(object sender, KeyEventArgs e) {
			handleKeyUp(e.KeyCode);
		}


		private void handleKeyDown(Keys key) {
			if(key == Key1 || key == Key2) {
				if(key == Key1) {
					TurnCW();
				} else if(key == Key2) {
					TurnCCW();
				}
			}
		}


		private void handleKeyUp(Keys key) {
			if(key == Key1 || key == Key2) {
				Stop();
			}
		}

		private void btnTurnClockwise_MouseDown(object sender, MouseEventArgs e) {
			TurnCW();
		}

		private void btnTurnClockwise_MouseUp(object sender, MouseEventArgs e) {
			Stop();
		}

		private void btnTurnCounterClockwise_MouseDown(object sender, MouseEventArgs e) {
			TurnCCW();
		}

		private void btnTurnCounterClockwise_MouseUp(object sender, MouseEventArgs e) {
			Stop();
		}

		#endregion

		#region Motor functionality

		[Browsable(false)]
		public bool IsRunning {
			get {
				return _isRunning;
			}
			private set {
				_isRunning = value;
			}
		}
		bool _isRunning = false;

		private void TurnCW() {
			if(Motor == null) {
				MessageBox.Show("Can't turn - there is no motor connected to this control.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			} else {
				if(!IsRunning) {
					Motor.Turn(-Power, 0);
					IsRunning = true;
				}
			}
		}

		private void TurnCCW() {
			if(Motor == null) {
				MessageBox.Show("Can't turn - there is no motor connected to this control.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			} else {
				if(!IsRunning) {
					Motor.Turn(Power, 0);
					IsRunning = true;
				}
			}
		}

		private void Stop() {
			if(Motor != null && IsRunning) {
				if(Brake) {
					Motor.Brake();
				} else {
					Motor.Coast();
				}
				IsRunning = false;
			}
		}

		#endregion
	}

	public enum NxtMotorControlOrientation {
		Horizontal,
		Vertical
	}

	public enum NxtMotorControlStopMode {
		/// <summary>
		/// Coast when the motor is stopped
		/// </summary>
		Coast,
		/// <summary>
		/// Put the motor in brake mode when stopped
		/// </summary>
		Brake
	}
}
