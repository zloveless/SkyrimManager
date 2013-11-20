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

namespace Skyrim.Manager.Linq
{
	using System;

	public static partial class Extensions
	{
		/// <summary>
		///     <para>Converts a <see cref="System.Int64" /> into a <see cref="System.DateTime" /> </para>
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static DateTime FromUnixTimestamp(this long source)
		{
			var origin = new DateTime(1970, 1, 1, 0, 0, 0);

			throw new NotImplementedException();
		}

		/// <summary>
		///     <para>Converts a <see cref="System.DateTime" /> into it's corresponding UNIX Timestamp.</para>
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static long ToUnixTimestamp(this DateTime source)
		{
			var origin = new DateTime(1970, 1, 1, 0, 0, 0);

			return Convert.ToInt64(Math.Floor((source.ToUniversalTime() - origin).TotalSeconds));
		}
	}
}
