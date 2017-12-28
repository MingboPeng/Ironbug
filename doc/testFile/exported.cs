public class Falsecolor:RadianceCommand
{ 
	public Falsecolor (Object inputImageFile,Object outputFile,Object falsecolorParameters)
	{
		PythonEngine engine = new PythonEngine();
		dynamic pyModule = engine.ImportFrom(From: "honeybee.radiance.command.falsecolor", Import: "Falsecolor");
		if (pyModule != null)
		{
			this.RawObj = pyModule(inputImageFile,outputFile,falsecolorParameters);
		}
	}
	public toRadString (Bool relativePath)
	{
		return RawObj.toRadString(relativePath);
	}
	public execute ()
	{
		return RawObj.execute();
	} 
}