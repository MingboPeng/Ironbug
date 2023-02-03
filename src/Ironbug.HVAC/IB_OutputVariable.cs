using System.Runtime.Serialization;
using System;

namespace Ironbug.HVAC.BaseClass
{
    public class IB_OutputVariable : IEquatable<IB_OutputVariable>
    {
        [DataMember]
        public string VariableName { get; private set; }
        [DataMember]
        public string TimeStep { get; private set; }

        private IB_OutputVariable() { }
        public IB_OutputVariable(string variableName, TimeSteps timeStep)
        {
            this.VariableName = variableName;
            this.TimeStep = timeStep.ToString();

        }

        public bool ToOS(OpenStudio.Model model, string keyName)
        {
            var outV = new OpenStudio.OutputVariable(this.VariableName, model);
            var success = outV.setReportingFrequency(this.TimeStep);
            success &= outV.setKeyValue(keyName);
            return success;
        }

        public override bool Equals(object obj) => this.Equals(obj as IB_OutputVariable);
        public bool Equals(IB_OutputVariable other)
        {
            if (other == null)
                return false;
            return this.VariableName == other.VariableName && this.TimeStep == other.TimeStep;
        }

        public static bool operator ==(IB_OutputVariable x, IB_OutputVariable y)
        {
            if (x is null)
                return y is null ? true : false;
            return x.Equals(y);
        }

        public static bool operator !=(IB_OutputVariable x, IB_OutputVariable y) => !(x == y);


        public enum TimeSteps
        {
            Detail,
            Hourly,
            Daily,
            Monthly,
            RunPeriod
        }
    }
}