using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ironbug.RhinoOpenStudio.GeometryConverter
{
    public interface IRHIB_GeometryBase
    {
        //IDictionary<string, string> m_OpenStudioProperties;

        bool UpdateIdfData(int IddFieldIndex, string Value, string brepFaceCenterAreaID = "");

        string GetIdfString();



    }
}
