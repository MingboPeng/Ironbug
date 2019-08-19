using System;
using System.Collections.Generic;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CentralHeatPumpSystem : IB_HVACObject, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CentralHeatPumpSystem();

        private static CentralHeatPumpSystem NewDefaultOpsObj(Model model) => new CentralHeatPumpSystem(model);

        public List<IB_CentralHeatPumpSystemModule> Modules { get; private set; } = new List<IB_CentralHeatPumpSystemModule>();

        public IB_CentralHeatPumpSystem():base(NewDefaultOpsObj(new Model()))
        {
        }

        public void AddModule(IB_CentralHeatPumpSystemModule Module)
        {
            this.Modules.Add(Module);
        }
        public override HVACComponent ToOS(Model model)
        {
            var newObj = base.OnNewOpsObj(NewDefaultOpsObj, model);

            var currentAddedModule = newObj.modules().Count;
            if (currentAddedModule==0)
            {
                foreach (var item in this.Modules)
                {
                    newObj.addModule(item.ToOS(model) as CentralHeatPumpSystemModule);
                }
            }
            
            return newObj;
        }
        public override IB_ModelObject Duplicate()
        {
            //Duplicate self;
            var newObj = base.Duplicate() as IB_CentralHeatPumpSystem;
            foreach (var item in this.Modules)
            {
                newObj.AddModule(item.Duplicate() as IB_CentralHeatPumpSystemModule);
            }

            return newObj;
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            var newObj = ToOS(model) as CentralHeatPumpSystem;

            var isInCW = newObj.coolingPlantLoop().is_initialized();
            var isInHW = newObj.heatingPlantLoop().is_initialized();
            var isInDW = newObj.sourcePlantLoop().is_initialized();
          
            if (isInCW && isInHW && !isInDW)
                throw new ArgumentException("Failed to add CentralHeatPumpSystem to source plantloop, \nplease move the associated condenser water loop to the first item of HVACsystem's plantloops.");

            var lpCount = 0;
            lpCount = isInCW ? lpCount + 1 : lpCount;
            lpCount = isInHW ? lpCount + 1 : lpCount;
            lpCount = isInDW ? lpCount + 1 : lpCount;
            
            if (lpCount<2)
            {
                return newObj.addToNode(node);
            }
            else
            {
                return newObj.addToTertiaryNode(node);
            }

           
        }
    }

    public sealed class IB_CentralHeatPumpSystem_FieldSet
        : IB_FieldSet<IB_CentralHeatPumpSystem_FieldSet, CentralHeatPumpSystem>
    {
        private IB_CentralHeatPumpSystem_FieldSet() {}
        
    }
}
