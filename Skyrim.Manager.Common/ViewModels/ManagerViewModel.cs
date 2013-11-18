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
namespace Skyrim.Manager.Common.ViewModels
{
	using System;
	using System.Collections.Generic;

	public class ManagerViewModel : ObservableObject
	{
		private SaveManager savesManager;

		private IList<String> characters;

		public IList<String> Characters
		{
			get { return savesManager.Characters; }
		}

	}
}