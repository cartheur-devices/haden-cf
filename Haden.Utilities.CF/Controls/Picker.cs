using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Haden.Utilities.CF {
	public partial class Picker : UserControl {
		public Picker() {
			InitializeComponent();
		}

		public bool AllowNull {
			get {
				return _allowNull;
			}
			set {
				_allowNull = value;
				updateButtons();
			}
		}
		bool _allowNull = true;

		private void updateButtons() {
			btnNone.Visible = AllowNull;
		}

		public object Value {
			get {
				return _value;
			}
			set {
				if(_value != value) {
					_value = value;
					if(value == null) {
						setText("-");
					} else {
						setText(value.ToString());
					}
					if(ValueChanged != null) {
						ValueChanged(this, value);
					}
				}
			}
		}
		object _value;

		void setText(string s) {
			label1.Text = s;
			toolTip1.SetToolTip(label1, s);
		}

		private void btnNone_Click(object sender, EventArgs e) {
			Value = null;
		}

		private void button1_Click(object sender, EventArgs e) {
			Value = GetObject();
		}

		protected virtual object GetObject() {
			return null;
		}

		public event PickerValueChanged ValueChanged;

	}

	public delegate void PickerValueChanged(Picker picker, object newValue);
}
