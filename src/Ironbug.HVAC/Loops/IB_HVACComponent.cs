using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public abstract class IB_HVACComponent
    {
        protected Dictionary<string, object> CustomAttributes { get; set; }
        
        protected HVACComponent ghostHVACComponent { get; set; }

        //Must override in child class
        abstract public bool AddToNode(ref Model model, Node node);

        public IB_HVACComponent()
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
            this.ghostHVACComponent.SetCustomAttribute(AttributeName, data);

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
            var com = this.ghostHVACComponent;
            
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
            return this.ghostHVACComponent.GetAttributeValue(AttributeName);
        }

        public void SetAttribute(IB_DataAttribute DataAttribute, object AttributeValue)
        {
            this.AddCustomAttribute(DataAttribute.FullName, AttributeValue);
            
        }

        public void SetAttribute(string AttributeName, object AttributeValue)
        {
            this.AddCustomAttribute(AttributeName, AttributeValue);
            
        }


    }

    public abstract class IB_DataAttributeSet
    {
        protected static readonly Type dbType = typeof(double);

        protected static IEnumerable<IB_DataAttribute> GetList<T>() where T : IB_DataAttributeSet
        {
            return typeof(T).GetFields()
                            .Select(_ => (IB_DataAttribute)_.GetValue(null));
        }

        protected static IB_DataAttribute GetAttributeByName<T>(string name) where T : IB_DataAttributeSet
        {
            var field = typeof(T).GetField(name);
            return (IB_DataAttribute)field.GetValue(null);
        }


    }


    //public interface IB_Viz
    //{
    //    //base point to draw the object.
    //    Point3d basePoint { get; set; }
    //}
}
