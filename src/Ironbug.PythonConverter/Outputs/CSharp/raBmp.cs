namespace Honeybee.Radiance.Command
{

	// this is a class
	public class RaBmp : RadianceCommand
    {
		// this is a class Property
		public object RaBmpParameters
		{
			get { return this.RawObj.raBmpParameters; }
			set { this.RawObj.raBmpParameters = value; }
		}

		// this is a class Property
		public object InputFiles
		{
			get { return this.RawObj.inputFiles; }
			set { this.RawObj.inputFiles = value; }
		}

		// this is a class constructor
		public RaBmp(object inputHdrFile,object outputBmpFile,object raBmpParameters)
		{
			PythonEngine engine = new PythonEngine();
			dynamic pyModule = engine.ImportFrom(From: "honeybee.radiance.command.raBmp", Import: "honeybee.radiance.command.raBmp");

			if (pyModule != null)
			{
				this.RawObj = pyModule(inputHdrFile,outputBmpFile,raBmpParameters);
			}

		}

		// this is a class method
		public override object ToRadString(bool relativePath)
		{
			return base.toRadString(relativePath);
		}




    }



} //namespace

