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

        public static T GetIfInModel<T>(this T component, Model model) where T: ModelObject
        {
            var type = component.GetType();
            return GetFromModel(type.Name, component.comment(), model) as T;
        }

        public static ModelObject GetFromModel(string typeName, string trackingID, Model model)
        {

            if (typeName == "ModelObject") throw new ArgumentNullException($"GetFromModel() doesn't work correctly!");
            var getmethodName = $"get{typeName}s";
            var methodInfo = typeof(Model).GetMethod(getmethodName);
            if (methodInfo is null) throw new ArgumentNullException($"{getmethodName} is not available in OpenStuido.Model!");
            var objresults = methodInfo.Invoke(model, null);
            var objList = (objresults as IEnumerable<ModelObject>).ToList();
            var matchObj = objList.FirstOrDefault(_ => _.comment() == trackingID);
            
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
            //Autosize 
            if (value is double v)
            {
                if (v == -9999)
                {
                    return AutosizeFieldValue(component, iB_Field.FullName);
                }
            }
            //Set to value
            if (iB_Field.SetterMethod is null)
            {
                return SetFieldValue(component, $"set{iB_Field.FullName}", value, iB_Field.DataType);
            }
            else
            {
                return InvokeMethod(component, iB_Field.SetterMethod, value);
            }
            
        }

        private static object SetFieldValue(this ModelObject component, string setterMethodName, object value, Type paramType = default)
        {
            var tp = paramType ?? value.GetType();
            var methodInfo = component.GetType().GetMethod(setterMethodName, new[] { tp });

            //try to look and match all methods
            if (methodInfo is null) 
            {
                var lowerCase = setterMethodName.ToLower();
                methodInfo = component.GetType().GetMethods().FirstOrDefault(_ => _.Name.ToLower() == lowerCase);
            }

            if (methodInfo is null)
                throw new Exception($"{setterMethodName} method is not available in {component}!");
            return InvokeMethod(component, methodInfo, value);
            
        }

        private static object AutosizeFieldValue(this ModelObject component, string FieldName)
        {
            var methodInfo = component.GetType().GetMethod($"autosize{FieldName}");
            if (methodInfo is null) throw new Exception($"{FieldName} cannot be autosized!");
            return InvokeMethod(component, methodInfo, null);

        }



        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions]
        private static object InvokeMethod(ModelObject component, MethodInfo methodInfo, object value)
        {
            var method = methodInfo;
            
            bool lockWasTaken = false;
            var tempComp = component;
            object invokeResult = null;
            var tryRunCount = 0;
            var tryFinallyCount = 1;

            try
            {
               
                Monitor.Enter(tempComp, ref lockWasTaken);
                while (tryRunCount < tryFinallyCount)
                {
                    try
                    {
                        var parm = value is null ? null : new object[] { value };
                        invokeResult = method.Invoke(tempComp, parm);
                        if (invokeResult is bool b)
                        {
                            if (!b) throw new ArgumentException($"Failed to {method.Name} with {value} to {tempComp.GetType()}!");
                        }

                    }
                    catch (Exception e)
                    {
                       
                        // try 5 times
                        if (e.InnerException != null)
                        {
                            if (e.InnerException.Message.Contains("Attempted to read or write protected memory"))
                            {
                                tryFinallyCount = 5;
                                continue;
                            }
                            else
                                throw;
                        }
                        else
                            throw;
                    }
                    finally { tryRunCount++; }
                    
                }

            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    if (e.InnerException.Message.Contains("Attempted to read or write protected memory"))
                    {
                        throw new ArgumentException($"Something went wrong! \r\n\r\nUsually rerun this component would fix it. But you should save the file first!\r\n\r\n" + e.InnerException.Message);
                    }
                    else
                    {
                        throw new ArgumentException($"{e.InnerException.Message}");
                    }
                }
                else
                {
                    throw e;
                }
                
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

        }
        public static bool AddEmsInternalVariables(this ModelObject component, IEnumerable<IB_EnergyManagementSystemInternalVariable> inVariables)
        {
            foreach (var item in inVariables)
            {
                var added = item.ToOS(component);
                if (added == null)
                    throw new ArgumentNullException("Failed to add EnergyManagementSystemInternalVariable");
            }
            return true;

        }
        public static bool AddEmsActuators(this ModelObject component, ICollection<IB_EnergyManagementSystemActuator> actuators)
        {
            foreach (var item in actuators)
            {
                var added = item.ToOS(component);
                if (added == null)
                    throw new ArgumentNullException("Failed to add EnergyManagementSystemActuator");
            }
            return true;
        }

        public static bool AddEmsSensors(this ModelObject component, ICollection<IB_EnergyManagementSystemSensor> sensors)
        {
            var md = component.model();
            foreach (var item in sensors)
            {
                var added = item.ToOS(md);
                if (added == null)
                    throw new ArgumentNullException("Failed to add EnergyManagementSystemSensor");
            }
            return true;

        }
        public static bool SetOutputVariables(this ModelObject component, ICollection<BaseClass.IB_OutputVariable> outputVariables)
        {
            var success = true;
            var vs = outputVariables;
            var keyName = component.nameString();
            var md = component.model();
            foreach (var item in vs)
            {
                success &= item.ToOS(md, keyName);
            }
            return success;

        }
        public static List<string> SetCustomAttributes(this ModelObject component, BaseClass.IB_FieldArgumentSet fieldArgs)
        {
            var invokeResults = new List<string>();
            var md = component.model();
            foreach (var item in fieldArgs)
            {
                var field = item.Field;
                var value = item.Value;
                //check types
                if (value is BaseClass.IB_Curve c)
                {
                    value = c.ToOS(md);
                }
                else if(value is BaseClass.IB_Schedule sch)
                {
                    value = sch.ToOS(md);
                }
                else if (value is BaseClass.IB_AvailabilityManager am)
                {
                    value = new AvailabilityManagerVector(new[] { am.ToOS(md) }.ToList());
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


        public static ModelObject CastToOsType(this IdfObject modelObject)
        {
            try
            {
                var tp = modelObject.iddObject().type().valueDescription().Replace("OS:", "").Replace(":", "");
                if (modelObject == null) throw new ArgumentException($"Failed to initiate {tp} from parameter source! Double check if it includes its children.");

                var methodInfo = modelObject.GetType().GetMethod($"to_{tp}");
                var optionalObj = methodInfo.Invoke(modelObject, null);

                var getterMethodInfo = optionalObj.GetType().GetMethod("get");
                var obj = getterMethodInfo.Invoke(optionalObj, null) as ModelObject;
                return obj;
            }
            catch (Exception e)
            {
                throw e;
            }
           
        }
    }
}
