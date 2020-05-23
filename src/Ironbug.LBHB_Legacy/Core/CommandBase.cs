using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Ironbug.LBHB_Legacy
{
    public abstract class CommandBase : CommandExecuteBase
    {
        //protected dynamic RawObj { get; set; }

        //protected override string RadString {
        //    get => this.ToRadString();
        //    set => base.RadString = value;
        //}

        //protected override string RadBinPath
        //{
        //    get => this.RawObj.radbinPath;
        //    set => this.RawObj.radbinPath = value;
        //}

        //protected override string RadLibPath
        //{
        //    get => this.RawObj.radlibPath;
        //    set => this.RawObj.radlibPath = value;
        //}

        ////TODO: this return type need to be changed to Tuple, or RadianceDataType
        //public object InputFiles
        //{
        //    get => this.RawObj.inputFiles;
        //}
        
        //public virtual string ExecuteFromPython()
        //{
        //    this.RawObj.execute();
        //    return "";
        //}
        
        //public virtual string ToRadString(bool relativePath = false)
        //{
        //    return RawObj.toRadString(relativePath);
        //}

        //public override string ToString()
        //{
        //    return ToRadString();
        //}

        
    }
}
