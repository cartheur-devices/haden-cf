using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Haden.NxtSharp {
	public partial class NxtTankControl : UserControl {
		public NxtTankControl() {
			InitializeComponent();
		}

		#region Behaviour

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
		/// The tank drive this control is assigned to
		/// </summary>
		[Category("MotorControl"), Description("The tank drive this control is assigned to")]
		public NxtTankDrive TankDrive {
			get {
				return _tankDrive;
			}
			set {
				_tankDrive = value;
			}
		}
		NxtTankDrive _tankDrive = null;


		#endregion

		#region Event handlers

		private void btnForward_MouseDown(object sender, MouseEventArgs e) {
			if(TankDrive == null) {
				throw new InvalidOperationException("Can't drive forward - no NxtTankDrive assigned to this control");
			} else {
				TankDrive.MoveForward(Power, 0);
			}
		}

		private void btnForward_MouseUp(object sender, MouseEventArgs e) {
			stop();
		}

		private void btnBack_MouseDown(object sender, MouseEventArgs e) {
			if(TankDrive == null) {
				throw new InvalidOperationException("Can't drive back - no NxtTankDrive assigned to this control");
			} else {
				TankDrive.MoveBack(Power, 0);
			}
		}

		private void btnBack_MouseUp(object sender, MouseEventArgs e) {
			stop();
		}

		private void btnTurnLeft_MouseDown(object sender, MouseEventArgs e) {
			if(TankDrive == null) {
				throw new InvalidOperationException("Can't turn left - no NxtTankDrive assigned to this control");
			} else {
				TankDrive.TurnLeft(Power, 0);
			}
		}

		private void btnTurnLeft_MouseUp(object sender, MouseEventArgs e) {
			stop();
		}

		private void btnTurRight_MouseDown(object sender, MouseEventArgs e) {
			if(TankDrive == null) {
				throw new InvalidOperationException("Can't turn right - no NxtTankDrive assigned to this control");
			} else {
				TankDrive.TurnRight(Power, 0);
			}
		}

		private void btnTurRight_MouseUp(object sender, MouseEventArgs e) {
			stop();
		}

		private void stop() {
			if(TankDrive != null) {
				if(Brake) {
					TankDrive.Brake();
				} else {
					TankDrive.Coast();
				}
			}
		}

		#endregion

	}
}
