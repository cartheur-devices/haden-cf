#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Haden.Utilities.CF {
	/// <summary>
	/// This is a very simple string tokenizer
	/// </summary>
	public class StringTokenizer {
		private string[] token;
		private int current;

		public StringTokenizer(string str, string delimiter) {
			string[] del = new string[1];
			del[0] = delimiter;
			token = str.Split(del, StringSplitOptions.RemoveEmptyEntries);
		}

		/// <summary>
		/// returns true if the tokenizer has more tokens
		/// </summary>
		/// <returns></returns>
		public bool HasNext() {
			return current < token.Length;
		}

		/// <summary>
		/// returns the next token
		/// </summary>
		/// <returns></returns>
		public string Next() {
			if(HasNext()) {
				return token[current++];
			} else {
				return "";
			}
		}

		/// <summary>
		/// Returns next token without advancing the pointer 
		/// </summary>
		/// <returns></returns>
		public string Peek() {
			if(HasNext()) {
				return token[current];
			} else {
				return "";
			}
		}
	}
}
