using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Haden.NxtSharp {
	public partial class NxtSoundSensor : NxtSensor {
		public NxtSoundSensor() {
			InitializeComponent();
		}

		public NxtSoundSensor(IContainer container) {
			container.Add(this);

			InitializeComponent();
		}

		[Category("Lego NXT"), Description("Should the sensor compensate for the sensitivity of the human ear?")]
		/// <summary>
		/// Should the sensor adjust for the sensitivity of the human ear?
		/// </summary>
		public bool AdjustForHumanEar {
			get {
				return _adjustForHumanEar;
			}
			set {
				_adjustForHumanEar = value;
			}
		}
		bool _adjustForHumanEar = true;



		/// <summary>
		/// Use the boolean mode
		/// </summary>
		protected override NxtSensorMode Mode {
			get {
				return NxtSensorMode.Percentage;
			}
		}

		/// <summary>
		/// Type of this sensor
		/// </summary>
		protected override NxtSensorType Type {
			get {
				if(AdjustForHumanEar) {
					return NxtSensorType.SoundDBA;
				} else {
					return NxtSensorType.SoundDB;
				}
			}
		}

		[Browsable(false)]
		/// <summary>
		/// returns the value in %
		/// </summary>
		public int Value {
			get {
				return (int)LastResult.ScaledValue;
			}
		}

		protected override bool IsSensorReadingDifferent(NxtGetInputValues previousValue, NxtGetInputValues newValue) {
			return previousValue.ScaledValue != newValue.ScaledValue;
		}

	}
}
