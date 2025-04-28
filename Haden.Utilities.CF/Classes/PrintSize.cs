using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Printing;

namespace Haden.Utilities.CF {
	/// <summary>
	/// A print scalar just contains a size
	/// The difference with a float is that it's automatically downcast to an int
	/// if necessary
	/// </summary>
	public struct PrintScalar {
		#region Constructors

		/// <summary>
		/// Constructs a PrintSize with the specified size
		/// </summary>
		/// <param name="newvalue"></param>
		public PrintScalar(int newvalue) {
			_value = newvalue;
		}

		/// <summary>
		/// Constructs a PrintSize with the specified size
		/// </summary>
		/// <param name="newvalue"></param>
		public PrintScalar(float newvalue) {
			_value = newvalue;
		}

		/// <summary>
		/// Constructs a PrintSize with the specified size
		/// </summary>
		/// <param name="newvalue"></param>
		public PrintScalar(PrintScalar p) {
			_value = p.Value;
		}

		#endregion

		#region Value

		/// <summary>
		/// Contains the value in this scalar
		/// </summary>
		public float Value {
			get {
				return _value;
			}
			set {
				_value = value;
			}
		}
		float _value;

		#endregion

		#region Casting

		/// <summary>
		/// Allows a size to be used instead of an int
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public static implicit operator float(PrintScalar p) {
			return p.Value;
		}


		/// <summary>
		/// Allows an int to be used instead of a print size
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public static implicit operator PrintScalar(float p) {
			return new PrintScalar(p);
		}

		#endregion

		#region ToString
		
		public override string ToString() {
			return Value.ToString();
		}
		
		#endregion
	}

	public struct PrintCoordinate {
		public PrintCoordinate(PrintScalar x, PrintScalar y) {
			_x = x;
			_y = y;
		}

		public override string ToString() {
			return "(" + X + ", " + Y + ")";
		}

		/// <summary>
		/// This is X coordinate
		/// </summary>
		public PrintScalar X {
			get {
				return _x;
			}
			set {
				_x = value;
			}
		}
		PrintScalar _x;

		/// <summary>
		/// This is Y coordinate
		/// </summary>
		public PrintScalar Y {
			get {
				return _y;
			}
			set {
				_y = value;
			}
		}
		PrintScalar _y;

		/// <summary>
		/// cast to pointf
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public static implicit operator PointF(PrintCoordinate p) {
			return new PointF(p.X, p.Y);
		}
	}

	public struct PrintRange {
		public PrintRange(PrintScalar p1, PrintScalar p2) {
			_min = p1;
			_max = p2;
			reorder();
		}

		public override string ToString() {
			return "[" + Min + ", " + Max + ">";
		}


		/// <summary>
		/// This is start of the range
		/// </summary>
		public PrintScalar Min {
			get {
				return _min;
			}
			set {
				_min = value;
				reorder();
			}
		}
		PrintScalar _min;

		/// <summary>
		/// This is end of the range coordinate
		/// </summary>
		public PrintScalar Max {
			get {
				return _max;
			}
			set {
				_max = value;
				reorder();
			}
		}
		PrintScalar _max;

		/// <summary>
		/// Intersects these two ranges
		/// </summary>
		/// <param name="range"></param>
		/// <returns></returns>
		public PrintRange Intersect(PrintRange range) {
			if(Min < range.Min) {
				if(range.Min >= Max) {
					return Empty;
				} else {
					return new PrintRange(range.Min, Math.Min(Max, range.Max));
				}
			} else {
				if(Min >= range.Max) {
					return Empty;
				} else {
					return new PrintRange(Min, Math.Min(Max, range.Max));
				}
			}
		}

		/// <summary>
		/// Returns an empty printrange
		/// </summary>
		public static PrintRange Empty {
			get {
				return new PrintRange();
			}
		}

		void reorder() {
			if(Max < Min) {
				PrintScalar temp = Min;
				_min = Max;
				_max = temp;
			}
		}

		/// <summary>
		/// Is this range empty?
		/// </summary>
		public bool IsEmpty {
			get {
				return Size <= 0;
			}
		}

		/// <summary>
		/// Returns the size of this range
		/// </summary>
		public PrintScalar Size {
			get {
				return Max - Min;
			}
		}
	}

	public struct PrintSize {
		public PrintSize(PrintScalar width, PrintScalar height) {
			_width = width;
			_height = height;
		}


		public override string ToString() {
			return "(" + Width + ", " + Height + ")";
		}

		/// <summary>
		/// This is width
		/// </summary>
		public PrintScalar Width {
			get {
				return _width;
			}
			set {
				_width = value;
			}
		}
		PrintScalar _width;

		/// <summary>
		/// This is Y coordinate
		/// </summary>
		public PrintScalar Height {
			get {
				return _height;
			}
			set {
				_height = value;
			}
		}
		PrintScalar _height;

