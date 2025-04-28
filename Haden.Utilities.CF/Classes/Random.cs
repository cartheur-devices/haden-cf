#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Haden.Utilities.CF {
	public class Distribution<T> {
		private float max;
		private Dictionary<T, Entry> categories = new Dictionary<T, Entry>();

		/// <summary>
		/// Returns a random category according to the current distribution
		/// </summary>
		/// <value></value>
		public T Random {
			get {
				float random = max * (float)Util.Random.NextDouble();
				foreach(Entry entry in categories.Values) {
					if(random < entry.CumulativeProportion) {
						return entry.Category;
					}
				}
				throw new Exception("This should not happen - random value larger than largest cumulative value");
			}
		}

		/// <summary>
		/// Adds a category with a given proportion
		/// </summary>
		/// <param name="category"></param>
		/// <param name="proportion"></param>
		public void AddCategory(T category, float proportion) {
			if(categories.ContainsKey(category)) {
				throw new Exception("Category already exists");
			}

			Entry entry = new Entry();
			max += proportion;
			entry.Proportion = proportion;
			entry.Category = category;
			entry.CumulativeProportion = max;

			categories.Add(category, entry);
		}

		/// <summary>
		/// Updates the proportion of a given category.
		/// </summary>
		/// <param name="category"></param>
		/// <param name="proportion"></param>
		public void UpdateCategory(T category, float proportion) {
			Entry entry;
			if(categories.TryGetValue(category, out entry)) {
				entry.Proportion = proportion;
				updateProportions();
			} else {
				throw new Exception("Category does not exist");
			}
		}

		private void updateProportions() {
			max = 0;
			foreach(Entry category in categories.Values) {
				max += category.Proportion;
				category.CumulativeProportion = max;
			}
		}


		class Entry {
			public T Category;
			public float Proportion;
			public float CumulativeProportion;
		}
	}
}
