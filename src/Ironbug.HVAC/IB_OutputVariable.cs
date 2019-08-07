namespace Ironbug.HVAC.BaseClass
{
    public class IB_OutputVariable
    {
        public string VariableName { get; private set; }
        public string TimeStep { get; private set; }

        public IB_OutputVariable(string variableName, TimeSteps timeStep)
        {
            this.VariableName = variableName;
            this.TimeStep = timeStep.ToString();

        }

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