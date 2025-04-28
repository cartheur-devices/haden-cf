using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Haden.Utilities.CF.WireScene {
	public partial class WireView : UserControl {
		public WireView() {
			InitializeComponent();

			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		}

		/// <summary>
		/// The target this View is rendering
		/// </summary>
		public IRenderable Target {
			get {
				return _target;
			}
			set {
				_target = value;
			}
		}
		IRenderable _target = null;

		/// <summary>
		/// The view mode (i.e. projection)
		/// </summary>
		public WireViewMode ViewMode {
			get {
				return _viewMode;
			}
			set {
				if(value != _viewMode) {
					_viewMode = value;
					switch(value) {
						case WireViewMode.OrthoGraphicXY:
							Projection = new ProjectionXY();
							break;

						case WireViewMode.OrthoGraphicXZ:
							Projection = new ProjectionXZ();
							break;

						case WireViewMode.OrthoGraphicYZ:
							Projection = new ProjectionYZ();
							break;

						default:
							throw new Exception("Unexpected FillMode");
					}
					Invalidate();
				}
			}
		}
		WireViewMode _viewMode = WireViewMode.OrthoGraphicXY;


		/// <summary>
		/// The target this View is rendering
		/// </summary>
		private Projection Projection {
			get {
				return _projection;
			}
			set {
				_projection = value;
			}
		}
		Projection _projection = new ProjectionXY();



		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaintBackground(e);
			base.OnPaint(e);

			Graphics g = e.Graphics;
			paintGrid(g);
			paintScene(g);
		}

		private void paintGrid(Graphics g) {
		}

		private void paintScene(Graphics g) {
			if(Target != null) {
				Target.Render(g, Projection);
			}
		}

		public enum WireViewMode {
			OrthoGraphicXY,
			OrthoGraphicXZ,
			OrthoGraphicYZ
		}
	}
}
