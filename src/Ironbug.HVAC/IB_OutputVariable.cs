using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC.BaseClass
{
    public class IB_OutputVariable
    {
        public string VariableName { get; set; }
        public string TimeStep { get; set; }
        public IB_OutputVariable(string variableName, OutputVariableTimeStep timeStep)
        {
            this.VariableName = variableName;
            switch (timeStep)
            {
                case OutputVariableTimeStep.Detail:
                    this.TimeStep = "Detail";
                    break;
                case OutputVariableTimeStep.Hourly:
                    this.TimeStep = "Hourly";
                    break;
                case OutputVariableTimeStep.Monthly:
                    this.TimeStep = "Monthly";
                    break;
                default:
                    this.TimeStep = "Monthly";
                    break;
            }
        }
        public enum OutputVariableTimeStep
        {
            Detail,
            Hourly,
            Monthly
        }
    }
}
