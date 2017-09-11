using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ironbug.Radiance.Command
{
    //ef95a65  on Dec 1, 2016
    //https://github.com/ladybug-tools/honeybee/blob/master/honeybee/radiance/command/raTiff.py

    public class RaTiff : RadianceCommand
    {
        private string inputHdrFile;

        public string InputHdrFile
        {
            get { return inputHdrFile; }
            private set { inputHdrFile = value; }
        }

        private string outputTiffFile;

        public string OutputTiffFile
        {
            get { return outputTiffFile; }
            private set { outputTiffFile = value; }
        }

        public RaTiff(string inputHdrFile, string outputTiffFile):base("ra_tiff.exe")
        {
            this.InputHdrFile = inputHdrFile;
            this.OutputTiffFile = outputTiffFile;
            
        }


        public sealed override string ToRadString(bool relativePath = false)
        {
            string cmdName = normspace(Path.Combine(RadbinPath, "ra_tiff"));
            //string cmdParams = this.raTiffParameters.toRadString();
            string inputFile = this.InputHdrFile;
            string outputFile = this.OutputTiffFile;

            string radString = String.Format("{0} {1} {2}", cmdName,inputFile,outputFile);

            return radString;
        }
    }
}
