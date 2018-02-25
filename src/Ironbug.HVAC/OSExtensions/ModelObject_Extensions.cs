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
            var uid = component.comment();
            var type = component.iddObject().type();
            var objs = model.getObjectsByType(type);
            var existed = false;
            foreach (var item in objs)
            {
                if (item.comment().Equals(uid))
                {
                    existed = true;
                }
            }
            return existed;
        }

        public static object GetDataFieldValue(this ModelObject component, string getterMethodName)
        {
            string methodName = getterMethodName;

            var method = component.GetType().GetMethod(methodName, new Type[] { });
            var invokeResult = method.Invoke(component, null);

            return invokeResult;
        }
        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions]
        public static object SetCustomAttribute(this ModelObject component, string setterMethodName, object AttributeValue)
        {

            string methodName = setterMethodName;
            object[] parm = new object[] { AttributeValue };
            
            var method = component.GetType().GetMethod(methodName, new[] { AttributeValue.GetType() });

            //TODO: catch AccessViolationException
            object invokeResult = null;
            try
            {
                invokeResult = method.Invoke(component, parm);
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong!"+ e.InnerException);
                //invokeResult = e.ToString();
            }
            

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

        //public static string CheckName(this ModelObject component)
        //{
        //    var name = component.nameString();
        //    return component.CheckName(name);
        //}
        //public static string CheckName(this ModelObject component, string NewName)
        //{
        //    var name = CheckString(NewName);

        //    if (name != NewName)
        //    {
                
        //        component.setName(name);
        //    }


        //    return name;
        //}
        
    }
}
