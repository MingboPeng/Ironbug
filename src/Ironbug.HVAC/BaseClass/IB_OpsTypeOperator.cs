using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ironbug.HVAC.BaseClass
{
    public static class IB_OpsTypeOperator
    {
        public static IddObject GetIddObject(Type OSType)
        {
            var iddType = OSType?.GetMethod("iddObjectType", BindingFlags.Public | BindingFlags.Static)?.Invoke(null, null) as IddObjectType;
            return new IdfObject(iddType).iddObject();

        }

        public static IEnumerable<MethodInfo> GetOSSetters(Type OSType)
        {

            var setterMethods =  OSType
                        .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                            .Where(_ =>
                            {
                                //get all setting methods
                                var name = _.Name;
                                var ps = _.GetParameters();
                                if (!name.StartsWith("set")) return false;
                                if (name.Contains("NodeName")) return false;
                                if (ps.Count() != 1) return false;

                                //Check types
                                var paramType = ps.First().ParameterType;
                                var isValidType =
                                paramType == typeof(string) ||
                                paramType == typeof(double) ||
                                paramType == typeof(bool) ||
                                paramType == typeof(int) ||
                                typeof(Curve).IsAssignableFrom(paramType) ||
                                typeof(Schedule).IsAssignableFrom(paramType);

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
