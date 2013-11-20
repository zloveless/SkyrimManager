// *********************************************************************
// [DCOM Productions]
// [Copyright (C) DCOM Productions All rights reserved.]
// *********************************************************************

namespace BitFlex.IO {
    using System;
    using System.IO;

    /// <summary>
    /// Provides a base definition for a local system file with basic information about the file object
    /// </summary>
    public abstract class FileSystemBase {
        /// <summary>
        /// Instantiates a new instance of the BitFlex.IO.FileSystemBase class.
        /// </summary>
        /// <param name="directory">The directory that this file resides in</param>
        /// <param name="name">The name of the file, with or without the extension</param>
        public FileSystemBase(String directory, String name) {
            m_Directory = directory;
            m_Name = name;
            m_FullName = Path.Combine(directory, name);
        }

        #region Properties

        private String m_Directory = String.Empty;
        /// <summary>
        /// Gets the full path of the directory that owns this file
        /// </summary>
        public String Directory {
            get {
                return m_Directory;
            }
        }

        /// <summary>
        /// Gets a System.Boolean value indicating whether this file exists
        /// </summary>
        public Boolean Exists {
            get {
                return File.Exists(FullName);
            }
        }

        private String m_FullName = String.Empty;
        /// <summary>
        /// Gets the full name of the file, including the full path and file extension
        /// </summary>
        public String FullName {
            get {
                return m_FullName;
            }
        }

        private String m_Name = String.Empty;
        /// <summary>
        /// Gets the name of the file, including the file extension
        /// </summary>
        public String Name {
            get {
                return m_Name;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Copies this file to the specified location on disk
        /// </summary>
        /// <param name="newDirectory">The new directory to copy the file to</param>
        /// <param name="overwrite">True to overwrite any existing file with the same name, otherwise false</param>
        public void Copy(String directory, Boolean overwrite) {
            File.Copy(FullName, Path.Combine(directory, Name), overwrite);
        }

        /// <summary>
        /// Deletes this file from the local hard disk
        /// </summary>
        public void Delete() {
            if (Exists) {
                File.Delete(FullName);
            }
        }

        /// <summary>
        /// Moves this file to the specified directory using the specified name
        /// </summary>
        /// <param name="directory">The new directory that will contain this file</param>
        /// <param name="name">The new name of this file</param>
        public void Move(String directory, String name) {
            File.Move(FullName, Path.Combine(directory, name));
        }

        /// <summary>
        /// Moves this file to the specified directory
        /// </summary>
        /// <param name="directory">The new directory that will contain this file</param>
        public void Move(String directory) {
            File.Move(FullName, Path.Combine(directory, Name));
        }

        /// <summary>
        /// Renames this file using the specified new name
        /// </summary>
        /// <param name="name">The new name of the file</param>
        public void Rename(String name) {
            File.Copy(FullName, Path.Combine(Directory, name));
            File.Delete(FullName);

            m_Name = name;
            m_FullName = Path.Combine(Directory, name);
        }

        #endregion
    }
}