		/// <summary>
		/// Centers the specified rectangle within this rectangle
		/// </summary>
		/// <param name="rectangle"></param>
		/// <returns></returns>
		public PrintRectangle CenterIn(PrintRectangle rectangle) {
			float myAspectRatio = Width / Height;
			float targetAspectRatio = rectangle.Width / rectangle.Height;
			PrintRectangle result = new PrintRectangle();
			if(myAspectRatio > targetAspectRatio) {
				// I am wider, center horizontally
				result.Height = rectangle.Height;
				result.Width = result.Height * myAspectRatio;
				result.Y = rectangle.Y;
				result.X = rectangle.X + (rectangle.Width - result.Width) / 2.0f;
			} else {
				// I am taller, center vertically
				result.Width = rectangle.Width;
				result.Height = rectangle.Width / myAspectRatio;
				result.X = rectangle.X;
				result.Y = rectangle.Y + (rectangle.Height - result.Height) / 2.0f;
			}
			return result;
		}
	}
	
	public struct PrintRectangle {
		/// <summary>
		/// Constructs a printrectangle
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public PrintRectangle(PrintScalar x, PrintScalar y, PrintScalar width, PrintScalar height) {
			_location = new PrintCoordinate(x, y);
			_size = new PrintSize(width, height);
		}

		/// <summary>
		/// ToString()
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return Location + "-" + BottomRight;
		}

		/// <summary>
		/// The location of this rectangle
		/// </summary>
		public PrintCoordinate Location {
			get {
				return _location;
			}
			set {
				_location = value;
			}
		}
		PrintCoordinate _location;

		/// <summary>
		/// The size of this rectangle
		/// </summary>
		public PrintSize Size {
			get {
				return _size;
			}
			set {
				_size = value;
			}
		}
		PrintSize _size;

		/// <summary>
		/// Gets or sets the X coordinate of this rectangle
		/// </summary>
		public PrintScalar X {
			get {
				return Location.X;
			}
			set {
				_location.X = value;
			}
		}

		/// <summary>
		/// Gets or sets the Y coordinate of this rectangle
		/// </summary>
		public PrintScalar Y {
			get {
				return Location.Y;
			}
			set {
				_location.Y = value;
			}
		}

		/// <summary>
		/// Gets or sets the width of this rectangle
		/// </summary>
		public PrintScalar Width {
			get {
				return Size.Width;
			}
			set {
				_size.Width = value;
			}
		}

		/// <summary>
		/// Gets or sets the height of this rectangle
		/// </summary>
		public PrintScalar Height {
			get {
				return Size.Height;
			}
			set {
				_size.Height = value;
			}
		}

		/// <summary>
		/// Gets the y coordinate of the top
		/// </summary>
		public PrintScalar Top {
			get {
				return Location.Y;
			}
		}

		/// <summary>
		/// Gets the y coordinate of the bottom
		/// </summary>
		public PrintScalar Bottom {
			get {
				return Location.Y + Size.Height;
			}
		}

		/// <summary>
		/// Gets the X coordinate of the left side
		/// </summary>
		public PrintScalar Left {
			get {
				return Location.X;
			}
		}

		/// <summary>
		/// Gets the x coordinate of the right side
		/// </summary>
		public PrintScalar Right {
			get {
				return Location.X + Size.Width;
			}
		}

		/// <summary>
		/// Returns the top left coordinate of this rectangle
		/// </summary>
		public PrintCoordinate TopLeft {
			get {
				return Location;
			}
		}

		/// <summary>
		/// Returns the top right coordinate of this rectangle
		/// </summary>
		public PrintCoordinate TopRight {
			get {
				return new PrintCoordinate(Right, Top);
			}
		}

		/// <summary>
		/// Returns the top right coordinate of this rectangle
		/// </summary>
		public PrintCoordinate BottomLeft {
			get {
				return new PrintCoordinate(Left, Bottom);
			}
		}

		/// <summary>
		/// Returns the top right coordinate of this rectangle
		/// </summary>
		public PrintCoordinate BottomRight {
			get {
				return new PrintCoordinate(Right, Bottom);
			}
		}

		/// <summary>
		/// The horizontal range this rectangle spans
		/// </summary>
		public PrintRange HorizontalRange {
			get {
				return new PrintRange(Left, Right);
			}
		}

		/// <summary>
		/// The vertical range this rectangle spans
		/// </summary>
		public PrintRange VerticalRange {
			get {
				return new PrintRange(Top, Bottom);
			}
		}

		/// <summary>
		/// Intersects two rectangles
		/// </summary>
		/// <param name="rectangle"></param>
		/// <returns></returns>
		public PrintRectangle Intersect(PrintRectangle rectangle) {
			PrintRange hrange = HorizontalRange.Intersect(rectangle.HorizontalRange);
			PrintRange vrange = VerticalRange.Intersect(rectangle.VerticalRange);
			if(hrange.IsEmpty || vrange.IsEmpty) {
				return Empty;
			} else {
				PrintRectangle result = new PrintRectangle();
				result.X = hrange.Min;
				result.Width = hrange.Size;
				result.Y = vrange.Min;
				result.Height = vrange.Size;
				return result;
			}
		}

