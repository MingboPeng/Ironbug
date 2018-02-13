using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ironbug.HVAC
{
    public static class ModelObjectExtensions
    {
        public static string OSType(this ModelObject component)
        {
            return component.iddObjectType().valueDescription();
        }

        public static bool IsNode(this ModelObject component)
        {
            return component.OSType() == "OS:Node";
        }

        public static bool IsInModel(this ModelObject component, Model model)
        {
            var isInModel = !model.getGenericModelObjectByName(component.nameString()).isNull();
            return isInModel;
        }

        public static object GetAttributeValue(this ModelObject component, string getterMethodName)
        {
            string methodName = getterMethodName;

            var method = component.GetType().GetMethod(methodName);
            var invokeResult = method.Invoke(component, null);

            return invokeResult;
        }
        public static object SetCustomAttribute(this ModelObject component, string setterMethodName, object AttributeValue)
        {

            string methodName = setterMethodName;
            object[] parm = new object[] { AttributeValue };

            var method = component.GetType().GetMethod(methodName, new[] { AttributeValue.GetType() });
            var invokeResult = method.Invoke(component, parm);

            return invokeResult;
        }

        public static List<string> SetCustomAttributes(this ModelObject component, Dictionary<string, object> dataField)
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
