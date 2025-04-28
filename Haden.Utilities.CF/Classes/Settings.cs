using System;
using System.Windows.Forms;
using System.Text;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;

namespace Haden.Utilities.CF {
	/// <summary>
	/// A subclass of this class should import System.ComponentModel to
	/// be able to use the proper attributes.
	/// 
	/// These are: 
	///		Browsable(bool) - Indicates if this setting shows up
	///                       in a PropertyGrid
	///     Description(string)
	///     Category(string)
	///     DefaultValue
	/// </summary>
	[Serializable]
	public class Settings {
		/// <summary>
		/// The filename to which this settings object is serialized
		/// </summary>
		protected string FileName {
			get {
				return getFileName(GetType());
			}
		}


		private ArrayList changed = new ArrayList();
		private Hashtable handler = new Hashtable();
		private XmlSerializer serializer;
		

		/// <summary>
		/// Constructs a Settings object
		/// </summary>
		public Settings() {
			serializer = new XmlSerializer(GetType());
		}

		#region Public interface

		public static Settings Load(Type type) {
			XmlSerializer s = new XmlSerializer(type);
			try {
				TextReader r = new StreamReader(getFileName(type));
				Settings result = (Settings)s.Deserialize( r );
				r.Close();
				return result;		
			}
			catch(FileNotFoundException) {
				return (Settings)Activator.CreateInstance(type);
			}
		}

		/// <summary>
		/// Saves the settings to disk
		/// </summary>
		public void Save() {
			// Serialization
			TextWriter w = new StreamWriter( FileName );
			serializer.Serialize( w, this );
			w.Close();
			if(Saved != null) {
				Saved(this);
			}
		}

		/// <summary>
		/// Copies this settings object
		/// </summary>
		/// <returns>a copy of the settings object</returns>
		public Settings Copy() {
			return (Settings)this.MemberwiseClone();
		}

		/// <summary>
		/// Displays a settings form
		/// </summary>
		/// <returns></returns>
		public Settings Form() {
			SettingsForm form = new SettingsForm();
			form.Settings = Copy();
			form.ShowDialog();
			if(form.Accepted) {
				form.Settings.Save();
				return form.Settings;
			} else {
				return this;
			}
		}

		public delegate void SettingsDelegate(Settings sender);

		public event SettingsDelegate Saved;

		#endregion
        
		#region Helper functions
		private static string getFileName(Type type) {
			return Application.StartupPath + "\\" + type.Name + ".xml";
		}
		#endregion
	}
}
