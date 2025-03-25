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

        private IB_CurveBiquadratic _CCFofT => this.GetChild<IB_CurveBiquadratic>(0);
        private IB_CurveBiquadratic _EItoCORFofT => this.GetChild<IB_CurveBiquadratic>(1);
        private IB_CurveQuadratic _EItoCORFofPLR => this.GetChild<IB_CurveQuadratic>(2);

        public IB_ChillerElectricEIR() : base(NewDefaultOpsObj)
        {
            
        }

        public IB_ChillerElectricEIR(IB_CurveBiquadratic CCFofT, IB_CurveBiquadratic EItoCORFofT, IB_CurveQuadratic EItoCORFofPLR) 
            : base((Model m) => NewDefaultOpsObj(m, CCFofT, EItoCORFofT, EItoCORFofPLR))
        {
            this.AddChild(CCFofT);
            this.AddChild(EItoCORFofT);
            this.AddChild(EItoCORFofPLR);

            // add a fake condenser loop for water cooled chiller to be added to the demand side
            var ghost = this.GhostOSObject as ChillerElectricEIR;
            var ghostModel = ghost.TryGetObjectModel(this.GhostOSModel);
            var addGhostCondenserLoop = new PlantLoop(ghostModel);
            addGhostCondenserLoop.addDemandBranchForComponent(ghost);

        }

        internal HVACComponent ToOS(Model model, bool withAttributes)
        {
            if (this.Children.Count > 0)
            {
                // water cooled chiller
                return base.OnNewOpsObj(InitMethodWithChildren, model, withAttributes);

            }
            else
            {
                // air cooled chiller
                return base.OnNewOpsObj(NewDefaultOpsObj, model, withAttributes);
            }


            //Local Method
            ChillerElectricEIR InitMethodWithChildren(Model md)
            {

                return new ChillerElectricEIR(md,
                    _CCFofT.ToOS(model) as CurveBiquadratic,
                    _EItoCORFofT.ToOS(model) as CurveBiquadratic,
                    _EItoCORFofPLR.ToOS(model) as CurveQuadratic);
            }

        }

        public override HVACComponent ToOS(Model model)
        {
            var obj = ToOS(model, false);
            //CondenserType will be adjusted automatically by OpenStudio
            this.CustomAttributes.RemoveAll(_ => _.Field == IB_ChillerElectricEIR_FieldSet.Value.CondenserType);
            this.ApplyAttributesToObj(model, obj);
            return obj;
        }
        
    }



    public sealed class IB_ChillerElectricEIR_FieldSet
        : IB_FieldSet<IB_ChillerElectricEIR_FieldSet, ChillerElectricEIR>
    {
        private IB_ChillerElectricEIR_FieldSet() { }

        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name");
        public IB_Field ReferenceCapacity { get; }
            = new IB_BasicField("ReferenceCapacity", "Capacity");
        
        public IB_Field ReferenceCOP { get; }
            = new IB_BasicField("ReferenceCOP", "COP");

        public IB_Field CondenserType { get; }
            = new IB_BasicField("CondenserType", "CondenserType");

        public IB_Field ReferenceLeavingChilledWaterTemperature { get; }
            = new IB_BasicField("ReferenceLeavingChilledWaterTemperature", "LeavingT");
    }
}
