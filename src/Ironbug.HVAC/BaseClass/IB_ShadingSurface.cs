using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OpenStudio;

namespace Ironbug.HVAC.BaseClass
{
    public class IB_ShadingSurface : IB_ModelObject
    {

        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilCoolingWater();
        private static ShadingSurface NewDefaultOpsObj(Model model) => new ShadingSurface(new Point3dVector(), model);

        [DataMember]
        public List<string> Points { get; private set; }
        public IB_ShadingSurface() : base(NewDefaultOpsObj)
        {
        }

        public void SetPoints(List<string> points)
        {
            Points = points;
        }


        public ShadingSurface ToOS(Model model)
        {
            if (Points == null || Points.Count == 0)
                throw new ArgumentException("Invalid points for ShadingSurfce!");

            var opsPts = Points.Select(_ =>{
                var xyz = _.Split(',').Select(_ => double.Parse(_)).ToList();
                return new Point3d(xyz[0], xyz[1], xyz[2]);
            }
          );

            var opsPtVector = new Point3dVector(opsPts);
            var opsObj = base.OnNewOpsObj((m) => new ShadingSurface(opsPtVector, m), model);

            return opsObj;
        }

    }

   
}
