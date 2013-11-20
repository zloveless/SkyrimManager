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

			DataPathBrowseCommand = new RelayCommand(o => Config.Paths.GameDataPath = Browse(Config.Paths.GameDataPath), o => true);
			ExitCommand = new RelayCommand(shutdownMethod, o => true);
			InstallPathBrowseCommand = new RelayCommand(o => Config.Paths.InstallPath = Browse(Config.Paths.InstallPath), o => true);
			SaveCommand = new RelayCommand(o => ConfigViewModel.Save(config, config.FileName), o => true);
		}

		public ICommand DataPathBrowseCommand { get; private set; }
		public ICommand ExitCommand { get; private set; }
		public ICommand InstallPathBrowseCommand { get; private set; }
		public ICommand SaveCommand { get; private set; }

		public ConfigViewModel Config
		{
			get { return config; }
		}

		/// <summary>
		///	Gets or sets a <see cref="T:Skyrim.Manager.Models.Character" /> value representing the currently selected character.
		/// </summary>
		public Character Current
		{
			get { return current; }
			set
			{

				current = value;
				OnPropertyChanged();
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
