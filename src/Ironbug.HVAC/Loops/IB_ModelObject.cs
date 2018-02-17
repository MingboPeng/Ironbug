using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ironbug.Core;

namespace Ironbug.HVAC
{
    public abstract class IB_ModelObject
    {
        public Dictionary<string, object> CustomAttributes { get; private set; }
        protected ParentObject ghostModelObject { get; set; }

        public IB_ModelObject()
        {
            this.CustomAttributes = new Dictionary<string, object>();
        }

        
        public object GetDataFieldValue(string DataFieldName)
        {
            return this.ghostModelObject.GetDataFieldValue(DataFieldName);
        }

        public void SetAttribute(IB_DataField DataAttribute, object AttributeValue)
        {
            var AttributeName = DataAttribute.SetterMethodName;
            var data = AttributeValue;

            if (AttributeName == "setName")
            {
                data = this.ghostModelObject.CheckName(data.ToString());
            }

            this.CustomAttributes.TryAdd(AttributeName, data);

            //dealing the ghost object
            this.ghostModelObject.SetCustomAttribute(AttributeName, data);
            
        }

        public void SetAttributes(Dictionary<IB_DataField, object> DataFields)
        {
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
            return !this.ghostModelObject.IsNotInModel(model);
        }

        protected delegate ParentObject DelegateDeclaration(Model model);
        protected virtual ParentObject ToOS(DelegateDeclaration handler, Model model)
        {
            if (handler == null)
            {
                return null;
            }

            var name = this.ghostModelObject.nameString();
            var objInModel = model.getParentObjectByName(name);
            
            var realObj = objInModel.isNull() ? handler(model) : objInModel.get();
            realObj.SetCustomAttributes(this.CustomAttributes);

            return realObj;
        }

        public abstract IB_ModelObject Duplicate();
        //protected delegate IB_ModelObject DelegateDuplicate(Model model);
        protected virtual IB_ModelObject Duplicate(Func<IB_ModelObject> func)
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
            this.ghostModelObject.SetCustomAttributes(this.CustomAttributes);
        }

        public abstract ParentObject ToOS(Model model);

        public override string ToString()
        {
            //var attributes = this.CustomAttributes.Select(_ => String.Format("{0}({1})", _.Key, _.Value));
            var attributes = GetDataFields();
            var outputString = String.Join("\r\n", attributes);
            return outputString;
        }

        public IEnumerable<string> GetDataFields()
        {
            var com = this.ghostModelObject;

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

        

        
    }
}
