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
            return OSType
                        .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                            .Where(_ =>
                            {
                                //get all setting methods
                                if (!_.Name.StartsWith("set")) return false;
                                if (_.GetParameters().Count() != 1) return false;

                                var paramType = _.GetParameters().First().ParameterType;
                                var isValidType =
                                paramType == typeof(string) ||
                                paramType == typeof(double) ||
                                paramType == typeof(bool) ||
                                paramType == typeof(int) ||
                                paramType == typeof(Curve);
                                //TODO: add supports of Schedule later

                                //if (!isValidType) return false;

                                return isValidType;

                            }
                            );

        }
    }
}
