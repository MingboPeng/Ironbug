namespace honeybee.radiance.command.raTiff
{

	// this is a class
	public class RaTiff : RadianceCommand
    {



				
		// this is a class Property
		public object RaTiffParameters
		{
			get { return this.RawObj.raTiffParameters; }
			set { this.RawObj.raTiffParameters = value; }
		}

			
		// this is a class Property
		public object InputFiles
		{
			get { return this.RawObj.inputFiles; }
			set { this.RawObj.inputFiles = value; }
		}

			


		// this is a class constructor
		public RaTiff(object inputHdrFile,object outputTiffFile,object raTiffParameters)
		{
			PythonEngine engine = new PythonEngine();
			dynamic pyModule = engine.ImportFrom(From: "honeybee.radiance.command.raTiff", Import: "RaTiff");

			if (pyModule != null)
			{
				this.RawObj = pyModule(inputHdrFile,outputTiffFile,raTiffParameters);
			}

		}
		
		// this is a class method
		public override object ToRadString(object relativePath)
		{
			return base.toRadString(relativePath);
				
		}
			



    }
	


} //namespace






