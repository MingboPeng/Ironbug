using System;
using System.Linq;
using System.Windows.Forms;
using GH_IO.Serialization;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_ZoneHVACTerminalUnitVariableRefrigerantFlow : Ironbug_DuplicableHVACWithParamComponent
    {

        /// Initializes a new instance of the Ironbug_ZoneHVACTerminalUnitVariableRefrigerantFlow class.
        
        
        public Ironbug_ZoneHVACTerminalUnitVariableRefrigerantFlow()
          : base("Ironbug_ZoneHVACTerminalUnitVariableRefrigerantFlow", "VRFTerminal",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(HVAC.IB_ZoneHVACTerminalUnitVariableRefrigerantFlow_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneHVACTerminalUnitVariableRefrigerantFlow", "VRFUnit", "Connect to Zone's equipment", GH_ParamAccess.item);
            pManager.AddGenericParameter("ZoneHVACTerminalUnitVariableRefrigerantFlow", "ToOutdoorUnit", "Connect to VRF system", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            this.Message = this._airloop ? "Airloop obj" : null;
            var obj = new HVAC.IB_ZoneHVACTerminalUnitVariableRefrigerantFlow();
            
            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
 
            for (int i = 0; i < this.Params.Output.Count; i++)
            {
                DA.SetDataList(i, objs);
            }
        }
        protected override System.Drawing.Bitmap Icon => Properties.Resources.VRFUnit;

        public override Guid ComponentGuid => new Guid("1aa85a4b-f306-41ba-9723-5d78ecbec750");
        private bool _airloop = false;
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            Menu_AppendItem(menu, "Airloop object", AirloopObjClicked, true, _airloop)
                   .ToolTipText = "Make this VRF terminal as an airloop object";
            Menu_AppendSeparator(menu);

            base.AppendAdditionalComponentMenuItems(menu);
        }

        private void AirloopObjClicked(object sender, EventArgs e)
        {
            this._airloop = !this._airloop;
            UpdateComponent();
        }

        private void UpdateComponent()
        {
            if (this._airloop)
            {
                if (this.Params.Output.Count == 2)
                {
                    var lastP = this.Params.Output.Last();
                    Params.UnregisterOutputParameter(lastP);
                }
            }
            else
            {
                if (this.Params.Output.Count == 1)
                {
                    IGH_Param newParam = new Param_GenericObject();

                    newParam.Name = "ZoneHVACTerminalUnitVariableRefrigerantFlow";
                    newParam.NickName = "ToOutdoorUnit";
                    newParam.Description = $"Connect to VRF system";
                    newParam.Access = GH_ParamAccess.item;
                    Params.RegisterOutputParam(newParam);
                }
            }

            this.ExpireSolution(true);
        }

        public override bool Read(GH_IReader reader)
        {
            if (reader.ItemExists(nameof(_airloop)))
            {
                _airloop = reader.GetBoolean(nameof(_airloop));
                UpdateComponent();
            }
            return base.Read(reader);
        }
        public override bool Write(GH_IWriter writer)
        {
            writer.SetBoolean(nameof(_airloop), _airloop);
            return base.Write(writer);
        }
    }
}