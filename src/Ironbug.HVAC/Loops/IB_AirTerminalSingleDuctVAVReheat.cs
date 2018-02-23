using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_AirTerminalSingleDuctVAVReheat: IB_ModelObject
    {
        public IB_HVACComponent ReheatCoil { get; private set; } 
        private static AirTerminalSingleDuctVAVReheat InitMethod(Model model) => 
            new AirTerminalSingleDuctVAVReheat(model, model.alwaysOnDiscreteSchedule(),new CoilHeatingWater(model));

        public IB_AirTerminalSingleDuctVAVReheat():base(InitMethod(new Model()))
        {
            base.SetName("AirTerminal:SingleDuct:VAV:Reheat");
        }
        
        public void SetReheatCoil(IB_HVACComponent ReheatCoil)
        {
            this.ReheatCoil = ReheatCoil;

            //TODO: no need to make the connection
            //var osModel = base.GhostOSObject.model();
            //var osCoil = (HVACComponent)this.ReheatCoil.ToOS(osModel);
            //((AirTerminalSingleDuctVAVReheat)base.GhostOSObject).setReheatCoil(osCoil);
        }

        public override IB_ModelObject Duplicate()
        {
            var newObj = (IB_AirTerminalSingleDuctVAVReheat)base.DuplicateIB_ModelObject(() => new IB_AirTerminalSingleDuctVAVReheat());
            var newCoil = (IB_HVACComponent)this.ReheatCoil.Duplicate();
            newObj.SetReheatCoil(newCoil);

            return newObj;
        }

        public override ModelObject ToOS(Model model)
        {
            var newOSObj = base.ToOS(InitMethod, model);
            var newOSCoil = (HVACComponent)this.ReheatCoil.ToOS(model);
            ((AirTerminalSingleDuctVAVReheat)newOSObj).setReheatCoil(newOSCoil);

            return newOSObj;
        }
    }
}
