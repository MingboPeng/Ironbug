using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_EnergyManagementSystemProgramCallingManager : Ironbug_Component
    {
        public Ironbug_EnergyManagementSystemProgramCallingManager()
          : base("IB_EMSProgramCallingManager", "EMSProgramCallingManager",
              "Description",
              "Ironbug", "06:Sizing & Controller")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        private HVAC.IB_EnergyManagementSystemProgramCallingManager_FieldSet _fieldSet = HVAC.IB_EnergyManagementSystemProgramCallingManager_FieldSet.Value;
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("_name_", "_name_", "Name", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddTextParameter("CallingPoint_", "CallingPoint_", _fieldSet.CallingPoint.DetailedDescription, GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("_programs", "_programs", "A list of EnergyManagementSystemPrograms", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("programCallingManager", "manager", "ProgramCallingManager", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_EnergyManagementSystemProgramCallingManager();
            string name = null;
            if(DA.GetData(0, ref name))
                obj.AddCustomAttribute(_fieldSet.Name, name);

            string callingPoint = null;
            if (DA.GetData(1, ref callingPoint))
                obj.SetFieldValue(_fieldSet.CallingPoint, callingPoint);

            var programs = new List<HVAC.IB_EnergyManagementSystemProgram>();
            DA.GetDataList(2, programs);
            obj.Programs = programs;

            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.EMS_ProgramManager;

        public override Guid ComponentGuid => new Guid("C1CFB67F-89D6-48BE-BD9A-1EA824D6FF5D");
    }
}