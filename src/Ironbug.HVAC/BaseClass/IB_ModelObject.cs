using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.Core;

namespace Ironbug.HVAC.BaseClass
{

    public abstract class IB_ModelObject : IIB_ModelObject
    {
        public string Memo { get; set; }
        public IEnumerable<string> SimulationOutputVariables { get; }
        public static bool IPUnit { get; set; } = false;
        //public event EventHandler<PuppetEventArg> PuppetEventHandler;
        protected abstract Func<IB_ModelObject> IB_InitSelf { get; }
        public IList<IB_Child> Children { get; private set; } = new List<IB_Child>();

        //public IB_PuppetableState CurrentState { get; private set; }
        public Dictionary<IB_Field, object> CustomAttributes { get; private set; } = new Dictionary<IB_Field, object>();
        protected ModelObject GhostOSObject { get; private set; }

        public List<IB_OutputVariable> CustomOutputVariables { get; private set; } = new List<IB_OutputVariable>();

        public IB_ModelObject(ModelObject GhostOSObject)
        {
            this.GhostOSObject = GhostOSObject;
            this.SetTrackingID();
            this.SimulationOutputVariables = GhostOSObject.outputVariableNames();
        }

        public void AddOutputVariables(List<IB_OutputVariable> outputVariable)
        {
            if (outputVariable is null)
            {
                return;
            }
            this.CustomOutputVariables.AddRange(outputVariable);
        }

        internal void AddChild(IB_ModelObject ChildObj)
        {
            var c = new IB_Child(ChildObj);
            this.Children.Add(c);
        }

        internal void SetChild<T>(T ChildObj) where T:IB_ModelObject
        {
            this.Children.FirstOrDefault(_ => _.IsType(typeof(T))).Set(ChildObj);
        }
        internal void SetChild<T>(int ChildIndex, T ChildObj) where T : IB_ModelObject
        {
            var child = this.Children[ChildIndex];
            if (child.IsType(typeof(T)))
            {
                child.Set(ChildObj);
            }
            else
            {
                throw new ArgumentException($"Set {ChildObj.GetType()} to {GhostOSObject.GetType()} failed!");
            }
        }

        //public void ChangeState(IB_PuppetableState newState)
        //{
        //    this.CurrentState = newState;
        //}
        
        //public bool IsPuppetHost()
        //{
        //    return this.CurrentState is IB_PuppetableState_Host;
        //}
        
        //public IB_ModelObject ToPuppetHost()
        //{
        //    this.CurrentState.ToPuppetHost();
        //    foreach (var child in this.Children)
        //    {
        //        child.Get().ToPuppetHost();
        //    }

        //    //this.PuppetEventHandler?.Invoke(this, new PuppetEventArg(this.CurrentState));
        //    return this;
        //}

        //public void ResetPuppetState()
        //{
        //    this.CurrentState = new IB_PuppetableState_None(this);
        //    foreach (var child in this.Children)
        //    {
        //        child.Get().ResetPuppetState();
        //    }
        //    //this.PuppetEventHandler?.Invoke(this, new PuppetEventArg(this.CurrentState));
        //}

        //public void PuppetStateUpdated()
        //{
        //    this.PuppetEventHandler?.Invoke(this, new PuppetEventArg(this.CurrentState));
        //    foreach (var child in this.Children)
        //    {
        //        child.PuppetStateUpdated();
        //    }
        //}


        //public IB_ModelObject DuplicateAsPuppet()
        //{

        //    var puppet = DuplicateAsPuppet(IB_InitSelf);
            
        //    puppet.Children.Clear();
        //    foreach (var child in this.Children)
        //    {
        //        var childPuppet = child.DuplicateAsPuppet();
        //        puppet.Children.Add(childPuppet);
        //    }

        //    //this.PuppetEventHandler?.Invoke(this, new PuppetEventArg(this.CurrentState));
        //    return puppet;

