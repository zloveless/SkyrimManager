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
