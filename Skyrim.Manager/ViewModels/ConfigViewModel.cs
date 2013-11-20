// ----------------------------------------------------------------
// Skyrim Manager
// Copyright (c) 2013. Zack Loveless.
// 
// Original author(s) for this source file: Zack Loveless
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// ----------------------------------------------------------------

namespace Skyrim.Manager.ViewModels
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Security;
	using System.Text;
	using System.Xml;
	using System.Xml.Serialization;
	using BitFlex.IO;
	using Ionic.Zip;
	using Ionic.Zlib;
	using Linq;
	using Microsoft.Win32;
	using Models;

	[Serializable]
	[XmlRoot("Settings")]
	public class ConfigViewModel : ObservableObject
	{
		private AppConfig app;
		private string applicationData;
		private CharacterCollection characters;
		private string fileName;
		private ConfigPath paths;

		private InitializationFile ini;

		private ConfigViewModel()
		{
			App = new AppConfig();
			Characters = new CharacterCollection();
			Paths = new ConfigPath();
			
			Characters.CurrentCharacterChangedEvent += CharacterChangedCallback;
		}
		
		#region Properties

		[XmlIgnore]
		public string ApplicationData
		{
			get { return applicationData; }
			set
			{
				applicationData = value;
				OnPropertyChanged();
			}
		}

		[XmlIgnore]
		public string FileName
		{
			get { return fileName; }
			set
			{
				fileName = value;
				OnPropertyChanged();
			}
		}

		[XmlElement("App")]
		public AppConfig App
		{
			get { return app; }
			set
			{
				app = value;
				OnPropertyChanged();
			}
		}

		[XmlElement("Paths")]
		public ConfigPath Paths
		{
			get { return paths; }
			set
			{
				paths = value;
				OnPropertyChanged();
			}
		}

		[XmlElement("Characters")]
		public CharacterCollection Characters
		{
			get { return characters; }
			set
			{
				characters = value;
				OnPropertyChanged();
			}
		}

		#endregion

		#region Methods

		/// <summary>
		///     Performs a one-time installation setup to configure and Skyrim Manager
		/// </summary>
		public void Install()
		{
			if (!App.Installed)
			{
				InitializeDefaults();
				InitializeCharacters();

				App.Installed = true;
			}
		}

		/// <summary>
		///     Initializes paths to their expected default values. Some variation is inevitable as Steam now allows installing
		///     games to different drives.
		/// </summary>
		public void InitializeDefaults()
		{
			if (App.Installed) return;

			var mydocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			Paths.GameDataPath = Path.Combine(mydocs, @"My Games\Skyrim");

			RegistryKey steamKey = null;
			try
			{
				steamKey = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam");
				if (steamKey == null)
				{
					// TODO: Report bug.
				}
				else
				{
					var steamPath = steamKey.GetValue("SteamPath").ToString();

					// sift the final resultant path through Path.GetFullPath as a way to normalize the slashes. Mostly a nit-pick at the time of applying this.
					Paths.InstallPath = Path.GetFullPath(Path.Combine(steamPath, @"steamapps\common\skyrim"));
				}
			}
			catch (SecurityException)
			{
				// TODO: Report bug.
			}
			finally
			{
				if (steamKey != null) steamKey.Close();
			}
		}

		protected void CharacterChangedCallback(object sender, EventArgs args)
		{
			// [General]
			// SLocalSavePath = Saves\<CharacterName>

			if (ini == null)
			{
				ini = new InitializationFile(Paths.GameDataPath, "Skyrim.ini");
				ini.Load();
			}

			ini.Sections["General"].Keys["SLocalSavePath"] = Path.Combine("Saves", Characters.Current.Name);
			ini.Save();
		}

		/// <summary>
		///     Archives the specified character, if provided, otherwise backs up the entire saves directory to the pre-defined
		///     backup directory.
		/// </summary>
		/// <param name="character"></param>
		public void Backup(Character character = null)
		{
			var archiveDirectory = Path.Combine(ApplicationData, "backups");
			if (!Directory.Exists(archiveDirectory))
			{
				Directory.CreateDirectory(archiveDirectory);
			}

			if (character == null)
			{
				var backupDirectory = Path.Combine(Paths.GameDataPath, "Saves");
				var archiveFile = Path.Combine(archiveDirectory,
					string.Format("Characters_FullBackup_{0}.zip", DateTime.Now.ToUnixTimestamp()));

				using (var zip = new ZipFile(archiveFile, Encoding.UTF8))
				{
					zip.CompressionLevel = CompressionLevel.BestSpeed;
					zip.UseZip64WhenSaving = Zip64Option.AsNecessary;
					zip.AddDirectory(backupDirectory, "Characters");
					zip.Save();
				}
			}
			else
			{
				// backup the individual character.
				throw new NotImplementedException();
			}
		}

		private string GetCharacterName(string savedGame)
		{
			// Credit: Ben "aca20031" Buzbee
			// http://www.benbuzbee.com
			var data = File.ReadAllBytes(savedGame);
			var nameLength = data[26] << 8 | data[25];

			return Encoding.UTF8.GetString(data, 27, nameLength);
		}

		public void InitializeCharacters()
		{
			if (!(Characters.Count > 0) && !App.Installed)
			{
				// Perform a full backup of all the existing saved games.
				Backup();

				var charactersDirectory = Path.Combine(Paths.GameDataPath, "Saves");
				var games = Directory.GetFiles(charactersDirectory, "*.ess");

				foreach (var item in games)
				{
					var cName = GetCharacterName(item);
					Character c = null;
					if (!Characters.Contains(cName))
					{
						c = new Character {Name = cName};
						Characters.Add(c);
					}

					if (c == null) c = Characters[cName];
					if (c != null) c.Saves.Add(item);
				}

				foreach (var c in Characters)
				{
					var cDirectory = Path.Combine(charactersDirectory, c.Name);
					if (!Directory.Exists(cDirectory))
					{
						Directory.CreateDirectory(cDirectory);
					}

					var saves = new List<String>();
					foreach (var s in c.Saves)
					{
						var fileInfo = new FileInfo(s);
						var sNew = Path.Combine(cDirectory, fileInfo.Name);

						saves.Add(sNew);
#if DEBUG
						File.Copy(s, sNew);
#else
						File.Move(s, sNew);
#endif
					}

					c.Saves.Clear();
					saves.ForEach(x => c.Saves.Add(x));
				}
			}
		}

		#endregion

		#region Factory Methods

		public static ConfigViewModel Load(string configPath)
		{
			var serializer = new XmlSerializer(typeof (ConfigViewModel));
			using (var reader = XmlReader.Create(configPath))
			{
				var config = (ConfigViewModel)serializer.Deserialize(reader);
				config.FileName = configPath;
				config.ApplicationData = Path.GetDirectoryName(configPath);

				if (!config.App.Installed)
				{
					config.Install();
				}

				return config;
			}
		}

		public static void Save(ConfigViewModel config, string configPath)
		{
			var serializer = new XmlSerializer(typeof (ConfigViewModel));
			var settings = new XmlWriterSettings {Indent = true, Encoding = Encoding.UTF8};

			using (var writer = XmlWriter.Create(configPath, settings))
			{
				serializer.Serialize(writer, config);
			}
		}

		#endregion

		#region Nested Type: AppConfig

		public class AppConfig : ObservableObject
		{
			private bool autoSave;
			private bool installed;
			private bool keepOpen;

			[XmlAttribute("installed")]
			public bool Installed
			{
				get { return installed; }
				set
				{
					installed = value;
					OnPropertyChanged();
				}
			}

			[XmlAttribute("remainOpen")]
			public bool KeepOpen
			{
				get { return keepOpen; }
				set
				{
					keepOpen = value;
					OnPropertyChanged();
				}
			}

			[XmlAttribute("autosave")]
			public bool AutoSave
			{
				get { return autoSave; }
				set
				{
					autoSave = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion

		#region Nested Type: ConfigPath

		public class ConfigPath : ObservableObject
		{
			private string gameDataPath;
			private string installPath;

			[XmlElement("Install")]
			public string InstallPath
			{
				get { return installPath; }
				set
				{
					installPath = value;
					OnPropertyChanged();
				}
			}

			[XmlElement("GameData")]
			public string GameDataPath
			{
				get { return gameDataPath; }
				set
				{
					gameDataPath = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion
	}
}
