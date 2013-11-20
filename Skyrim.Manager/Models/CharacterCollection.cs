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
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Xml.Serialization;

	[Serializable]
	public class CharacterCollection : IList<Character>, INotifyPropertyChanged
	{
		private readonly List<Character> characters;
		private Character current;

		public CharacterCollection()
		{
			characters = new List<Character>();
		}

		public CharacterCollection(IEnumerable<Character> characters)
		{
			this.characters = new List<Character>(characters);
		}

		#region Properties

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

		#region Methods

		public Character this[string characterName]
		{
			get
			{
				return !Contains(characterName)
					? null
					: characters.SingleOrDefault(x => x.Name.Equals(characterName, StringComparison.InvariantCultureIgnoreCase));
			}
		}

		public bool Contains(string characterName)
		{
			return characters.Any(x => x.Name.Equals(characterName, StringComparison.InvariantCultureIgnoreCase));
		}

		protected virtual void OnCurrentCharacterChangedEvent()
		{
			var handler = CurrentCharacterChangedEvent;
			if (handler != null) handler(this, EventArgs.Empty);
		}

		#endregion

		#region Implementation of INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

		#region Implementation of IEnumerable

		/// <summary>
		///     Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		///     A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<Character> GetEnumerator()
		{
			return characters.GetEnumerator();
		}

		/// <summary>
		///     Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		///     An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region Implementation of ICollection<Character>

		/// <summary>
		///     Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
		/// <exception cref="T:System.NotSupportedException">
		///     The <see cref="T:System.Collections.Generic.ICollection`1" /> is
		///     read-only.
		/// </exception>
		public void Add(Character item)
		{
			characters.Add(item);
		}

		/// <summary>
		///     Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">
		///     The <see cref="T:System.Collections.Generic.ICollection`1" /> is
		///     read-only.
		/// </exception>
		public void Clear()
		{
			characters.Clear();
		}

		/// <summary>
		///     Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
		/// </summary>
		/// <returns>
		///     true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />;
		///     otherwise, false.
		/// </returns>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
		public bool Contains(Character item)
		{
			return characters.Contains(item);
		}

		/// <summary>
		///     Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an
		///     <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
		/// </summary>
		/// <param name="array">
		///     The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied
		///     from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have
		///     zero-based indexing.
		/// </param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="array" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex" /> is less than 0.</exception>
		/// <exception cref="T:System.ArgumentException">
		///     The number of elements in the source
		///     <see cref="T:System.Collections.Generic.ICollection`1" /> is greater than the available space from
		///     <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.
		/// </exception>
		public void CopyTo(Character[] array, int arrayIndex)
		{
			characters.CopyTo(array, arrayIndex);
		}

		/// <summary>
		///     Removes the first occurrence of a specific object from the
		///     <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </summary>
		/// <returns>
		///     true if <paramref name="item" /> was successfully removed from the
		///     <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if
		///     <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </returns>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
		/// <exception cref="T:System.NotSupportedException">
		///     The <see cref="T:System.Collections.Generic.ICollection`1" /> is
		///     read-only.
		/// </exception>
		public bool Remove(Character item)
		{
			return characters.Remove(item);
		}

		/// <summary>
		///     Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </summary>
		/// <returns>
		///     The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </returns>
		public int Count
		{
			get { return characters.Count; }
		}

		/// <summary>
		///     Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
		/// </summary>
		/// <returns>
		///     true if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, false.
		/// </returns>
		public bool IsReadOnly
		{
			get { return false; }
		}

		#endregion

		#region Implementation of IList<Character>

		/// <summary>
		///     Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1" />.
		/// </summary>
		/// <returns>
		///     The index of <paramref name="item" /> if found in the list; otherwise, -1.
		/// </returns>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1" />.</param>
		public int IndexOf(Character item)
		{
			return characters.IndexOf(item);
		}

		/// <summary>
		///     Inserts an item to the <see cref="T:System.Collections.Generic.IList`1" /> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
		/// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///     <paramref name="index" /> is not a valid index in the
		///     <see cref="T:System.Collections.Generic.IList`1" />.
		/// </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1" /> is read-only.</exception>
		public void Insert(int index, Character item)
		{
			characters.Insert(index, item);
		}

		/// <summary>
		///     Removes the <see cref="T:System.Collections.Generic.IList`1" /> item at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///     <paramref name="index" /> is not a valid index in the
		///     <see cref="T:System.Collections.Generic.IList`1" />.
		/// </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1" /> is read-only.</exception>
		public void RemoveAt(int index)
		{
			characters.RemoveAt(index);
		}

		/// <summary>
		///     Gets or sets the element at the specified index.
		/// </summary>
		/// <returns>
		///     The element at the specified index.
		/// </returns>
		/// <param name="index">The zero-based index of the element to get or set.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///     <paramref name="index" /> is not a valid index in the
		///     <see cref="T:System.Collections.Generic.IList`1" />.
		/// </exception>
		/// <exception cref="T:System.NotSupportedException">
		///     The property is set and the
		///     <see cref="T:System.Collections.Generic.IList`1" /> is read-only.
		/// </exception>
		public Character this[int index]
		{
			get { return characters[index]; }
			set { characters[index] = value; }
		}

		#endregion
	}
}
