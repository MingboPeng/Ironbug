using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Kernel = Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_HVACScenario : Ironbug_Component, IGH_VariableParameterComponent
    {
        private string _resultParamName = "HVAC";

        public Ironbug_HVACScenario()
          : base("IB_HVACScenario", "IB_HVACScenario",
              "Group multiple HVAC systems as a HVAC design option (aka. HVACScenario)",
              "Ironbug", "HVAC")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "A human readable name for this HVAC scenario. By default, the ID will be used for name.", GH_ParamAccess.item);
            pManager.AddGenericParameter($"{_resultParamName}[1]", $"{_resultParamName}[1]", "A HVAC system from IB_HVACSystem. Please right-click the parameter to rename the system.", GH_ParamAccess.list);

            pManager[0].Optional = true;
            pManager[1].DataMapping = GH_DataMapping.Flatten;
        }
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Scenario", "Scenario", "A HVAC Scenario that contains a group of HVAC systems", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var id = this.InstanceGuid.ToString().Substring(0, 6);
            this.Message = $"ID: {id}";
            var allSystems = new List<HVAC.IB_HVACSystem>();
            var name = "Unnamed";

            DA.GetData(0, ref name);
            var inputs = this.Params.Input;
            for (int i = 1; i < inputs.Count; i++)
            {
                var systems = new List<HVAC.IB_HVACSystem>();
                DA.GetDataList(i, systems);
                systems = systems.Where(_ => _ != null).ToList();
                if (!systems.Any())
                    continue;
                var paramName = inputs[i].NickName ?? $"Unnamed System {i}";
                if (systems.Count > 1)
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, $"Found more than one system from the input [{paramName}]. Only the first system is taken!");
                var system = systems.FirstOrDefault();
                system.DisplayName = paramName;
                allSystems.Add(system);
            }
            
            var hvac = new HVAC.IB_HVACScenario(id, name, allSystems);
            DA.SetData(0, hvac);
        }


        #region VariableParameterMethods
        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            if (side != GH_ParameterSide.Input)
                return false;

            // directory input or the first result
            if (index <= 0)
                return false;

            return true;
        }

        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            if (side != GH_ParameterSide.Input)
                return false;

            // directory input or the first result
            if (index <= 0)
                return false;

            return true;
        }

        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            var inParam = new Kernel.Parameters.Param_GenericObject();
            return inParam;
        }

        public bool DestroyParameter(GH_ParameterSide side, int index)
        {
            return true;
        }

        public void VariableParameterMaintenance()
        {
            int inputParamCount = this.Params.Input.Count;
            for (int i = 1; i < inputParamCount; i++)
            {
                var inParam = this.Params.Input[i];
                //var refObj = inParam.Sources.FirstOrDefault();

                var name = inParam.NickName == "Data" ? $"{_resultParamName}[{i}]" : inParam.NickName;
                name = inParam.NickName.StartsWith($"{_resultParamName}[") ? $"{_resultParamName}[{i}]" : name;

                // inputs
                inParam.Name = name;
                inParam.NickName = name;
                inParam.Description = "A HVAC system from IB_HVACSystem. Please right-click the parameter to rename the system.";
                inParam.Access = GH_ParamAccess.list;
                inParam.DataMapping = GH_DataMapping.Flatten;
                inParam.Optional = true;

                inParam.ObjectChanged -= InParam_ObjectChanged;   
                inParam.ObjectChanged += InParam_ObjectChanged;
                //inParam.MutableNickName = false;

            }
        }

        private void InParam_ObjectChanged(IGH_DocumentObject sender, GH_ObjectChangedEventArgs e)
        {
            if (e.Type == GH_ObjectEventType.NickName)
                this.ExpireSolution(true);

            else if (e.Type == GH_ObjectEventType.Sources)
            {
                if (!this.Params.Input.LastOrDefault().Sources.Any())
                    return;

                var index = Params.Input.Count;
                IGH_Param newParam = CreateParameter(GH_ParameterSide.Input, index);
                Params.RegisterInputParam(newParam, index);
                VariableParameterMaintenance();
                this.Params.OnParametersChanged();
            }
        }

        //This is for if any source connected, reconnected, removed, replacement 
        private void ParamSourcesChanged(Object sender, GH_ParamServerEventArgs e)
        {
            //if (e.ParameterSide != GH_ParameterSide.Input)
            //    return;

            //if (this.Params.Input.LastOrDefault().Sources.Any())
            //{
            //    var index = Params.Input.Count;
            //    IGH_Param newParam = CreateParameter(GH_ParameterSide.Input, index);
            //    Params.RegisterInputParam(newParam, index);
            //    VariableParameterMaintenance();
            //    this.Params.OnParametersChanged();
            //}

        }



        #endregion


        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("DE766640-4BA1-44BE-AE66-138B2D97E4CD");


    }
}