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

        public static OptionalParentObject GetIfInModel(this ModelObject component, Model model)
        {
            var uid = component.comment();
            var type = component.iddObject().type();
            var objs = model.getObjectsByType(type);
            var optionObj = new OptionalParentObject();
            foreach (var item in objs)
            {
                if (item.comment().Equals(uid))
                {
                    
                    optionObj.set(model.getParentObject(item.handle()).get());
                }
            }
            return optionObj;
        }
        

        public static object GetFieldValue(this ModelObject component, string getterMethodName)
        {
            string methodName = getterMethodName;

            var method = component.GetType().GetMethod(methodName, new Type[] { });
            var invokeResult = method.Invoke(component, null);

            return invokeResult;
        }
        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions]
        public static object SetFieldValue(this ModelObject component, string setterMethodName, object value)
        {

            string methodName = setterMethodName;
            object[] parm = new object[] { value };
            
            var method = component.GetType().GetMethod(methodName, new[] { value.GetType() });

            //TODO: catch AccessViolationException
            object invokeResult = null;
            try
            {
                invokeResult = method.Invoke(component, parm);
            }
            catch (Exception e)
            {
                //the second try 
                if (e.InnerException.Message.StartsWith("Attempted to read or write protected memory"))
                {
                    try
                    {
                        System.Threading.Thread.Sleep(100);
                        invokeResult = method.Invoke(component, parm);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Just prevented an irrecoverable crashing!\r\nPlease save the file immediately!\r\n\r\n" + ex.InnerException);

                    }
                }
                throw new Exception("Something went wrong! \r\n\r\n" + e.InnerException?? e.Message);
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
                var invokeResult = component.SetFieldValue(name, item.Value);

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
