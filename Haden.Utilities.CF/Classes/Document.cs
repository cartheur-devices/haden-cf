using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Windows.Forms;
using System.ComponentModel;

namespace Haden.Utilities.CF {
	/// <summary>
	/// This is the base class for a document
	/// in a Single Document interface
	/// </summary>
	[Serializable]
	public abstract class Document {
		public Document() {
		}

		#region Generic information about this file

		/// <summary>
		/// Contains the path this file was last saved to
		/// </summary>
		public string Path {
			get {
				return _path;
			}
			private set {
				_path = value;
			}
		}
		[NonSerialized]
		string _path;


		/// <summary>
		/// Creation date
		/// </summary>
		public DateTime Created {
			get {
				return _created;
			}
		}
		DateTime _created = DateTime.Now;


		/// <summary>
		/// Author of this 
		/// </summary>
		public string Author {
			get {
				return _author;
			}
			set {
				_author = value;
			}
		}
		string _author;

		#endregion

		#region Serialization

		/// <summary>
		/// This method is called after deserialization
		/// </summary>
		protected virtual void OnDeserialization() {
			if(Deserialized != null) {
				Deserialized(this);
			}
		}

		/// <summary>
		/// This event is fired after deserialization
		/// </summary>
		public static event DocumentEvent Deserialized;


		#endregion

		#region Public interface


		/// <summary>
		/// Creates a new document object from file
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static Document Open(string path) {
			BinaryFormatter formatter = new BinaryFormatter();
			TextReader r = null;
			try {
				Stream stream = File.OpenRead(path);
				Document result = (Document)formatter.Deserialize(stream);
				result.Path = path;
				result.OnDeserialization();
				return result;
			}
			catch(Exception e) {
				if(r != null) {
					r.Close();
				}
				throw e;
			}
		}

		/// <summary>
		/// Creates a new document object from file
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static T Open<T>(string path) where T : Document, new() {
			BinaryFormatter formatter = new BinaryFormatter();
			TextReader r = null;
			try {
				Stream stream = File.OpenRead(path);
				T result = (T)formatter.Deserialize(stream);
				result.Path = path;
				result.OnDeserialization();
				return result;
			}
			catch(Exception e) {
				if(r != null) {
					r.Close();
				}
				throw e;
			}
		}

		/// <summary>
		/// Saves a smobo object to a file
		/// </summary>
		/// <param name="path"></param>
		public void Save(string path) {
			StreamWriter w = null;
			try {
				BinaryFormatter formatter = new BinaryFormatter();
				Stream stream = File.Create(path);
				formatter.Serialize(stream, this);
				this.Path = path;
			}
			catch(Exception e) {
				if(w != null) {
					w.Close();
				}
				throw e;
			}
		}


		/// <summary>
		/// Saves the file
		/// </summary>
		public void Save() {
			Save(Path);
		}

/*
		/// <summary>
		/// Finds a document by walking up the control
		/// tree from the specified control, until a
		/// IDocumentContainer is found
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static Document Find(Control c) {
			Control current = c;
			while(c != null) {
				IDocumentContainer container = c as IDocumentContainer;
				if(container != null) {
					return container.Document;
				} else {
					c = c.Parent;
				}
			}
			return null;
		}

		/// <summary>
		/// Finds a document by walking up the control
		/// tree from the specified control, until a
		/// IDocumentContainer<T> is found
		/// </summary>
		/// <typeparam name="T">Desired document type</typeparam>
		/// <param name="c"></param>
		/// <returns></returns>
		public static T Find<T>(Control c) where T : Document, new() {
			while(c != null) {
				Form f = c as Form;
				
				if(container != null) {
					return container.Document;
				} else {
					c = c.Parent;
				}
			}
			return null;
		}*/

		#endregion

		#region Destruction

		~Document() {
			Dispose(false);
		}

		public void Dispose() {
			// dispose of the managed and unmanaged resources
			Dispose(true);
			// tell the GC that the Finalize process no longer needs
			// to be run for this object.
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) {
			if(!disposed) {
				disposed = true;
			}
		}

		bool disposed = false;
		#endregion

		#region Large Operations

		/// <summary>
		/// Starts a large operation
		/// </summary>
		/// <param name="status"></param>
		/// <param name="length"></param>
		protected virtual void OnOperationStart(string status, int length) {
			if(OperationStarted != null) {
				OperationStarted(this, status, length);
			}
		}

		public static event DocumentOperationStart OperationStarted;

		/// <summary>
		/// Sends a progress report about a large operation
		/// </summary>
		/// <param name="status"></param>
		/// <param name="length"></param>
		protected virtual void OnOperationProgressed(string status, int count) {
			if(OperationProgressed != null) {
				OperationProgressed(this, status, count);
			}
		}

		public static event DocumentOperationProgress OperationProgressed;

		/// <summary>
		/// Ends a large operation
		/// </summary>
		/// <param name="status"></param>
		/// <param name="length"></param>
		protected virtual void OnOperationStart() {
			if(OperationEnded != null) {
				OperationEnded(this);
			}
		}

		public static event DocumentOperationEnd OperationEnded;

		#endregion

	}

	public delegate void DocumentEvent(Document sender);

	public delegate void DocumentOperationStart(Document sender, string status, int length);
	
	public delegate void DocumentOperationProgress(Document sender, string status, int count);

	public delegate void DocumentOperationEnd(Document sender);


	public interface ISettingsContainer {
		/// <summary>
		/// Saves settings from GUI to backend store
		/// </summary>
		void Save();

		/// <summary>
		/// Loads settings from backend store to GUI
		/// </summary>
		void Load();
	}
}
