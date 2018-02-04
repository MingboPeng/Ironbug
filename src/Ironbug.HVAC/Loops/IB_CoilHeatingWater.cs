using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingWater: IIB_HVACComponent
    {
        public Dictionary<string, double> dataField = new Dictionary<string, double>();
        //"s"

        public CoilHeatingWater osCoilHeatingWater { get; set; }


        public IB_CoilHeatingWater()
        {
            //var model = new Model();
            //this.osCoilHeatingWater = new CoilHeatingWater(model);
        }

        public bool AddToNode(ref Model model, Node node)
        {
            this.osCoilHeatingWater = new CoilHeatingWater(model);
            this.ApplyDataField(this.osCoilHeatingWater);
            return this.osCoilHeatingWater.addToNode(node);

        }

        public void SetDataField(string methodName, double data)
        {
            this.dataField.Add(methodName, data);
        }

        public void ApplyDataField(CoilHeatingWater component)
        {
            foreach (var item in this.dataField)
            {
                string methodName = item.Key;
                double data = item.Value;
                object[] parm = new object[] { data };
                var method = this.osCoilHeatingWater.GetType().GetMethod(methodName);
                var invokeResult = method.Invoke(component, parm);
            }
            
        }
        

        public HVACComponent GetHVACComponent()
        {
            return this.osCoilHeatingWater;
        }
    }
}
