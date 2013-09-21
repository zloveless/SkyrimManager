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

using NUnit.Framework;
using Should;
using Skyrim.Manager.Common;

namespace Skyrim.Manager.Tests.Saves
{
	// ReSharper disable InconsistentNaming
	// ReSharper disable PossibleNullReferenceException

	[TestFixture]
	public class SaveManagerFixture
	{
		[SetUp]
		public void Setup()
		{
			SUT = new SaveManager();
		}

		private SaveManager SUT;

		[Test]
		public void Load_Characters_CharactersNotEmpty()
		{
			// Arrange
			SUT.Load();

			// Assert
			SUT.Characters.ShouldNotBeEmpty();
		}
	}

	// ReSharper enable InconsistentNaming
	// ReSharper enable PossibleNullReferenceException
}
