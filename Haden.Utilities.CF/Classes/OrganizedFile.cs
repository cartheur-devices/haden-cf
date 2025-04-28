using System;
using System.IO;


namespace Haden.Utilities.CF
{
	/// <summary>
	/// Summary description for OrganizedFile.
	/// </summary>
	public class OrganizedFile {
		
		#region Runtime properties

		/// <summary>
		/// The name of the category
		/// </summary>
		public string Category {
			get {
				return _category;
			}
		}
		protected string _category;

		/// <summary>
		/// ID (serial number)
		/// </summary>
		public int ID {
			get {
				return _id = 0;
			}
		}
		private int _id;

		public bool Append {
			get {
				return _append;
			}
		}
		private bool _append;

		/// <summary>
		/// the path to which the logfiles will be saved
		/// </summary>
		public string Path {
			get {
				string path = BasePath;
				path = BasePath;
				if(LogCategory != LogFileEnum.File) {
					path += "\\" + Category;
				}
				if(LogYear != LogFileEnum.File) {
					path += "\\" + Date.Year;
				}
				if(LogMonth != LogFileEnum.File) {
					path += "\\" + ("" + Date.Month).PadLeft(2, '0');
				}
				if(LogDay != LogFileEnum.File) {
					path += "\\" + ("" + Date.Day).PadLeft(2, '0');
				}
				if(LogID != LogFileEnum.File) {
					path += "\\" + ("" + ID).PadLeft(_idLength, '0');
				}
				return path;
			}
		}

		/// <summary>
		/// the filename of the current file
		/// </summary>
		public string FileName {
			get {
				string baseName = FileBaseName;
				string result;
				if(!Append) {
					if(LogID != LogFileEnum.Directory) {
						do {
							_id++;
							result = baseName + "-" + ("" + ID).PadLeft(_idLength, '0') + "." + Extension;
						} while(File.Exists(result));
						return result;
					}
				}
				result = baseName + "." + Extension;
				return result;
			}
		}

		public string FileBaseName {
			get {
				string result = Category;
				if(LogYear != LogFileEnum.Directory) {
					result += "-" + Date.Year;
				}
				if(LogMonth != LogFileEnum.Directory) {
					result += ("" + Date.Month).PadLeft(2, '0');
				}
				if(LogDay != LogFileEnum.Directory) {
					result += ("" + Date.Day).PadLeft(2, '0');
				}
				return result;
			}
		}

		/// <summary>
		/// The date to use for this file entry.
		/// Default to 'Now'
		/// </summary>
		public DateTime Date {
			get {
				return _date;
			}
			set {
				_date = value;
			}
		}
		private DateTime _date = DateTime.Now;

		#endregion

		#region Settings
		
		/// <summary>
		/// the path to which the logfiles will be saved
		/// </summary>
		public string BasePath {
			get {
				return _basePath;
			}
			set {
				_basePath = value;
			}
		}
		private string _basePath = System.Windows.Forms.Application.StartupPath;

		/// <summary>
		/// The extension (e.g. logfile)
		/// </summary>
		public string Extension {
			get {
				return _extension;
			}
			set {
				_extension = value.Replace(".", "");
			}
		}
		private string _extension;

		/// <summary>
		/// Determines whether the year should be reflected in the directory, the filename
		/// or both
		/// </summary>
		public LogFileEnum LogYear {
			get {
				return _logYear;
			}
			set {
				_logYear = value;
			}
		}
		private LogFileEnum _logYear = LogFileEnum.Both;

		/// <summary>
		/// Determines whether the Month should be reflected in the directory, the filename
		/// or both
		/// </summary>
		public LogFileEnum LogMonth {
			get {
				return _logMonth;
			}
			set {
				_logMonth = value;
			}
		}
		private LogFileEnum _logMonth = LogFileEnum.Both;

		/// <summary>
		/// Determines whether the Day should be reflected in the directory, the filename
		/// or both
		/// </summary>
		public LogFileEnum LogDay {
			get {
				return _logDay;
			}
			set {
				_logDay = value;
			}
		}
		private LogFileEnum _logDay = LogFileEnum.File;


		/// <summary>
		/// Determines whether the Category should be reflected in the directory, the filename
		/// or both
		/// </summary>
		public LogFileEnum LogCategory {
			get {
				return _logCategory;
			}
			set {
				_logCategory = value;
			}
		}
		private LogFileEnum _logCategory = LogFileEnum.Both;


		/// <summary>
		/// Determines whether the ID should be reflected in the directory, the filename
		/// or both
		/// </summary>
		public LogFileEnum LogID {
			get {
				return _logID;
			}
			set {
				_logID = value;
			}
		}
		private LogFileEnum _logID = LogFileEnum.File;

		/// <summary>
		/// Determines the length of the autonumber ID
		/// </summary>
		public int IDLength {
			get {
				return _idLength;
			}
			set {
				_idLength = value;
			}
		}
		private int _idLength = 4;

		#endregion

		/// <summary>
		/// Creates an organized file object
		/// </summary>
		/// <param name="category"></param>
		/// <param name="append"></param>
		public OrganizedFile(string category, bool append) {
			//
			// TODO: Add constructor logic here
			//
			_category = category;
			_append = append;
		}


		public void CreatePath() {
			Directory.CreateDirectory(Path);
		}
	}

	public enum LogFileEnum {
		File,
		Directory,
		Both
	}
}
