namespace Ironbug.HVAC.BaseClass
{
    public class IB_OutputVariable
    {
        public string VariableName { get; private set; }
        public string TimeStep { get; private set; }

        public IB_OutputVariable(string variableName, TimeSteps timeStep)
        {
            this.VariableName = variableName;
            switch (timeStep)
            {
                case TimeSteps.Detail:
                    this.TimeStep = "Detail";
                    break;

                case TimeSteps.Hourly:
                    this.TimeStep = "Hourly";
                    break;

                case TimeSteps.Monthly:
                    this.TimeStep = "Monthly";
                    break;

                default:
                    this.TimeStep = "Monthly";
                    break;
            }
        }

        public enum TimeSteps
        {
            Detail,
            Hourly,
            Monthly
        }
    }
}