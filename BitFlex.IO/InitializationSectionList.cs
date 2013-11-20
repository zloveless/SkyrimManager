// *********************************************************************
// [DCOM Productions]
// [Copyright (C) DCOM Productions All rights reserved.]
// *********************************************************************

namespace BitFlex.IO {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Collections;

    /// <summary>
    /// Class that stores a keyed collection of Initialization Sections
    /// </summary>
    public sealed class InitializationSectionList : IEnumerable {
        /// <summary>
        /// Instantiates a new instance of the BitFlex.IO.InitializationSectionList class
        /// </summary>
        public InitializationSectionList() {
            //
            // Default constructor
            //
        }

        #region Fields

        private List<InitializationSection> m_Sections = new List<InitializationSection>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets an InitializationSection using the specified name
        /// </summary>
        /// <param name="name">The section's name</param>
        /// <returns>Returns the section</returns>
        public InitializationSection this[String name] {
            get {
                return m_Sections.Find(s => s.Name == name);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new InitializationSection to the collection
        /// </summary>
        /// <param name="name">The name of the section to add</param>
        public void Add(String name) {
            if (m_Sections.Find(s => s.Name.ToLower() == name.ToLower()) != null) {
                throw new System.ArgumentException("The specified section already exists", "name");
            }
            else {
                m_Sections.Add(new InitializationSection(name));
            }
        }

        internal void Add(InitializationSection section) {
            m_Sections.Add(section);
        }

        /// <summary>
        /// Removes all InitializationSections from the collection
        /// </summary>
        public void Clear() {
            m_Sections.Clear();
        }

        /// <summary>
        /// Gets a value indicating whether a section with the specified name exists
        /// </summary>
        /// <param name="name">The name to look up</param>
        /// <returns>Returns true if the section exists, otherwise returns false</returns>
        public Boolean Contains(String name) {
            Boolean retVal = false;

            foreach (InitializationSection item in m_Sections) {
                if (item.Name == name) {
                    retVal = true;
                    break;
                }
            }

            return retVal;
        }

        

        /// <summary>
        /// Removes the InitializationSection from the collection
        /// </summary>
        /// <param name="name">The name of the section to remove</param>
        public void Remove(String name) {
            if (m_Sections.Find(s => s.Name == name) == null) {
                throw new System.ArgumentException("The specified section could not be found", "name");
            }
            else {
                m_Sections.RemoveAll(s => s.Name == name);
            }
        }

        /// <summary>
        /// Renames a section to the specified new name
        /// </summary>
        /// <param name="name">The name of the section to rename</param>
        /// <param name="newName">The new name of the section</param>
        public void Rename(String name, String newName) {
            if (Contains(name)) {
                if (Contains(newName)) {
                    throw new System.ArgumentException("A section with the specified name already exists", "newName");
                }
                else {
                    foreach (InitializationSection item in m_Sections) {
                        if (item.Name == name) {
                            item.m_Name = newName;
                            break;
                        }
                    }
                }
            }
            else {
                throw new System.ArgumentException("A section with the specified name could not be found", "name");
            }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator() {
            return m_Sections.GetEnumerator();
        }

        #endregion
    }
}