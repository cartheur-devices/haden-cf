using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Haden.NxtSharp {
	public partial class NxtTankDrive : Component {
		public NxtTankDrive() {
			InitializeComponent();
		}

		public NxtTankDrive(IContainer container) {
			container.Add(this);

			InitializeComponent();
		}

		//[Category("Lego NXT"), Description("The NxtBrick this sensor is connected to.")]
		/// <summary>
		/// The NxtBrick this sensor is connected to
		/// </summary>
		public NxtBrick Brick {
			get {
				return _nxtBrick;
			}
			set {
				_nxtBrick = value;
			}
		}
		NxtBrick _nxtBrick = null;

		//[Category("Lego NXT"), Description("The first motor of the Tank Drive")]
		/// <summary>
		/// The first motor of the Tank Drive
		/// </summary>
		public NxtMotor Motor1 {
			get {
				return _motor1;
			}
			set {
				_motor1 = value;
			}
		}
		NxtMotor _motor1 = null;


		//[Category("Lego NXT"), Description("The second motor of the Tank Drive")]
		/// <summary>
		/// The second motor of the Tank Drive
		/// </summary>
		public NxtMotor Motor2 {
			get {
				return _motor2;
			}
			set {
				_motor2 = value;
			}
		}
		NxtMotor _motor2 = null;


		#region Commands

		/// <summary>
		/// Moves the tank drive forward while synchronizing movement
		/// </summary>
		/// <param name="power">The motor power [0-100]</param>
		/// <param name="distance">Distance to move, use 0 for infinite</param>
		public void MoveForward(int power, int tachoLimit) {
			Move(Math.Abs(power), tachoLimit);
		}

		/// <summary>
		/// Moves the tank drive back while synchronizing movement
		/// </summary>
		/// <param name="power">The motor power [0-100]</param>
		/// <param name="distance">Distance to move, use 0 for infinite</param>
		public void MoveBack(int power, int tachoLimit) {
			Move(-Math.Abs(power), tachoLimit);
		}

		/// <summary>
		/// Moves the tank while synchronizing movement
		/// </summary>
		/// <param name="power"></param>
		/// <param name="tachoLimit"></param>
		public void Move(int power, int tachoLimit) {
			Reset(true);
			Motor1.SetOutputState(power, NxtMotorMode.Brake | NxtMotorMode.MotorOn | NxtMotorMode.Regulated, NxtMotorRegulationMode.MotorSynchronization, 0, NxtMotorRunState.Running, tachoLimit);
			Motor2.SetOutputState(power, NxtMotorMode.Brake | NxtMotorMode.MotorOn | NxtMotorMode.Regulated, NxtMotorRegulationMode.MotorSynchronization, 0, NxtMotorRunState.Running, tachoLimit);

		}

		/// <summary>
		/// Turns the Tank left on its place
		/// </summary>
		public void TurnLeft(int power, int tachoLimit) {
			Turn(power, tachoLimit, -100);
		}

		/// <summary>
		/// Turns the tank right on its place
		/// </summary>
		public void TurnRight(int power, int tachoLimit) {
			Turn(power, tachoLimit, 100);
		}

		/// <summary>
		/// Turns the tank in its place
		/// </summary>
		/// <param name="power"></param>
		/// <param name="tachoLimit"></param>
		/// <param name="?"></param>
		public void Turn(int power, int tachoLimit, int turnRate) {
			Reset(true);
			Motor1.SetOutputState(power, NxtMotorMode.Brake | NxtMotorMode.MotorOn | NxtMotorMode.Regulated, NxtMotorRegulationMode.MotorSynchronization, turnRate, NxtMotorRunState.Running, tachoLimit);
			Motor2.SetOutputState(power, NxtMotorMode.Brake | NxtMotorMode.MotorOn | NxtMotorMode.Regulated, NxtMotorRegulationMode.MotorSynchronization, turnRate, NxtMotorRunState.Running, tachoLimit);
		}

		/// <summary>
		/// Put the drive in coast mode (idle)
		/// </summary>
		public void Coast() {
			Motor1.Coast();
			Motor2.Coast();
		}

		/// <summary>
		/// Put the drive in brake mode
		/// </summary>
		public void Brake() {
			Motor1.Brake();
			Motor2.Brake();
		}

		/// <summary>
		/// Reset motor position counters
		/// </summary>
		public void Reset(bool relative) {
			Motor1.ResetPosition(relative);
			Motor2.ResetPosition(relative);
		}




		#endregion

	}
}
