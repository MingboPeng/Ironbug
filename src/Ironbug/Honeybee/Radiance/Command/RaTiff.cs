using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ironbug.Honeybee.Radiance.Command
{
    //ef95a65  on Dec 1, 2016
    //https://github.com/ladybug-tools/honeybee/blob/master/honeybee/radiance/command/raTiff.py
    //C:\Users\Mingbo\AppData\Roaming\McNeel\Rhinoceros\5.0\scripts\honeybee\radiance\command

    public class RaTiff : RadianceBaseCommand
    {
        private string inputHdrFile;

        private string InputHdrFile
        {
            get { return inputHdrFile; }
            set { inputHdrFile = value; }
        }

        private string outputTiffFile;

        private string OutputTiffFile
        {
            get { return outputTiffFile; }
            set { outputTiffFile = value; }
        }

        public RaTiff(string inputHdrFile, string outputTiffFile):base("ra_tiff.exe")
        {
            this.InputHdrFile = inputHdrFile;
            this.OutputTiffFile = outputTiffFile;
            
        }
        
        protected override string ToRadString(bool relativePath = false)
        {
            string cmdName = normspace(Path.Combine(RadbinPath, "ra_tiff.exe"));
            //string cmdParams = this.raTiffParameters.toRadString();
            string inputFile = this.InputHdrFile;
            string outputFile = this.OutputTiffFile;
            string radString = String.Format("{0} {1} {2}", cmdName, inputFile, outputFile);
            //string radString = String.Format("{1} {2}", cmdName, inputFile, outputFile);

            return radString;
        }

        public override string ToString()
        {
            return this.ToRadString(relativePath:false);
        }
    }
}
