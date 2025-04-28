using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Haden.Utilities.CF {
	public partial class DirectoryPicker : Picker {
		public DirectoryPicker() {
			InitializeComponent();
		}

		public string Description {
			get {
				return folderBrowser.Description;
			}
			set {
				folderBrowser.Description = value;
			}
		}


		public string Path {
			get {
				return Value as string;
			}
			set {
				Value = value;
				folderBrowser.SelectedPath = value;
			}
		}

		public Environment.SpecialFolder RootFolder {
			get {
				return folderBrowser.RootFolder;
			}
			set {
				folderBrowser.RootFolder = value;
			}
		}

		protected override object GetObject() {
			if(folderBrowser.ShowDialog() == DialogResult.OK) {
				return folderBrowser.SelectedPath;
			} else {
				return Value;
			}
		}


	}
}
