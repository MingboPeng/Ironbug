using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ironbug.HVAC
{
    public class IB_HVACSystem
    {
        public List<IB_AirLoopHVAC> AirLoops { get; private set; }
        public List<IB_PlantLoop> PlantLoops { get; private set; }
        public List<IB_AirConditionerVariableRefrigerantFlow> VariableRefrigerantFlows { get; private set; }

        private string _existFile = "";

        public IB_HVACSystem(List<IB_AirLoopHVAC> airLoops, List<IB_PlantLoop> plantLoops, List<IB_AirConditionerVariableRefrigerantFlow> vrfs)
        {
            this.AirLoops = airLoops;
            this.PlantLoops = plantLoops;
            this.VariableRefrigerantFlows = vrfs;
            
            var existingA = airLoops.Where(_=>_ is IIB_ExistingLoop).Select(_=>((IIB_ExistingLoop)_).ExistingObj.OsmFile);
            var existingP = plantLoops.Where(_ => _ is IIB_ExistingLoop).Select(_ => ((IIB_ExistingLoop)_).ExistingObj.OsmFile);

            var existing = existingA.ToList();
            existing.AddRange(existingP);
            existing.Distinct();
            if (existing.Count >1)
            {
                throw new ArgumentException("Cannot merge loops from different osm files");
            }
            else if (existing.Count==1)
            {
                _existFile = existing[0];
            }

        }

        /// <summary>
        /// Save the model to a default temp file in temp folder
        /// </summary>
        /// <returns>A full osm model file path</returns>
        public string Save2Temp()
        {
            var tempPath = Path.GetTempPath() + @"\Ladybug\HVAC";
            Directory.CreateDirectory(tempPath);

            tempPath = $"{tempPath}\\temp.osm";

            return this.SaveHVAC(tempPath) ? tempPath : string.Empty;
            
        }

        public bool SaveHVAC(string filepath)
        {
            var airLoops = this.AirLoops;
            var plantLoops = this.PlantLoops;
            var vrfs = this.VariableRefrigerantFlows;

            var osmFile = filepath;

            //here means editing current existing file 
            if (!string.IsNullOrEmpty( this._existFile))
            {
                osmFile = this._existFile;
            }

            //get Model from file if exists
            var model = GetOrNewModel(osmFile);

            var airlps = model.getAirLoopHVACs().Select(_ => _.nameString()).ToList();
           

            //add loops
            foreach (var airLoop in airLoops)
            {
                airLoop.ToOS(model);
            }

            foreach (var plant in plantLoops)
            {
                plant.ToOS(model);
            }

            foreach (var vrf in vrfs)
            {
                vrf.ToOS(model);
            }

            CheckInternalSourceConstruction(model);
            var tol = model.getOutputControlReportingTolerances();
            tol.setToleranceforTimeCoolingSetpointNotMet(1.11);
            tol.setToleranceforTimeHeatingSetpointNotMet(1.11);

            //save osm file

            var osmPath = OpenStudio.OpenStudioUtilitiesCore.toPath(filepath);
            return model.save(osmPath, true);
            
        }

        private static OpenStudio.Model GetOrNewModel(string opsModelFilePath)
        {
            var model =  new OpenStudio.Model();
            if (File.Exists(opsModelFilePath))
            {
                var osmPath = OpenStudio.OpenStudioUtilitiesCore.toPath(opsModelFilePath);
                var optionalModel = OpenStudio.Model.load(osmPath);

                if(optionalModel.is_initialized()) model = optionalModel.get();
            }
            
            return model;
        }

        //This is due to how HB sets up the this type of construction
        private static void CheckInternalSourceConstruction(OpenStudio.Model model)
        {
            var tempM_o = model.getMaterialByName("INTERNAL SOURCE");
            if (tempM_o.isNull()) return;
            var tempM = tempM_o.get();
            var handle = tempM.handle().__str__();

            foreach (var item in tempM.sources())
            {
                if (item.to_Construction().isNull()) continue;

                var tempC = item.to_Construction().get();
                var mLayers = tempC.layers();

                var newLayers = new OpenStudio.MaterialVector(mLayers);
                var index = newLayers.ToList().FindIndex(_ => _.nameString() == "INTERNAL SOURCE");
                newLayers.RemoveAt(index);

                //create ConstructionWithInternalSource
                var opC = new OpenStudio.ConstructionWithInternalSource(model);
                opC.setName(tempC.nameString());
                opC.setLayers(newLayers);
                opC.setSourcePresentAfterLayerNumber(index);

                //assign to surfaces 
                foreach (var i in tempC.sources())
                {
                    if (i.to_PlanarSurface().isNull()) continue;
                    var srf = i.to_PlanarSurface().get();
                    srf.setConstruction(opC);
                }

                tempC.remove();
            }
            
            
        }
        
    }
}
