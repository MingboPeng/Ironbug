﻿using System;
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
                _refOsType = typeof(Model).Assembly.GetType("OpenStudio.AirTerminalSingleDuctUncontrolled");
                
            }
            else
            {
                _refOsType = typeof(Model).Assembly.GetType("OpenStudio.AirTerminalSingleDuctConstantVolumeNoReheat");
            }
            return _refOsType;

        }
        
        private static StraightComponent NewDefaultOpsObj(Model model) 
        {
            
            return new AirTerminalSingleDuctConstantVolumeNoReheat(model, model.alwaysOnDiscreteSchedule());
          
        } 

        public IB_AirTerminalSingleDuctConstantVolumeNoReheat():base(NewDefaultOpsObj)
        {
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
        
        
    }


    public sealed class IB_AirTerminalSingleDuctConstantVolumeNoReheat_FieldSet
        : IB_FieldSet<IB_AirTerminalSingleDuctConstantVolumeNoReheat_FieldSet>
    {
        internal override Type RefOpsType => IB_AirTerminalSingleDuctConstantVolumeNoReheat.GetRefOsType();
        internal override Type RefEpType => RefOpsType;
        private IB_AirTerminalSingleDuctConstantVolumeNoReheat_FieldSet() { }

    }

}
