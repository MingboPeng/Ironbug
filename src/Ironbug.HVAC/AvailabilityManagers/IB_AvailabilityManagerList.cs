using OpenStudio;
using System;
using System.Collections.Generic;

namespace Ironbug.HVAC.BaseClass
{
    public class IB_AvailabilityManagerList: IB_AvailabilityManager
    {

        public List<IB_AvailabilityManager> Mangers
        {
            get => this.TryGetList<IB_AvailabilityManager>();
            set => this.Set(value);
        }

        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AvailabilityManagerList();


        public IB_AvailabilityManagerList() : base(new Node(new Model()))
        {
        }

        public void SetManagers(List<IB_AvailabilityManager> mangers)
        {
            this.Mangers = mangers;
        }


        public OpenStudio.AvailabilityManagerVector ToAMVector(Model model)
        {
            var vec = new OpenStudio.AvailabilityManagerVector();
            foreach (var item in Mangers)
            {
                vec.Add(item.ToOS(model));
            }
            return vec;
        }

        public override OpenStudio.AvailabilityManager ToOS(Model model)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            var s = $"{this.Mangers.Count} managers";
            return s;
        }
    }


}
