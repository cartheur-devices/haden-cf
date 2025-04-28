using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Haden.Utilities.CF.Components
{
	/// <summary>
	/// Summary description for TetraProgressWindowForm.
	/// </summary>
	public class ProgressWindowForm : System.Windows.Forms.Form
	{
		internal System.Windows.Forms.Button btnCancel;
		private ProgressWindow window;
		internal System.Windows.Forms.ProgressBar progress;
		internal System.Windows.Forms.Label lblProgress;


		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ProgressWindowForm(ProgressWindow w)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			window = w;
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnCancel = new System.Windows.Forms.Button();
			this.progress = new System.Windows.Forms.ProgressBar();
			this.lblProgress = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(168, 32);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Text = "Annuleren";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// progress
			// 
			this.progress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.progress.Location = new System.Drawing.Point(8, 8);
			this.progress.Name = "progress";
			this.progress.Size = new System.Drawing.Size(242, 16);
			this.progress.Step = 4;
			this.progress.TabIndex = 1;
			// 
			// lblProgress
			// 
			this.lblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblProgress.Location = new System.Drawing.Point(16, 32);
			this.lblProgress.Name = "lblProgress";
			this.lblProgress.Size = new System.Drawing.Size(136, 16);
			this.lblProgress.TabIndex = 2;
			this.lblProgress.Text = "...";
			// 
			// TetraProgressWindowForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(258, 63);
			this.ControlBox = false;
			this.Controls.Add(this.lblProgress);
			this.Controls.Add(this.progress);
			this.Controls.Add(this.btnCancel);
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "TetraProgressWindowForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Voortgang";
			this.ResumeLayout(false);

		}
		#endregion

		

		private void btnCancel_Click(object sender, System.EventArgs e) {
			window.Interrupt();
			Close();
		}
	}
}
