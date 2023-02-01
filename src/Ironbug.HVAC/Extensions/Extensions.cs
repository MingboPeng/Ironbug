using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public static class Extensions
    {
        public static int CountWithBranches(this IEnumerable<IB_HVACObject> enumerable)
        {
            var count = 0;
            foreach (var item in enumerable)
            {
                if (item is IB_PlantLoopBranches pb)
                {
                    count += pb.Count();
                }
                else if (item is IB_AirLoopBranches ab)
                {
                    foreach (var zb in ab.Branches)
                    {
                        var zone = zb[0] as IB_ThermalZone;
                        if (zone.AirTerminal is IB_AirTerminalSingleDuctInletSideMixer)
                        {
                            count += 3; // because added air terminal with each zone
                        }
                        else
                        {
                            count += 2; // because added air terminal with each zone
                        }
                    }
                    
                }
                else
                {
                    count++;
                }
            }

            return count;
        }


        public static T To<T>(this object fromObject)
        {
            if (fromObject is T tv)
                return tv;

            if (fromObject is JToken jtoken)
            {
                var tobj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jtoken?.ToString(), IB_JsonSetting.ConvertSetting);
                return tobj;
            }

            // convert to T
            return (T)Convert.ChangeType(fromObject, typeof(T));
        }
        public static T To<T>(this object fromObject, T anonymousTypeObject)
        {
            return fromObject.To<T>();
        }
        public static object To(this object fromObject, Type type)
        {
            if (fromObject.GetType() == type)
                return fromObject;

            var instance = Activator.CreateInstance(type);
            return fromObject.To(instance);
        }
    }
}
