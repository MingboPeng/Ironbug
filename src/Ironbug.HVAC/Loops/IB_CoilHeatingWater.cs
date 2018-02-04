using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingWater: IB_HVACComponent
    {
        public enum AttributeNames
        {
            RatedInletWaterTemperature,
            RatedOutletWaterTemperature
        }

        //Real obj to be saved in OS model
        private CoilHeatingWater osCoilHeatingWater { get; set; }

        //Ghost obj for place holder
        private CoilHeatingWater ghostCoilHeatingWater { get; set; }

        //dealing with the ghost object
        public IB_CoilHeatingWater()
        {
            var model = new Model();
            this.ghostCoilHeatingWater = new CoilHeatingWater(model);

            this.CustomAttributes = new Dictionary<string, object>();
        }

        //dealing with the real object, only when it is ready to be added to os model
        public override bool AddToNode(ref Model model, Node node)
        {
            this.osCoilHeatingWater = new CoilHeatingWater(model);
            this.osCoilHeatingWater.SetCustomAttributes(this.CustomAttributes);
            return this.osCoilHeatingWater.addToNode(node);
            
        }

        //this method for internal use, needed to be protected. call SetAttribute() instead
        protected override void AddCustomAttribute(string AttributeName, object AttributeValue)
        {
            //adding attributes for real object to use later
            base.AddCustomAttribute(AttributeName, AttributeValue);
            //dueling the ghost object
            this.ghostCoilHeatingWater.SetCustomAttribute(AttributeName, AttributeValue);
        }

        
        public void SetAttribute(AttributeNames AttributeName, object AttributeValue)
        {
            this.AddCustomAttribute(AttributeName.ToString(), AttributeValue);
        }

        
    }

}
