using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;

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

        //public static OptionalParentObject GetIfInModel(this ModelObject component, Model model)
        //{
        //    var uid = component.comment();
        //    var type = component.iddObject().type();
        //    var objs = model.getObjectsByType(type);
        //    var optionObj = new OptionalParentObject();
        //    foreach (var item in objs)
        //    {
        //        if (item.comment().Equals(uid))
        //        {
        //            var h = item.handle();
        //            optionObj = model.getParentObject(h);
                    
        //            //optionObj.set(model.getParentObject(h).get());
        //        }
        //    }
        //    return optionObj;
        //}

        public static T GetIfInModel<T>(this T component, Model model) where T: ModelObject
        {

            if (component.GetType().Name == "ModelObject") throw new ArgumentNullException($"GetIfInModel() doesn't work correctly!");
            var getmethodName = $"get{component.GetType().Name}s";
            var methodInfo = typeof(Model).GetMethod(getmethodName);
            if (methodInfo is null) throw new ArgumentNullException($"{getmethodName} is not available in OpenStuido.Model!");

            var objresults = methodInfo.Invoke(model, null);
            var objList = (objresults as IEnumerable<T>).ToList();
            var matchObj = objList.FirstOrDefault(_ => _.comment() == component.comment());

            return matchObj;
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
            var tp = value.GetType();
            var methodInfo = component.GetType().GetMethod(setterMethodName, new[] {tp });
            if (methodInfo is null) throw new Exception($"{setterMethodName} is not available in {component}!");
            return InvokeMethod(component, methodInfo, value);
            
        }



        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions]
        private static object InvokeMethod(ModelObject component, MethodInfo methodInfo, object value)
        {
            object[] parm = new object[] { };
            var method = methodInfo;
            
            bool lockWasTaken = false;
            var tempComp = component;
            object invokeResult = null;
            try
            {
                Monitor.Enter(tempComp,ref lockWasTaken);
                {
                    parm = new object[] { value };
                    invokeResult = method.Invoke(tempComp, parm);
                    if (invokeResult is bool b)
                    {
                        if(!b) throw new ArgumentException($"Failed to set {value} to {tempComp.GetType()}!");
                    }
                }
                
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Attempted to read or write protected memory"))
                {
                    throw new ArgumentException($"Something went wrong! \r\n\r\nUsually rerun this component would fix it. But you should save the file first!\r\n\r\n" + e.Message);
                }
                else
                {
                    throw new ArgumentException($"{e.Message}");
                }
                
                //invokeResult = e.ToString();
            }
            finally
            {
                if (lockWasTaken)
                {
                    Monitor.Exit(tempComp);
                }
                //component = tempComp;
            }

            return invokeResult;

            //belonging check
            //object CheckBelonging(ModelObject c, object v)
            //{
            //    object obj = v;
            //    if (v is Curve curve)
            //    {
            //        //TODO: add supports of Schedule later
            //        //dealing the ghost object
            //        var idf = curve.toIdfObject().clone(true);
            //        obj = c.model().addObject(idf).get().to_Curve().get();

                    
            //        //obj = ((Curve)v).clone(c.model()).to_Curve().get();
                    
            //    }

            //    return obj;
            //}
        }

        public static List<string> SetCustomAttributes(this ModelObject component, Dictionary<BaseClass.IB_Field, object> dataField)
        {
            var invokeResults = new List<string>();
            foreach (var item in dataField)
            {
                var field = item.Key;
                var value = item.Value;
                //check types
                if (value is BaseClass.IB_Curve c)
                {
                    value = c.ToOS(component.model());
                }
                else if(value is BaseClass.IB_Schedule sch)
                {
                    value = sch.ToOS(component.model());
                }

                var invokeResult = component.SetFieldValue(field, value);

                invokeResults.Add(field.FullName + " :: " + invokeResult);
            }

            return invokeResults;
        }

        public static IEnumerable<string> GetUserFriendlyFieldInfo(this ModelObject component, bool ifIPUnits = false)
        {
            var com = component.idfObject();
            IddObject iddObject = com.iddObject();
            
            var valueWithInfo = new List<string>();
            var fieldCount = com.numFields();

            for (int i = 0; i < fieldCount; i++)
            {
                var ifIPUnit = ifIPUnits;

                uint index = (uint)i;
                var field = iddObject.getField(index).get();
                
                if (!field.IsWorkableField()) continue;
                var fieldProp = field.properties();
                var valueStr = com.getString(index).get();

                OSOptionalQuantity oQuantity = null;
                var customStr = String.Empty;
                if (field.IsRealType())
                {
                    //try to convert the unit and value
                    oQuantity = com.getQuantity(index, true, ifIPUnit);
                    customStr = oQuantity.isSet() ? oQuantity.get().value().ToString() : valueStr;
                }
                else
                {
                    customStr = valueStr;
                }


                var dataname = field.name();
                var defaultValue = GetDefaultValue(field, ifIPUnit);
                var unit = GetUnit(field, ifIPUnit);


                var shownValue = string.IsNullOrWhiteSpace(customStr) ? defaultValue : customStr;
                shownValue = ReplaceGUIDString(shownValue);
                var shownUnit = string.IsNullOrWhiteSpace(unit) ? string.Empty : string.Format(" [{0}]", unit);
                var shownDefault = string.IsNullOrWhiteSpace(defaultValue) ? string.Empty : string.Format(" (Default: {0})", defaultValue);

                var att = String.Format("{0,-23} !- {1}{2}{3}", shownValue, dataname, shownUnit, shownDefault);

                valueWithInfo.Add(att);

            }


            return valueWithInfo;

            string ReplaceGUIDString(string s)
            {
                var match = System.Text.RegularExpressions.Regex.Match(s, @"\{[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}\}",
       RegexOptions.IgnoreCase);

                return match.Success ? "#[GUID]" : s;
            }

            string GetUnit(IddField field, bool ifIPUnit)
            {
                var optionalUnit = field.getUnits(ifIPUnit);
                var unit = string.Empty;
                if (optionalUnit.is_initialized())
                {

                    var unit2 = optionalUnit.get();
                    var prettyString = unit2.prettyString();
                    var standardString = unit2.standardString();

                    unit = string.IsNullOrWhiteSpace(prettyString) ? standardString : prettyString;

                }
                else
                {
                    unit = string.Empty;
                }

                return unit;
            }

            string GetDefaultValue(IddField field, bool ifIPUnit)
            {
                var sd = field.properties().stringDefault;
                var defaultStrStr = sd.isNull() ? string.Empty : sd.get();
                var defaultNumStr = GetDefaultNumStr(field, ifIPUnit);

                return string.IsNullOrWhiteSpace(defaultNumStr) ? defaultStrStr : defaultNumStr;
            }
            string GetDefaultNumStr(IddField field, bool ifIPUnit)
            {
                var numStr = String.Empty;
                var fieldProp = field.properties();
                if (fieldProp.numericDefault.is_initialized())
                {
                    var num = fieldProp.numericDefault.get();
                    //autosized
                    if (num == -9999) return numStr;

                    var si = fieldProp.units;
                    if (si.isNull()) return numStr;

                    if (!ifIPUnit) return num.ToString();

                    var siStr = si.get();

                    var ipStr = GetUnit(field, true);
                    if (string.IsNullOrWhiteSpace(ipStr)) return numStr;


                    var uconvert = OpenStudioUtilitiesUnits.convert(num, siStr, ipStr);

                    numStr = uconvert.is_initialized() ? uconvert.get().ToString() : "#SI" + num.ToString();


                }
                return numStr;
            }
        }

    }
}
