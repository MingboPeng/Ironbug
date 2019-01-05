using Rhino.DocObjects.Custom;
using System.Collections;


namespace Ironbug.RhinoOpenStudio
{
    public class OsmObjectData : UserData
    {
        #region Private constants

        /// <summary>
        /// The major and minor version number of this data.
        /// </summary>
        private const int MAJOR_VERSION = 1;

        private const int MINOR_VERSION = 0;

        #endregion Private constants

        #region Public properties
        
        public string IDFString { get; set; }

        public Rhino.Collections.ArchivableDictionary OsmObjProperties { get; set; } = new Rhino.Collections.ArchivableDictionary();
        #endregion Public properties
        
        #region Userdata overrides

        /// <summary>
        /// Descriptive name of the user data.
        /// </summary>
        public override string Description => "OpenStudio Data";

        public bool IsValid => !string.IsNullOrEmpty(IDFString);
        public override bool ShouldWrite => IsValid;

        public override string ToString() => Description;

        /// <summary>
        /// Is called when the object is being duplicated.
        /// </summary>
        protected override void OnDuplicate(UserData source)
        {
            if (source is OsmObjectData src)
            {
                IDFString = src.IDFString;
                //OsmObjProperties.AddContentsFrom(src.OsmObjProperties);
                OsmObjProperties = src.OsmObjProperties.Clone();
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
                    IDFString = archive.ReadString();
                    OsmObjProperties = archive.ReadDictionary();
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
            archive.WriteString(IDFString);
            archive.WriteDictionary(OsmObjProperties);

            // Note, if you every roll the minor version number,
            // then write those fields here.

            return !archive.WriteErrorOccured;
        }

        #endregion Userdata overrides

        //public bool UpdateIdfString(int IddFieldIndex, string Value)
        //{
            
        //    var idfObj = OpenStudio.IdfObject.load(this.IDFString).get();
        //    idfObj.setString((uint)IddFieldIndex, Value);

        //    var osmObj = IronbugRhinoPlugIn.Instance.OsmModel.getObject(idfObj.handle()).get();
        //    osmObj.setString((uint)IddFieldIndex, Value);

        //    var newIdfString = idfObj.__str__();
        //    var newOsmString = osmObj.__str__();

        //    //var osmObjtest = IronbugRhinoPlugIn.Instance.OsmModel.getObject(idfObj.handle()).get();
        //    //osmObjtest.setString((uint)IddFieldIndex, Value);
        //    //var newOsmStringTest = osmObjtest.__str__();

        //    if (newIdfString.Contains(Value))
        //    {
        //        if (newIdfString == newOsmString)
        //        {
        //            this.IDFString = newIdfString;
        //        }
        //        else
        //        {
        //            throw new System.ArgumentException("Failed to update OpenStudio model!");
        //        }


                
        //        return true;
        //    }

        //    return false;
        //}

        //private static bool UpdateOsm(string idfString)
        //{
            
        //    var idf = OpenStudio.IdfObject.load(idfString).get();
        //    var handle = idf.handle();
        //    var m = IronbugRhinoPlugIn.Instance.OsmModel;
        //    m.toIdfFile().getObject(handle).get().
        //    .getObject(handle).get().re
        //    var handle =
        //}
    }

}