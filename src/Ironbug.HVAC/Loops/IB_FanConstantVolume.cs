using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_FanConstantVolume : IB_HVACComponent
    {
        //Fan Constant Volume 1 !- Name  
        //0.7                  !- Fan Efficiency(Default: 0.7)
        //250.0                !- Pressure Rise[kg / m * s ^ 2]  (Default: 250.0)
        //AutoSize             !- Maximum Flow Rate[m ^ 3 / s] 
        //0.9                  !- Motor Efficiency(Default: 0.9)
        //1.0                  !- Motor In Airstream Fraction(Default: 1.0)
        //General              !- End-Use Subcategory(Default: General)

        //private FanConstantVolume osFanConstantVolume { get; set; }
        private static FanConstantVolume InitMethod(ref Model model) => new FanConstantVolume(model);

        public IB_FanConstantVolume()
        {
            this.ghostModelObject = new FanConstantVolume(new Model());
        }

        public override bool AddToNode(ref Model model, Node node)
        {
            //this.osFanConstantVolume = new FanConstantVolume(model);
            //this.osFanConstantVolume.SetCustomAttributes(this.CustomAttributes);
            return ((FanConstantVolume)this.ToOS(ref model)).addToNode(node);
        }

        public override ParentObject ToOS(ref Model model)
        {
            return (FanConstantVolume)this.ToOS(InitMethod, ref model);
        }
    }

    //A = fan.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

    //Void Dispose()
    //OpenStudio.Schedule availabilitySchedule()
    //Double fanEfficiency()
    //Double pressureRise()
    //Double motorEfficiency()
    //Double motorInAirstreamFraction()
    //System.String endUseSubcategory()
    //Boolean setAvailabilitySchedule(OpenStudio.Schedule)
    //Void setFanEfficiency(Double)
    //Void setPressureRise(Double)
    //Void setMotorEfficiency(Double)
    //Void setMotorInAirstreamFraction(Double)
    //Void setEndUseSubcategory(System.String)
    //OpenStudio.OptionalDouble maximumFlowRate()
    //OpenStudio.OSOptionalQuantity getMaximumFlowRate(Boolean)
    //OpenStudio.OSOptionalQuantity getMaximumFlowRate()
    //Boolean isMaximumFlowRateAutosized()
    //Boolean setMaximumFlowRate(Double)
    //Boolean setMaximumFlowRate(OpenStudio.Quantity)
    //Void resetMaximumFlowRate()
    //Void autosizeMaximumFlowRate()
    //OpenStudio.OptionalDouble autosizedMaximumFlowRate()

    public class IB_FanConstantVolume_DataFields : IB_DataFieldSet
    {
        protected override IddObject RefIddObject => new FanConstantVolume(new Model()).iddObject();
        //https://openstudio-sdk-documentation.s3.amazonaws.com/cpp/OpenStudio-2.4.0-doc/model/html/classopenstudio_1_1model_1_1_fan_constant_volume.html

        public static readonly IB_DataField Name
            = new IB_DataField("Name", "Name", strType, true);

        public static readonly IB_DataField FanEfficiency
            = new IB_DataField("FanEfficiency", "Efficiency", dbType, true);

        public static readonly IB_DataField PressureRise
            = new IB_DataField("PressureRise", "PressureRise", dbType);

        public static readonly IB_DataField MotorEfficiency
            = new IB_DataField("MotorEfficiency", "MotorEfficiency", dbType);


        public static IEnumerable<IB_DataField> GetList()
        {
            return GetList<IB_FanConstantVolume_DataFields>();
        }

        public static IB_DataField GetAttributeByName(string name)
        {
            return GetAttributeByName<IB_FanConstantVolume_DataFields>(name);
        }


    }
}
