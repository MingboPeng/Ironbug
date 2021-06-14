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
        private string _name { get; set; }
        private string _programBody { get; set; }

        public void SetName(string name)
        {
            this._name = name;
            var p = this.GhostOSObject as EnergyManagementSystemProgram;
            p.setName(name);
        }

        public void SetProgramBody(string programBody)
        {
            this._programBody = programBody;
            var p = this.GhostOSObject as EnergyManagementSystemProgram;
            p.setBody(programBody);
        }
     

        private Dictionary<string, string> _idMapper = new Dictionary<string, string>();
        public void AddMapper(string tagId, string realId)
        {
            _idMapper.Add(tagId, realId);
        }


        public EnergyManagementSystemProgram ToOS(Model model, Dictionary<string, string> idMapper)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            if (string.IsNullOrEmpty(_name))
                obj.setName(_name);

            // replace mapper
            var mappedBody = this._programBody;
            foreach (var id in idMapper)
            {
                mappedBody = mappedBody.Replace(id.Key, id.Value);
            }

            obj.setBody(mappedBody);
            return obj;
        }

    }

    
}
