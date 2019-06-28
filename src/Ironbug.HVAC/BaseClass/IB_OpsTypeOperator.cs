using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ironbug.HVAC.BaseClass
{
    public static class IB_OpsTypeOperator
    {
        public static IddObject GetIddObject(Type OSType)
        {
            var iddType = OSType.GetMethod("iddObjectType", BindingFlags.Public | BindingFlags.Static).Invoke(null, null) as IddObjectType;
            return new IdfObject(iddType).iddObject();
        }

        public static IEnumerable<MethodInfo> GetOSSetters(Type OSType)
        {

            var setterMethods =  OSType
                        .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                            .Where(_ =>
                            {
                                //get all setting methods
                                if (!_.Name.StartsWith("set")) return false;
                                if (_.Name.Contains("NodeName")) return false;
                                if (_.GetParameters().Count() != 1) return false;

                                var paramType = _.GetParameters().First().ParameterType;
                                var isValidType =
                                paramType == typeof(string) ||
                                paramType == typeof(double) ||
                                paramType == typeof(bool) ||
                                paramType == typeof(int) ||
                                typeof(Curve).IsAssignableFrom(paramType) ||
                                typeof(Schedule).IsAssignableFrom(paramType);

                                //if (!isValidType) return false;

                                return isValidType;

                            }
                            ).ToList();

            var nameSetter = OSType.GetMethod("setName");
            if (nameSetter != null)
                setterMethods.Add(nameSetter);
          
            return setterMethods;

        }
    }
}
