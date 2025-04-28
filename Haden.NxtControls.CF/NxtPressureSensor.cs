using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Haden.NxtSharp {
	/// <summary>
	/// The Switch sensor represents the button like switch which
	/// is normally attached to port 1.
	/// </summary>
	public partial class NxtPressureSensor : NxtSensor {
		public NxtPressureSensor() {
			InitializeComponent();
		}

		public NxtPressureSensor(IContainer container) {
			container.Add(this);

			InitializeComponent();
		}

		/// <summary>
		/// Use the boolean mode
		/// </summary>
		protected override NxtSensorMode Mode {
			get {
				return NxtSensorMode.Boolean;
			}
		}

		/// <summary>
		/// Type of this sensor
		/// </summary>
		protected override NxtSensorType Type {
			get {
				return NxtSensorType.Switch;
			}
		}

		/// <summary>
		/// Was the button pressed at the last poll?
		/// </summary>
		public bool IsPressed {
			get {
				return LastResult.ScaledValue == 1;
			}
		}
	}
}
