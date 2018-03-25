using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_SaveOSModel : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_SaveOSModel class.
        /// </summary>
        public Ironbug_SaveOSModel()
          : base("Ironbug_SaveOSModel", "Nickname",
              "Description",
              "Ironbug", "HVAC")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("FilePath", "path", "file path", GH_ParamAccess.item);
            pManager.AddGenericParameter("PlantLoops", "PlantLoops", "PlantLoops", GH_ParamAccess.list);
            pManager.AddGenericParameter("AirLoops", "AirLoops", "Zone with HVAC system set", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Write", "_write", "Write the OpenStudio file.", GH_ParamAccess.item, false);

            pManager[0].Optional = true;
            pManager[1].Optional = true;

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FilePath", "path", "file path", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string filepath = string.Empty;
            //filepath = @"C:\Users\Mingbo\Documents\GitHub\Ironbug\doc\osmFile\savedFromGH.osm";
            var airLoops = new List<HVAC.IB_AirLoopHVAC>();
            var plantLoops = new List<HVAC.IB_PlantLoop>();
            bool write = false;

            //var model = new OpenStudio.Model();

            DA.GetData(0, ref filepath);
            DA.GetDataList(1,  plantLoops);
            DA.GetDataList(2,  airLoops);
            DA.GetData(3, ref write);

            if (!write) return;
            
            if (string.IsNullOrEmpty(filepath)) return;


            var osmPath = new OpenStudio.Path(filepath);

            //get Model from file if exists
            var model = new OpenStudio.Model();
            if (File.Exists(filepath))
            {
                model.Dispose();
                var optionalModel = OpenStudio.Model.load(osmPath);
                model = optionalModel.is_initialized() ? optionalModel.get() : model;
            }

            //remove current thermalzones
            //TODO:need to duplicate thermalzone's schedules, etc.
            var currThermalZones = model.getThermalZones();
            foreach (var item in currThermalZones)
            {
                //TODO: memory protected if path connected before airloop
                item.disconnect();
                item.remove();
                item.Dispose();
                
                
            }

            var currentAirloop = model.getAirLoopHVACs();
            foreach (var item in currentAirloop)
            {
                item.remove();
            }

            var currentPlantloop = model.getPlantLoops();
            foreach (var item in currentPlantloop)
            {
                item.remove();
            }

            //add loops
            foreach (var airLoop in airLoops)
            {
                airLoop.ToOS(model);
            }
            

            foreach (var plant in plantLoops)
            {
                plant.ToOS( model);
            }

            //link ThermalZone to Space(HBZone)
            var thermalZones = model.getThermalZones();
            var spaces = model.getSpaces();

            
            if (thermalZones.Count != spaces.Count)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "It seems not all HBZones have HVAC set correctly!");
                
            }

            foreach (var space in spaces)
            {
                //TODO: check what if couldn't find it by name??
                var name = space.nameString().Replace("_space","");
                var thermalZone = thermalZones.Where(_ => _.nameString() == name).First();
                space.setThermalZone(thermalZone);
            }


            //save osm file
            var modelTobeSaved = model.clone();
            var saved = modelTobeSaved.save(osmPath, true);
            if (saved)
            {
                model.Dispose();
                modelTobeSaved.Dispose();

                DA.SetData(0, filepath);

            }
            
            
            
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("3246f516-d4cf-45e0-b0a7-abb47bb014c1"); }
        }
    }
}