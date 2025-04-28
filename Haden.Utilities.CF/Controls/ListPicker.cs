using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Haden.Utilities.CF.Controls {
	public partial class ListPicker : Picker {
		public ListPicker() {
			InitializeComponent();
		}

		protected override object GetObject() {
			List<object> fields;
			OnQueryBuildList(out fields);
			if(fields != null) {
				ListPickForm form = new ListPickForm();
				foreach(object o in fields) {
					form.AddPossibleValue(o);
				
				}
				return form.ShowGetObject();
			}
			return null;
		}

		protected virtual void OnQueryBuildList(out List<object> values) {
			values = null;
			if(QueryBuildList != null) {
				QueryBuildList(this, out values);
			}
		}

		public event QueryBuildListEventHandler QueryBuildList;
	}

	public delegate void QueryBuildListEventHandler(object sender, out List<object> values);
}