		/// <summary>
		/// Returns a new rectangle containing the portion of this rectangle
		/// below the specified y value
		/// </summary>
		/// <param name="y"></param>
		/// <returns></returns>
		public PrintRectangle Below(PrintScalar y) {
			PrintRectangle copy = this;
			copy.Y = y;
			return Intersect(copy);
		}


		/// <summary>
		/// Returns a new rectangle containing the portion of this rectangle
		/// above the specified y value
		/// </summary>
		/// <param name="y"></param>
		/// <returns></returns>
		public PrintRectangle Above(PrintScalar y) {
			PrintRectangle copy = this;
			copy.Y = Y - (Bottom - y);
			return Intersect(copy);
		}

		/// <summary>
		/// Returns a new rectangle containing the portion of this rectangle
		/// to the left of the specified x value
		/// </summary>
		/// <param name="y"></param>
		/// <returns></returns>
		public PrintRectangle RightOf(PrintScalar x) {
			PrintRectangle copy = this;
			copy.X = x;
			return Intersect(copy);
		}


		/// <summary>
		/// Returns a new rectangle containing the portion of this rectangle
		/// above the specified y value
		/// </summary>
		/// <param name="y"></param>
		/// <returns></returns>
		public PrintRectangle LeftOf(PrintScalar y) {
			PrintRectangle copy = this;
			copy.X = X - (Right - X);
			return Intersect(copy);
		}

		/// <summary>
		/// Is this rectangle empty?
		/// </summary>
		public bool IsEmpty {
			get {
				return Width == 0 || Height == 0;
			}
		}

		#region Static candy
		/// <summary>
		/// Returns an empty rectangle
		/// </summary>
		public static PrintRectangle Empty {
			get {
				return new PrintRectangle();
			}
		}

		/// <summary>
		/// Creates a print rectangle from the specified bounds and margins
		/// 
		/// The unit will be millimeters
		/// </summary>
		/// <param name="bounds"></param>
		/// <param name="margins"></param>
		/// <returns></returns>
		public static PrintRectangle CreatePrintRectangle(Rectangle bounds, Margins margins) {
			PrintRectangle rectangle = new PrintRectangle();
			rectangle.Width = bounds.Width - (margins.Left + margins.Right);
			rectangle.Height = bounds.Height - (margins.Top + margins.Bottom);
			rectangle.X = bounds.Left + margins.Left;
			rectangle.Y = bounds.Top + margins.Top;
			return rectangle * 0.2541f;
		}

		#endregion

		#region Overloaded operators

		public static PrintRectangle operator *(PrintRectangle p1, float x) {
			return new PrintRectangle(p1.X * x, p1.Y * x, p1.Width * x, p1.Height * x);
		}

		public static PrintRectangle operator *(PrintScalar x, PrintRectangle p1) {
			return p1 * x;
		}

		/// <summary>
		/// Allows an int to be used instead of a print size
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public static implicit operator RectangleF(PrintRectangle rectangle) {
			return new RectangleF(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
		}

		#endregion
	}

	public struct PrintTable {
		/// <summary>
		/// The origin of the table
		/// </summary>
		public PrintCoordinate Origin {
			get {
				return _origin;
			}
			set {
				_origin = value;
			}
		}
		PrintCoordinate _origin;

		/// <summary>
		/// The size of a single cell
		/// </summary>
		public PrintSize CellSize {
			get {
				return _cellSize;
			}
			set {
				_cellSize = value;
			}
		}
		PrintSize _cellSize;

		/// <summary>
		/// The size of the horizontal gutter
		/// </summary>
		public PrintScalar HorizontalGutter {
			get {
				return _horizontalGutter;
			}
			set {
				_horizontalGutter = value;
			}
		}
		PrintScalar _horizontalGutter;

		/// <summary>
		/// The size of the vertical gutter
		/// </summary>
		public PrintScalar VerticalGutter {
			get {
				return _verticalGutter;
			}
			set {
				_verticalGutter = value;
			}
		}
		PrintScalar _verticalGutter;

		/// <summary>
		/// Constructs a table
		/// </summary>
		/// <param name="area">area the table should occupy</param>
		/// <param name="cols"># of columns</param>
		/// <param name="rows"># of rows</param>
		/// <param name="horizontalGutter">horizontal gutter</param>
		/// <param name="verticalGutter">vertical gutter</param>
		public PrintTable(PrintRectangle area, int cols, int rows, PrintScalar horizontalGutter, PrintScalar verticalGutter) {
			area.Width += horizontalGutter;
			area.Width /= cols;
			area.Width -= horizontalGutter;

			area.Height += verticalGutter;
			area.Height /= rows;
			area.Height -= verticalGutter;
			
			_origin = area.Location;
			_cellSize = area.Size;
			_horizontalGutter = horizontalGutter;
			_verticalGutter = verticalGutter;
		}

		/// <summary>
		/// Gets the rectangle containing the specified cell
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public PrintRectangle Cell(int x, int y) {
			PrintRectangle result = new PrintRectangle();
			result.Size = CellSize;
			result.X = Origin.X + (x * (HorizontalGutter + CellSize.Width));
			result.Y = Origin.Y + (y * (VerticalGutter + CellSize.Height));
			return result;
		}
	}

}
