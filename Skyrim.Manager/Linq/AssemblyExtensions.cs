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
	using System.IO;
	using System.Linq;
	using System.Reflection;

	public static partial class Extensions
	{
		/// <summary>
		///     <para>Extracts the specified resource from the assembly and writes it out to the specified target file.</para>
		/// </summary>
		/// <param name="source"></param>
		/// <param name="resourceName"></param>
		/// <param name="targetFile"></param>
		/// <returns></returns>
		/// <exception cref="InvalidResourceException" />
		public static bool SaveResource(this Assembly source, string resourceName, string targetFile)
		{
			var resources = source.GetManifestResourceNames();
			var resource = resources.SingleOrDefault(r => r.EndsWith(resourceName, StringComparison.CurrentCultureIgnoreCase));

			using (var s = source.GetManifestResourceStream(resource))
			{
				if (s == null)
				{
					throw new InvalidResourceException(resource, "The specified resource could not be loaded.");
				}

				var buffer = new byte[s.Length];
				s.Read(buffer, 0, buffer.Length);

				using (var writer = new BinaryWriter(File.Open(targetFile, FileMode.Create)))
				{
					writer.Write(buffer);
				}
			}

			return (File.Exists(targetFile) && (new FileInfo(targetFile).Length > 0));
		}

		public static Stream GetResource(this Assembly source, string resourceName)
		{
			var resources = source.GetManifestResourceNames();
			var resource = resources.SingleOrDefault(r => r.EndsWith(resourceName, StringComparison.CurrentCultureIgnoreCase));

			return source.GetManifestResourceStream(resource);
		}
	}

	public class InvalidResourceException : Exception
	{
		#region Constructors

		public InvalidResourceException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public InvalidResourceException(string resourceName, string message)
			: this(resourceName, message, null)
		{
		}

		public InvalidResourceException(string resourceName, string message, Exception innerException)
			: this(message, innerException)
		{
			Resource = resourceName;
		}

		public InvalidResourceException(string assemblyName, string resourceName, string message, Exception innerException)
			: this(resourceName, message, innerException)
		{
			Assembly = assemblyName;
		}

		#endregion

		#region Properties

		/// <summary>
		///     <para>Gets the assembly name of the invalid resource.</para>
		/// </summary>
		public string Assembly { get; private set; }

		/// <summary>
		///     <para>Gets the resource name that is invalid.</para>
		/// </summary>
		public string Resource { get; private set; }

		#endregion
	}
}
