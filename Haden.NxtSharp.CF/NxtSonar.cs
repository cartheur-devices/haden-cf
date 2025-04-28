using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Haden.NxtSharp {
	public partial class NxtSonar : NxtSensor {
		public NxtSonar() {
			InitializeComponent();
		}

		public NxtSonar(IContainer container) {
			container.Add(this);

			InitializeComponent();
		}


		/// <summary>
		/// Use the boolean mode
		/// </summary>
		protected override NxtSensorMode Mode {
			get {
				return NxtSensorMode.Raw;
			}
		}

		/// <summary>
		/// Type of this sensor
		/// </summary>
		protected override NxtSensorType Type {
			get {
				return NxtSensorType.LowSpeed_9V;
			}
		}

		/// <summary>
		/// The raw value of this sensor reading
		/// </summary>
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

			//int status = Brick.Comm.LSGetStatus(Port);
			// Read garbage
			if(Brick.Comm.LSGetStatus(Port) > 0) {
				Brick.Comm.LSRead(Port);
			}
			// Init continuous mode
			Brick.Comm.I2CSetByte(Port, (byte)NxtSonarRegister.Mode, 0x02);

		}

		/// <summary>
		/// Reads information from the sonar sensor
		/// </summary>
		public override void Poll() {
			if(Brick != null) {
				int previous = RawValue;
				_rawValue = readSonar();
				OnPolled();
				if(previous != RawValue) {
					OnValueChanged();
				}
			}
		}

		/// <summary>
		/// reads a value from the sonar
		/// </summary>
		/// <returns></returns>
		private int readSonar() {
			return Brick.Comm.I2CGetByte(Port, (byte)NxtSonarRegister.MeasurementByte0);
		}
	}


	public enum NxtSonarRegister {
		MeasurementUnits = 0x14,
		PollInterval = 0x40,
		Mode = 0x41,
		MeasurementByte0 = 0x42,
		MeasurementByte1 = 0x43,
		MeasurementByte2 = 0x44,
		MeasurementByte3 = 0x45,
		MeasurementByte4 = 0x46,
		MeasurementByte5 = 0x47,
		MeasurementByte6 = 0x48
	}
}
