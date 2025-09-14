using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.HVAC
{
    public class IB_GroundHeatExchangerVertical : IB_HVACObject, IIB_PlantLoopObjects
    {
        public class GFuncItem : IEquatable<GFuncItem>
        {
            public double Ln { get; set; }
            public double GValue { get; set; }

            public override bool Equals(object obj)
            {
                return Equals(obj as GFuncItem);
            }

            public bool Equals(GFuncItem other)
            {
                return other != null &&
                       Ln == other.Ln &&
                       GValue == other.GValue;
            }

            public override int GetHashCode()
            {
                unchecked // Overflow is fine, just wrap
                {
                    int hash = 17;
                    hash = hash * 23 + Ln.GetHashCode();
                    hash = hash * 23 + GValue.GetHashCode();
                    return hash;
                }
            }
        }

        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_GroundHeatExchangerVertical();
        private static GroundHeatExchangerVertical NewDefaultOpsObj(Model model) => new GroundHeatExchangerVertical(model);

        public List<GFuncItem> GFuncs
        {
            get => TryGetList<GFuncItem>();
            private set => Set(value);
        }

        public IB_GroundHeatExchangerVertical() : base(NewDefaultOpsObj)
        {
        }

        public void AddGFuncs(List<string> GFuncStrings)
        {
            var cleanedV = GFuncStrings.Select(_ => GetValue(_)).ToList();
            if (cleanedV.Count % 2 != 0)
            {
                throw new ArgumentException("Input list is not even count. The odd item has to be gFunctionLN, and its value the next item");
            }
            var c = cleanedV.Count / 2;
            var obj = this.GhostOSObject as GroundHeatExchangerVertical;
            obj.removeAllGFunctions();

            for (int i = 0; i < c; i++)
            {
                var ln = cleanedV[i * 2];
                var gV = cleanedV[i * 2 + 1];
                this.GFuncs.Add(new GFuncItem { Ln = ln, GValue = gV });
                obj.addGFunction(ln, gV);
            }

            double GetValue(string strValue)
            {
                var value = strValue.Split(',')[0].Split(';')[0].Trim().Split(' ')[0];
                return double.Parse(value.Trim());
            }
        }

        public override HVACComponent ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            obj.removeAllGFunctions();
            var gf = this.GFuncs;
            foreach (var item in gf)
            {
                obj.addGFunction(item.Ln, item.GValue);
            }
            return obj;
        }
    }

    public sealed class IB_GroundHeatExchangerVertical_FieldSet
        : IB_FieldSet<IB_GroundHeatExchangerVertical_FieldSet, GroundHeatExchangerVertical>
    {
        private IB_GroundHeatExchangerVertical_FieldSet()
        {
        }
    }
}
