using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public abstract class IB_HVACComponent
    {
        public Dictionary<string, object> CustomAttributes { get; set; }
        //HVACComponent GetHVACComponent();

        //Must override in child class
        abstract public bool AddToNode(ref Model model, Node node);
        

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
            
        }


        public override string ToString()
        {
            var attributes = this.CustomAttributes.Select(_ => String.Format("{0}({1})", _.Key, _.Value));
            var outputString = String.Join("\r\n", attributes);
            return outputString;
        }


    }

    public static class HVACComponent_Extensions
    {
        public static object SetCustomAttribute(this HVACComponent component, string AttritbuteName, object AttributeValue)
        {
            
            string methodName = "set" + AttritbuteName;
            object[] parm = new object[] { AttributeValue };

            var method = component.GetType().GetMethod(methodName);
            var invokeResult = method.Invoke(component, parm);
            
            return invokeResult;
        }

        public static List<string> SetCustomAttributes(this HVACComponent component, Dictionary<string, object> dataField)
        {
            var invokeResults = new List<string>();
            foreach (var item in dataField)
            {
                var name = item.Key;
                var invokeResult=  component.SetCustomAttribute(name, item.Value);

                invokeResults.Add(name + " :: " + invokeResult);
            }

            return invokeResults;
        }
    }

    //public interface IB_Viz
    //{
    //    //base point to draw the object.
    //    Point3d basePoint { get; set; }
    //}
}
