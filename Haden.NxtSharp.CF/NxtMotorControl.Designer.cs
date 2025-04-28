namespace Haden.NxtSharp {
	partial class NxtMotorControl {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
			this.btnTurnClockwise = new System.Windows.Forms.Button();
			this.btnTurnCounterClockwise = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnTurnClockwise
			// 
			this.btnTurnClockwise.Font = new System.Drawing.Font("Marlett", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.btnTurnClockwise.Location = new System.Drawing.Point(3, 3);
			this.btnTurnClockwise.Name = "btnTurnClockwise";
			this.btnTurnClockwise.Size = new System.Drawing.Size(54, 24);
			this.btnTurnClockwise.TabIndex = 0;
			this.btnTurnClockwise.Text = "5";
			this.btnTurnClockwise.UseVisualStyleBackColor = true;
			this.btnTurnClockwise.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnTurnClockwise_MouseDown);
			this.btnTurnClockwise.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnTurnClockwise_MouseUp);
			// 
			// btnTurnCounterClockwise
			// 
			this.btnTurnCounterClockwise.Font = new System.Drawing.Font("Marlett", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.btnTurnCounterClockwise.Location = new System.Drawing.Point(3, 33);
			this.btnTurnCounterClockwise.Name = "btnTurnCounterClockwise";
			this.btnTurnCounterClockwise.Size = new System.Drawing.Size(54, 24);
			this.btnTurnCounterClockwise.TabIndex = 1;
			this.btnTurnCounterClockwise.Text = "6";
			this.btnTurnCounterClockwise.UseVisualStyleBackColor = true;
			this.btnTurnCounterClockwise.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnTurnCounterClockwise_MouseDown);
			this.btnTurnCounterClockwise.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnTurnCounterClockwise_MouseUp);
			// 
			// NxtMotorControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnTurnCounterClockwise);
			this.Controls.Add(this.btnTurnClockwise);
			this.MinimumSize = new System.Drawing.Size(60, 60);
			this.Name = "NxtMotorControl";
			this.Size = new System.Drawing.Size(61, 62);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnTurnClockwise;
		private System.Windows.Forms.Button btnTurnCounterClockwise;
	}
}
