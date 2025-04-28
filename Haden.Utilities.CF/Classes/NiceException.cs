using System;
using System.Collections.Generic;
using System.Text;

namespace Haden.Utilities.CF {
	public class NiceException : ApplicationException {
		public string Title {
			get {
				return _title;
			}
			set {
				_title = value;
			}
		}
		string _title;

		public NiceException(string title, string message)	: base(message) {
			_title = title;
		}
	}
}