        //    //Local method
        //    T DuplicateAsPuppet<T>(Func<T> DupMethodHandler) where T : IB_ModelObject
        //    {
        //        T p = null;
        //        if (this.CurrentState is IB_PuppetableState_Host stateParent)
        //        {
        //            p = this.DuplicateIBObj(DupMethodHandler);
        //            //puppet.ToPuppet();
        //            p.SetTrackingID();
        //            stateParent.AddPuppet(p);
        //        }
        //        else
        //        {
        //            throw new ArgumentException("Item has to be a puppet host.");//TODO: show the item's name
        //                                                                         //currently only two available
        //        }

        //        return p;


        //    }
        //}
        
        
        

        //public IList<IIB_ModelObject> GetPuppetsOrSelf()
        //{

        //    //TODO: move this part to IB_PuppetableState later
        //    var puppetsOrSelf = this.GetPuppets();
        //    if (!puppetsOrSelf.Any())
        //    {
        //        puppetsOrSelf.Add(this);
        //    }

        //    return puppetsOrSelf;
        //}

        //public IList<IIB_ModelObject> GetPuppets()
        //{

        //    //TODO: move this part to IB_PuppetableState later
        //    var puppets = new List<IIB_ModelObject>();
        //    if (this.CurrentState is IB_PuppetableState_Host state)
        //    {
        //        puppets.AddRange(state.Puppets);
        //        //state.ExpireState();
        //    }

        //    return puppets;
        //}

        public string GetTrackingID()
        {
            return this.GhostOSObject.comment();
        }

        
        //public object GetFieldValue(string fieldName)
        //{
        //    return this.GhostOSObject.GetDataFieldValue(fieldName);
        //}

        public string SetTrackingID()
        {
            //var attributeName = "setComment";
            var ib_field = IB_Field_Comment.Instance;
            
            var data = CreateUID();

            this.SetFieldValue(ib_field, data);
            //this.CustomAttributes.TryAdd(ib_field, data);
            //this.GhostOSObject.setComment(data);
            return data;

        }
        /// <summary>
        /// this is for inherited classes to override all their associated child IB objects
        /// SetTrackingID only updates self object, but SetAllTrackingIDs will update self object and its child.
        /// Use this after called DuplicateAsPuppet method.
        /// </summary>
        //public virtual void SetAllTrackingIDs()
        //{
        //    //this is default. 
        //    this.SetTrackingID();
        //}

        public void SetFieldValue(IB_Field field, object value)
        {
            var realValue = value;
            //check types
            if (value is IB_Curve c)
            {
                realValue = c.ToOS(this.GhostOSObject.model());
            }
            else if (value is IB_Schedule sch)
            {
                realValue = sch.ToOS(this.GhostOSObject.model());
            }

            this.CustomAttributes.TryAdd(field, value);

            //apply the value to the ghost ops obj.
            //remember this ghost is only for preview purpose
            //meaning it will not be saved in real OpenStudio.Model, 
            //but it should have all the same field values as the real one, except handles.
            this.GhostOSObject.SetFieldValue(field, realValue);
            

        }

