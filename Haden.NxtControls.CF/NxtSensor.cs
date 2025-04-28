using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Haden.Utilities.CF;

namespace Haden.NxtSharp {
	public partial class NxtSensor : Component {
		public NxtSensor() {
			InitializeComponent();
		}

		public NxtSensor(IContainer container) {
			container.Add(this);

			InitializeComponent();
		}

		#region Sensor setup

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

		//[Category("Lego NXT"), Description("The port this sensor is connected to.")]
		/// <summary>
		/// The port this sensor is connected to
		/// </summary>
		public NxtSensorPort Port {
			get {
				if(Brick != null) {
					foreach(NxtSensorPort p in Brick.SensorPorts()) {
						if(Brick.GetSensor(p) == this) {
							return p;
						}
					}
				}
				return NxtSensorPort.None;
			}
			set {
				if(Brick != null) {
					Brick.AttachSensor(value, this);
				}
			}
		}


		#endregion

		#region Sensor polling

		//[Category("Lego NXT"), Description("When AutoPoll is set, the NxtBrick will continuously poll the state of the sensor. How often this is done is controlled by the AutoPollDelay property.\r\n\r\nNOTE: The AutoPoll property must also be set on the NxtBrick Component")]
		/// <summary>
		/// When AutoPoll is set, the NxtBrick will continuously poll the state of
		/// the sensor. How often this is done is controlled by the 
		/// AutoPollDelay property.
		/// 
		/// NOTE: The AutoPoll property must also be set on the NxtBrick Component
		/// </summary>
		public bool AutoPoll {
			get {
				return _autoPoll;
			}
			set {
				_autoPoll = value;
			}
		}
		bool _autoPoll = false;

		//[Category("Lego NXT"), Description("Delay between sensor polls. 0 = as often as possible\r\n\r\nNOTE: The AutoPoll property must be set for this property to have any effect")]
		/// <summary>
		/// Delay between sensor polls. 0 = as often as possible
		/// NOTE: The AutoPoll property must be set for this property to have any effect
		/// </summary>
		public int AutoPollDelay {
			get {
				return _autoPollDelay;
			}
			set {
				_autoPollDelay = value;
			}
		}
		int _autoPollDelay = 100;


		//[Browsable(false)]
		/// <summary>
		/// The last read raw value
		/// </summary>
		public virtual int RawValue {
			get {
				return LastResult.RawAD;
			}
		}

		//[Browsable(false)]
		/// <summary>
		/// The last InputValues result
		/// </summary>
		public NxtGetInputValues LastResult {
			get {
				return _lastResult;
			}
			private set {
				_lastResult = value;
			}
		}
		NxtGetInputValues _lastResult;


		//[Browsable(false)]
		/// <summary>
		/// The last time the sensor was polled (using Haden.Utilities.MilliSeconds())
		/// </summary>
		public long LastPollTimestamp {
			get {
				return _lastPollTimestamp;
			}
			private set {
				_lastPollTimestamp = value;
			}
		}
		long _lastPollTimestamp = 0;


		/// <summary>
		/// Polls the sensor value
		/// </summary>
		public virtual void Poll() {
			if(Brick != null) {
				LastPollTimestamp = Util.MilliSeconds();
				NxtGetInputValues previous = LastResult;
				LastResult = Brick.Comm.GetInputValues(Port);
				OnPolled();
				if(IsSensorReadingDifferent(previous, LastResult)) {
					OnValueChanged();
				}
			}			
		}

		/// <summary>
		/// Initializes the sensor
		/// </summary>
		public virtual void InitSensor() {
			if(Brick != null) {
				Brick.Comm.SetInputMode(Port, Type, Mode);
			}
		}

		//[Browsable(false)]
		/// <summary>
		/// Override this to set the sensor type
		/// </summary>
		protected virtual NxtSensorType Type {
			get {
				return NxtSensorType.None;
			}
		}

		//[Browsable(false)]
		/// <summary>
		/// Override this to set the sensor mode
		/// </summary>
		protected virtual NxtSensorMode Mode {
			get {
				return NxtSensorMode.Raw;
			}
		}

		/// <summary>
		/// Override this to define sensor thresholds, etc...
		/// </summary>
		/// <param name="previousValue">previous value</param>
		/// <param name="newValue">new value</param>
		/// <returns></returns>
		protected virtual bool IsSensorReadingDifferent(NxtGetInputValues previousValue, NxtGetInputValues newValue) {
			if(previousValue.RawAD != newValue.RawAD) {
				return true;
			}
			if(previousValue.CalibratedValue != newValue.CalibratedValue) {
				return true;
			}
			if(previousValue.NormalizedAD != newValue.NormalizedAD) {
				return true;
			}
			if(previousValue.ScaledValue != newValue.ScaledValue) {
				return true;
			}
			if(previousValue.CalibratedValue != newValue.CalibratedValue) {
				return true;
			}
			if(previousValue.Mode != newValue.Mode) {
				return true;
			}
			if(previousValue.Type != newValue.Type) {
				return true;
			}
			return false;
		}


		/// <summary>
		/// Raises the Polled event
		/// </summary>
		protected virtual void OnPolled() {
			if(Polled != null) {
				Polled(this);
			}
		}

		/// <summary>
		/// Is called whenever a poll has occurred
		/// </summary>
		public event SensorEvent Polled;

		/// <summary>
		/// Raises the ValueChanged event
		/// </summary>
		protected virtual void OnValueChanged() {
			if(ValueChanged != null) {
				ValueChanged(this);
			}
		}
		
		/// <summary>
		/// Is called whenever the polled value has changed
		/// </summary>
		public event SensorEvent ValueChanged;



		#endregion

	}

	public delegate void SensorEvent(NxtSensor sensor);
}
