using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Haden.Utilities.CF.Components
{
	/// <summary>
	/// Summary description for TetraProgressWindow.
	/// </summary>
	public class ProgressWindow : System.ComponentModel.Component
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private ProgressWindowForm form;
		private Thread thread;

		public ProgressWindow(System.ComponentModel.IContainer container)
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			container.Add(this);
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public ProgressWindow()
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion

		#region Public properties

		/// <summary>
		/// The plural name of the object in which the progress is measured
		/// </summary>
		public string Plural {
			get {
				return _plural;
			}
			set {
				_plural = value;
			}
		}
		private string _plural;

		/// <summary>
		/// The singular name of the object in which the progress is measured
		/// </summary>
		public string Singular {
			get {
				return _singular;
			}
			set {
				_singular = value;
			}
		}
		private string _singular;

		/// <summary>
		/// The minimum value
		/// </summary>
		public int Min {
			get {
				return _min;
			}
			set {
				_min = value;
			}
		}
		private int _min = 0;

		/// <summary>
		/// The value
		/// </summary>
		public int Value {
			get {
				return _value;
			}
			set {
				if(value >= _min && value <= _max) {
                    _value = value;
					if(form == null || !form.InvokeRequired) {
						updateProgress();
					} else {
						form.Invoke(new MethodInvoker(updateProgress));
					}
				}
			}
		}
		private int _value = 0;

		/// <summary>
		/// The maximum value
		/// </summary>
		public int Max {
			get {
				return _max;
			}
			set {
				_max = value;
			}
		}
		private int _max = 100;

		/// <summary>
		/// Determines if the window has a cancel button
		/// </summary>
		public bool HasCancelButton {
			get {
				return _hasCancelButton;
			}
			set {
				_hasCancelButton = value;
			}
		}
		private bool _hasCancelButton;

		/// <summary>
		/// The application form this progress window belongs to
		/// </summary>
		public Form ApplicationForm {
			get {
				return _appForm;
			}
			set {
				_appForm = value;
			}
		}
		private Form _appForm;

		#endregion

		#region Public interface
		/// <summary>
		/// Displays a progress window and starts the work
		/// </summary>
		/// <param name="work">delegate of the work function</param>
		public void Show(ThreadStart work) {
			if(form == null) {
				form = createForm();
				Value = Min;
				thread = new Thread(new ThreadStart(work));
				thread.Start();
				
				if(ApplicationForm != null) {
					ApplicationForm.Enabled = false;
				}

				form.ShowDialog();
				form = null;
				thread = null;
			}
		}

		/// <summary>
		/// Interrupts the work and closes the progress window
		/// </summary>
		public void Interrupt() {
			if(thread != null) {
				thread.Abort();
			}
			Done();
		}

		/// <summary>
		/// Closes the progress window
		/// </summary>
		public void Done() {
			if(form != null) {
				if(ApplicationForm != null) {
					ApplicationForm.Invoke(new MethodInvoker(enableAppForm));
				}

				form.Close();
				form = null;
			}
		}
		#endregion

		#region Helper methods
		private void enableAppForm() {
			ApplicationForm.Enabled = true;
		}

		/// <summary>
		/// Creates a Progress form
		/// </summary>
		/// <returns></returns>
		private ProgressWindowForm createForm() {
			ProgressWindowForm form = new ProgressWindowForm(this);
			form.progress.Minimum = Min;
			form.progress.Maximum = Max;
			form.progress.Value = 0;
			form.btnCancel.Visible = HasCancelButton;
			return form;
		}

		/// <summary>
		/// Updates the progress bar
		/// </summary>
		private void updateProgress() {
			if(form != null) {
				form.lblProgress.Text = Value + "/" + Max + " ";
				if(Value == 1) {
					form.lblProgress.Text += Singular;
				} else {
					form.lblProgress.Text += Plural;
				}
				form.progress.Value = Value;
				Application.DoEvents();
			}
		}
		#endregion
	}
}
