using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_SizingZone : IB_ModelObject
    {
        private static SizingZone InitMethod(Model model) => new SizingZone(model, new ThermalZone(model));
        
        public IB_SizingZone():base(InitMethod(new Model()))
        {
        }
        
        /// <summary>
        /// This is the base Duplicate() for IB_SizingZone, you need to call SetSizingZone in IB_ThermalZone to link SizingZone to ThermalZone; 
        /// Or you can use DuplicateToZone(IB_ThermalZone ThermalZone) instead.
        /// </summary>
        /// <returns>IB_ModelObject</returns>
        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIB_ModelObject(() => new IB_SizingZone());
        }

        ///// <summary>
        ///// It makes no sense to call this when there is no ThermalZone!
        ///// Use ToOS(ThermalZone thermalZone) instead.
        ///// </summary>
        ///// <returns>IB_ModelObject</returns>
        //public override ModelObject ToOS(Model model)
        //{
        //    ////create non-zone-connected sizingZone.
        //    //return base.ToOS(InitMethod, model);
        //    return null;
        //}

        public ModelObject ToOS(ThermalZone thermalZone)
        {
            //create a sizingZone to target thermalZone
            var targetModel = thermalZone.model();
            return base.ToOS((Model model)=> new SizingZone(model, thermalZone), targetModel);
        }
        
    }

    //public class IB_SizingZone_DataFieldSet : IB_DataFieldSet
    //{
    //    protected override IddObject RefIddObject => new ThermalZone(new Model()).sizingZone().iddObject();

        
    //    public static readonly IB_DataField Multiplier
    //        = new IB_DataField("Multiplier", "Multiplier", dbType, BasicSetting: true);

    //    public static readonly IB_DataField ZoneInsideConvectionAlgorithm
    //        = new IB_DataField("ZoneInsideConvectionAlgorithm", "InConvection", dbType, true);


    //    public static readonly IB_DataField ZoneOutsideConvectionAlgorithm
    //        = new IB_DataField("ZoneOutsideConvectionAlgorithm", "OutConvection", dbType, BasicSetting: true);


    //    public static IEnumerable<IB_DataField> GetList()
    //    {
    //        return GetList<IB_SizingZone_DataFieldSet>();
    //    }

    //    public static IB_DataField GetAttributeByName(string name)
    //    {
    //        return GetAttributeByName<IB_SizingZone_DataFieldSet>(name);
    //    }



    //}
}
