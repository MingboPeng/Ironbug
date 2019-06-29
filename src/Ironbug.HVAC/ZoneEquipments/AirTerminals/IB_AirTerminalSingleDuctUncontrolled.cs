using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;


namespace Ironbug.HVAC
{
    public class IB_AirTerminalSingleDuctConstantVolumeNoReheat: IB_AirTerminal
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AirTerminalSingleDuctConstantVolumeNoReheat();

        private static Type _refOsType;
       
        public static Type GetRefOsType()
        {
            if (_refOsType != null) return _refOsType;

            var v0 = typeof(Model).Assembly.GetName().Version;
            var v1 = new System.Version("2.7.0");
            var isOldVersion = v0.CompareTo(v1) < 0;
            //AirTerminalSingleDuctUncontrolled
            //AirTerminalSingleDuctConstantVolumeNoReheat

            if (isOldVersion)
            {
                return typeof(Model).Assembly.GetType("OpenStudio.AirTerminalSingleDuctUncontrolled");
            }
            else
            {
                return typeof(Model).Assembly.GetType("OpenStudio.AirTerminalSingleDuctConstantVolumeNoReheat");
            }

        }
        
        private static StraightComponent NewDefaultOpsObj(Model model) {
            
            var tp = GetRefOsType();

            var instance = (StraightComponent)Activator.CreateInstance(tp, new Object[] { model, model.alwaysOnDiscreteSchedule()});
            return instance;
          
        } 

        public IB_AirTerminalSingleDuctConstantVolumeNoReheat():base(NewDefaultOpsObj(new Model()))
        {
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
        
        
    }


    public sealed class IB_AirTerminalSingleDuctConstantVolumeNoReheat_FieldSet
        : IB_FieldSet<IB_AirTerminalSingleDuctConstantVolumeNoReheat_FieldSet, AirTerminalSingleDuctConstantVolumeNoReheat>
    {
        internal override Type RefOpsType => IB_AirTerminalSingleDuctConstantVolumeNoReheat.GetRefOsType();
        internal override Type RefEpType => RefOpsType;
        private IB_AirTerminalSingleDuctConstantVolumeNoReheat_FieldSet() { }

    }

}
