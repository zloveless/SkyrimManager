// *********************************************************************
// [DCOM Productions]
// [Copyright (C) DCOM Productions All rights reserved.]
// *********************************************************************

namespace BitFlex.IO {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Class to manipulate Initialization files
    /// </summary>
    public sealed class InitializationFile : DataFileBase {
        /// <summary>
        /// Instantiates a new instance of the BitFlex.IO.InitializationFile class
        /// </summary>
        /// <param name="directory">The directory that contains this Initialization file</param>
        /// <param name="name">The file name of this Initialization file</param>
        /// <remarks>You can specify the name parameter with or without the file extension</remarks>
        public InitializationFile(String directory, String name) 
            : base(directory, name) {
            //
            // No default constructor implementation required
            //
        }

        #region Properties

        private InitializationSectionList m_Sections;
        /// <summary>
        /// Gets the collection of Initialization sections in this Initialization file
        /// </summary>
        public InitializationSectionList Sections {
            get {
                if (m_Sections == null) {
                    m_Sections = new InitializationSectionList();
                }
                return m_Sections;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads all the sections and keys into memory for this Initialization file
        /// </summary>
        public override void Load() {
            if (!Exists) {
                throw new System.IO.FileNotFoundException();
            }

            String[] lines = File.ReadAllLines(FullName);
            InitializationSection currentSection = null;
            Sections.Clear();

            for (int i = 0; i < lines.Length; i++) {
                String currentLine = lines[i];

                if (currentLine.StartsWith(" ", StringComparison.OrdinalIgnoreCase)) {
                    while (currentLine.StartsWith(" ", StringComparison.OrdinalIgnoreCase)) {
                        currentLine = currentLine.Remove(0, 1);
                    }
                }

                Match sectionMatch = Regex.Match(currentLine, @"\[[A-Za-z0-9 ]*\]");

                if (sectionMatch.Success) {
                    currentLine = currentLine.Remove(0, 1);
                    currentLine = currentLine.Remove(currentLine.LastIndexOf(']'));
                    currentSection = new InitializationSection(currentLine);

                    m_Sections.Add(currentSection);

                    continue;
                }

                Match keyMatch = Regex.Match(currentLine, @"[A-Za-z0-9]*=[A-Za-z0-9 \t]*");

                if (keyMatch.Success) {
                    String keyName = currentLine.Substring(0, currentLine.IndexOf('='));
                    String keyValue = currentLine.Substring(currentLine.IndexOf('=') + 1);

                    if (keyValue.StartsWith(" ", StringComparison.OrdinalIgnoreCase)) {
                        while (keyValue.StartsWith(" ", StringComparison.OrdinalIgnoreCase)) {
                            keyValue = keyValue.Remove(0, 1);
                        }
                    }

                    m_Sections[currentSection.Name].Keys.Add(keyName, keyValue);
                }
            }
        }

        /// <summary>
        /// Saves the contents of this Initialization file to disk
        /// </summary>
        public override void Save() {
            StringBuilder contents = new StringBuilder();

            foreach (InitializationSection section in Sections) {
                contents.AppendLine(String.Format(CultureInfo.InvariantCulture, "[{0}]", section.Name));

                foreach (KeyValuePair<String, String> item in section.Keys) {
                    contents.AppendLine(String.Format(CultureInfo.InvariantCulture, "{0}={1}", 
                        item.Key, item.Value));
                }

                contents.AppendLine();
            }

            File.WriteAllText(FullName, contents.ToString());
        }

        #endregion
    }
}