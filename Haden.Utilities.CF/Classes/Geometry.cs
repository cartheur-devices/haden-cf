using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Haden.Utilities.CF.Geometry {
	
		/// <summary>
	/// A vector is an immutable object containing a vector.
	/// NOTE: All methods in this struct assume a right-handed coordinate system.
	/// This means if you look at the XY plane, with the X-axis pointing to the 
	/// right and the Y-axis pointing up, the Z-axis will be pointing towards you
	/// </summary>
	public struct Vector2 {
		public double X {
			get {
				return _x;
			}
		}
		double _x;

		public double Y {
			get {
				return _y;
			}
		}
		double _y;


		public Vector2(double x, double y) {
			_x = x;
			_y = y;
			_length = 0;
		}

		#region Operators
		public static Vector2 operator +(Vector2 p1, Vector2 p2) {
			return new Vector2(p1.X + p2.X, p1.Y + p2.Y);
		}

		public static Vector2 operator -(Vector2 p1, Vector2 p2) {
			return new Vector2(p1.X - p2.X, p1.Y - p2.Y);
		}

		public static Vector2 operator *(Vector2 p1, double n) {
			return new Vector2(p1.X * n, p1.Y * n);
		}

		public static Vector2 operator *(double n, Vector2 p1) {
			return p1 * n;
		}

		public static Vector2 operator /(Vector2 p1, double n) {
			return new Vector2(p1.X / n, p1.Y / n);
		}

		public static bool operator ==(Vector2 v1, Vector2 v2) {
			return v1.X == v2.X && v1.Y == v2.Y;
		}

		public static bool operator !=(Vector2 v1, Vector2 v2) {
			return v1.X != v2.X || v1.Y != v2.Y;
		}

		/// <summary>
		/// Returns the dot product of the two fectors
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <returns></returns>
		public static double operator *(Vector2 p1, Vector2 p2) {
			return p1.X * p2.X + p1.Y * p2.Y;
		}

		/// <summary>
		/// Creates a vector from a string
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static implicit operator Vector2(string s) {
			s = s.Replace("[", "");
			s = s.Replace("]", "");
			s = s.Replace("(", "");
			s = s.Replace(")", "");
			string[] components = s.Trim().Split(new char[] { ',', ';' });
			if(components.Length != 2) {
				throw new ArgumentException("Can't create vector from string '" + s + "' The string contains too many components");
			} else {
				double x = double.Parse(components[0].Trim().Replace(",", "."));
				double y = double.Parse(components[1].Trim().Replace(",", "."));
				return new Vector2(x, y);
			}
		}

		#endregion

		#region basic operations

		/// <summary>
		/// Calculates the distance between this point and a specified other point
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public double DistanceTo(Vector2 p) {
			return (this - p).Length;
		}

		/// <summary>
		/// Determines if two vectors are perpendicular
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public bool IsPerpendicularTo(Vector2 v) {
			return (v * this) == 0;
		}

		/// <summary>
		/// Calculates the angle between this vector and another vector
		/// </summary>
		/// <returns>the angle in radians [0, 2pi]</returns>
		public double Angle(Vector2 v) {
			double dot = this * v;
			dot /= (v.Length * Length);
			return Math.Acos(dot);
		}

		/// <summary>
		/// Calculates the angle this vector makes with the X unit vector.
		/// [0-2PI]
		/// </summary>
		/// <returns></returns>
		public double Angle() {
			if(Y > 0) {
				return Angle(UnitX);
			} else {
				return Math.PI * 2 - Angle(UnitX);
			}
		}

		/// <summary>
		/// Calculates the angle between this angle and the X unit
		/// vector, between [max-2pi, max]
		/// </summary>
		/// <param name="max"></param>
		/// <returns></returns>
		public double Angle(double max) {
			double a = Angle();
			if(a > max) {
				return a - 2.0 * Math.PI;
			} else {
				return a;
			}
		}


		/// <summary>
		/// Returns a normalized version of this vector
		/// </summary>
		/// <returns></returns>
		public Vector2 Normalize() {
			if(Length == 0) {
				return Vector2.Empty;
			} else {
				return this / Length;
			}
		}

		/// <summary>
		/// projects 'v' onto this vector.
		/// Remains the parameter 't', so that
		/// t * 'this vector' equals the projection
		/// of v onto this vector
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public double Project(Vector2 v) {
			return (this * v) / (this * this);
		}

		public override string ToString() {
			return "(" + X + " , " + Y + ")";
		}

		/// <summary>
		/// Calculates the length of this vector
		/// </summary>
		/// <returns></returns>
		public double Length {
			get {
				if(_length == 0) {
					_length = Math.Sqrt(this * this);
				}
				return _length;
			}
		}
		double _length;

		#endregion


		#region Constants
		public static Vector2 Empty = new Vector2();

		public static Vector2 Unit = "[1, 1]";

		public static Vector2 UnitX = "[1, 0]";

		public static Vector2 UnitY = "[0, 1]";

		#endregion

	}

	/// <summary>
	/// A vector is a immutable object containing a vector.
	/// NOTE: All methods in this struct assume a right-handed coordinate system.
	/// This means if you look at the XY plane, with the X-axis pointing to the 
	/// right and the Y-axis pointing up, the Z-axis will be pointing towards you
	/// </summary>
	public struct Vector3 {
		public double X {
			get {
				return _x;
			}
		}
		double _x;

		public double Y {
			get {
				return _y;
			}
		}
		double _y;


		public double Z {
			get {
				return _z;
			}
		}
		double _z;

		public Vector3(double x, double y, double z) {
			_x = x;
			_y = y;
			_z = z;
			_length = 0;
		}

		#region Operators
		public static Vector3 operator +(Vector3 p1, Vector3 p2) {
			return new Vector3(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
		}

		public static Vector3 operator -(Vector3 p1, Vector3 p2) {
			return new Vector3(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
		}

		public static Vector3 operator *(Vector3 p1, double n) {
			return new Vector3(p1.X * n, p1.Y * n, p1.Z * n);
		}

		public static Vector3 operator *(double n, Vector3 p1) {
			return p1 * n;
		}

		public static Vector3 operator /(Vector3 p1, double n) {
			return new Vector3(p1.X / n, p1.Y / n, p1.Z / n);
		}

		public static bool operator ==(Vector3 v1, Vector3 v2) {
			return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
		}

		public static bool operator !=(Vector3 v1, Vector3 v2) {
			return v1.X != v2.X || v1.Y != v2.Y || v1.Z != v2.Z;
		}

		/// <summary>
		/// Returns the dot product of the two fectors
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <returns></returns>
		public static double operator *(Vector3 p1, Vector3 p2) {
			return p1.X * p2.X + p1.Y * p2.Y + p1.Z * p2.Z;
		}

		/// <summary>
		/// Creates a vector from a string
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static implicit operator Vector3(string s) {
			s = s.Replace("[", "");
			s = s.Replace("]", "");
			s = s.Replace("(", "");
			s = s.Replace(")", "");
			string[] components = s.Trim().Split(new char[] { ',', ';' });
			if(components.Length != 3) {
				throw new ArgumentException("Can't create vector from string '" + s + "' The string contains too many components");
			} else {
				double x = double.Parse(components[0].Trim().Replace(",", "."));
				double y = double.Parse(components[1].Trim().Replace(",", "."));
				double z = double.Parse(components[2].Trim().Replace(",", "."));
				return new Vector3(x, y, z);
			}
		}

		#endregion

		#region Basic operations

		/// <summary>
		/// Returns a copy of this vector
		/// </summary>
		/// <returns></returns>
		public Vector3 Copy() {
			return new Vector3(X, Y, Z);
		}
		
		/// <summary>
		/// Calculates the length of this vector
		/// </summary>
		/// <returns></returns>
		public double Length {
			get {
				if(_length == 0) {
					_length = Math.Sqrt(this * this);
				}
				return _length;
			}
		}
		double _length;

		/// <summary>
		/// Returns a version of this vector with the Z component
		/// set to 0
		/// </summary>
		/// <returns></returns>
		public Vector3 FlattenX() {
			return new Vector3(0, Y, Z);
		}

		public Vector3 FlattenY() {
			return new Vector3(X, 0, Z);
		}

		public Vector3 FlattenZ() {
			return new Vector3(X, Y, 0);
		}

		public Point ToPointXY() {
			return new Point((int)X, (int)Y);
		}

		public Point ToPointXZ() {
			return new Point((int)X, (int)Z);
		}

		public Point ToPointYZ() {
			return new Point((int)Y, (int)Z);
		}
		


		/// <summary>
		/// Calculates the distance between this point and a specified other point
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public double DistanceTo(Vector3 p) {
			return (this - p).Length;
		}

		/// <summary>
		/// Determines if two vectors are perpendicular
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public bool IsPerpendicularTo(Vector3 v) {
			return (v * this) == 0;
		}

		/// <summary>
		/// Calculates the angle between this vector and another vector
		/// </summary>
		/// <returns>the angle in radians [0, 2pi]</returns>
		public double Angle(Vector3 v) {
			double dot = this * v;
			dot /= (v.Length * Length);
			return Math.Acos(dot);
		}

		/// <summary>
		/// Returns a normalized version of this vector
		/// </summary>
		/// <returns></returns>
		public Vector3 Normalize() {
			if(Length == 0) {
				return Vector3.Empty;
			} else {
				return this / Length;
			}
		}

		/// <summary>
		/// projects 'v' onto this vector.
		/// Remains the parameter 't', so that
		/// t * 'this vector' equals the projection
		/// of v onto this vector
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public double Project(Vector3 v) {
			return (this * v) / (this * this);
		}

		public override string ToString() {
			return "(" + X + " , " + Y + " , " + Z + ")";
		}

		/// <summary>
		/// Checks if the 2D projection if this vector
		/// lies within the specified rectangle
		/// </summary>
		/// <param name="rectangle"></param>
		/// <returns></returns>
		public bool Intersects(Rectangle2D rectangle) {
			if(X > rectangle.MaxX || X < rectangle.MinY) {
				return false;
			}
			if(Y > rectangle.MaxY || Y < rectangle.MinY) {
				return false;
			}
			return true;
		}



		public override int GetHashCode() {
			double xf = X % 2.0;
			double yf = Y % 2.0;
			double zf = Z % 2.0;
			double tf = ((xf + yf + zf) % 2.0) - 1.0;
			return (int)(2000000000.0 * tf);
		}
	
		#endregion

		#region Translations


		/// <summary>
		/// Rotates this vector around the X axis
		/// </summary>
		/// <param name="angle"></param>
		/// <returns></returns>
		public Vector3 RotateX(double a) {
			Matrix3 m = new Matrix3(
				new double[] { 1, 0          , 0 },
				new double[] { 0, Math.Cos(a), -Math.Sin(a) },
				new double[] { 0, Math.Sin(a), Math.Cos(a)  }
			);
			return Rotate(m);
		}

		/// <summary>
		/// Rotates this vector around the Y axis
		/// </summary>
		/// <param name="angle"></param>
		/// <returns></returns>
		public Vector3 RotateY(double a) {
			Matrix3 m = new Matrix3(
				new double[] { Math.Cos(a) , 0, Math.Sin(a), 0 },
				new double[] { 0           , 1, 0 },
				new double[] { -Math.Sin(a), 0, Math.Cos(a), 0 }
			);
			return Rotate(m);
		}

		/// <summary>
		/// Rotates this vector around the Z axis
		/// </summary>
		/// <param name="angle"></param>
		/// <returns></returns>
		public Vector3 RotateZ(double a) {
			Matrix3 m = new Matrix3(
				new double[] { Math.Cos(a), -Math.Sin(a), 0 },
				new double[] { Math.Sin(a), Math.Cos(a) , 0 },
				new double[] { 0          , 0           , 1 }
			);
			return Rotate(m);
		}

		/// <summary>
		/// Rotates this vector using a rotation matrix
		/// </summary>
		/// <param name="r"></param>
		/// <returns></returns>
		public Vector3 Rotate(Matrix3 r) {
			return new Vector3(
				r[0, 0] * X + r[0, 1] * Y + r[0, 2] * Z,
				r[1, 0] * X + r[1, 1] * Y + r[1, 2] * Z,
				r[2, 0] * X + r[2, 1] * Y + r[2, 2] * Z
			);
		}

		#endregion

		#region Constants
		public static Vector3 Empty = new Vector3();

		public static Vector3 Unit = "[1, 1, 1]";

		public static Vector3 UnitX = "[1, 0, 0]";

		public static Vector3 UnitY = "[0, 1, 0]";

		public static Vector3 UnitZ = "[0, 0, 1]";

		#endregion

	}

	/*
	public class Line2D : IIntersectsRectangle2D {
		public Vector2D Origin {
			get {
				return _origin;
			}
		}
		Vector2D _origin;
		public Vector2D Normal {
			get {
				return _normal;
			}
		}
		Vector2D _normal;
		public Vector2D End {
			get {
				return Origin + Normal;
			}
		}

		public Line2D(Vector2D origin, Vector2D normal) {
			_normal = normal;
			_origin = origin;
		}

		/// <summary>
		/// Projects a 
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public Vector2D Project(Vector2D v) {
			double t = Normal.Project(v - Origin);
			return Origin + t * Normal;
		}

		public bool Intersects(Line2D b) {
			throw new Exception("The method or operation is not implemented.");
		}

		public bool Intersects(Rectangle2D rectangle) {
			throw new Exception("The method or operation is not implemented.");
		}
	}
	 * */

	public class Line3D : IIntersectsRectangle2D {
		public Vector3 Origin {
			get {
				return _origin;
			}
		}
		Vector3 _origin;
		public Vector3 Normal {
			get {
				return _normal;
			}
		}
		Vector3 _normal;
		public Vector3 End {
			get {
				return Origin + Normal;
			}
		}

		public Line3D(Vector3 origin, Vector3 normal) {
			_normal = normal;
			_origin = origin;
		}

		/// <summary>
		/// Projects a vector on this line
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public Vector3 Project(Vector3 v) {
			double t = Normal.Project(v - Origin);
			return Origin + t * Normal;	
		}

		/// <summary>
		/// Does the 2D projection of this line intersect
		/// with the specified rectangle?
		/// </summary>
		/// <param name="rectangle"></param>
		/// <returns></returns>
		public bool Intersects(Rectangle2D rectangle) {
			return false;	
		}

	//	public bool TryIntersect(Rectangle2D rectangle, out Vector2D location, out LineIntersectionResult result) {
//
	//	}
	}

	public class Matrix3 {

		public Matrix3(double[] row1, double[] row2, double[] row3) {
			for(int column = 0; column < 3; column++) {
				this[0, column] = row1[column];
				this[1, column] = row2[column];
				this[2, column] = row3[column];
			}
		}

		/// <summary>
		/// Default indexer
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		public double this[int row, int column] {
			get {
				return _value[row, column];
			}
			set {
				_value[row, column] = value;
			}
		}

		private double[,] _value = new double[3, 3];
	}

	public enum LineIntersectionResult {
		None,
		Intersecting,
		Parallel,
		Skew
	}

	public class Plane3D : Line3D {
		public Plane3D(Vector3 origin, Vector3 normal) : base(origin, normal) {
		}

		/// <summary>
		/// Projects the point p onto this plane.
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public new Vector3 Project(Vector3 p) {
			Vector3 transformed = p - Origin;
			Line3D normalLine = new Line3D(Vector3.Empty, Normal);
			Vector3 projection = normalLine.Project(transformed);
			return p - projection;
		}
	}

	public struct Circle2 {

		/// <summary>
		/// Returns the center of this circle
		/// </summary>
		public Vector2 Center {
			get {
				return _center;
			}
		}
		Vector2 _center;

		/// <summary>
		/// X
		/// </summary>
		public double X {
			get {
				return _center.X;
			}
		}

		/// <summary>
		/// Y
		/// </summary>
		public double Y {
			get {
				return _center.Y;
			}
		}

		/// <summary>
		/// The radius of this circle
		/// </summary>
		public double R {
			get {
				return _r;
			}
		}
		double _r;

		public Circle2(double x, double y, double r) {
			_center = new Vector2(x, y);
			_r = r;
		}

		/// <summary>
		/// Returns true if this circle is inside the supplied circle
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public bool IsInside(Circle2 c) {
			if(Center.DistanceTo(c.Center) + R < c.R) {
				return true;
			} else {
				return false;
			}
		}

		/// <summary>
		/// Returns true if this circle is inside the supplied circle
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public bool IsOutside(Circle2 c) {
			if(Center.DistanceTo(c.Center) - R > c.R) {
				return true;
			} else {
				return false;
			}
		}

		/// <summary>
		/// Returns true if this circle is touching the supplied
		/// circle (i.e. the intersect at 1 point)
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public bool IsTouching(Circle2 c) {
			if(c.R + R == Center.DistanceTo(c.Center)) {
				return true;
			} else {
				return false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="c"></param>
		/// <param name="i1"></param>
		/// <param name="i2"></param>
		/// <returns></returns>
		public bool TryIntersect(Circle2 c, out Vector2 i1, out Vector2 i2) {
			i1 = Vector2.Empty;
			i2 = Vector2.Empty;
			if(IsInside(c) || c.IsInside(this)) {
				return false;
			} else if(IsOutside(c)) {
				return false;
			} else {
				double d = Center.DistanceTo(c.Center);
				double d2 = d * d;
				double dx = c.X - X;
				double dy = c.Y - Y;
				double dr = c.R - R;
				double sx = c.X + X;
				double sy = c.Y + Y;
				double sr = c.R + R;
				double K = Math.Sqrt((sr * sr - d2) * (d2 - dr * dr));
				double dr2 = (R * R - c.R * c.R);
				double x = 0.5 * sx + 0.5 * dx * dr2 / d2;
				double y = 0.5 * sy + 0.5 * dy * dr2 / d2;
				i1 = new Vector2(x + (0.5 * dy * K / d2), y - (0.5 * dx * K / d2));
				i2 = new Vector2(x - (0.5 * dy * K / d2), y + (0.5 * dx * K / d2));
				return true;
			}
		}

		public override string ToString() {
			return "(" + X + ", " + Y + "),R = " + R;
		}
	}

	
}
