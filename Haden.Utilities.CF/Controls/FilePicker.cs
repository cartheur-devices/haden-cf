using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Haden.Utilities.CF {
	public partial class FilePicker : Picker {
		public FilePicker() {
			InitializeComponent();
		}

		/// <summary>
		/// The file currently picked by this filepicker
		/// </summary>
		public string File {
			get {
				return Value as string;
			}
			set {
				Value = value;
				fileDialog.FileName = value;
			}
		}

		/// <summary>
		/// Controls the title of the dialog
		/// </summary>
		public string Title {
			get {
				return fileDialog.Title;
			}
			set {
				fileDialog.Title = value;
			}
		}


		/// <summary>
		/// Controls the default extension
		/// </summary>
		public string Extension {
			get {
				return fileDialog.DefaultExt;
			}
			set {
				fileDialog.DefaultExt = value;
			}
		}

		/// <summary>
		/// Controls the filter (e.g.C# files|*.cs|All files|*.*
		/// </summary>
		public string Filter {
			get {
				return fileDialog.Filter;
			}
			set {
				fileDialog.Filter = value;
			}
		}


		/// <summary>
		/// Controls the initial directory
		/// </summary>
		public string InitialDirectory {
			get {
				return fileDialog.InitialDirectory;
			}
			set {
				fileDialog.InitialDirectory = value;
			}
		}

		protected override object GetObject() {
			if(fileDialog.ShowDialog() == DialogResult.OK) {
				return fileDialog.FileName;
			} else {
				return Value;
			}
		}
	}
}
