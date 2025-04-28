using System;
using System.Collections.Generic;
using System.Text;

namespace Haden.Utilities.CF {

	/// <summary>
	/// A vector is immutable
	/// </summary>
	public class Vector {
		/// <summary>
		/// returns a component of the vector
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public double this[int i] {
			get {
				return entry[i];
			}
		}
		double[] entry;

		/// <summary>
		/// Returns the dimensionality of this vector
		/// </summary>
		public int Dimensionality {
			get {
				return entry.Length;
			}
		}

		/// <summary>
		/// Returns the magnitude (length) of this vector
		/// </summary>
		public double Length {
			get {
				if(_length == -1) {
					_length = Math.Sqrt(this * this);
				}
				return _length;
			}
		}
		double _length = -1;

		public Vector(double[] values) {
			entry = values;
		}


		/// <summary>
		/// Returns a string representation of this vector
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			string result = "(";
			for(int i = 0; i < entry.Length; i++) {
				if(i > 0) {
					result += ", ";
				}
				result += this[i].ToString("0.00");
			}
			return result + ")";
		}

		#region operations

		public static double operator *(Vector v1, Vector v2) {
			if(v1.Dimensionality != v2.Dimensionality) {
				throw new InvalidOperationException("Can't take dot product of two vectors of different cardinality");
			}
			double result = 0;
			for(int i = 0; i < v1.Dimensionality; i++) {
				result += v1[i] * v2[i];
			}
			return result;
		}

		#endregion

	}
}
