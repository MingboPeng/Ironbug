using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public abstract class IB_ModelObject
    {
        public Dictionary<string, object> CustomAttributes { get; private set; }
        protected ModelObject ghostModelObject { get; set; }

        public IB_ModelObject()
        {
            this.CustomAttributes = new Dictionary<string, object>();
        }

        protected virtual void AddCustomAttribute(string AttributeName, object data)
        {
            if (CustomAttributes.ContainsKey(AttributeName))
            {
                this.CustomAttributes[AttributeName] = data;
            }
            else
            {
                this.CustomAttributes.Add(AttributeName, data);
            }

            //dealing the ghost object
            this.ghostModelObject.SetCustomAttribute(AttributeName, data);

        }


        public override string ToString()
        {
            //var attributes = this.CustomAttributes.Select(_ => String.Format("{0}({1})", _.Key, _.Value));
            var attributes = GetDataFields();
            var outputString = String.Join("\r\n", attributes);
            return outputString;
        }

        public IEnumerable<string> GetDataFields()
        {
            var com = this.ghostModelObject;

            var iddObject = com.iddObject();
            var dataFields = new List<string>();
            foreach (var item in com.dataFields())
            {

                var customStr = com.getString(item).get();
                //var customDouble = com.getDouble(item).is_initialized()? com.getDouble(item).get(): -9999;

                var field = iddObject.getField(item).get();
                var dataname = field.name();

                var unit = field.getUnits().isNull() ? string.Empty : field.getUnits().get().standardString();
                var stringDefault = field.properties().stringDefault;
                var defaultStr = stringDefault.isNull() ? string.Empty : stringDefault.get();
                //strDefault has numDefault already
                //var numDefault = field.getUnits().isNull() ? -9999 : field.properties().numericDefault.get();

                var shownStr = string.IsNullOrWhiteSpace(customStr) ? defaultStr : customStr;
                var shownUnit = string.IsNullOrWhiteSpace(unit) ? string.Empty : string.Format(" [{0}]", unit);
                var shownDefault = string.IsNullOrWhiteSpace(defaultStr) ? string.Empty : string.Format(" (Default: {0})", defaultStr);
                var att = String.Format("{0,-20} !- {1} {2} {3}", shownStr, dataname, shownUnit, shownDefault);


                dataFields.Add(att);
            }

            return dataFields;

        }

        public object GetAttributeValue(string AttributeName)
        {
            return this.ghostModelObject.GetAttributeValue(AttributeName);
        }

        public void SetAttribute(IB_DataField DataAttribute, object AttributeValue)
        {
            this.AddCustomAttribute(DataAttribute.SetterMethodName, AttributeValue);

        }

        public void SetAttribute(string AttributeName, object AttributeValue)
        {

            this.AddCustomAttribute(AttributeName, AttributeValue);

        }
    }
}
