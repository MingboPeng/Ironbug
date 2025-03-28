﻿// using System;
// using Grasshopper.Kernel;
// using Ironbug.HVAC;
// using Ironbug.HVAC.BaseClass;
// namespace Ironbug.Grasshopper.Component
// {
//     public class Ironbug_SolarCollectorFlatPlateWater : Ironbug_DuplicableHVACWithParamComponent
//     {
//         public Ironbug_SolarCollectorFlatPlateWater()
//           : base("IB_SolarCollectorFlatPlateWater", "SolarCollectorFlatPlateWater",
//               "Description",
//               "Ironbug", "02:LoopComponents",
//               typeof(HVAC.IB_SolarCollectorFlatPlateWater_FieldSet))
//         {
//         }

//         public override GH_Exposure Exposure => GH_Exposure.quarternary;

//         protected override void RegisterInputParams(GH_InputParamManager pManager)
//         {
//             pManager.AddGenericParameter("Surface", "surface_", "Surface Name -  BuildingSurface:Detailed or the Shading:Zone:Detailed objects", GH_ParamAccess.item);
//             pManager.AddGenericParameter("Solar Collector Performance", "sc_performance_", "Heating coil to provide heating source..", GH_ParamAccess.item);
//         }
//         protected override void RegisterOutputParams(GH_OutputParamManager pManager)
//         {
//             pManager.AddGenericParameter("SolarCollectorFlatPlateWater", "SolarCollectorFlatPlateWater", "Connect to hot water loop's supply side.", GH_ParamAccess.item);
//         }

//         protected override void SolveInstance(IGH_DataAccess DA)
//         {
//             var obj = new HVAC.IB_SolarCollectorFlatPlateWater();
//             var surface = (IB_ModelObject)null;
//             var sc_p = (IB_ModelObject)null;

//             if (DA.GetData(0, ref surface))
//             {
//                 obj.SetSurface(surface);
//             }

//             if (DA.GetData(1, ref sc_p))
//             {
//                 obj.SetSolarCollectorPerformance(sc_p);
//             }


//             this.SetObjParamsTo(obj);
//             var objs = this.SetObjDupParamsTo(obj);
//             DA.SetDataList(0, objs);
//         }

//         protected override System.Drawing.Bitmap Icon => Properties.Resources.WaterHeaterMix;

//         public override Guid ComponentGuid => new Guid("ABDBE4EE-CCB7-4DB8-B7DF-171F800331E5");

//     }
// }