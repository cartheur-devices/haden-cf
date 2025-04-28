namespace Haden.NxtSharp {
	partial class NxtTankControl {
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.btnForward = new System.Windows.Forms.Button();
			this.btnTurnLeft = new System.Windows.Forms.Button();
			this.btnBack = new System.Windows.Forms.Button();
			this.btnTurRight = new System.Windows.Forms.Button();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel1.Controls.Add(this.btnTurRight, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.btnForward, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnTurnLeft, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.btnBack, 0, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(130, 149);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// btnForward
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.btnForward, 2);
			this.btnForward.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnForward.Font = new System.Drawing.Font("Marlett", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.btnForward.Location = new System.Drawing.Point(3, 3);
			this.btnForward.Name = "btnForward";
			this.btnForward.Size = new System.Drawing.Size(124, 43);
			this.btnForward.TabIndex = 0;
			this.btnForward.Text = "5";
			this.btnForward.UseVisualStyleBackColor = true;
			this.btnForward.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnForward_MouseDown);
			this.btnForward.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnForward_MouseUp);
			// 
			// btnTurnLeft
			// 
			this.btnTurnLeft.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnTurnLeft.Font = new System.Drawing.Font("Marlett", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.btnTurnLeft.Location = new System.Drawing.Point(3, 52);
			this.btnTurnLeft.Name = "btnTurnLeft";
			this.btnTurnLeft.Size = new System.Drawing.Size(59, 43);
			this.btnTurnLeft.TabIndex = 1;
			this.btnTurnLeft.Text = "3";
			this.btnTurnLeft.UseVisualStyleBackColor = true;
			this.btnTurnLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnTurnLeft_MouseDown);
			this.btnTurnLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnTurnLeft_MouseUp);
			// 
			// btnBack
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.btnBack, 2);
			this.btnBack.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnBack.Font = new System.Drawing.Font("Marlett", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.btnBack.Location = new System.Drawing.Point(3, 101);
			this.btnBack.Name = "btnBack";
			this.btnBack.Size = new System.Drawing.Size(124, 45);
			this.btnBack.TabIndex = 3;
			this.btnBack.Text = "6";
			this.btnBack.UseVisualStyleBackColor = true;
			this.btnBack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnBack_MouseDown);
			this.btnBack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnBack_MouseUp);
			// 
			// btnTurRight
			// 
			this.btnTurRight.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnTurRight.Font = new System.Drawing.Font("Marlett", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.btnTurRight.Location = new System.Drawing.Point(68, 52);
			this.btnTurRight.Name = "btnTurRight";
			this.btnTurRight.Size = new System.Drawing.Size(59, 43);
			this.btnTurRight.TabIndex = 2;
			this.btnTurRight.Text = "4";
			this.btnTurRight.UseVisualStyleBackColor = true;
			this.btnTurRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnTurRight_MouseDown);
			this.btnTurRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnTurRight_MouseUp);
			// 
			// NxtTankControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "NxtTankControl";
			this.Size = new System.Drawing.Size(130, 149);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button btnForward;
		private System.Windows.Forms.Button btnTurnLeft;
		private System.Windows.Forms.Button btnBack;
		private System.Windows.Forms.Button btnTurRight;

	}
}
