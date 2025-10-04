using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Ironbug.HVAC
{
    public class IB_GeneratorPVWatts : IB_Generator
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_GeneratorPVWatts();

        private static GeneratorPVWatts NewDefaultOpsObj(Model model) => new(model, 10000);

        [DataMember]
        private string SurfaceID { get; set; }
        public IB_GeneratorPVWatts() : base(NewDefaultOpsObj)
        {
        }
        public void SetSurface(string surfaceID)
        {
            SurfaceID = surfaceID;
        }
        public override Generator ToOS(Model model)
        {
            if (string.IsNullOrEmpty(SurfaceID))
                throw new ArgumentException("Invalid PV surface ID");

            var oShade = model.getSurfaceByName(SurfaceID);
            if (oShade == null || oShade.isNull())
                throw new ArgumentException($"Invalid PV surface ID: {SurfaceID}");
            if (!oShade.is_initialized())
                throw new ArgumentException($"Invalid PV surface ID: {SurfaceID}");

            var shd = oShade.get();
            var opsObj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            opsObj.setSurface(shd);


            return opsObj;
        }

    }

    public sealed class IB_GeneratorPVWatts_FieldSet
       : IB_FieldSet<IB_GeneratorPVWatts_FieldSet, GeneratorPVWatts>
    {

        private IB_GeneratorPVWatts_FieldSet() { }

    }


}
