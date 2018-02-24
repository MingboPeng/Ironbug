using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ThermalZone : IB_ModelObject
    {
        public IB_ModelObject AirTerminal { get; private set; }
        public List<IB_HVACComponent> ZoneEquipments { get; set; }
        
        private IB_SizingZone IB_SizingZone { get;  set; }
        private static ThermalZone InitMethod(Model model) => new ThermalZone(model);
        public IB_ThermalZone():base(InitMethod(new Model()))
        {
            //TODO: this should be the same as HBZone name
            this.IB_SizingZone = new IB_SizingZone(this);
            
        }

        public ModelObject GetModelObject()
        {
            return base.GhostOSObject;
        }

        /// <summary>
        /// Do not use this 
        /// Use this method to add a new SizingZone to this ThermalZone;
        /// Or construct the IB_SizingZone by IB_SizingZone(IB_ThermalZone);
        /// </summary>
        /// <param name="NewSizingZone"></param>
        public void SetSizingZone(IB_SizingZone NewSizingZone)
        {
            this.IB_SizingZone = NewSizingZone.DuplicateToZone(this);
        }

        
        public void SetAirTerminal(IB_ModelObject AirTerminal)
        {
            this.AirTerminal = AirTerminal;
        }
        
        public override ModelObject ToOS(Model model)
        {
            var zone = (ThermalZone)base.ToOS(InitMethod, model);
            var sizing = (SizingZone)this.IB_SizingZone.ToOS(model,zone);

            //TODO:AirTerminal, equipments
            //zone.ter this.AirTerminal.ToOS(model);
            return zone;
        }

        public override IB_ModelObject Duplicate()
        {
            //TODO: need to duplicate child objs as well
            //TODO: no need to make connections
            var newObj = (IB_ThermalZone)base.DuplicateIB_ModelObject(() => new IB_ThermalZone());
            newObj.SetAirTerminal((IB_HVACComponent)this.AirTerminal.Duplicate());
            this.IB_SizingZone.DuplicateToZone(newObj);


            foreach (var item in this.ZoneEquipments)
            {
                var newItem = (IB_HVACComponent)item.Duplicate();
                newObj.ZoneEquipments.Add(newItem);
            }

            
            return newObj;
        }
    }

    public class IB_ThermalZone_DataFieldSet : IB_DataFieldSet
    {
        protected override IddObject RefIddObject => new ThermalZone(new Model()).iddObject();


        //https://bigladdersoftware.com/epx/docs/8-8/input-output-reference/group-thermal-zone-description-geometry.html#field-zone-inside-convection-algorithm
        //Following list items are fields that I want to have picked for GH user to edit
        public static readonly IB_DataField Name
            = new IB_DataField("Name", "Name", strType, true)
            {
                Description = "A unique identifying name for each coil."
            };


        public static readonly IB_DataField Multiplier
            = new IB_DataField("Multiplier", "Multiplier", intType, BasicSetting: true)
            {
                Description = "Zone Multiplier is designed as a “multiplier” for floor area, zone loads, and energy consumed by internal gains. "+
                "It takes the calculated load for the zone and multiplies it, sending the multiplied load to the attached HVAC system. "+
                "The HVAC system size is specified to meet the entire multiplied zone load and will report the amount of the load met in the Zone Air System Sensible Heating or Cooling Energy/Rate output variable. "+
                "Autosizing automatically accounts for multipliers. Metered energy consumption by internal gains objects such as Lights or Electric Equipment will be multiplied. "+
                "The default is 1."
            };

        public static readonly IB_DataField ZoneInsideConvectionAlgorithm
            = new IB_DataField("ZoneInsideConvectionAlgorithm", "InConvection", strType, BasicSetting: true)
            {
                Description = "The Zone Inside Convection Algorithm field is optional.\n"+
                "This field specifies the convection model to be used for the inside face of heat transfer surfaces associated with this zone. \n\n"+
                "The choices are: \n" +
                "1) Simple (constant natural convection - ASHRAE) \n" +
                "2) TARP (combines natural and wind-driven convection correlations from laboratory measurements on flat plates)\n" +
                "3) CeilingDiffuser (ACH based forced and mixed convection correlations for ceiling diffuser configuration with simple natural convection limit), \n"+
                "4) AdaptiveConvectionAlgorithm (complex arrangement of various models that adapt to various zone conditions and can be customized)\n"+
                "5) TrombeWall (variable natural convection in an enclosed rectangular cavity).\n"+
                "See the Inside Convection Algorithm object for further descriptions of the available models."+
                "If omitted or blank, the algorithm specified in the SurfaceConvectionAlgorithm:Inside object is the default."
            };


        public static readonly IB_DataField ZoneOutsideConvectionAlgorithm
            = new IB_DataField("ZoneOutsideConvectionAlgorithm", "OutConvection", strType, BasicSetting: true)
            {
                Description = "The Zone Outside Convection Algorithm field is optional. "+
                "This field specifies the convection model to be used for the outside face of heat transfer surfaces associated with this zone. "+
                "The choices are: 1) SimpleCombined, 2) TARP, 3) DOE-2, 4) MoWiTT, and 5) AdaptiveConvectionAlgorithm. "+
                "The simple convection model applies heat transfer coefficients depending on the roughness and windspeed. "+
                "This is a combined heat transfer coefficient that includes radiation to sky, ground, and air. "+
                "The correlation is based on Figure, Page 25.1 (Thermal and Water Vapor Transmission Data), 2001 ASHRAE Handbook of Fundamentals."+
                "\n\nThe other convection models apply heat transfer coefficients depending on the roughness, windspeed,and terrain of the building’s location." +
                "These are convection only heat transfer coefficients; radiation heat transfer coefficients are calculated automatically by the program."+
                "The TARP algorithm was developed for the TARP software and combines natural and wind-driven convection correlations from laboratory measurements on flat plates."+
                "The DOE-2 and MoWiTT were derived from field measurements.The AdaptiveConvectionAlgorithm model is an dynamic algorithm that organizes a large number of different convection models and automatically selects the one that best applies. "+
                "The adaptive convection algorithm can also be customized using the SurfaceConvectionAlgorithm:Outside:AdaptiveModelSelections input object."+
                "All algorithms are described more fully in the Engineering Reference."+
                "If omitted or blank, the algorithm specified in the SurfaceConvectionAlgorithm:Outside object is the default."
            };
        


    }
}
