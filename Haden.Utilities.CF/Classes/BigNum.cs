#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Haden.Utilities.CF.CF {
	public class BigNum : ICloneable {
		#region Private parts
		private const int MAX_DIGITS = 200;

		private int[] _digit = new int[MAX_DIGITS];
		#endregion

		#region Public Properties
		/// <summary>
		/// Returns the digit at index 'index'.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public int this[int index] {
			get {
				index -= _exponent;
				index = -1 - index;
				if(index < 0 || index >= MAX_DIGITS) {
					return 0;
				} else {
					return _digit[index];
				}
			}
		}

		/// <summary>
		/// What is the exponent of this value?
		/// </summary>
		/// <value></value>
		public int Exponent {
			get {
				return _exponent;
			}
		}
		private int _exponent;

		/// <summary>
		/// Is this value negative?
		/// </summary>
		/// <value></value>
		public bool Negative {
			get {
				return _negative;
			}
		}
		private bool _negative;

		/// <summary>
		/// Returns the number of digits
		/// 
		/// i.e.: 
		///   if the number is 0.81, the value of LastDigit is '2'.
		///   if the number is 83.2, the value of LastDigit is '3', since internally this is represented as 0.832 * 10^2
		/// </summary>
		/// <value></value>
		public int Digits {
			get {
				int result = 0;
				for(int i = 0; i < MAX_DIGITS; i++) {
					if(_digit[i] != 0) {
						result = i + 1;
					}
				}
				return result;
			}
		}

		#endregion

		#region Constructors
		/// <summary>
		/// Constructs a BigNum with value 0
		/// </summary>
		public BigNum() {
		}

		/// <summary>
		/// Constructs a BigNum with the value contained in 'number'
		/// </summary>
		/// <param name="number"></param>
		public BigNum(string number) {
			number = number.Trim();

			if(number == "") {
				return;	// default is 0
			}

			if(number[0] == '-') {
				_negative = true;
				number = number.Substring(1);
			}

			int dotIndex = number.IndexOf('.');
			if(dotIndex == -1) {
				_exponent = number.Length;
			} else {
				if(number.IndexOf('.') != number.LastIndexOf('.')) {
					throw new Exception("Can't parse this number");
				}
			}

			int index = 0;
			for(int i = 0; i < number.Length; i++) {
				if(number[i] == '.') {
					_exponent = i;
				} else {
					_digit[index++] = Int32.Parse("" + number[i]);
				}
			}

			Normalize();
		}
		#endregion

		#region Public Interface

		#region Representation
		/// <summary>
		/// Returns a numeric representation of the BigNum
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return GetShiftedLeft(0);
		}

		/// <summary>
		/// Returs the number, shifted by some digits.
		/// </summary>
		/// <param name="digits">The number of digits to shift. shift(2) would return the number multiplied by 100</param>
		/// <returns></returns>
		public string GetShiftedLeft(int digits) {
			StringBuilder builder = new StringBuilder();
			if(Negative) {
				builder.Append("-");
			}

			if(Exponent == -digits) {
				builder.Append("0");
			}

			if(Exponent < -digits) {
				builder.Append("0.");
				for(int i = 0; i < Math.Abs(Exponent + digits); i++) {
					builder.Append("0");
				}
			}

			for(int i = 0; i < Math.Max(Digits, Exponent); i++) {
				if(i == Exponent + digits) {
					builder.Append(".");
				}
				builder.Append(_digit[i]);
			}
			return builder.ToString();
		}
		#endregion

		/// <summary>
		/// Clones this object
		/// </summary>
		/// <returns></returns>
		public object Clone() {
			BigNum n = new BigNum();
			n._negative = Negative;
			n._exponent = Exponent;
			for(int i = 0; i < MAX_DIGITS; i++) {
				n._digit[i] = _digit[i];
			}
			return n;
		}

		#region Mathematical operations
		/// <summary>
		/// Shifts left the digits (i.e. multiplies by 10^digits)
		/// </summary>
		/// <param name="digits"></param>
		public void ShiftLeft(int digits) {
			if(_digit[0] == 0) {
				for(int i = 0; i < MAX_DIGITS - digits; i++) {
					_digit[i] = _digit[i + digits];
				}

				for(int i = MAX_DIGITS - digits; i < MAX_DIGITS; i++) {
					_digit[i] = 0;
				}
			} else {
				_exponent++;
			}
		}

		/// <summary>
		/// Addition
		/// </summary>
		/// <param name="n1"></param>
		/// <param name="n2"></param>
		/// <returns></returns>
		public static BigNum operator +(BigNum n1, BigNum n2) {
			if(n1.Negative && n2.Negative) {
				return -((-n1) + (-n2));
			}

			if(n1.Negative && !n2.Negative) {
				return n2 - (-n1);
			}

			if(!n1.Negative && n2.Negative) {
				return n1 - (-n2);
			}

			int start = Math.Max(n1.Exponent, n2.Exponent) + 1;
			int carry = 0;

			BigNum result = new BigNum();
			result._exponent = start;

			int j = MAX_DIGITS - 2;
			for(int i = start - (MAX_DIGITS - 1); i < start; i++) {
				int addition = n1[i] + n2[i] + carry;
				if(addition > 9) {
					carry = 1;
					addition -= 10;
				} else {
					carry = 0;
				}
				result._digit[j] = addition;
				j--;
			}
			result._digit[0] = carry;
			result.Normalize();
			return result;
		}

		/// <summary>
		/// Subtraction
		/// </summary>
		/// <param name="n1"></param>
		/// <param name="n2"></param>
		/// <returns></returns>
		public static BigNum operator -(BigNum n1, BigNum n2) {
			if(n1.Negative && n2.Negative) {
				return -((-n1) - (-n2));
			}

			if(n1.Negative && !n2.Negative) {
				return -(n2 + (-n1));
			}

			if(!n1.Negative && n2.Negative) {
				return n1 + (-n2);
			}

			if(n1 < n2) {
				return -(n2 - n1);
			}

			int start = Math.Max(n1.Exponent, n2.Exponent) + 1;
			int borrow = 0;

			BigNum result = new BigNum();
			result._exponent = start;

			int j = MAX_DIGITS - 2;
			for(int i = start - (MAX_DIGITS - 1); i < start; i++) {
				int subtraction = n1[i] - (n2[i] + borrow);

				if(subtraction < 0) {
					borrow = 1;
					subtraction += 10;
				} else {
					borrow = 0;
				}
				result._digit[j] = subtraction;
				j--;
			}

			result.Normalize();
			return result;
		}

		/// <summary>
		/// Returns a negated copy of the specified number
		/// </summary>
		/// <param name="n1"></param>
		/// <returns></returns>
		public static BigNum operator -(BigNum n1) {
			BigNum result = (BigNum)n1.Clone();
			result._negative = !result._negative;
			return result;
		}

		#endregion

		#region Comparators
		public static bool operator <(BigNum n1, BigNum n2) {
			if(n1.Exponent < n2.Exponent) {
				return true;
			}

			if(n1.Exponent > n2.Exponent) {
				return false;
			}

			for(int i = 0; i < MAX_DIGITS; i++) {
				if(n1._digit[i] < n2._digit[i]) {
					return true;
				}

				if(n1._digit[i] > n2._digit[i]) {
					return false;
				}
			}

			return false;
		}

		public static bool operator >(BigNum n1, BigNum n2) {
			if(n1.Exponent > n2.Exponent) {
				return true;
			}

			if(n1.Exponent < n2.Exponent) {
				return false;
			}

			for(int i = 0; i < MAX_DIGITS; i++) {
				if(n1._digit[i] > n2._digit[i]) {
					return true;
				}

				if(n1._digit[i] < n2._digit[i]) {
					return false;
				}
			}

			return false;
		}

		public static bool operator <=(BigNum n1, BigNum n2) {
			return !(n1 > n2);
		}

		public static bool operator >=(BigNum n1, BigNum n2) {
			return !(n1 < n2);
		}

		#endregion

		#endregion

		#region helper functions
		/// <summary>
		/// Normalizes thebignum
		/// </summary>
		private void Normalize() {
			if(Digits > 0) {
				while(_digit[0] == 0) {
					ShiftLeft(1);
					_exponent--;
				}
			}
		}

		#endregion
	}

}
