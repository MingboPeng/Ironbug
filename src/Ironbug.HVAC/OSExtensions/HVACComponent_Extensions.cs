using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public static class HVACComponent_Extensions
    {
        public static object GetAttributeValue(this HVACComponent component, string AttributeName)
        {
            string methodName = AttributeName;

            var method = component.GetType().GetMethod(methodName);
            var invokeResult = method.Invoke(component, null);

            return invokeResult;
        }
        public static object SetCustomAttribute(this HVACComponent component, string AttributeName, object AttributeValue)
        {

            string methodName = "set" + AttributeName;
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
                var invokeResult = component.SetCustomAttribute(name, item.Value);

                invokeResults.Add(name + " :: " + invokeResult);
            }

            return invokeResults;
        }
    }
}
