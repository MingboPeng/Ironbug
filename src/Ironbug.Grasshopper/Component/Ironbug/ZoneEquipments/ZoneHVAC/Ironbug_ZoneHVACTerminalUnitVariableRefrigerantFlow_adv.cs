using System;
using System.Linq;
using System.Windows.Forms;
using GH_IO.Serialization;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_ZoneHVACTerminalUnitVariableRefrigerantFlow_Adv : Ironbug_HVACWithParamComponent
    {
        
        public Ironbug_ZoneHVACTerminalUnitVariableRefrigerantFlow_Adv()
          : base("IB_ZoneHVACTerminalUnitVariableRefrigerantFlow+", "VRFTerminal+",
              "Use this component to set customized cooling coil, heating coil, or fan for VRF terminal unit.",
              "Ironbug", "04:ZoneEquipments",
              typeof(HVAC.IB_ZoneHVACTerminalUnitVariableRefrigerantFlow_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager[pManager.AddGenericParameter("CoilCoolingDXVariableRefrigerantFlow", "ClnCoil",
                "Optional input, but use CoilCoolingDXVariableRefrigerantFlow only", GH_ParamAccess.item)].Optional = true;

            pManager[pManager.AddGenericParameter("CoilHeatingDXVariableRefrigerantFlow", "HtnCoil",
                "Optional input, but use CoilHeatingDXVariableRefrigerantFlow only", GH_ParamAccess.item)].Optional = true;

            pManager[pManager.AddGenericParameter("FanOnOff", "Fan",
                "Optional input, but use FanOnOff only", GH_ParamAccess.item)].Optional = true;
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneHVACTerminalUnitVariableRefrigerantFlow", "VRFUnit", "Connect to Zone's equipment or airloop supply side", GH_ParamAccess.item);
            pManager.AddGenericParameter("ZoneHVACTerminalUnitVariableRefrigerantFlow", "ToOutdoorUnit", "Connect to VRF system", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            this.Message = this._airloop ? "Airloop obj" : null;
            IB_CoilCoolingDXVariableRefrigerantFlow cCoil= null;
            IB_CoilHeatingDXVariableRefrigerantFlow hCoil = null;
            IB_FanOnOff fan = null;

            if (!DA.GetData(0, ref cCoil)) cCoil = new IB_CoilCoolingDXVariableRefrigerantFlow();
            if (!DA.GetData(1, ref hCoil)) hCoil = new IB_CoilHeatingDXVariableRefrigerantFlow();
            if (!DA.GetData(2, ref fan)) fan = new IB_FanOnOff();

            var obj = new HVAC.IB_ZoneHVACTerminalUnitVariableRefrigerantFlow(cCoil, hCoil, fan);
            
            this.SetObjParamsTo(obj);

            for (int i = 0; i < this.Params.Output.Count; i++)
            {
                DA.SetData(i, obj);
            }
        }
        protected override System.Drawing.Bitmap Icon => Properties.Resources.VRFUnit_adv;

        public override Guid ComponentGuid => new Guid("{6DF5E370-7A09-4CB7-9A75-AA1D822346E9}");

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