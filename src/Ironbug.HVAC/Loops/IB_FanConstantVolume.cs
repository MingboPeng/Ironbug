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

        private FanConstantVolume osFanConstantVolume { get; set; }
        
        public IB_FanConstantVolume()
        {
            this.ghostHVACComponent = new FanConstantVolume(new Model());
            
        }

        public override bool AddToNode(ref Model model, Node node)
        {
            this.osFanConstantVolume = new FanConstantVolume(model);
            this.osFanConstantVolume.SetCustomAttributes(this.CustomAttributes);
            return this.osFanConstantVolume.addToNode(node);
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

    public class IB_FanConstantVolume_Attributes : IB_DataAttributeSet
    {
        private static readonly FanConstantVolume refObj = new FanConstantVolume(new Model());


        //https://openstudio-sdk-documentation.s3.amazonaws.com/cpp/OpenStudio-1.7.0-doc/model/html/classopenstudio_1_1model_1_1_fan_constant_volume.html
        public static readonly IB_DataAttribute FanEfficiency
            = new IB_DataAttribute("FanEfficiency", "Efficiency", dbType);

        public static readonly IB_DataAttribute PressureRise
            = new IB_DataAttribute("PressureRise", "PressureRise", dbType);

        public static readonly IB_DataAttribute MotorEfficiency
            = new IB_DataAttribute("MotorEfficiency", "MotorEfficiency", dbType);


        public static IEnumerable<IB_DataAttribute> GetList()
        {
            return typeof(IB_FanConstantVolume_Attributes).GetFields()
                            .Select(_ => (IB_DataAttribute)_.GetValue(null));
        }

        public static IB_DataAttribute GetAttributeByName(string name)
        {
            var field = typeof(IB_FanConstantVolume_Attributes).GetField(name);
            return (IB_DataAttribute)field.GetValue(null);
        }
        

    }
}
