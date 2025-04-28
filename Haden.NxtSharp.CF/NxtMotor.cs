using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Haden.NxtSharp {
	public partial class NxtMotor : Component {
		public NxtMotor() {
			InitializeComponent();
		}

		public NxtMotor(IContainer container) {
			container.Add(this);

			InitializeComponent();
		}

		#region Brick Setup

		[Category("Lego NXT"), Description("The NxtBrick this sensor is connected to.")]
		/// <summary>
		/// The NxtBrick this motor is connected to
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

		[Category("Lego NXT"), Description("The port this motor is connected to.")]
		/// <summary>
		/// The port this motor is connected to
		/// </summary>
		public NxtMotorPort Port {
			get {
				if(Brick != null) {
					foreach(NxtMotorPort p in Brick.MotorPorts()) {
						if(Brick.GetMotor(p) == this) {
							return p;
						}
					}
				}
				return NxtMotorPort.None;
			}
			set {
				if(Brick != null) {
					Brick.AttachMotor(value, this);
				}
			}
		}

		[Category("Lego NXT"), Description("Flip motor direction?")]
		/// <summary>
		/// Flip motor direction?
		/// </summary>
		public bool Flip {
			get {
				return _flip;
			}
			set {
				_flip = value;
			}
		}
		private bool _flip = false;

		public int FlipFactor {
			get {
				return Flip ? -1 : 1;
			}
		}

		#endregion



		#region Commands

		/// <summary>
		/// Turns the motor
		/// </summary>
		/// <param name="speed">Speed [-100,100], positive values being clockwise, negative values counterclockwise</param>
		/// <param name="degrees">+ of degrees to turn, 0 being infinite. 
		/// NOTE: After the command is finished, the motor will coast about 10-30 degrees
		/// so don't rely on this command for precise positioning
		/// </param>
		public void Turn(int speed, int degrees) {
			if(Brick == null) {
				throw new InvalidOperationException("This motor must be connected to a brick first");
			} else {
				SetOutputState(
					speed,
					NxtMotorMode.MotorOn | NxtMotorMode.Regulated,
					NxtMotorRegulationMode.MotorSpeed,
					0,
					NxtMotorRunState.Running,
					degrees
				);
			}
		}

		/// <summary>
		/// Puts the motor into coast mode
		/// </summary>
		public void Coast() {
			if(Brick == null) {
				throw new InvalidOperationException("This motor must be connected to a brick first");
			} else {
				SetOutputState(
					0,
					NxtMotorMode.None,
					NxtMotorRegulationMode.Idle,
					0,
					NxtMotorRunState.Idle,
					0
				);
			}
		}

		/// <summary>
		/// Puts the motor into brake mode (the motor will move to 
		/// retain current position)
		/// </summary>
		public void Brake() {
			if(Brick == null) {
				throw new InvalidOperationException("This motor must be connected to a brick first");
			} else {
				SetOutputState(
					0,
					NxtMotorMode.MotorOn | NxtMotorMode.Brake | NxtMotorMode.Regulated,
					NxtMotorRegulationMode.MotorSpeed,
					0,
					NxtMotorRunState.Running,
					0
				);
			}
		}

		/// <summary>
		/// Resets the motor position
		/// </summary>
		public void ResetPosition(bool relative) {
			if(Brick == null) {
				throw new InvalidOperationException("This motor must be connected to a brick first");
			} else {
				Brick.Comm.ResetMotorPosition(Port, relative);
			}
		}

		/// <summary>
		/// Sets the output state of this motor
		/// </summary>
		/// <param name="power"></param>
		/// <param name="mode"></param>
		/// <param name="regulationMode"></param>
		/// <param name="turnRate"></param>
		/// <param name="runState"></param>
		/// <param name="tachoLimit"></param>
		public void SetOutputState(int power, NxtMotorMode mode, NxtMotorRegulationMode regulationMode, int turnRate, NxtMotorRunState runState, int tachoLimit) {
			Brick.Comm.SetOutputState(
				Port, 
				(sbyte)(power * FlipFactor), 
				mode, 
				regulationMode, 
				(sbyte)turnRate, 
				runState, 
				(uint)tachoLimit
			);
		}


		#endregion
	}
}
