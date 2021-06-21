using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_EnergyManagementSystemProgram : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_EnergyManagementSystemProgram();

        private static EnergyManagementSystemProgram NewDefaultOpsObj(Model model) => new EnergyManagementSystemProgram(model);
        public IB_EnergyManagementSystemProgram() : base(NewDefaultOpsObj(new Model()))
        {
        }
       
        public void SetName(string name)
        {
            var f = IB_EnergyManagementSystemProgram_FieldSet.Value;
            this.AddCustomAttribute(f.Name, name);
        }
        public void SetProgramBody(string programBody)
        {
            var f = IB_EnergyManagementSystemProgram_FieldSet.Value;
            this.AddCustomAttribute(f.Body, programBody);
        }

        public EnergyManagementSystemProgram ToOS(Model model, Dictionary<string, string> idMapper)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
      
            // replace mapper
            var mappedBody = obj.body();
            foreach (var id in idMapper)
            {
                mappedBody = mappedBody.Replace(id.Key, id.Value);
            }

            obj.setBody(mappedBody);
            return obj;
        }

        public void ApplyAttributesToObj(ModelObject osObj, Dictionary<string, string> idMapper)
        {
            base.ApplyAttributesToObj(osObj);
            var obj = osObj as EnergyManagementSystemProgram;
            // replace mapper
            var mappedBody = obj.body();
            foreach (var id in idMapper)
            {
                mappedBody = mappedBody.Replace(id.Key, id.Value);
            }

            obj.setBody(mappedBody);
        }


    }

    public sealed class IB_EnergyManagementSystemProgram_FieldSet
       : IB_FieldSet<IB_EnergyManagementSystemProgram_FieldSet, EnergyManagementSystemProgram>
    {

        private IB_EnergyManagementSystemProgram_FieldSet() { }

        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name");

        public IB_Field Body { get; }
            = new IB_BasicField("Body", "Body");

       
    }


}
