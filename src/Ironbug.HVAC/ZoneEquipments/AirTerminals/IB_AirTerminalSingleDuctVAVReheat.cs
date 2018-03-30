using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_AirTerminalSingleDuctVAVReheat: IB_AirTerminal
    {
        public IB_Coil ReheatCoil { get; private set; } = new IB_CoilHeatingWater();
        private static AirTerminalSingleDuctVAVReheat InitMethod(Model model) => 
            new AirTerminalSingleDuctVAVReheat(model, model.alwaysOnDiscreteSchedule(),new CoilHeatingWater(model));

        public IB_AirTerminalSingleDuctVAVReheat():base(InitMethod(new Model()))
        {
        }
        
        public void SetReheatCoil(IB_Coil ReheatCoil)
        {
            this.ReheatCoil = ReheatCoil;
            
        }

        public override IB_ModelObject Duplicate()
        {
            var newObj = (IB_AirTerminalSingleDuctVAVReheat)base.DuplicateIBObj(() => new IB_AirTerminalSingleDuctVAVReheat());
            var newCoil = (IB_Coil)this.ReheatCoil.Duplicate();
            newObj.SetReheatCoil(newCoil);

            return newObj;
        }

        public override ModelObject ToOS(Model model)
        {
            var newOSObj = base.ToOS(InitMethod, model).to_AirTerminalSingleDuctVAVReheat().get();
            var newOSCoil = (HVACComponent)this.ReheatCoil.ToOS(model);
            newOSObj.setReheatCoil(newOSCoil);

            return newOSObj;
        }
    }

    public class IB_AirTerminalSingleDuctVAVReheat_DataFieldSet : IB_DataFieldSet
    {

        protected override IddObject RefIddObject => new IdfObject(AirTerminalSingleDuctVAVReheat.iddObjectType()).iddObject();

        protected override Type ParentType => typeof(AirTerminalSingleDuctVAVReheat);

    }
}
