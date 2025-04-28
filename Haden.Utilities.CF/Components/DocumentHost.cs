using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using Haden.Utilities.CF;
using System.ComponentModel.Design;

namespace Haden.Utilities.CF.Components {
	public partial class DocumentHost : UserControl {
		public DocumentHost() {
			InitializeComponent();
			InitializeManager();
			
		}

		public DocumentHost(IContainer container) {
			container.Add(this);
			InitializeComponent();
			InitializeManager();
		}

		/// <summary>
		/// Returns the document this component manages
		/// </summary>
		public Document ActiveDocument {
			get {
				return _activeDocument;
			}
			set {
				_activeDocument = value;
			}
		}
		Document _activeDocument;

		/// <summary>
		/// Returns the status of this DocumentManager
		/// </summary>
		public string Status {
			get {
				if(ActiveDocument == null) {
					return "No document attached";
				}
				return _status;
			}
			private set {
				_status = value;
				if(StatusLabel != null) {
					StatusLabel.Text = Status;
				}
			}
		}
		string _status;

		/// <summary>
		/// Status label the status is reported to
		/// </summary>
		public ToolStripStatusLabel StatusLabel {
			get {
				return _statusLabel;
			}
			set {
				_statusLabel = value;
			}
		}
		ToolStripStatusLabel _statusLabel;

		/// <summary>
		/// progress bar the status is reported to
		/// </summary>
		public ProgressBar StatusProgressBar {
			get {
				return _statusProgressBar;
			}
			set {
				_statusProgressBar = value;
			}
		}
		ProgressBar _statusProgressBar;


		/// <summary>
		/// The toolbar button to use for the save function
		/// </summary>
		public ToolStripButton SaveToolBarButton {
			get {
				return _saveToolBarButton;
			}
			set {
				if(_saveToolBarButton != null) {
					_saveToolBarButton.Click -= new EventHandler(DoSave);
				}
				_saveToolBarButton = value;
				if(_saveToolBarButton != null) {
					_saveToolBarButton.Click += new EventHandler(DoSave);
				}
			}
		}
		ToolStripButton _saveToolBarButton;

		/// <summary>
		/// The toolbar button to use for the save function
		/// </summary>
		public ToolStripMenuItem SaveMenuItem {
			get {
				return _saveMenuItem;
			}
			set {
				if(_saveMenuItem != null) {
					_saveMenuItem.Click -= new EventHandler(DoSave);
				}
				_saveMenuItem = value;
				if(_saveMenuItem != null) {
					_saveMenuItem.Click += new EventHandler(DoSave);
				}
			}
		}
		ToolStripMenuItem _saveMenuItem;

		void DoSave(object sender, EventArgs e) {
			ActiveDocument.Save();
		}



		public void InitializeManager() {
			Document.OperationStarted += new DocumentOperationStart(Document_OperationStarted);
			Document.OperationProgressed += new DocumentOperationProgress(Document_OperationProgressed);
			Document.OperationEnded += new DocumentOperationEnd(Document_OperationEnded);
		}

		void Document_OperationStarted(Document sender, string status, int length) {
			if(StatusProgressBar != null) {
				StatusProgressBar.Minimum = 0;
				StatusProgressBar.Maximum = length;
			}
			Status = status;
		}

		void Document_OperationProgressed(Document sender, string status, int count) {
			if(StatusProgressBar != null) {
				count = Math.Max(count, StatusProgressBar.Minimum);
				count = Math.Min(count, StatusProgressBar.Maximum);
				StatusProgressBar.Value = count;
			}
			Status = status;
		}

		void Document_OperationEnded(Document sender) {
			Status = "Gereed";
		}
	}
}
