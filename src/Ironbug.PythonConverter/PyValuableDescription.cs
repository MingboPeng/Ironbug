namespace Ironbug.PythonConverter
{
    //valuableObj = {"Name":name,"Type":objType,"DefaultValue":data}
    public class PyValuableDescription
    {
        public string Name { get; set; }
        public dynamic DefaultValue { get; set; }
        public string Type { get; set; }

        public string NameCS { get; set; }
        public string TypeName { get; set; }
        public string TypeNameValue { get; set; }

        public PyValuableDescription()
        {

        }

        public PyValuableDescription(dynamic PyValueObj)
        {
            this.Name = PyValueObj["Name"];
            this.Type = PyValueObj["Type"];
            this.DefaultValue = PyValueObj["DefaultValue"];

            this.NameCS = this.Name.CheckNamingForCS();
            this.TypeName = string.Format("{0} {1}", this.Type, this.NameCS);

            if (this.DefaultValue != null)
            {
                if (this.Type == "string")
                {
                    if (string.IsNullOrWhiteSpace(this.DefaultValue))
                    {
                        this.DefaultValue = "\"\"";
                    }
                }
                this.TypeNameValue = string.Format("{0} {1} = {2}", this.Type, this.NameCS, this.DefaultValue);
            }
            else
            {
                this.TypeNameValue = this.TypeName;
            }
            


        }


    }
}