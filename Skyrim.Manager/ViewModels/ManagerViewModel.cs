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
	using System.Windows.Forms;
	using System.Windows.Input;
	using Commands;
	using Models;

	public class ManagerViewModel : ObservableObject
	{
		private readonly ConfigViewModel config;
		private Character current;

		public ManagerViewModel(ConfigViewModel config, Action<Object> shutdownMethod)
		{
			this.config = config;

			ExitCommand = new RelayCommand(shutdownMethod, o => true);
			DataPathBrowseCommand = new RelayCommand(o => Config.Paths.GameDataPath = Browse(Config.Paths.GameDataPath), o => true);
			InstallPathBrowseCommand = new RelayCommand(o => Config.Paths.InstallPath = Browse(Config.Paths.InstallPath), o => true);
			SaveCommand = new RelayCommand(o => ConfigViewModel.Save(config, config.FileName), o => true);
			ShowAboutDialogCommand = new RelayCommand(ShowAboutDialog, o => true);
		}

		public ICommand DataPathBrowseCommand { get; private set; }
		public ICommand ExitCommand { get; private set; }
		public ICommand InstallPathBrowseCommand { get; private set; }
		public ICommand SaveCommand { get; private set; }
		public ICommand ShowAboutDialogCommand { get; private set; }

		public ConfigViewModel Config
		{
			get { return config; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		public void ShowAboutDialog(object obj)
		{
			using (var dialog = new AboutWindow())
			{
				dialog.ShowInTaskbar = false;
				dialog.ShowDialog();
			}
		}

		/// <summary>
		///     Browses for a folder on the file system and returns the path as a <see cref="T:System.String" />
		/// </summary>
		/// <param name="defaultPath"></param>
		/// <returns></returns>
		public string Browse(string defaultPath = "")
		{
			using (var dialog = new FolderBrowserDialog())
			{
				dialog.RootFolder = Environment.SpecialFolder.MyComputer;
				dialog.SelectedPath = defaultPath;

				return (dialog.ShowDialog() == DialogResult.OK) ? dialog.SelectedPath : defaultPath;
			}
		}
	}
}