        public void SetFieldValues(Dictionary<IB_Field, object> DataFields)
        {
            if (DataFields is null)
            {
                return;
            }

            foreach (var item in DataFields)
            {
                try
                {
                    this.SetFieldValue(item.Key, item.Value);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            
        }

        public bool IsInModel(Model model)
        {
            return !(this.GhostOSObject.GetIfInModel(model) is null);
        }
        //this is for override
        //public abstract ModelObject ToOS(Model model);
        //protected abstract ModelObject NewOpsObj(Model model);
        //protected abstract T InitOpsObj<T>(Model model);
        //protected delegate ModelObject InitMethodDelegate(Model model);

        protected T OnNewOpsObj<T>(Func<Model, T> InitMethodHandler, Model model) where T: ModelObject
        {
            if (InitMethodHandler == null)
            {
                return null;
            }
            
            ModelObject realObj = null;
            if (this is IIB_DualLoopObj)
            {
                var objInModel = this.GhostOSObject.GetIfInModel(model);
                var ifNull = objInModel is null;
                realObj = ifNull ? InitAndSetAttributes(): objInModel;
                
            }
            else
            {
                realObj = InitAndSetAttributes();
            }
            var realName = realObj.nameString();
            AddOutputVariablesToModel(this.CustomOutputVariables, realName, model);
            
            return realObj as T;


            ModelObject InitAndSetAttributes()
            {
                
                var obj = InitMethodHandler(model);
                obj.SetCustomAttributes(this.CustomAttributes);
                return obj;
            }
            


        }
       

        
        static internal bool AddOutputVariablesToModel(ICollection<IB_OutputVariable> outputVariables, string keyName, Model md)
        {
            var success = true;
            var vs = outputVariables;
            foreach (var item in vs)
            {
                var outV = new OutputVariable(item.VariableName, md);
                success &= outV.setReportingFrequency(item.TimeStep);
                success &= outV.setKeyValue(keyName);
            }
            return success;
        }

        //protected virtual ModelObject ToOS(Model model, Func<ModelObject> GetFromModelfunc)
        //{
        //    var realObj = GetFromModelfunc.Invoke();
        //    realObj.SetCustomAttributes(this.CustomAttributes);

        //    return realObj;
        //}

        //TODO: need to revisit this. this method has been overridden, but not used.
        public virtual IB_ModelObject Duplicate()
        {
            var newObj = this.DuplicateIBObj(IB_InitSelf);
            newObj.Children.Clear();
            foreach (var child in this.Children)
            {
                newObj.Children.Add(child.Duplicate());
            }

            
            return newObj;
        }

        
        protected IB_ModelObject DuplicateIBObj(Func<IB_ModelObject> func)
        {
            if (func == null)
            {
                return null;
            }

            var newObj = func.Invoke();

            foreach (var item in this.CustomAttributes)
            {
                newObj.CustomAttributes.TryAdd(item.Key, item.Value);
            }

            newObj.UpdateOSModelObjectWithCustomAttr();
            newObj.AddOutputVariables(this.CustomOutputVariables);
            return newObj;
        }

        protected T DuplicateIBObj<T>(Func<T> func) where T : IB_ModelObject
        {
            if (func == null)
            {
                return null;
            }

            var newObj = func.Invoke();

            foreach (var item in this.CustomAttributes)
            {
                newObj.CustomAttributes.TryAdd(item.Key, item.Value);
            }

            newObj.UpdateOSModelObjectWithCustomAttr();

            return newObj;
        }

        protected void UpdateOSModelObjectWithCustomAttr()
        {
            this.GhostOSObject.SetCustomAttributes(this.CustomAttributes);
        }
        
        public override string ToString()
        {
            ////var attributes = this.CustomAttributes.Select(_ => String.Format("{0}({1})", _.Key, _.Value));
            var attributes = this.GhostOSObject.GetUserFriendlyFieldInfo(IPUnit);
            var outputString = String.Join("\r\n", attributes);
            return outputString;
            //return this.GhostOSObject.__str__();
        }

        public virtual List<string> ToStrings()
        {
            var s = new List<string>();
            s.Add(this.GhostOSObject.__str__());

            var parentObj = this.Duplicate().GhostOSObject.to_ParentObject();
            if (parentObj.is_initialized())
            {
                var obj = parentObj.get();
                var children = obj.children();
                foreach (var item in children)
                {
                    s.Add(item.__str__());
                }

            }
            return s;
        }

        

        private static string CreateUID()
        {
            var idKey = "TrackingID:#[";
            var uid = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("=", "").Replace("/", "").Replace("+", "").Substring(0, 8);
            var trackingID = String.Format("{0}{1}{2}", idKey, uid, "]");

            return trackingID;
        }

        

    }
}
