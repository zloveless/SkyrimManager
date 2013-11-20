// *********************************************************************
// [DCOM Productions]
// [Copyright (C) DCOM Productions All rights reserved.]
// *********************************************************************

namespace BitFlex.IO {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;
    using System.IO;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents a flat format, comma delimited data file format
    /// </summary>
    /// <remarks>
    /// This implementation does not fully support the latest RFC4810 specification. Currently the supported features
    /// are single line, comma delimited values. This does NOT support multi-line data or custom delimiters.
    /// </remarks>
    public class CsvFile : DataFileBase {
        /// <summary>
        /// Initializes a new instance of the BitFlex.IO.CsvFile class
        /// </summary>
        /// <param name="directory">The directory that contains the file</param>
        /// <param name="name">The name of the file</param>
        public CsvFile(string directory, string name)
            : base(directory, name) {
        }

        #region Constants

        /// <devdoc>
        /// Regex Pattern for Parsing CSV files with Embedded commas, double quotes and line breaksa
        /// </devdoc>
        private const string REGEX_CSVRECORD = "^((\"(?:[^\"]|\"\")*\"|[^,]*)(,(\"(?:[^\"]|\"\")*\"|[^,]*))*)$";

        #endregion

        #region Properties

        private List<string> m_Columns = new List<string>();
        /// <summary>
        /// Gets or sets the collection of columns
        /// </summary>
        public List<string> Columns {
            get {
                return m_Columns;
            }
            set {
                m_Columns = value;
            }
        }

        private bool m_FirstRowColumnNames = false;
        /// <summary>
        /// Gets  or sets a System.Boolean value indicating whether the first row contains the column names
        /// </summary>
        public bool FirstRowColumnNames {
            get {
                return m_FirstRowColumnNames;
            }
            set {
                m_FirstRowColumnNames = value;
            }
        }

        private List<string[]> m_Rows = new List<string[]>();
        /// <summary>
        /// Gets or sets the collection of columns
        /// </summary>
        public List<string[]> Rows {
            get {
                return m_Rows;
            }
            set {
                m_Rows = value;
            }
        }

        #endregion

        #region DataFileBase Members

        /// <summary>
        /// Loads the Csv file from disk
        /// </summary>
        public override void Load() {
            Columns.Clear();
            Rows.Clear();
            string s = File.ReadAllText(FullName);
            using (StringReader reader = new StringReader(s)) {
                bool initialLine = true;
                string line = string.Empty;
                while ((line = reader.ReadLine()) != null) {
                    // Check and Parse out Column Names if exist & enabled
                    if (FirstRowColumnNames) {
                        if (initialLine) {
                            initialLine = false;
                            Match columnMatch = Regex.Match(line, REGEX_CSVRECORD);
                            if (columnMatch.Success) {
                                string[] columns = columnMatch.Value.Split(',');
                                foreach (string column in columns) {
                                    Columns.Add(column);
                                }
                            }
                            continue;
                        }
                    }
                    else {
                        initialLine = false;
                    }
                    // Now parse the records
                    if (!initialLine) {
                        Match match = Regex.Match(line, REGEX_CSVRECORD);
                        if (match.Success) {
                            string[] items = match.Value.Split(',');
                            Rows.Add(items);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Saves the Csv file to disk
        /// </summary>
        public override void Save() {
            StringBuilder contents = new StringBuilder();
            bool savedColumns = false;
            if (FirstRowColumnNames) {
                if (!savedColumns) {
                    savedColumns = true;
                    for (int i = 0; i < Columns.Count; i++) {
                        contents.Append(Columns[i]);
                        if (i != Columns.Count - 1) {
                            contents.Append(",");
                        }
                    }
                    contents.AppendLine();
                }
            }
            foreach (string[] row in Rows) {
                for (int i = 0; i < row.Length; i ++) {
                    contents.Append(row[i]);
                    if (i != row.Length - 1) {
                        contents.Append(",");
                    }
                }
                contents.AppendLine();
            }
            File.WriteAllText(FullName, contents.ToString());
        }

        #endregion
    }
}
