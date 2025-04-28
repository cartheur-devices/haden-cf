#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Haden.Utilities.CF {
	/// <summary>
	/// This class encapsulates a high precision stopwatch
	/// </summary>
	public class StopWatch {
		private long start;
		private long mark;

		/// <summary>
		/// Returns the total number of milliseconds expired since
		/// the last Reset
		/// </summary>
		/// <value></value>
		public long Total {
			get {
				return _total;
			}
		}
		private long _total;


		/// <summary>
		/// Returns the total number of milliseconds expired since
		/// the last Mark
		/// </summary>
		/// <value></value>
		public long Round {
			get {
				return _round;
			}
		}
		private long _round;

		public StopWatch() {
			Reset();
		}


		public void Reset() {
			start = Util.MilliSeconds();
			mark = start;
			_total = 0;
			_round = 0;
		}

		public void Mark() {
			long previous = mark;
			mark = Util.MilliSeconds();
			_round = mark - previous;
			_total = mark - start;
		}
	}
}
