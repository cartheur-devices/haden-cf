using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Haden.NxtSharp {

	/// <summary>
	/// This class enables using the HiTechnic Compass sensor.
	/// 
	/// Many thanks to the HiTechnic guys for supplying a sample
	/// of the sensor!
	/// </summary>
	public partial class NxtCompassSensor : NxtSensor {
		public NxtCompassSensor() {
			InitializeComponent();
		}

		public NxtCompassSensor(IContainer container) {
			container.Add(this);

			InitializeComponent();
		}


		/// <summary>
		/// Should the sensor use double precision? This is more precies but also slower.
		/// </summary>
		//[Category("Compass sensor settings"), Description("Should the compass sensor use double precision? This is more precies but also slower.")]
		public bool UseDoublePrecision {
			get {
				return _useDoublePrecision;
			}
			set {
				_useDoublePrecision = value;
			}
		}
		bool _useDoublePrecision = true;


		/// <summary>
		/// Use the boolean mode
		/// </summary>
		//[Browsable(false)]
		protected override NxtSensorMode Mode {
			get {
				return NxtSensorMode.Raw;
			}
		}

		/// <summary>
		/// Type of this sensor
		/// </summary>
		//[Browsable(false)]
		protected override NxtSensorType Type {
			get {
				return NxtSensorType.LowSpeed_9V; // Use I2C communication
			}
		}

		/// <summary>
		/// The raw value of this sensor reading
		/// </summary>
		//[Browsable(false)]
		public override int RawValue {
			get {
				return _rawValue;
			}
		}
		int _rawValue = 0;

		/// <summary>
		/// Initializes the sonar sensor
		/// </summary>
		public override void InitSensor() {
			base.InitSensor();

			// Read garbage
			Brick.Comm.LSRead(Port);
		}

		/// <summary>
		/// Reads information from the sonar sensor
		/// </summary>
		public override void Poll() {
			if(Brick != null) {
				int previous = RawValue;
				_rawValue = readCompass();
				OnPolled();
				if(previous != RawValue) {
					OnValueChanged();
				}
			}
		}

		/// <summary>
		/// reads a value from the compass sensor
		/// </summary>
		/// <returns></returns>
		private int readCompass() {
			int result = 2 * Brick.Comm.I2CGetByte(Port, (byte)NxtCompassRegister.HeadingTwoDegrees);
			if(UseDoublePrecision) {
				result += Brick.Comm.I2CGetByte(Port, (byte)NxtCompassRegister.HeadingAdder);
			}
			return result;
		}
	}


	public enum NxtCompassRegister {
		SensorVersion = 0x00,
		Manufacturer = 0x08,
		SensorType = 0x10,
		ModeControl = 0x41,
		HeadingTwoDegrees = 0x42,
		HeadingAdder = 0x43,
		HeadingWord = 0x44,
	}
}
