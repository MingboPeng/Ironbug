public class Falsecolor:RadianceCommand
{ 
	public Falsecolor (Object inputImageFile,Object outputFile,Object falsecolorParameters)
	{
		PythonEngine engine = new PythonEngine();
		dynamic pyModule = engine.ImportFrom(From: "honeybee.radiance.command.falsecolor", Import: "Falsecolor");
		if (pyModule != null)
		{
			this.RawObj = pyModule(HdrFile, TiffFile);
		}
	} 
}