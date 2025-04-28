using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Haden.Utilities.CF {
	public partial class ListPickForm : Form {
		public ListPickForm() {
			InitializeComponent();
		}

		object value;
		List<object> possibleValues = new List<object>();

		public void AddPossibleValue(object value) {
			possibleValues.Add(value);
		}

		public object ShowGetObject() {
			listBox.Items.Clear();
			foreach(object o in possibleValues) {
				listBox.Items.Add(o);
			}
			ShowDialog();
			return value;
		}

		private void btnOK_Click(object sender, EventArgs e) {
			value = listBox.SelectedItem;
			Close();
		}

		private void btnCancel_Click(object sender, EventArgs e) {
			value = null;
			Close();
		}
	}
}