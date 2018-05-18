using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_OutdoorAirSystem : IB_HVACObject, IIB_AirLoopObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_OutdoorAirSystem();
        private static AirLoopHVACOutdoorAirSystem InitMethod(Model model) => new AirLoopHVACOutdoorAirSystem(model, new ControllerOutdoorAir(model));
        private IB_Child IB_ControllerOutdoorAir => this.Children.GetChild<IB_ControllerOutdoorAir>();

        //public IB_ControllerOutdoorAir IB_ControllerOutdoorAir { get; private set; } = new IB_ControllerOutdoorAir();
        
        
        public IB_OutdoorAirSystem():base(InitMethod(new Model()))
        {
            var controller = new IB_Child(new IB_ControllerOutdoorAir(), (obj) => this.SetController(obj as IB_ControllerOutdoorAir));
            this.Children.Add(controller);
        }
        

        public void SetController(IB_ControllerOutdoorAir ControllerOutdoorAir)
        {
            this.IB_ControllerOutdoorAir.Set(ControllerOutdoorAir);
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((AirLoopHVACOutdoorAirSystem)this.ToOS(model)).addToNode(node);
        }
        
        protected override ModelObject InitOpsObj(Model model)
        {
            var newObj = (AirLoopHVACOutdoorAirSystem)base.OnInitOpsObj(InitMethod, model);
            newObj.setControllerOutdoorAir((ControllerOutdoorAir)this.IB_ControllerOutdoorAir.Get<IB_ControllerOutdoorAir>().ToOS(model));
            return newObj;
        }
        

        public override IB_ModelObject Duplicate()
        {
            //Duplicate self;
            var newObj = (IB_OutdoorAirSystem)base.DuplicateIBObj(() => new IB_OutdoorAirSystem());

            //Duplicate child member;
            var newCtrl = (IB_ControllerOutdoorAir)this.IB_ControllerOutdoorAir.DuplicateChild();

            //add new child member to new object;
            newObj.SetController(newCtrl);

            return newObj;
        }

        //public override ModelObject ToOS(Model model)
        //{
        //    return base.ToOS(model);
        //}
        //public void ToOS_Demand(Loop Loop)
        //{
        //    this.ToOS()
        //}
    }
}
