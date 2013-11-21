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

namespace Skyrim.Manager.Models
{
	using System;
	using System.CodeDom;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Xml.Serialization;

	public class CharacterManager : ObservableObject, IEnumerable<Character>
	{
		private ObservableCollection<Character> characters;
		private Character current;

		public CharacterManager()
		{
			Characters = new ObservableCollection<Character>();
		}

		public CharacterManager(IEnumerable<Character> characters)
		{
			Characters = new ObservableCollection<Character>(characters);
		}

		#region Properties

		[XmlElement("Characters")]
		public ObservableCollection<Character> Characters
		{
			get { return characters; }
			private set
			{
				characters = value;
				OnPropertyChanged();
			}
		}

		public int Count
		{
			get { return Characters.Count; }
		}

		/*
		[XmlAttribute("current")]
		public string CurrentCharacter
		{
			get { return Current.Name; }
			set { SetCurrentCharacterByName(value); }
		} */
		
		[XmlAttribute("current")]
		public Character Current
		{
			get { return current; }
			set
			{
				current = value;
				OnCurrentCharacterChangedEvent();
				OnPropertyChanged();
			}
		}

		#endregion

		#region Events

		public event EventHandler CurrentCharacterChangedEvent;

		#endregion

		public Character this[string characterName]
		{
			get
			{
				return Characters.SingleOrDefault(x => x.Name.Equals(characterName, StringComparison.InvariantCultureIgnoreCase));
			}
		}

		#region Methods

		public void Add(Character character)
		{
			Characters.Add(character);
		}

		public bool Contains(string characterName)
		{
			return Characters.Any(x => x.Name.Equals(characterName, StringComparison.InvariantCultureIgnoreCase));
		}

		protected virtual void OnCurrentCharacterChangedEvent()
		{
			var handler = CurrentCharacterChangedEvent;
			if (handler != null) handler(this, EventArgs.Empty);
		}

		protected void SetCurrentCharacterByName(string value)
		{
			var c = Characters.SingleOrDefault(x => x.Name.Equals(value, StringComparison.InvariantCultureIgnoreCase));
			if (c != null)
			{
				Current = c;
			}
		}

		#endregion

		#region Implementation of IEnumerable

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<Character> GetEnumerator()
		{
			return Characters.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}

