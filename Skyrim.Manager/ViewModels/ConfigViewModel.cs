// ----------------------------------------------------------------
// The MIT License (MIT)
// 
// Copyright (c) 2013+ Zack Loveless
// Original author(s) for this source file: Zack Loveless
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ----------------------------------------------------------------

namespace Skyrim.Manager.ViewModels
{
	using System;
	using System.IO;
	using System.Security;
	using System.Text;
	using System.Xml;
	using System.Xml.Serialization;
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

		private ConfigViewModel()
		{
			App = new AppConfig();
			Characters = new CharacterCollection();
			Paths = new ConfigPath();
		}

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

		public void Install()
		{
			InitializeDefaults();
			InitializeCharacters();

			App.Installed = true;
		}

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

					Paths.InstallPath = Path.Combine(steamPath, @"steamapps\common\skyrim");
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

		public void InitializeCharacters()
		{
			if (!(Characters.Count > 0) && !App.Installed)
			{
				// TODO: Load characters from file system.
			}
		}

		public static ConfigViewModel Load(string configPath)
		{
			var serializer = new XmlSerializer(typeof (ConfigViewModel));
			using (var reader = XmlReader.Create(configPath))
			{
				var config = (ConfigViewModel)serializer.Deserialize(reader);
				config.FileName = configPath;

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
	}
}
