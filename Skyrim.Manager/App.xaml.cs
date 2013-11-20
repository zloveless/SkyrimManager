// ----------------------------------------------------------------
// Skyrim Manager
// Copyright (c) 2013. Zack "Genesis2001" Loveless.
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

namespace Skyrim.Manager
{
	using System;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.IO;
	using System.Reflection;
	using System.Windows;
	using Linq;
	using ViewModels;

	/// <summary>
	///     Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		private ConfigViewModel config;

		private string configPath;
		private ManagerViewModel context;

		#region Overrides of Application

		/// <summary>
		///     Raises the <see cref="E:System.Windows.Application.Startup" /> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.</param>
		protected override void OnStartup(StartupEventArgs e)
		{
			if (MainWindow == null)
			{
				MainWindow = new MainWindow();
			}

			var asm = Assembly.GetAssembly(typeof (App));
			var info = FileVersionInfo.GetVersionInfo(asm.Location);

			var applicationData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var configPathBase = Path.Combine(applicationData, info.ProductName);
			configPath = Path.Combine(configPathBase, "Settings.xml");

			if (!File.Exists(configPath))
			{
				if (!Directory.Exists(configPathBase))
				{
					Directory.CreateDirectory(configPathBase);
				}

				if (!asm.SaveResource("Settings.xml", configPath))
				{
					throw new FileFormatException("Unable to extract default Settings.xml from application manifest.");
				}
			}

			config = ConfigViewModel.Load(configPath);
			config.PropertyChanged += ConfigOnPropertyChanged;
			context = new ManagerViewModel(config, Disposal);

			MainWindow.DataContext = context;
			MainWindow.Show();
		}

		/// <summary>
		///     Raises the <see cref="E:System.Windows.Application.Exit" /> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.Windows.ExitEventArgs" /> that contains the event data.</param>
		protected override void OnExit(ExitEventArgs e)
		{
			Disposal(e);

			base.OnExit(e);
		}

		#endregion

		private void ConfigOnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (config.App.AutoSave)
			{
				ConfigViewModel.Save(config, config.FileName);
			}
		}

		private void Disposal(object obj)
		{
			ConfigViewModel.Save(config, config.FileName);

			Shutdown(0);
		}
	}
}
