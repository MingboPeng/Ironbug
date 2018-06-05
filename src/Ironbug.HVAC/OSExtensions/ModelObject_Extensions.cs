using OpenStudio;
using System;
using System.Collections.Generic;
using System.Reflection;

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
        
        public static object SetFieldValue(this ModelObject component, BaseClass.IB_Field iB_Field, object value)
        {
            if (iB_Field.SetterMethod is null)
            {
                return SetFieldValue(component, $"set{iB_Field.FullName}" , value);
            }
            else
            {
                return InvokeMethod(component, iB_Field.SetterMethod, value);
            }
            
        }

        private static object SetFieldValue(this ModelObject component, string setterMethodName, object value)
        {
            var methodInfo = component.GetType().GetMethod(setterMethodName, new[] { value.GetType() });
            return InvokeMethod(component, methodInfo, value);
            
        }



        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions]
        private static object InvokeMethod(ModelObject component, MethodInfo methodInfo, object value)
        {
            object[] parm = new object[] { };
            var method = methodInfo;
            //TODO: catch AccessViolationException
            object invokeResult = null;
            try
            {
                parm = new object[] { CheckBelonging(component, value) };
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
                throw new Exception("Something went wrong! \r\n\r\n" + e.InnerException ?? e.Message);
                //invokeResult = e.ToString();
            }

            return invokeResult;

            //belonging check
            object CheckBelonging(ModelObject c, object v)
            {
                object obj = v;
                if (v.GetType() == typeof(Curve))
                {
                    //TODO: add supports of Schedule later
                    //dealing the ghost object

                    obj = ((Curve)value).clone(c.model()).to_Curve().get();
                    
                }

                return obj;
            }
        }

        public static List<string> SetCustomAttributes(this ModelObject component, Dictionary<BaseClass.IB_Field, object> dataField)
        {
            var invokeResults = new List<string>();
            foreach (var item in dataField)
            {
                var field = item.Key;

                var invokeResult = component.SetFieldValue(field, item.Value);

                invokeResults.Add(field.FullName + " :: " + invokeResult);
            }

            return invokeResults;
        }

        public static IEnumerable<string> GetUserFriendlyFieldInfo(this ModelObject component, bool ifIPUnits = false)
        {
            var com = component.clone();
            IddObject iddObject = com.iddObject();
            
            var valueWithInfo = new List<string>();
            var fieldCount = com.numFields();

            for (int i = 0; i < fieldCount; i++)
            {
                var ifIPUnit = ifIPUnits;

                uint index = (uint)i;
                var field = iddObject.getField(index).get();
                
                if (!field.IsWorkableField()) continue;

                OSOptionalQuantity oQuantity = null;
                if (field.IsRealType() && ifIPUnit)
                {
                    //try to convert the unit and value
                    oQuantity = com.getQuantity(index, true, ifIPUnit);
                    //true means it is unit-convertible
                    ifIPUnit = oQuantity.isSet();
                }
                else
                {
                    //set to false for all other non-real type value
                    ifIPUnit = false;
                }

                var customStr = ifIPUnit? oQuantity.get().value().ToString(): com.getString(index).get();

                var dataname = field.name();

                var optionalUnit = field.getUnits(ifIPUnit);
                var unit = optionalUnit.isNull() ? string.Empty : optionalUnit.get().standardString();
                var stringDefault = field.properties().stringDefault;
                var defaultStr = stringDefault.isNull() ? string.Empty : stringDefault.get();
                //strDefault has numDefault already
                //var numDefault = field.getUnits().isNull() ? -9999 : field.properties().numericDefault.get();

                var shownStr = string.IsNullOrWhiteSpace(customStr) ? defaultStr : customStr;
                var shownUnit = string.IsNullOrWhiteSpace(unit) ? string.Empty : string.Format(" [{0}]", unit);
                var shownDefault = string.IsNullOrWhiteSpace(defaultStr) ? string.Empty : string.Format(" (Default: {0})", defaultStr);

                var att = String.Format("{0,-23} !- {1}{2}{3}", shownStr, dataname, shownUnit, shownDefault);

                valueWithInfo.Add(att);

            }


            return valueWithInfo;

        }

    }
}
