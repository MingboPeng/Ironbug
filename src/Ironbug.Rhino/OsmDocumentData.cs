using System.Collections.Generic;

namespace Ironbug.RhinoOpenStudio
{
    public class OsmDocumentData
    {
        /// <summary>
        /// Class major and minor verson numbers
        /// </summary>
        private const int MAJOR = 1;

        private const int MINOR = 0;

        // The data...
        private readonly List<string> m_string_table;

        /// <summary>
        /// Public constructor
        /// </summary>
        public OsmDocumentData()
        {
            m_string_table = new List<string>(16);
        }

        /// <summary>
        /// Returns the number of strings in the string table.
        /// </summary>
        public int Count
        {
            get { return m_string_table.Count; }
        }

        /// <summary>
        /// Returns a string from the string table.
        /// </summary>
        public string Item(int index)
        {
            string str = null;
            if (index >= 0 && index < Count)
                str = m_string_table[index];
            return str;
        }

        /// <summary>
        /// Find a string in the string table.
        /// </summary>
        public int Find(string str)
        {
            if (string.IsNullOrEmpty(str))
                return -1;

            return m_string_table.FindIndex(s => s == str);
        }

        /// <summary>
        /// Adds a string to the string table.
        /// </summary>
        public int Add(string str)
        {
            if (string.IsNullOrEmpty(str))
                return -1;

            int index = Find(str);
            if (index >= 0) return index;

            m_string_table.Add(str);
            return Count;
        }

        /// <summary>
        /// Adds strings to the string table
        /// </summary>
        public void AddRange(IEnumerable<string> collection)
        {
            m_string_table.AddRange(collection);
        }

        /// <summary>
        /// Deletes a string from the string table.
        /// </summary>
        public bool Delete(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            int index = Find(str);
            if (index >= 0 && index < Count)
            {
                m_string_table.RemoveAt(index);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Clears the string table
        /// </summary>
        public void Clear()
        {
            m_string_table.Clear();
        }

        /// <summary>
        /// Returns an array of strings
        /// </summary>
        public string[] ToArray()
        {
            return m_string_table.ToArray();
        }

        /// <summary>
        /// Should write to binary archive?
        /// </summary>
        public bool ShouldWriteDocument()
        {
            return 0 != m_string_table.Count;
        }

        /// <summary>
        /// Write to binary archive
        /// </summary>
        public void WriteDocument(Rhino.FileIO.BinaryArchiveWriter archive)
        {
            archive.Write3dmChunkVersion(MAJOR, MINOR);
            archive.WriteStringArray(m_string_table);
        }

        /// <summary>
        /// Read from binary archive
        /// </summary>
        public void ReadDocument(Rhino.FileIO.BinaryArchiveReader archive)
        {
            archive.Read3dmChunkVersion(out var major, out var minor);
            if (MAJOR == major && MINOR == minor)
            {
                var string_table = archive.ReadStringArray();
                if (null != string_table)
                    m_string_table.AddRange(string_table);
            }
        }
    }
}