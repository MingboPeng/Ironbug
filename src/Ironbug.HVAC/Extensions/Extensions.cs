using Ironbug.HVAC.BaseClass;
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
                if (item is IB_PlantLoopBranches)
                {
                    count += ((IB_PlantLoopBranches)item).Count();
                }
                else if (item is IB_AirLoopBranches)
                {
                    count += ((IB_AirLoopBranches)item).Count() * 2; // because added air terminal with each zone
                }
                else
                {
                    count++;
                }
            }

            return count;
        }

        public static T Get<T>(this IEnumerable<IB_Child> children) where T:IB_ModelObject
        {
            return children.FirstOrDefault(_ => _.IsType(typeof(T)))?.To<T>();
        }

        public static T Get<T>(this IEnumerable<IB_Child> children, int ChildIndex) where T : IB_ModelObject
        {
            if (!(children.Count()> ChildIndex)) return null;

            var child = children.ElementAt(ChildIndex);
            if (child.IsType(typeof(T)))
            {
                return child.To<T>();
            }
            else
            {
                throw new ArgumentException($"Child at {ChildIndex} doesn't match {typeof(T)}!");
            }
        }
    }
}
