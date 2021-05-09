using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.Core;
using Ironbug.HVAC;
using System.Runtime.Serialization;

namespace Ironbug.HVAC.BaseClass
{
    [DataContract]
    public abstract class IB_ModelObject : IIB_ModelObject, IEquatable<IB_ModelObject>
    {
        public string Memo { get; set; }
        public IEnumerable<string> SimulationOutputVariables { get; }
        public static bool IPUnit { get; set; } = false;
        //public event EventHandler<PuppetEventArg> PuppetEventHandler;
        protected abstract Func<IB_ModelObject> IB_InitSelf { get; }
        [DataMember]
        public IList<IB_Child> Children { get; private set; } = new List<IB_Child>();

        //public IB_PuppetableState CurrentState { get; private set; }
        [DataMember]
        public IB_FieldArgumentSet CustomAttributes { get; private set; } = new IB_FieldArgumentSet();
        //public Dictionary<IB_Field, object> CustomAttributes { get; private set; } = new Dictionary<IB_Field, object>();
   

        protected ModelObject GhostOSObject { get; private set; }
        [DataMember]
        public List<IB_OutputVariable> CustomOutputVariables { get; private set; } = new List<IB_OutputVariable>();

        private IList<string> RefObjects { get; set; } = new List<string>();

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

        public void SetRefObject(IList<string> RefObjectStrs)
        {
            if (RefObjectStrs is null) return;
            GhostOSObject = this.InitFromRefObj(GhostOSObject.model(), RefObjectStrs);
            this.RefObjects = RefObjectStrs;
        }

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
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Schedule"))
                    {
                        throw new ArgumentException($"Failed to set {item.Key}! Please double check input data type, or schedule type! \n" +
                            $"In most cases, some components only accepts \"Temperature\" schedule type instead of default \"Dimensionless\". \n" +
                            $"If this is the case, please use ScheduleTypeLimits to set \"UnitType\" to an appropriate value.\n" +
                            $"Detail error message:\n{ex.Message}");
                    }
                    else
                    {
                        throw new ArgumentException($"Failed to set {item.Key}! Please double check input data type, or typo! \nDetail error message:\n{ex.Message}");
                    }
                    
                }
            }
            
        }

        public bool IsInModel(Model model)
        {
            return !(this.GhostOSObject.GetIfInModel(model) is null);
        }

        public ModelObject GetOsmObjInModel(Model model)
        {
            return this.GhostOSObject.GetIfInModel(model);
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
            
            return realObj as T;


            ModelObject InitAndSetAttributes()
            {

                var obj = this.RefObjects.Any() ? InitFromRefObj(model, this.RefObjects) : InitMethodHandler(model);
                obj.SetCustomAttributes(this.CustomAttributes);
                obj.SetOutputVariables(this.CustomOutputVariables);
                return obj;
            }

           
        }

        ModelObject InitFromRefObj(Model model, IList<string> ParamSource)
        {
            try
            {
                var tempModel = new OpenStudio.Model();
                var idfs = new IdfObjectVector();
                var idfobjs = ParamSource
                    .Select(_ => IdfObject.load(_))
                    .Where(_ => _.is_initialized())
                    .Select(_ => _.get());

                foreach (var item in idfobjs)
                {
                    idfs.Add(item);
                }

                //model.addObjects(idfs,true);
                var addedObjs = tempModel.insertObjects(idfs);
                var counts = addedObjs.Count;
                //get the main object if it has children
                WorkspaceObject mainObj = addedObjs.FirstOrDefault(_ => _.iddObject().name() == this.GhostOSObject.iddObject().name());
                ModelObject obj = mainObj.CastToOsType();

                var clonedObj = obj.clone(model).CastToOsType();
                //obj.remove();
                return clonedObj;
            }
            catch (Exception e)
            {

                throw new ArgumentException($"Error at InitFromRefObj. {e.Message}");
            }


        }

        
        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
     
     
        public static T FromJson<T>(string json) where T: IB_ModelObject
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, IB_JsonSetting.ConvertSetting);
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
            return this.Duplicate(IB_InitSelf);
        }

        
        protected T Duplicate<T>(Func<T> func) where T : IB_ModelObject
        {
            if (func == null)
            {
                return null;
            }

            var newObj = func.Invoke();

            foreach (var item in this.CustomAttributes)
            {
                newObj.CustomAttributes.TryAdd(item.Field, item.Value);
            }

            newObj.UpdateOSModelObjectWithCustomAttr();
            newObj.AddOutputVariables(this.CustomOutputVariables);
            newObj.RefObjects = this.RefObjects;

            newObj.Children.Clear();
            foreach (var child in this.Children)
            {
                newObj.Children.Add(child.Duplicate());
            }
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
            var selfString = this.GhostOSObject.__str__();
            s.Add(selfString);

            //if (GhostOSObject is ParentObject pObj)
            //{
            //    var copiedObj = pObj.clone().to_ParentObject().get();
            //    var childrenStrs = copiedObj.children().Select(_=>_.__str__());
            //    s.AddRange(childrenStrs);
            //}


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
            var uid = Guid.NewGuid().ToString().Substring(0, 8);
            var trackingID = String.Format("{0}{1}{2}", idKey, uid, "]");

            return trackingID;
        }

        public bool Equals(IB_ModelObject other)
        {
            if (other is null)
                return this is null ? true : false;
            var same = this.CustomAttributes.SequenceEqual(other.CustomAttributes);
            same &= this.CustomOutputVariables.SequenceEqual(other.CustomOutputVariables);
            same &= this.Children.SequenceEqual(other.Children);
            same &= this.GetType() == other.GetType();
            return same;
        }
        public static bool operator ==(IB_ModelObject x, IB_ModelObject y)
        {
            if (x is null)
                return y is null ? true : false;
            return x.Equals(y);
        }

        public static bool operator !=(IB_ModelObject x, IB_ModelObject y) => !(x == y);
    }
}
