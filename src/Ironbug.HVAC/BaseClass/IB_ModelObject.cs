using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ironbug.Core;

namespace Ironbug.HVAC.BaseClass
{
    public class PuppetEventArg : EventArgs
    {
        public IB_PuppetableState State { get; private set; }
        public PuppetEventArg(IB_PuppetableState state)
        {
            this.State = state;
        }

    }
    public abstract class IB_ModelObject : IIB_ModelObject
    {
        public event EventHandler<PuppetEventArg> PuppetEventHandler;
        protected abstract Func<IB_ModelObject> IB_InitSelf { get; }
        public IList<IB_Child> Children { get; private set; } = new List<IB_Child>();

        public IB_PuppetableState CurrentState { get; private set; }
        public Dictionary<string, object> CustomAttributes { get; private set; }
        protected ModelObject GhostOSObject { get; private set; }

        public IB_ModelObject(ModelObject GhostOSObject)
        {
            this.CurrentState = new IB_PuppetableState_None(this);
            this.CustomAttributes = new Dictionary<string, object>();
            this.GhostOSObject = GhostOSObject;
            this.SetTrackingID();
        }

        public void ChangeState(IB_PuppetableState newState)
        {
            this.CurrentState = newState;
            

        }

        //public bool IsPuppet()
        //{
        //    return this.CurrentState is IB_PuppetableState_Puppet;
        //}

        public bool IsPuppetHost()
        {
            return this.CurrentState is IB_PuppetableState_Host;
        }

        ///// <summary>
        ///// Duplicate self as a "Puppet", including its sub-relationship connections
        ///// </summary>
        ///// <returns></returns>
        //public IB_ModelObject DuplicateAsPuppet()
        //{
        //    var puppet = this.Duplicate();
        //    puppet.SetAllTrackingIDs();

        //    if (this.CurrentState is IB_PuppetableState_None state)
        //    {
        //        state.ToParentState().AddPuppet(puppet);

        //    }
        //    else if (this.CurrentState is IB_PuppetableState_Parent stateParent)
        //    {
        //        stateParent.AddPuppet(puppet);
        //    }
        //    else
        //    {
        //        //currently only two available
        //    }

        //    return puppet;

        //}
        public IB_ModelObject ToPuppetHost()
        {
            this.CurrentState.ToPuppetHost();
            foreach (var child in this.Children)
            {
                child.Get().ToPuppetHost();
            }

            return this;
        }
        

        public virtual IB_ModelObject DuplicateAsPuppet()
        {

            var puppet = this.DuplicateAsPuppet(IB_InitSelf);
            
            puppet.Children.Clear();
            foreach (var child in this.Children)
            {
                var childPuppet = child.DuplicateAsPuppet();
                puppet.Children.Add(childPuppet);
            }

            this.PuppetEventHandler?.Invoke(this, new PuppetEventArg(this.CurrentState));
            return puppet;
        }
        
        
        protected T DuplicateAsPuppet<T>(Func<T> DupMethodHandler) where T: IB_ModelObject
        {
            T puppet = null;
            if (this.CurrentState is IB_PuppetableState_Host stateParent)
            {
                puppet = this.DuplicateIBObj(DupMethodHandler);
                //puppet.ToPuppet();
                puppet.SetTrackingID();
                stateParent.AddPuppet(puppet);
            }
            else
            {
                throw new ArgumentException("Item has to be a puppet host.");//TODO: show the item's name
                //currently only two available
            }

            return puppet;

            //if (this.CurrentState is IB_PuppetableState_None state)
            //{
            //    state.ToPuppetHost().AddPuppet(puppet);

            //}
            //else if (this.CurrentState is IB_PuppetableState_Host stateParent)
            //{
            //    stateParent.AddPuppet(puppet);
            //}
            //else
            //{
            //    //currently only two available
            //}



        }
        

        public IList<IIB_ModelObject> GetPuppetsOrSelf()
        {

            //TODO: move this part to IB_PuppetableState later
            var puppetsOrSelf = this.GetPuppets();
            if (!puppetsOrSelf.Any())
            {
                puppetsOrSelf.Add(this);
            }

            return puppetsOrSelf;
        }

        public IList<IIB_ModelObject> GetPuppets()
        {

            //TODO: move this part to IB_PuppetableState later
            var puppets = new List<IIB_ModelObject>();
            if (this.CurrentState is IB_PuppetableState_Host state)
            {
                puppets.AddRange(state.Puppets);
                //state.ExpireState();
            }

            return puppets;
        }

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
            var attributeName = "setComment";
            var data = CreateUID();

            this.CustomAttributes.TryAdd(attributeName, data);
            this.GhostOSObject.setComment(data);
            return data;

        }
        /// <summary>
        /// this is for inherited classes to override all their associated child IB objects
        /// SetTrackingID only updates self object, but SetAllTrackingIDs will update self object and its child.
        /// Use this after called DuplicateAsPuppet method.
        /// </summary>
        public virtual void SetAllTrackingIDs()
        {
            //this is default. 
            this.SetTrackingID();
        }

