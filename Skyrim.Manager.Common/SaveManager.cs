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

using System;
using System.Collections.Generic;

namespace Skyrim.Manager.Common
{
	public class SaveManager
	{
		public SaveManager()
		{
			Characters = new List<string>();
		}

		#region Public Properties

		public IList<String> Characters { get; set; }

		#endregion

		#region Public Methods

		public void Load()
		{
			Characters.Add(string.Empty);
		}

		#endregion

	}
}
