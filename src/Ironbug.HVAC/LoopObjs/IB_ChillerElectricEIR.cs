using System;
using Ironbug.HVAC.BaseClass;
using Ironbug.HVAC.Curves;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ChillerElectricEIR : IB_HVACObject, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        //protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ChillerElectricEIR();
        protected override Func<IB_ModelObject> IB_InitSelf => 
            delegate()
            {
                if (this.Children.Count>0)
                {
                    return new IB_ChillerElectricEIR(this._CCFofT,this._EItoCORFofT, this._EItoCORFofPLR);
                }
                else
                {
                    return new IB_ChillerElectricEIR();
                }
            };
        

        private static ChillerElectricEIR NewDefaultOpsObj(Model model) => new ChillerElectricEIR(model);
        private static ChillerElectricEIR NewDefaultOpsObj(Model model, IB_CurveBiquadratic CCFofT, IB_CurveBiquadratic EItoCORFofT, IB_CurveQuadratic EItoCORFofPLR) 
            => new ChillerElectricEIR(model, CCFofT.ToOS(model) as CurveBiquadratic, EItoCORFofT.ToOS(model) as CurveBiquadratic, EItoCORFofPLR.ToOS(model) as CurveQuadratic);

        private IB_CurveBiquadratic _CCFofT => this.Children.Get<IB_CurveBiquadratic>(0);
        private IB_CurveBiquadratic _EItoCORFofT => this.Children.Get<IB_CurveBiquadratic>(1);
        private IB_CurveQuadratic _EItoCORFofPLR => this.Children.Get<IB_CurveQuadratic>(2);

        public IB_ChillerElectricEIR() : base(NewDefaultOpsObj(new Model()))
        {
            
        }

        public IB_ChillerElectricEIR(IB_CurveBiquadratic CCFofT, IB_CurveBiquadratic EItoCORFofT, IB_CurveQuadratic EItoCORFofPLR) 
            : base(NewDefaultOpsObj(new Model(), CCFofT, EItoCORFofT, EItoCORFofPLR))
        {
            this.AddChild(CCFofT);
            this.AddChild(EItoCORFofT);
            this.AddChild(EItoCORFofPLR);
        }
        
        public override HVACComponent ToOS(Model model)
        {
            if (this.Children.Count > 0)
            {
                return base.OnNewOpsObj(InitMethodWithChildren, model);

                //Local Method
                ChillerElectricEIR InitMethodWithChildren(Model md)
                {

                    //var c1 = md.addObject(_CCFofT.ToOS().toIdfObject().clone(true)).get().to_CurveBiquadratic().get();
                    //var c2 = md.addObject(_EItoCORFofT.ToOS().toIdfObject().clone(true)).get().to_CurveBiquadratic().get();
                    //var c3 = md.addObject(_EItoCORFofPLR.ToOS().toIdfObject().clone(true)).get().to_CurveQuadratic().get();
                    //var c3 = _EItoCORFofPLR.ToOS().to_CurveQuadratic().get();

                    //return new ChillerElectricEIR(md, c1 , c2 , c3);
                    return new ChillerElectricEIR(md,
                        _CCFofT.ToOS(model) as CurveBiquadratic,
                        _EItoCORFofT.ToOS(model) as CurveBiquadratic,
                        _EItoCORFofPLR.ToOS(model) as CurveQuadratic);

                }
            }
            else
            {
                return base.OnNewOpsObj(NewDefaultOpsObj, model);
            }
             

        }
        
    }



    public sealed class IB_ChillerElectricEIR_DataFieldSet
        : IB_FieldSet<IB_ChillerElectricEIR_DataFieldSet, ChillerElectricEIR>
    {
        private IB_ChillerElectricEIR_DataFieldSet() { }

        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name");
        public IB_Field ReferenceCapacity { get; }
            = new IB_BasicField("ReferenceCapacity", "Capacity");
        
        public IB_Field ReferenceCOP { get; }
            = new IB_BasicField("ReferenceCOP", "COP");
        
        public IB_Field ReferenceLeavingChilledWaterTemperature { get; }
            = new IB_BasicField("ReferenceLeavingChilledWaterTemperature", "LeavingT");
    }
}
