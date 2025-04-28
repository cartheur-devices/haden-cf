using System;
using System.Collections.Generic;
using System.Text;

namespace Haden.Utilities.CF {
	/// <summary>
	/// This class can contain a number of classes.
	/// 
	/// An instance of this object will contain an instance
	/// of each of the classes. This can be convenient to store
	/// a discrete number of possible options, each encapsulated
	/// by a class.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Serializable]
	public class OptionRange<T> where T: class, new() {
		#region Instance interface

		public OptionRange() {
			foreach(string name in OptionTypes.Keys) {
				Type t = OptionTypes[name];
				T option = t.GetConstructor(Type.EmptyTypes).Invoke(null) as T;
				if(option == null) {
					throw new InvalidOperationException("Can't construct optionrange, unable to dynamically construct " + t.Name);
				}

				Options[name] = option;

				if(Option == null) {
					_option = option;	
				}
			}

			if(DefaultOptionName != "" && DefaultOptionName != null) {
				OptionName = DefaultOptionName;
			}
		}

		/// <summary>
		/// Sets the current option
		/// </summary>
		public T Option {
			get {
				return _option;
			}
			set {
				if(!Options.ContainsValue(value)) {
					throw new InvalidOperationException("Can't set option to " + value + "; this option does not exist in the Option Range.");
				}

				if(_option != value) {
					_option = value;
					if(OptionChanged != null) {
						OptionChanged(this, value);
					}
				}
			}
		}
		T _option;

		/// <summary>
		/// Sets the option name
		/// </summary>
		public string OptionName {
			set {
				if(!Options.ContainsKey(value)) {
					throw new InvalidOperationException("Can't set option to " + value + "; there is no option with that name in this Option Range.");
				}

				_option = Options[value];
			}
		}

		/// <summary>
		/// Returns the option objects in this OptionRange
		/// </summary>
		public Dictionary<string, T> Options {
			get {
				return _options;
			}
		}
		Dictionary<string, T> _options = new Dictionary<string, T>();



		#endregion

		#region Static Options

		/// <summary>
		/// Adds an option type
		/// </summary>
		/// <param name="name">name of the option</param>
		/// <param name="option"></param>
		public static void AddOptionType(string name, Type option) {
			_optionTypes[name] = option;
		}

		/// <summary>
		/// Adds the option with the default name
		/// </summary>
		/// <param name="option"></param>
		public static void AddOptionType(Type option) {
			_optionTypes[option.Name] = option;
		}

		/// <summary>
		/// Enumerates the classes of all options
		/// </summary>
		public static Dictionary<string, Type> OptionTypes {
			get {
				return _optionTypes;
			}
		}
		[NonSerialized]
		static Dictionary<string, Type> _optionTypes = new Dictionary<string, Type>();

		/// <summary>
		/// The defaultoption sets the name of the default option,
		/// </summary>
		public static string DefaultOptionName {
			get {
				return _defaultOption;
			}
			set {
				_defaultOption = value;
			}
		}
		[NonSerialized]
		static string _defaultOption;

		#endregion

		public event OptionRangeEvent OptionChanged;

		public delegate void OptionRangeEvent(OptionRange<T> range, T option);
	}

}
