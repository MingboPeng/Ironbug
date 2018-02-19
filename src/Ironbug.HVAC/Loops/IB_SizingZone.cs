using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_SizingZone : IB_ModelObject
    {
        private static SizingZone InitMethod(Model model) => new SizingZone(model, new ThermalZone(model));
        private static SizingZone InitMethod(ThermalZone thermalZone) => new SizingZone(thermalZone.model(), thermalZone);

        public IB_SizingZone():base(InitMethod(new Model()))
        {
            base.SetName("SizingZone");
        }
        public IB_SizingZone(IB_ThermalZone ThermalZone) : base(InitMethod((ThermalZone)ThermalZone.GetModelObject()))
        {
            base.SetName("SizingZone");
        }


        //public IB_SizingZone(Func<SizingZone> func):base(func.Invoke())
        //{

        //}


        /// <summary>
        /// This is the base Duplicate() for IB_SizingZone, you need to call SetSizingZone in IB_ThermalZone to link SizingZone to ThermalZone; 
        /// Or you can use DuplicateToZone(IB_ThermalZone ThermalZone) instead.
        /// </summary>
        /// <returns>IB_ModelObject</returns>
        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIB_ModelObject(() => new IB_SizingZone());
        }

        public IB_SizingZone DuplicateToZone(IB_ThermalZone ThermalZone)
        {
            return (IB_SizingZone)base.DuplicateIB_ModelObject(() => new IB_SizingZone(ThermalZone));
        }
        
        public override ModelObject ToOS(Model model)
        {
            return base.ToOS(InitMethod, model);
        }

        public ModelObject ToOS(Model model, ThermalZone thermalZone)
        {
            return base.ToOS((Model m)=> new SizingZone(m, thermalZone), model);
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
