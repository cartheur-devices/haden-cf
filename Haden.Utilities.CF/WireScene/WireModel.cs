using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Haden.Utilities.CF.Geometry;

namespace Haden.Utilities.CF.WireScene {

	public class Model : IRenderable {
		public List<IRenderable> Parts {
			get {
				return _parts;
			}
			set {
				_parts = value;
			}
		}
		private List<IRenderable> _parts = new List<IRenderable>();

		public void Render(Graphics g, Projection p) {
			foreach(IRenderable part in Parts) {
				part.Render(g, p);
			}
		}
	}

	public abstract class Primitive : IRenderable {
		public Primitive() {
		}

		public Primitive(Color c) {
			_color = c;
		}

		public Color Color {
			get {
				return _color;
			}
			set {
				_color = value;
			}
		}
		Color _color = Color.White;

		public abstract void Render(Graphics g, Projection p);
	}

	public class WLine : Primitive {

		public WLine(Vector3 start, Vector3 end) : base() {
			_start = start;
			_end = end;
		}

		public WLine(Vector3 start, Vector3 end, Color color) : base(color) {
			_start = start;
			_end = end;
		}

		/// <summary>
		/// The start location of this line
		/// </summary>
		public Vector3 Start {
			get {
				return _start;
			}
			set {
				_start = value;
			}
		}
		Vector3 _start = Vector3.Empty;


		/// <summary>
		/// The end location of this line
		/// </summary>
		public Vector3 End {
			get {
				return _end;
			}
			set {
				_end = value;
			}
		}
		Vector3 _end = Vector3.Empty;

		public override void Render(Graphics g, Projection p) {
			using(Pen pen = new Pen(Color)) {
				g.DrawLine(pen, p.Transform(Start), p.Transform(End));
			}
		}
	}



	public class WPoint : Primitive {

		public WPoint(Vector3 position)
			: base() {
			_position = position;
		}

		public WPoint(Vector3 position, Color color)
			: base(color) {
			_position = position;
		}

		/// <summary>
		/// The position of this point
		/// </summary>
		public Vector3 Position {
			get {
				return _position;
			}
			set {
				_position = value;
			}
		}
		Vector3 _position = Vector3.Empty;


		public override void Render(Graphics g, Projection p) {
			using(Pen pen = new Pen(Color)) {
				g.DrawLine(pen, p.Transform(Position), p.Transform(Position));
			}
		}
	}
}
