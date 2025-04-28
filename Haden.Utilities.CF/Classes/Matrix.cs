using System;
using System.Collections.Generic;
using System.Text;

namespace Haden.Utilities.CF.Geometry {
	/// <summary>
	/// Contains a rows*columns matrix
	/// </summary>
	public class Matrix {

		/// <summary>
		/// Default accessor
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		public double this[int row, int column] {
			get {
				return entry[row, column];
			}
			set {
				_max = Math.Max(_max, value);
				_min = Math.Min(_min, value);
				entry[row, column] = value;
				maxLength = Math.Max(maxLength, value.ToString("0.00").Length);
				determinantCalculated = false;
			}
		}
		private double[,] entry;

		/// <summary>
		/// Contains the number of rows in this matrix
		/// </summary>
		public int Rows {
			get {
				return _rows;
			}
		}
		private int _rows = 0;

		/// <summary>
		/// Contains the number of columns in this matrix
		/// </summary>
		public int Columns {
			get {
				return _columns;
			}
		}
		private int _columns = 0;


		/// <summary>
		/// Contains the maximum value of this matrix
		/// </summary>
		public double Max {
			get {
				return _max;
			}
		}
		private double _max = double.MinValue;

		/// <summary>
		/// Contains the minimum value of this matrix
		/// </summary>
		public double Min {
			get {
				return _min;
			}
		}
		private double _min = double.MaxValue;

		private int maxLength = 0;


		public Matrix(int rows, int columns) {
			if(rows < 1 || columns < 1) {
				throw new ArgumentException("A matrix can't have fewer than 1 rows and/or columns.");
			}
			_rows = rows;
			_columns = columns;
			entry = new double[rows, columns];
		}

		public Matrix(double[, ] source) {
			_rows = source.GetLength(0);
			_columns = source.GetLength(1);
			entry = new double[Rows, Columns];
			for(int r = 0; r < Rows; r++) {
				for(int c = 0; c < Columns; c++) {
					this[r, c] = source[r, c];
				}
			}
		}

		/// <summary>
		/// Returns the specified column vector
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public Vector Column(int column) {
			double[] vector = new double[Rows];
			for(int r = 0; r < Rows; r++) {
				vector[r] = this[r, column];
			}
			return new Vector(vector);
		}
		/// <summary>
		/// Returns the specified row vector
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public Vector Row(int row) {
			double[] vector = new double[Columns];
			for(int c = 0; c < Columns; c++) {
				vector[c] = this[row, c];
			}
			return new Vector(vector);
		}

		/// <summary>
		/// Returns a string representation of this matrix
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			string result = "";
			for(int y = 0; y < Rows; y++) {
				result += "|";
				for(int x = 0; x < Columns; x++) {
					if(x > 0) {
						result += " ";
					}
					result += this[y, x].ToString("0.00").PadLeft(maxLength);
				}
				result += "|\r\n";
			}
			return result;
		}

		#region operations

		public static Matrix operator +(Matrix m1, Matrix m2) {
			if(m1.Columns != m2.Columns || m1.Rows != m2.Rows) {
				throw new InvalidOperationException("Cannot add matrix m1 and m2. they are not of the same size");
			}
			Matrix m3 = new Matrix(m1.Rows, m1.Columns);
			for(int r = 0; r < m3.Rows; r++) {
				for(int c = 0; c < m3.Columns; c++) {
					m3[r, c] = m1[r, c] + m2[r, c];
				}
			}
			return m3;
		}

		public static Matrix operator -(Matrix m1, Matrix m2) {
			if(m1.Columns != m2.Columns || m1.Rows != m2.Rows) {
				throw new InvalidOperationException("Cannot subtract matrix m2 from m1. they are not of the same size");
			}
			Matrix m3 = new Matrix(m1.Rows, m1.Columns);
			for(int r = 0; r < m3.Rows; r++) {
				for(int c = 0; c < m3.Columns; c++) {
					m3[r, c] = m1[r, c] - m2[r, c];
				}
			}
			return m3;
		}

		public static Matrix operator *(Matrix m1, Matrix m2) {
			if(m1.Columns != m2.Rows) {
				throw new InvalidOperationException("Cannot multiply matrix m1 and m2 if m1.Columns != m2.Rows.");
			}
			Matrix m3 = new Matrix(m1.Rows, m2.Columns);
			for(int r = 0; r < m3.Rows; r++) {
				for(int c = 0; c < m3.Columns; c++) {
					m3[r, c] = m1.Row(r) * m2.Column(c);
				}
			}
			return m3;
		}

		public static Matrix operator *(Matrix m1, double n) {
			Matrix m2 = new Matrix(m1.Rows, m1.Columns);
			for(int r = 0; r < m2.Rows; r++) {
				for(int c = 0; c < m2.Columns; c++) {
					m2[r, c] = m1[r, c] * n;
				}
			}
			return m2;
		}


		public static Matrix operator /(Matrix m1, double n) {
			Matrix m2 = new Matrix(m1.Rows, m1.Columns);
			for(int r = 0; r < m2.Rows; r++) {
				for(int c = 0; c < m2.Columns; c++) {
					m2[r, c] = m1[r, c] / n;
				}
			}
			return m2;
		}


		/// <summary>
		/// Calls the determinant of this matrix and stores the result
		/// </summary>
		/// <param name="sign"></param>
		/// <returns></returns>
		public double Determinant() {
			return Determinant(1.0);
		}

		/// <summary>
		/// Calls the determinant of this matrix and stores the result
		/// </summary>
		/// <param name="sign"></param>
		/// <returns></returns>
		private double Determinant(double sign) {
			if(!determinantCalculated) {
				if(Rows != Columns) {
					throw new InvalidOperationException("Can't take the determinant of a non-square matrix");
				}

				if(Rows == 1) {
					_determinant = sign * this[0, 0];
				} else if(Rows == 2) {
					double ad = this[0, 0] * this[1, 1];
					double bc = this[1, 0] * this[0, 1];
					_determinant = sign * ad - bc;
				} else {
					double result = 0;
					Matrix minor = new Matrix(Rows - 1, Rows - 1);
					for(int column = 0; column < Columns; column++) {
						for(int r = 0; r < Rows - 1; r++) {
							for(int c = 0; c < Columns - 1; c++) {
								if(c < column) {
									minor[r, c] = this[r + 1, c];
								} else {
									minor[r, c] = this[r + 1, c + 1];
								}
							}
						}
						result += sign * Math.Pow(-1, column) * this[0, column] * minor.Determinant(sign);
						sign = -sign;
					}
					_determinant = result;
				}
				determinantCalculated = true;
			}
			return _determinant;
		}
		double _determinant;
		bool determinantCalculated = false;

		#endregion
	}
}
