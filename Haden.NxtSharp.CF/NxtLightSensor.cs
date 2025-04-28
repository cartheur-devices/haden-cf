using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Haden.NxtSharp {
	public partial class NxtLightSensor : NxtSensor {
		public NxtLightSensor() {
			InitializeComponent();
		}

		public NxtLightSensor(IContainer container) {
			container.Add(this);

			InitializeComponent();
		}

		[Category("Lego NXT"), Description("Should the light sensor generate its own light?")]
		/// <summary>
		/// Should the light sensor generate its own light?
		/// </summary>
		public bool Active {
			get {
				return _active;
			}
			set {
				_active = value;
			}
		}
		bool _active = false;



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
				if(Active) {
					return NxtSensorType.LightActive;
				} else {
					return NxtSensorType.LightInactive;
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
