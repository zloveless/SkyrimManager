// *********************************************************************
// [DCOM Productions]
// [Copyright (C) DCOM Productions All rights reserved.]
// *********************************************************************

namespace BitFlex.IO {
    using System;

    /// <summary>
    /// Defines the basic methods and properties needed for a file that can load, edit, and save data
    /// </summary>
    public abstract class DataFileBase : FileSystemBase {
        /// <summary>
        /// Defines the abstract definition for the underlying constructor to the FileSystemBase class.
        /// </summary>
        /// <param name="directory">The directory that this file resides in</param>
        /// <param name="name">The name of this file, with or without the extension</param>
        protected DataFileBase(String directory, String name) 
            : base(directory, name) {
            //
            // TODO: Any constructor code here
            //
        }

        #region Methods

        public abstract void Load();
        public abstract void Save();

        #endregion
    }
}
