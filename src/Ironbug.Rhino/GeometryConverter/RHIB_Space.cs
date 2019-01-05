using Rhino.DocObjects.Custom;
using Rhino.Geometry;
using System.Collections.Generic;
using OpenStudio;
using System.Linq;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using Rhino.Collections;

namespace Ironbug.RhinoOpenStudio.GeometryConverter
{
    public class RHIB_Space : CustomBrepObject,IRHIB_GeometryBase
    {
        public RHIB_Space(Brep m)
            : base(m)
        {
        }
        
        public RHIB_Space()
        {
        }
        
        public override string ToString() => "OS_Space";

        public override string ShortDescription(bool plural) => "OS_Space";
        public static RHIB_Space ToRHIB_Space(Brep brep, Rhino.Collections.ArchivableDictionary IdfData)
        {
            var space = new RHIB_Space(brep);
            space.Attributes.UserDictionary.Set("OpenStudioData", IdfData);
            return space;
        }

        public static (RHIB_Space space, List<RHIB_SubSurface> glzs) FromOpsSpace(Space OpenStudioSpace)
        {
            var ospace = OpenStudioSpace;
            var sfs = ospace.surfaces;
            var tol = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;
            
            var zonefaces = new List<Brep>();
            var glzs = new List<RHIB_SubSurface>();
            var zoneBrep3 = new Brep();
            
            var userDataDic = new Rhino.Collections.ArchivableDictionary();
            
            foreach (OpenStudio.Surface sf in sfs)
            {
                //Surface
                var rhBrep = sf.ToBrep();
                zonefaces.Add(rhBrep.SrfBrep);
                zoneBrep3.Append(rhBrep.SrfBrep);
                var srfID = rhBrep.SrfBrep.GetCentorAreaForID();
                if (srfID == "")
                {
                    var a = rhBrep.SrfBrep.GetCentorAreaForID();
                }
                userDataDic.Set(srfID, rhBrep.IDFString);

                //SubSurface (Glasses)
                var glzSurfaceBrep = sf.subSurfaces().Select(s => s.ToBrep());
                var rhib_glzs = glzSurfaceBrep.Select(_ => RHIB_SubSurface.ToRHIB_SubSurface(_.SrfBrep, _.IDFString));
                glzs.AddRange(rhib_glzs);

            }

            //Space
            zoneBrep3.JoinNakedEdges(tol);
            var closedBrep = Brep.JoinBreps(zonefaces, tol)[0];
            if (!closedBrep.IsSolid)
            {
                closedBrep = zoneBrep3;
            }
            userDataDic.Set("SpaceData", ospace.__str__());

            var space =  RHIB_Space.ToRHIB_Space(closedBrep, userDataDic);
            space.Name = ospace.nameString();

            return (space, glzs);
        }



        public bool UpdateIdfData(int IddFieldIndex, string Value, string BrepFaceCenterAreaID = "")
        {
            var idfString = string.Empty;
            var isSpace = string.IsNullOrEmpty(BrepFaceCenterAreaID);
            if (isSpace)
            {
                idfString = this.GetIdfString();
            }
            else
            {
                idfString = this.GetSurfaceIdfString(BrepFaceCenterAreaID);
            }
            
            //Update IdfString
            var osmIdfobj = OpenStudio.IdfObject.load(idfString).get();
            osmIdfobj.setString((uint)IddFieldIndex, Value);
            var newIdfString = osmIdfobj.__str__();


            if (!newIdfString.Contains(Value))
                return false; //TODO: add exception message

            //var m = IronbugRhinoPlugIn.Instance.OsmModel;
            //var osmObj_optional = m.getObject(osmIdfobj.handle());

            //if (osmObj_optional.isNull())
            //    return false; //TODO: add exception message

            //var osmobj = osmObj_optional.get();
            var num = Rhino.RhinoDoc.ActiveDoc.BeginUndoRecord(string.Format("OS:Space attribute updates: {0}",Value));

            var newobj = new RHIB_Space(this.BrepGeometry);
            var newUserDataDic = this.GetIdfData().Clone();
            newobj.Attributes.UserDictionary.Set("OpenStudioData", newUserDataDic);
            if (isSpace)
            {
                newUserDataDic.Set("SpaceData", newIdfString);
            }
            else
            {
                newUserDataDic.Set(BrepFaceCenterAreaID, newIdfString);
            }
            
            Rhino.RhinoDoc.ActiveDoc.Objects.Replace(new Rhino.DocObjects.ObjRef(this.Id), newobj);
            if (num > 0)
            {
                Rhino.RhinoDoc.ActiveDoc.EndUndoRecord(num);
            }



            return true;
        }
        
        private ArchivableDictionary GetIdfData() => this.Attributes.UserDictionary.GetDictionary("OpenStudioData");

        public string GetIdfString() => this.GetIdfData().GetString("SpaceData");
        public string GetSurfaceIdfString(string CentorAreaForID) => this.GetIdfData().GetString(CentorAreaForID);

        //public static byte[] ObjectToByteArray(Object obj)
        //{
        //    BinaryFormatter bf = new BinaryFormatter();
        //    using (var ms = new System.IO.MemoryStream())
        //    {
        //        bf.Serialize(ms, obj);
        //        return ms.ToArray();
        //    }
        //}


    }

    //public class RHIB_Surface: BrepFace
    //{
    //    private RHIB_Surface()
    //    {
    //        this = base.CreateExtrusion()
    //    }
    //}

    

}