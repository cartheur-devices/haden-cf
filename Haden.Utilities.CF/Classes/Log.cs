#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion
using System.IO;
using System.Drawing;

namespace Haden.Utilities.CF {


	/// <summary>
	/// This class encapsulates a log.
	/// A log can have several targets
	/// </summary>
	public class Logger : IDisposable {

		/// <summary>
		/// Shoudl this log write to the console?
		/// </summary>
		/// <value></value>
		public bool WriteToConsole {
			get {
				return _writeToConsole;
			}
			set {
				_writeToConsole = value;
			}
		}
		private bool _writeToConsole = true;

		/// <summary>
		/// The component this log belongs to
		/// </summary>
		/// <value></value>
		public string Component {
			get {
				return _component;
			}
		}
		private string _component;

		/// <summary>
		/// The minimum level that should be logged
		/// </summary>
		/// <value></value>
		public LogLevel CutOff {
			get {
				return _cutOff;
			}
			set {
				_cutOff = value;
			}
		}
		private LogLevel _cutOff = LogLevel.None;

		/// <summary>
		/// Contains the store of all Loggers
		/// </summary>
		/// <value></value>
		private static Dictionary<string, Logger> LoggerStore {
			get {
				return _loggerStore;
			}
		}
		private static Dictionary<string, Logger> _loggerStore = new Dictionary<string, Logger>();
		
		/// <summary>
		/// Contains the store of all targets
		/// </summary>
		/// <value></value>
		private List<ILogTarget> Targets {
			get {
				return _targets;
			}
		}
		private List<ILogTarget> _targets = new List<ILogTarget>();


		/// <summary>
		/// Constructs a log
		/// </summary>
		public Logger(string component) {
			_component = component;
			_loggerStore.Add(component, this);
		}

		/// <summary>
		/// Adds an entry to the log
		/// </summary>
		/// <param name="entry"></param>
		public void LogEntry(LogEntry entry) {
			if(entry.Level > CutOff) {
				if(Logged != null) {
					Logged(entry);
				}

				if(Logged == null || WriteToConsole) {
					Console.WriteLine(entry.ToString());
				}
			}
		}

		/// <summary>
		/// Writes a message to the log
		/// </summary>
		/// <param name="component"></param>
		/// <param name="level"></param>
		/// <param name="message"></param>
		public void Log(string message, LogLevel level, string component) {
			LogEntry entry = new LogEntry();
			entry.Message = message;
			entry.Level = level;
			entry.Component = component;

			LogEntry(entry);
		}

		/// <summary>
		/// Logs a message with the specified level
		/// </summary>
		/// <param name="message"></param>
		/// <param name="level"></param>
		public void Log(string message, LogLevel level) {
			LogEntry entry = new LogEntry();
			entry.Message = message;
			entry.Level = level;

			LogEntry(entry);
		}

		/// <summary>
		/// Logs a message with the specified component
		/// </summary>
		/// <param name="message"></param>
		/// <param name="level"></param>
		public void Log(string message, string component) {
			LogEntry entry = new LogEntry();
			entry.Message = message;
			entry.Component = component;

			LogEntry(entry);
		}


		/// <summary>
		/// Logs a message with all default settings
		/// </summary>
		/// <param name="message"></param>
		public void Log(string message) {
			LogEntry entry = new LogEntry();
			entry.Message = message;

			LogEntry(entry);
		}

		/// <summary>
		/// Enumerates all logs
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<Logger> Loggers() {
			foreach(Logger log in LoggerStore.Values) {
				yield return log;
			}
		}

		/// <summary>
		/// Adds a log target to all registered logs.
		/// </summary>
		/// <param name="target"></param>
		public static void AddTargetToAll(ILogTarget target) {
			foreach(Logger log in Loggers()) {
				log.AddTarget(target);
			}
		}


		/// <summary>
		/// Adds a log target
		/// </summary>
		/// <param name="target"></param>
		public void AddTarget(ILogTarget target) {
			_targets.Add(target);
			target.Attach(this);
			Logged += new LogEvent(target.OnLogged);
		}

		public event LogEvent Logged;

		#region IDisposable Members

		public void Dispose() {
			foreach(ILogTarget target in Targets) {
				target.Dispose();
			}
		}

		#endregion
	}

	public interface ILogTarget : IDisposable {
		/// <summary>
		/// Implement this member to handle the log event
		/// </summary>
		/// <param name="timestamp"></param>
		/// <param name="component"></param>
		/// <param name="level"></param>
		/// <param name="message"></param>
		void OnLogged(LogEntry entry);

		/// <summary>
		/// Attaches this target to a logger.
		/// </summary>
		/// <param name="log"></param>
		void Attach(Logger log);
	}

	/*
	public abstract class IQueueLogTarget : ILogTarget {
		/// <summary>
		/// Can a log entry currently be written?
		/// </summary>
		/// <value></value>
		public abstract bool CanLog {
			get;
		}

		/// <summary>
		/// Flush queue if the queue size exceeds this value or the entry cannot be written
		/// </summary>
		/// <value></value>
		public int QueueSize {
			get {
				return _queueSize;
			}
		}
		private int _queueSize;


	}
	*/

	public class LogFile : OrganizedFile, ILogTarget {
		
		/// <summary>
		/// The stream object to write to
		/// </summary>
		/// <value></value>
		protected StreamWriter File {
			get {
				return _file;
			}
		}
		private StreamWriter _file;

		/// <summary>
		/// Creates a log file target
		/// </summary>
		/// <param name="append">Should the log file be appended every time?</param>
		public LogFile(bool append) : base("", append) {
		}

		public void OnLogged(LogEntry entry) {
			if(File != null) {
				File.WriteLine(entry.ToString());
			} else {
				throw new Exception("LogFile.OnLogged() before  LogFile.Attach()");
			}
		}

		/// <summary>
		/// Attaches this Log Target to a logger
		/// </summary>
		/// <param name="log"></param>
		public void Attach(Logger log) {
			_category = log.Component;
			openFile();
		}

		public void Dispose() {
			closeFile();
		}

		#region Private methods

		/// <summary>
		/// Opens the file stream
		/// </summary>
		private void openFile() {
			if(File != null) {
				closeFile();
			}

			CreatePath();
			_file = new StreamWriter(Path + "\\" + FileName, Append, System.Text.Encoding.ASCII);
			File.AutoFlush = true;
		}

		/// <summary>
		/// Closes the file stream
		/// </summary>
		private void closeFile() {
			if(File != null) {
				File.Flush();
				File.Close();
				_file = null;
			}
		}
		#endregion
	}

	public enum LogLevel {
		None = 0,
		Trivial = 1,
		Info = 2,
		Warning = 3,
		Normal = 4,
		ImportantInfo = 5,
		ImportantWarning = 6,
		Error = 7,
		Critical = 8
	}

	public class LogEntry {
		/// <summary>
		/// The time stamp of this log entry
		/// </summary>
		public DateTime TimeStamp = DateTime.Now;

		/// <summary>
		/// The level (importance of this entry)
		/// </summary>
		public LogLevel Level = LogLevel.Normal;

		/// <summary>
		/// The component this entry is associated with
		/// </summary>
		public string Component = "";

		/// <summary>
		/// The message
		/// </summary>
		public string Message = "";
		
		/// <summary>
		/// The color (not used in many instances)
		/// </summary>
		public Color Color;

		/// <summary>
		/// Returns a string representation of this log entry
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return Util.ISOTimeStamp(TimeStamp) + ",\"" + Component + "\"," + (int)Level + ",\"" + Message + "\"";
		}

	}

	public delegate void LogEvent(LogEntry entry);

}
