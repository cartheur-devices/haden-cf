using System;
using System.Drawing;

namespace Haden.Utilities.CF.CF.BoxModel {
	[Serializable]
	public class Page : Box {
		/// <summary>
		/// Constructs a page with the specified DPI
		/// </summary>
		/// <param name="dpi"></param>
		public Page(float dpi)
			: base(null) {
			_dpi = dpi;
		}

		/// <summary>
		/// The dots per Inch
		/// </summary>
		public float DPI {
			get {
				return _dpi;
			}
		}
		float _dpi = 72;
		
		
		public override Page BasePage {
			get {
				return this;
			}
		}

	}

	[Serializable]
	public class Box {
		public Box(Box parent) {
			_parent = parent;
		}

		/// <summary>
		/// The parent this box is contained in
		/// </summary>
		public Box Parent {
			get {
				return _parent;
			}
		}
		Box _parent;

		/// <summary>
		/// Returns the page this box is on
		/// </summary>
		public virtual Page BasePage {
			get {
				if(Parent == null) {
					throw new ApplicationException("This box is not located on a page");
				}
				return Parent.BasePage;
			}
		}

		#region Relative Coordinates

		
		/// <summary>
		/// The width of the box
		/// </summary>
		public float Width {
			get {
				return _width;
			}
			set {
				_width = value;
			}
		}
		float _width;

		/// <summary>
		/// The height of the box
		/// </summary>
		public float Height {
			get {
				return _height;
			}
			set {
				_height = value;
			}
		}
		float _height;


		/// <summary>
		/// The x-location of the box
		/// </summary>
		public float X {
			get {
				return _x;
			}
			set {
				_x = value;
			}
		}
		public float Left {
			get {
				return _x;
			}
			set {
				_x = value;
			}
		}
		float _x;

		/// <summary>
		/// The y-location of the box
		/// </summary>
		public float Y {
			get {
				return _y;
			}
			set {
				_y = value;
			}
		}
		public float Top {
			get {
				return _y;
			}
			set {
				_y = value;
			}
		}
		float _y;

		#endregion

		#region World coordinates

		/// <summary>
		/// The world x of the box
		/// </summary>
		public float PageX {
			get {
				float result = X;
				if(Parent != null) {
					result += Parent.PageX;
				}
				return result;
			}
		}


		/// <summary>
		/// The world x of the box
		/// </summary>
		public float PageY {
			get {
				float result = Y;
				if(Parent != null) {
					result += Parent.PageY;
				}
				return result;
			}
		}

		/// <summary>
		/// The world top of the box
		/// </summary>
		public float PageTop {
			get {
				return PageY;
			}
		}

		/// <summary>
		/// The world left of the box
		/// </summary>
		public float PageLeft {
			get {
				return PageX;
			}
		}

	
		/// <summary>
		/// The world x of the box
		/// </summary>
		public float PageRight {
			get {
				return PageX + Width;
			}
		}

		/// <summary>
		/// The world x of the box
		/// </summary>
		public float PageBottom {
			get {
				return PageY + Height;
			}
		}



		#endregion

		#region Size helpers

		/// <summary>
		/// Converts a given string to a number of pixels
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public float PixelsWidth(string str) {
			str = str.Trim();
			if(str.EndsWith("%")) {
				return float.Parse(str.Substring(0, str.Length - 1)) * Width / 100.0f;
			}
			return Pixels(str);
		}

		/// <summary>
		/// Converts a given string to a number of pixels
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public float PixelsHeight(string str) {
			str = str.Trim();
			if(str.EndsWith("%")) {
				return float.Parse(str.Substring(0, str.Length - 1)) * Height / 100.0f;
			}
			return Pixels(str);
		}

		/// <summary>
		/// Converts a given string to a number of pixels
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public float Pixels(string str) {
			str = str.Trim();
			if(str.EndsWith("\"")) {
				return convertScaleToPixels(str.Substring(0, str.Length - 1), 1);
			}
			if(str.EndsWith("inch")) {
				return convertScaleToPixels(str.Substring(0, str.Length - 4), 1);
			}
			if(str.EndsWith("cm")) {
				return convertScaleToPixels(str.Substring(0, str.Length - 2), 2.541f);
			}
			if(str.EndsWith("mm")) {
				return convertScaleToPixels(str.Substring(0, str.Length - 2), 25.41f);
			}
			if(str.EndsWith("px")) {
				return float.Parse(str.Substring(0, str.Length - 2));
			}
			return float.Parse(str);
		}

		private float convertScaleToPixels(string number, float inchratio) {
			float n = float.Parse(number);
			return n * BasePage.DPI / inchratio;
		}

		#endregion

		#region Box model

		public BorderStruct<float> Margin;
		public Color BorderColor;
		public BorderStruct<float> Border;
		public BorderStruct<float> Padding;

		#endregion

		#region Public functionality

		public Box Add(string x, string y, string width, string height) {
			Box b = new Box(this);
			b.SetSize(x, y, width, height);
			return b;
		}

		public Box Add(Box b) {
			b._parent = this;
			return b;
		}

		public void SetSize(string x, string y, string width, string height) {
			X = PixelsWidth(x);
			Y = PixelsHeight(y);
			Width = PixelsWidth(width);
			Height = PixelsHeight(height);
		}

		/// <summary>
		/// Returns a RectangleF with the client area of this box,
		/// with absolute coordinates
		/// </summary>
		public RectangleF ClientArea {
			get {
				RectangleF result = new RectangleF();
				//result.X = PageX + Margin.Left;
				//result.Y = PageY + Margin.Top;
				//result.Width = Width - (Margin.Left + Margin.Right);
				//result.Height = Height - (Margin.Top + Margin.Bottom);
				return result;
			}
		}

		#endregion

		public RectangleF RectangleBox {
			get {
				return new RectangleF(PageX, PageY, Width, Height);
			}
		}
	}

	[Serializable]
	public struct BorderStruct<T> {
		public T Top {
			get {
				return _top;
			}
			set {
				_top = value;
				BorderPropertyChanged(this);
			}
		}
		T _top;

		public T Bottom {
			get {
				return _bottom;
			}
			set {
				_bottom = value;
				BorderPropertyChanged(this);
			}
		}
		T _bottom;

		public T Left {
			get {
				return _left;
			}
			set {
				_left = value;
				BorderPropertyChanged(this);
			}
		}
		T _left;

		public T Right {
			get {
				return _right;
			}
			set {
				_right = value;
				BorderPropertyChanged(this);
			}
		}
		T _right;


		public delegate void BorderPropertyChangedEvent(BorderStruct<T> source);

		public event BorderPropertyChangedEvent BorderPropertyChanged; 
	}


}