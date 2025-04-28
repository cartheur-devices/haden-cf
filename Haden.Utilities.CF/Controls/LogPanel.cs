#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

#endregion

namespace Haden.Utilities.CF.Components {
	public partial class TetraLogPanel : UserControl, ILogTarget {
		public TetraLogPanel() {
			InitializeComponent();
		}

		#region ILogTarget Members

		/// <summary>
		/// Logs the message
		/// </summary>
		/// <param name="timestamp"></param>
		/// <param name="message"></param>
		/// <param name="level"></param>
		/// <param name="component"></param>
		public void OnLogged(LogEntry entry) {
			lock(listLog) {
				if(listLog.IsHandleCreated) {
					listLog.BeginInvoke(new LogEvent(doLog), entry);
				}
			}
		}

		/// <summary>
		/// Attaches the logger to 
		/// </summary>
		/// <param name="log"></param>
		public void Attach(Logger log) {
		}

		#endregion

		#region Private parts
		private void doLog(LogEntry entry) {
			string[] text = new string[4];
			text[0] = Util.ISOTimeStamp(entry.TimeStamp);
			text[1] = entry.Component;
			text[2] = entry.Message;
			text[3] = entry.Level.ToString();

			ListViewItem item = new ListViewItem(text);
			if(entry.Color != null) {
				item.ForeColor = entry.Color;
			}

			listLog.Items.Add(item);
		}
		#endregion
	}
}
