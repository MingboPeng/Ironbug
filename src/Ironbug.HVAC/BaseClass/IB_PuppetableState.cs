using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_PuppetableState
    {
        internal IIB_ModelObject _refModelObject { get; set; }

        public abstract IB_PuppetableState_Host ToPuppetHost();
    }

    public class IB_PuppetableState_None : IB_PuppetableState
    {
        public IB_PuppetableState_None(IB_PuppetableState state)
        {
            this._refModelObject = state._refModelObject;
        }

        public IB_PuppetableState_None(IIB_ModelObject stateObj)
        {
            this._refModelObject = stateObj;
        }

        public override IB_PuppetableState_Host ToPuppetHost()
        {
            var state = new IB_PuppetableState_Host(this);
            this._refModelObject.ChangeState(state);
            return state;
        }

        //public IB_PuppetableState_Puppet ToPuppet()
        //{
        //    var state = new IB_PuppetableState_Puppet(this);
        //    this._refModelObject.ChangeState(state);
        //    return state;
        //}
    }

    public class IB_PuppetableState_Host : IB_PuppetableState
    {
        public IList<IIB_ModelObject> Puppets { get; private set; } = new List<IIB_ModelObject>();
        public IB_PuppetableState_Host(IB_PuppetableState state)
        {
            this.Puppets = new List<IIB_ModelObject>();
            this._refModelObject = state._refModelObject;
        }

        public override IB_PuppetableState_Host ToPuppetHost()
        {
            //return (IB_PuppetableState_Host)this._refModelObject.CurrentState;
            var state = new IB_PuppetableState_Host(this);
            this._refModelObject.ChangeState(state);
            return state;
        }

        //public IB_PuppetableState_Puppet ExpireState()
        //{
        //    var newState = new IB_PuppetableState_Puppet(this);
        //    this._refModelObject.ChangeState(newState);
        //    return newState;
        //}
        
        public void AddPuppet(IIB_ModelObject puppet)
        {
            this.Puppets.Add(puppet);
        }
    }

    //public class IB_PuppetableState_Puppet : IB_PuppetableState_None
    //{
    //    public IB_PuppetableState_Puppet(IB_PuppetableState state) : base(state)
    //    {
    //    }
        
    //}
}
