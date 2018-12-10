using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ironbug.Honeybee.Radiance.Command
{

    public class PValue_Legacy : RadianceBaseCommand
    {
        private string inputHdrFile;

        public int X = 0;
        public int Y = 0;

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

        public PValue_Legacy(string inputHdrFile):base("pvalue.exe")
        {
            this.InputHdrFile = inputHdrFile;
        }
        
        protected override string ToRadString(bool relativePath = false)
        {
            string cmdName = normspace(Path.Combine(RadbinPath, "pvalue.exe"));
            string cmdParams = "-o -d -h -b";
            string inputFile = this.InputHdrFile;
            string radString = String.Format("{0} {1} {2}", cmdName, cmdParams, inputFile);

            return radString;
        }

        public new IEnumerable<int> Execute()
        {
            var outputStr = base.Execute().Trim();
            var outputlist = outputStr.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            if (outputlist.Length==0)
            {
                new Exception("Failed to extract HDR image values!");
                return new List<int>();
            }
            else
            {
                var dim = outputlist[0].Split(' ');

                this.X = Convert.ToInt16(dim[3]);
                this.Y = Convert.ToInt16(dim[1]);
                var output = outputlist.Skip(1).Select(_ => Convert.ToInt32(double.Parse(_.Trim())));
                return output;
            }

            
        }
        


        public override string ToString()
        {
            return this.ToRadString(relativePath:false);
        }
    }
}
