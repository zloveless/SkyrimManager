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
