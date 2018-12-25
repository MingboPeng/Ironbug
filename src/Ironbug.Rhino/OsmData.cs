using Rhino.DocObjects.Custom;
using System.Collections.Generic;

namespace Ironbug.RhinoOpenStudio
{
    [System.Runtime.InteropServices.Guid("6674D055-C507-4A62-A601-D00A52060359")]
    public class OsmObjectData : Rhino.DocObjects.Custom.UserData
    {
        #region Private constants

        /// <summary>
        /// The major and minor version number of this data.
        /// </summary>
        private const int MAJOR_VERSION = 1;

        private const int MINOR_VERSION = 0;

        #endregion Private constants

        #region Public properties

        /// <summary>
        /// Returns true of our user data is valid.
        /// </summary>
        public bool IsValid => !string.IsNullOrEmpty(Notes);

        /// <summary>
        /// The notes field
        /// </summary>
        public string Notes { get; set; }
        #endregion Public properties

        #region Userdata overrides

        /// <summary>
        /// Descriptive name of the user data.
        /// </summary>
        public override string Description => "OsmString";

        /// <summary>
        /// If you want to save this user data in a 3dm file, override ShouldWrite and return
        /// true. If you do support serialization, you must also override the Read and Write
        /// functions.
        /// </summary>
        public override bool ShouldWrite => IsValid;

        public override string ToString()
        {
            return Description;
        }

        /// <summary>
        /// Is called when the object is being duplicated.
        /// </summary>
        protected override void OnDuplicate(UserData source)
        {
            if (source is OsmObjectData src)
            {
                Notes = src.Notes;
            }
        }
        /// <summary>
        /// Reads the content of this data from a stream archive.
        /// </summary>
        protected override bool Read(Rhino.FileIO.BinaryArchiveReader archive)
        {
            // Read the chuck version
            archive.Read3dmChunkVersion(out var major, out var minor);
            if (major == MAJOR_VERSION)
            {
                // Read 1.0 fields  here
                if (minor >= MINOR_VERSION)
                {
                    Notes = archive.ReadString();
                }

                // Note, if you every roll the minor version number,
                // then read those fields here.
            }

            return !archive.ReadErrorOccured;
        }

        /// <summary>
        /// Writes the content of this data to a stream archive.
        /// </summary>
        protected override bool Write(Rhino.FileIO.BinaryArchiveWriter archive)
        {
            // Write the chuck version
            archive.Write3dmChunkVersion(MAJOR_VERSION, MINOR_VERSION);

            // Write 1.0 fields
            archive.WriteString(Notes);

            // Note, if you every roll the minor version number,
            // then write those fields here.

            return !archive.WriteErrorOccured;
        }

        #endregion Userdata overrides
    }

}