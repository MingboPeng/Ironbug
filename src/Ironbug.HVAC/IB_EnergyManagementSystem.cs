using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace Ironbug.HVAC
{
    public class IB_EnergyManagementSystem
    {
        [DataMember]
        public List<IB_EnergyManagementSystemActuator> Actuators { get; private set; }
        [DataMember]
        public List<IB_EnergyManagementSystemSensor> Sensors { get; private set; }
        [DataMember]
        public List<IB_EnergyManagementSystemProgramCallingManager> ProgramClnManagers { get; private set; }
        [DataMember]
        public List<BaseClass.IB_EnergyManagementSystemVariable> Variables { get; private set; }

        private IB_EnergyManagementSystem() { }
        public IB_EnergyManagementSystem(
            List<IB_EnergyManagementSystemActuator> actuators, 
            List<IB_EnergyManagementSystemSensor> sensors,
            List<BaseClass.IB_EnergyManagementSystemVariable> variables,
            List<IB_EnergyManagementSystemProgramCallingManager> programManagers)
        {
            this.Actuators = actuators ?? new List<IB_EnergyManagementSystemActuator>();
            this.Sensors = sensors ?? new List<IB_EnergyManagementSystemSensor>();
            this.ProgramClnManagers = programManagers ?? new List<IB_EnergyManagementSystemProgramCallingManager>();
            this.Variables = variables ?? new List<BaseClass.IB_EnergyManagementSystemVariable>();
            
        }

        //public string SaveAsIBJson(string path)
        //{
        //    var json = JsonConvert.SerializeObject(this, Formatting.Indented);
          
        //    using (StreamWriter file = new StreamWriter(path, true))
        //    {
        //        file.Write(json);
        //    }
        //    return path;
        //}

        //public static IB_EnergyManagementSystem FromIBJson(string json)
        //{
        //    var hvac = JsonConvert.DeserializeObject<IB_EnergyManagementSystem>(json, IB_JsonSetting.ConvertSetting);
        //    return hvac;
        //}


        public bool SaveEMS(string filepath)
        {
            var actuators = this.Actuators;
            var sensors = this.Sensors;
            var prograManagers = this.ProgramClnManagers;
            var variables = this.Variables;

            var osmFile = filepath;

            //get Model from file if exists
            var model = IB_Utility.GetOrNewModel(osmFile);

            var mapper = new Dictionary<string, string>();
            foreach (var item in actuators)
            {
                var added = item.ToOS(model);
                mapper.Add(item.GetTrackingTagID(), added.handle().__str__());
            }

            foreach (var item in sensors)
            {
                var added = item.ToOS(model);
                mapper.Add(item.GetTrackingTagID(), added.handle().__str__());
            }
            foreach (var item in variables)
            {
                var added = item.ToOS(model);
                mapper.Add(item.GetTrackingTagID(), added.handle().__str__());
            }

            foreach (var item in prograManagers)
            {
                // add program first
                foreach (var p in item.Programs)
                {
                    var added = p.ToOS(model, mapper);
                }
                item.ToOS(model);

            }

            // check programs in PlantComponentUserDefined
            var pComs = model.getPlantComponentUserDefineds();
            foreach (var item in pComs)
            {
                var pInitP = item.plantInitializationProgram();
                if (pInitP.is_initialized())
                {
                    var p = pInitP.get();
                    var body = p.body();
                    var mappedBody = body;
                    foreach (var id in mapper)
                    {
                        mappedBody = mappedBody.Replace(id.Key, id.Value);
                    }
                    p.setBody(mappedBody);
                }

                var pSimuP = item.plantSimulationProgram();
                if (pSimuP.is_initialized())
                {
                    var p = pSimuP.get();
                    var body = p.body();
                    var mappedBody = body;
                    foreach (var id in mapper)
                    {
                        mappedBody = mappedBody.Replace(id.Key, id.Value);
                    }
                    p.setBody(mappedBody);
                }
            }


            //save osm file
            var osmPath = OpenStudio.OpenStudioUtilitiesCore.toPath(filepath);
            return model.save(osmPath, true);
            
        }


        
    }
}