        public void SetAttribute(IB_DataField dataField, object value)
        {
            var AttributeName = dataField.SetterMethodName;
            var data = value;
            
            this.CustomAttributes.TryAdd(AttributeName, data);

            //dealing the ghost object
            this.GhostOSObject.SetCustomAttribute(AttributeName, data);
            
        }

        public void SetAttributes(Dictionary<IB_DataField, object> DataFields)
        {
            if (DataFields==null)
            {
                return;
            }

            foreach (var item in DataFields)
            {
                try
                {
                    this.SetAttribute(item.Key, item.Value);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            
        }

        public bool IsInModel(Model model)
        {
            return !this.GhostOSObject.GetIfInModel(model).isNull();
        }
        //this is for override
        //public abstract ModelObject ToOS(Model model);
        protected abstract ModelObject InitOpsObj(Model model);
        //protected abstract T InitOpsObj<T>(Model model);
        //protected delegate ModelObject InitMethodDelegate(Model model);

        protected ModelObject OnInitOpsObj(Func<Model, ModelObject> InitMethodHandler, Model model)
        {
            if (InitMethodHandler == null)
            {
                return null;
            }

            ModelObject realObj = null;
            if (this is IIB_ShareableObj)
            {
                var objInModel = this.GhostOSObject.GetIfInModel(model);
                realObj = objInModel.isNull()? InitAndSetAttributes(): objInModel.get();
                
            }
            else
            {
                realObj = InitAndSetAttributes();
            }
            
            ModelObject InitAndSetAttributes()
            {
                var obj = InitMethodHandler(model);
                obj.SetCustomAttributes(this.CustomAttributes);
                return obj;
            }
            
            
            return realObj;
        }

        

        //TODO: need double check, this might not be working
        protected T OnInitOpsObj<T>(Func<Model, T> initMethod, Model model, Func<ModelObject, T> postProcess) where T : ModelObject
        {
            if (initMethod == null)
            {
                return null;
            }

            ModelObject realObj = null;
            if (this is IIB_ShareableObj)
            {
                var objInModel = this.GhostOSObject.GetIfInModel(model);
                realObj = objInModel.isNull() ? initMethod(model) : objInModel.get() as ModelObject;
            }
            else
            {
                realObj = initMethod(model);
            }

            realObj.SetCustomAttributes(this.CustomAttributes);
            
            
            return postProcess(realObj);
        }

        //protected virtual ModelObject ToOS(Model model, Func<ModelObject> GetFromModelfunc)
        //{
        //    var realObj = GetFromModelfunc.Invoke();
        //    realObj.SetCustomAttributes(this.CustomAttributes);

        //    return realObj;
        //}

        public virtual IB_ModelObject Duplicate()
        {
            var newObj = this.DuplicateIBObj(IB_InitSelf);

            foreach (var child in this.Children)
            {
                newObj.Children.Add(child.Duplicate());
            }

            return newObj;
        }

        
        protected virtual IB_ModelObject DuplicateIBObj(Func<IB_ModelObject> func)
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

        protected virtual T DuplicateIBObj<T>(Func<T> func) where T : IB_ModelObject
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
            var attributes = GetDataFields();
            var outputString = String.Join("\r\n", attributes);
            return outputString;
            //return this.GhostOSObject.__str__();
        }

        public IEnumerable<string> GetDataFields()
        {
            var com = this.GhostOSObject;

            var iddObject = com.iddObject();
            var dataFields = new List<string>();
            foreach (var item in com.dataFields())
            {

                var customStr = com.getString(item).get();
                //var customDouble = com.getDouble(item).is_initialized()? com.getDouble(item).get(): -9999;

                var field = iddObject.getField(item).get();
                var dataname = field.name();

                var unit = field.getUnits().isNull() ? string.Empty : field.getUnits().get().standardString();
                var stringDefault = field.properties().stringDefault;
                var defaultStr = stringDefault.isNull() ? string.Empty : stringDefault.get();
                //strDefault has numDefault already
                //var numDefault = field.getUnits().isNull() ? -9999 : field.properties().numericDefault.get();

                var shownStr = string.IsNullOrWhiteSpace(customStr) ? defaultStr : customStr;
                var shownUnit = string.IsNullOrWhiteSpace(unit) ? string.Empty : string.Format(" [{0}]", unit);
                var shownDefault = string.IsNullOrWhiteSpace(defaultStr) ? string.Empty : string.Format(" (Default: {0})", defaultStr);
                var att = String.Format("{0,-20} !- {1} {2} {3}", shownStr, dataname, shownUnit, shownDefault);


                dataFields.Add(att);
            }

            return dataFields;

        }

        private static string CreateUID()
        {
            var idKey = "TrackingID:#[";
            var uid = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("=", "").Replace("/", "").Replace("+", "").Substring(0, 6);
            var trackingID = String.Format("{0}{1}{2}", idKey, uid, "]");

            return trackingID;
        }




    }
}
