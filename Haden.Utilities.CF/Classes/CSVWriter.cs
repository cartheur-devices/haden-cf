#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

#endregion

// TODO: Make a Component from this CSV writer
namespace Haden.Utilities.CF {
	public class CSVWriter {
		/// <summary>
		/// The default indexer indexes the fields by their key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public CSVField this[string key] {
			get {
				return map[key];
			}
		}

		/// <summary>
		/// This property is set to false if changes to the CSV structure
		/// are still allowed.
		/// </summary>
		/// <value></value>
		public bool IsCommitted {
			get {
				return _isCommitted;
			}
		}
		private bool _isCommitted = false;

		/// <summary>
		/// Gets or sets the delimiter
		/// </summary>
		/// <value></value>
		public string Delimiter {
			get {
				return _delimiter;
			}
			set {
				_delimiter = value;
			}
		}
		private string _delimiter = "\"";

		/// <summary>
		/// Gets or sets the separator
		/// </summary>
		/// <value></value>
		public string Separator {
			get {
				return _separator;
			}
			set {
				_separator = value;
			}
		}
		private string _separator = ",";

		private List<CSVField> fields = new List<CSVField>();
		private Dictionary<string, CSVField> map = new Dictionary<string, CSVField>();

		public CSVWriter() {
		}

		private void Commit() {
			lock(this) {
				_isCommitted = true;
			}
		}

		/// <summary>
		/// Adds a field
		/// </summary>
		/// <param name="field"></param>
		public void Add(CSVField field) {
			fields.Add(field);
			map[field.Key] = field;
		}

		/// <summary>
		/// Returns a string representation of the current row.
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			Commit();

			StringBuilder builder = new StringBuilder();
			bool first = true;
			foreach(CSVField field in fields) {
				if(first) {
					first = false;
				} else {
					builder.Append(Separator);
				}
				builder.Append(Delimiter);
				builder.Append(field.ToString());
				builder.Append(Delimiter);
			}

			return builder.ToString();
		}

		/// <summary>
		/// Returns a string representation of the current header
		/// </summary>
		/// <returns></returns>
		public string Header() {
			Commit();

			StringBuilder builder = new StringBuilder();
			bool first = true;
			foreach(CSVField field in fields) {
				if(first) {
					first = false;
				} else {
					builder.Append(Separator);
				}
				builder.Append(Delimiter);
				builder.Append(field.Key);
				builder.Append(Delimiter);
			}

			return builder.ToString();
		}

		public void Reset() {
			foreach(CSVField field in fields) {
				field.Reset();
			}
		}
	}

	public class CSVFileWriter : CSVWriter, IDisposable {
		public bool Append {
			get {
				return _append;
			}
		}
		private bool _append;
		private StreamWriter writer;

		public CSVFileWriter(string filename) : base() {
			if(File.Exists(filename)) {
				_append = true;
			} else {
				_append = false;
			}
			writer = new StreamWriter(filename, true);
		}

		public void Write() {
			if(!Append) {
				writer.WriteLine(Header());
				_append = true;
			}
			writer.WriteLine(ToString());
		}

		public void Dispose() {
			writer.Close();
			writer = null;
		}
	}

	public class CSVField {
		/// <summary>
		/// The key this field is identified by
		/// </summary>
		/// <value></value>
		public string Key {
			get {
				return _key;
			}
		}
		private string _key;

		/// <summary>
		/// Can this value be set?
		/// </summary>
		/// <value></value>
		public virtual bool IsReadOnly {
			get {
				return false;
			}
		}

		/// <summary>
		/// Can this value be set?
		/// </summary>
		/// <value></value>
		public virtual bool IsPersistent {
			get {
				return IsReadOnly || _isPersistent;
			}
			set {
				_isPersistent = value;
			}
		}
		private bool _isPersistent;

		/// <summary>
		/// Gets the default value
		/// </summary>
		/// <value></value>
		public virtual object DefaultValue {
			get {
				return _defaultValue;
			}
		}
		private object _defaultValue;

		/// <summary>
		/// Gets the value
		/// </summary>
		/// <value></value>
		public virtual object Value {
			get {
				return _value;
			}
			set {
				if(IsReadOnly) {
					throw new NotSupportedException("Can't set this field, this field is read-only.");
				}

				_value = value;
			}
		}
		private object _value;

		/// <summary>
		/// Returns the delimiter of this field
		/// </summary>
		/// <value></value>
		public virtual string Delimiter {
			get {
				return "\"";
			}
		}


		/// <summary>
		/// Constructs a CSVField
		/// </summary>
		/// <param name="key"></param>
		public CSVField(string key) {
			_key = key;
		}

		/// <summary>
		/// Constructs a field with the specified default value
		/// </summary>
		/// <param name="key"></param>
		/// <param name="defaultValue"></param>
		public CSVField(string key, object defaultValue) : this(key) {
			_defaultValue = defaultValue;
		}

		/// <summary>
		/// Resets this field
		/// </summary>
		public virtual void Reset() {
			if(!IsPersistent) {
				_value = _defaultValue;
			}
		}

		/// <summary>
		/// Returns a string representation of the contained value
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			if(Value == null) {
				return "";
			} else {
				return Value.ToString();
			}
		}
	}

	public class CSVTimeStampField : CSVField {
		long mark;

		public CSVTimeStampField(string key) : base(key) {
		}

		public override void Reset() {
			mark = Util.MilliSeconds();
		}

		public override string ToString() {
			long difference = Util.MilliSeconds() - mark;
			return "" + difference;
		}
	}

	public class CSVAutoIncrementField : CSVField {
		public int Increment {
			get {
				return _increment;
			}
			set {
				_increment = value;
			}
		}
		private int _increment = 1;

		public CSVAutoIncrementField(string key) : base(key) {
			Value = 1;
		}

		public override string ToString() {
			int current = (int)Value;
			Value = current + Increment;
			return "" + current;
		}
	}

}
