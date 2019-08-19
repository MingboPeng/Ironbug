using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.HVAC
{
    public class IB_GroundHeatExchangerVertical : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_GroundHeatExchangerVertical();
        private static GroundHeatExchangerVertical NewDefaultOpsObj(Model model) => new GroundHeatExchangerVertical(model);

        private List<(double Ln, double GValue)> GFuncs { get; set; } = new List<(double Ln, double GValue)>();
        public IB_GroundHeatExchangerVertical():base(NewDefaultOpsObj(new Model()))
        {
        }

        public void AddGFuncs(List<string> GFuncStrings)
        {
            var cleanedV = GFuncStrings.Select(_ => GetValue(_)).ToList();
            if (cleanedV.Count %2 != 0)
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
                this.GFuncs.Add((ln, gV));
                obj.addGFunction(ln, gV);
            }

            double GetValue(string strValue)
            {
                var value = strValue.Split(',')[0].Split(';')[0].Trim().Split(' ')[0];
                return double.Parse(value.Trim());
            }
        }

        public override IB_ModelObject Duplicate()
        {
            var obj = base.Duplicate() as IB_GroundHeatExchangerVertical;
            obj.GFuncs = this.GFuncs;
            return obj;
        }

        public override HVACComponent ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            obj.removeAllGFunctions();
            var gf = this.GFuncs;
            foreach (var item in gf)
            {
                obj.addGFunction(item.Ln,item.GValue);
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
