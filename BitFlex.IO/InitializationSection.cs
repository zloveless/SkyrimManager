// *********************************************************************
// [DCOM Productions]
// [Copyright (C) DCOM Productions All rights reserved.]
// *********************************************************************

namespace BitFlex.IO {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Class that stores information about an Initialization file section
    /// </summary>
    public class InitializationSection {
        /// <summary>
        /// Instantiates a new instance of the BitFlex.IO.InitializationSection class
        /// </summary>
        /// <param name="name">The name of the section</param>
        public InitializationSection(String name) {
            if (name.StartsWith("[", StringComparison.OrdinalIgnoreCase) || name.EndsWith("]", StringComparison.OrdinalIgnoreCase)) {
                throw new System.ArgumentException("The section name cannot start with or end with braces.", "name");
            }

            m_Name = name;
        }

        #region Properties

        private Dictionary<String, String> m_Keys;
        /// <summary>
        /// Gets or Sets the collection of Initialization keys in this section
        /// </summary>
        public Dictionary<String, String> Keys {
            get {
                if (m_Keys == null) {
                    m_Keys = new Dictionary<String, String>();
                }
                return m_Keys;
            }
        }

        internal String m_Name;
        /// <summary>
        /// Gets the name of this InitializationSection
        /// </summary>
        public String Name {
            get {
                return m_Name;
            }
        }

        #endregion
    }
}
