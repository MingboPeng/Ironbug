using Rhino.DocObjects.Custom;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ironbug.RhinoOpenStudio.GeometryConverter
{
    public class RHIB_ShadingSurface : CustomBrepObject, IRHIB_GeometryBase
    {
        public RHIB_ShadingSurface(Brep brep):base(brep)
        {

        }

        public RHIB_ShadingSurface()
        {

        }

        public static RHIB_ShadingSurface ToRHIB_SubSurface(OpenStudio.ShadingSurface shadingSurface)
        {
            var shdG_o = shadingSurface.shadingSurfaceGroup();
            var vec = new Vector3d();
            if (shdG_o.is_initialized())
            {
                var g = shdG_o.get();
                var isOriginDef = g.isXOriginDefaulted() && g.isYOriginDefaulted() && g.isZOriginDefaulted();
                vec = new Vector3d(g.xOrigin(), g.yOrigin(), g.zOrigin());
                
            }
            
            var shd = shadingSurface.ToBrep();
            shd.SrfBrep.Translate(vec);
            var srf = new RHIB_ShadingSurface(shd.SrfBrep);
            var userDataDic = new Rhino.Collections.ArchivableDictionary();
            userDataDic.Set("ShadingSurfaceData", shd.IDFString);

            srf.Attributes.UserDictionary.Set("OpenStudioData", userDataDic);

            return srf;
        }
        private Rhino.Collections.ArchivableDictionary GetIdfData() => this.Attributes.UserDictionary.GetDictionary("OpenStudioData");
        public string GetIdfString() => this.GetIdfData().GetString("ShadingSurfaceData");

        public bool UpdateIdfData(int IddFieldIndex, string Value, string brepFaceCenterAreaID = "")
        {
            var idfString = this.GetIdfString();

            //Update IdfString
            var osmIdfobj = OpenStudio.IdfObject.load(idfString).get();
            osmIdfobj.setString((uint)IddFieldIndex, Value);
            var newIdfString = osmIdfobj.__str__();

            if (!newIdfString.Contains(Value))
                return false; //TODO: add exception message

            var num = Rhino.RhinoDoc.ActiveDoc.BeginUndoRecord(string.Format("OS:ShadingSurface attribute updates: {0}", Value));

            var newobj = new RHIB_ShadingSurface(this.BrepGeometry);
            var newUserDataDic = this.GetIdfData().Clone();
            newUserDataDic.Set("ShadingSurfaceData", newIdfString);
            newobj.Attributes.UserDictionary.Set("OpenStudioData", newUserDataDic);

            Rhino.RhinoDoc.ActiveDoc.Objects.Replace(new Rhino.DocObjects.ObjRef(this.Id), newobj);
            if (num > 0)
            {
                Rhino.RhinoDoc.ActiveDoc.EndUndoRecord(num);
            }


            return true;
        }
    }
}
