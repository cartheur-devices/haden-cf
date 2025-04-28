namespace Haden.Utilities.CF.Components {
	partial class TetraLogPanel {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.listLog = new System.Windows.Forms.ListView();
			this.colTime = new System.Windows.Forms.ColumnHeader("");
			this.colComponent = new System.Windows.Forms.ColumnHeader("");
			this.colMessage = new System.Windows.Forms.ColumnHeader("");
			this.colPriority = new System.Windows.Forms.ColumnHeader("");
			this.SuspendLayout();
// 
// listLog
// 
			this.listLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.listLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.listLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTime,
            this.colComponent,
            this.colMessage,
            this.colPriority});
			this.listLog.FullRowSelect = true;
			this.listLog.HideSelection = false;
			this.listLog.Location = new System.Drawing.Point(0, 0);
			this.listLog.Name = "listLog";
			this.listLog.Size = new System.Drawing.Size(284, 260);
			this.listLog.TabIndex = 0;
			this.listLog.View = System.Windows.Forms.View.Details;
// 
// colTime
// 
			this.colTime.Text = "Tijd";
// 
// colComponent
// 
			this.colComponent.Text = "Component";
// 
// colMessage
// 
			this.colMessage.Text = "Bericht";
// 
// colPriority
// 
			this.colPriority.Text = "Prioriteit";
// 
// TetraLogPanel
// 
			this.Controls.Add(this.listLog);
			this.Name = "TetraLogPanel";
			this.Size = new System.Drawing.Size(283, 260);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView listLog;
		private System.Windows.Forms.ColumnHeader colTime;
		private System.Windows.Forms.ColumnHeader colComponent;
		private System.Windows.Forms.ColumnHeader colMessage;
		private System.Windows.Forms.ColumnHeader colPriority;
	}
}
